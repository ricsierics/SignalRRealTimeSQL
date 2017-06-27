using System;
using System.Configuration;
using System.Data.SqlClient;

namespace SignalRRealTimeSQL
{
    public class Global : System.Web.HttpApplication
    {
        string constring = ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString;

        protected void Application_Start(object sender, EventArgs e)
        {   
            SqlDependency.Start(constring);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            SqlDependency.Stop(constring);
        }
    }
}