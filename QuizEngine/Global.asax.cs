using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Optimization;
using Newtonsoft.Json.Serialization;

namespace QuizEngine
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            //AreaRegistration.RegisterAllAreas(); // don't need to register when only using attribute routing in areas
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ConfigureApi(GlobalConfiguration.Configuration);
        }

        void ConfigureApi(HttpConfiguration config)
        {
            // Remove XML formatter, only return JSON results
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Set up camel casing for the JSON results
            config.Formatters.JsonFormatter.SerializerSettings
                .ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}