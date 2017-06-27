using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace SignalRRealTimeSQL
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static IEnumerable<Products> GetData()
        {
            string constring = ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString;
            using (var connection = new SqlConnection(constring))
            {
                connection.Open();
                string query = @"SELECT [ProductID],[Name],[UnitPrice],[Quantity] FROM [db_SignalRDemo].[dbo].[Products]";
                using (var command = new SqlCommand(query, connection))
                {
                    //Make sure the command object does not alread have a notification object associated with it.
                    command.Notification = null;

                    //SqlDependency.Start(constring); 

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        var retVal = reader.Cast<IDataRecord>()
                            .Select(x => new Products()
                            {
                                id = x.GetInt32(0),
                                Name = x.GetString(1),
                                PricDecimal = x.GetDecimal(2),
                                QuantDecimal = x.GetDecimal(3)
                            }).ToList();
                        return retVal;
                    }
                }
            }
        }

        public static void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            MyHub.Show();
        }
    }
}