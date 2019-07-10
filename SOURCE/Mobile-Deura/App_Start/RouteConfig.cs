using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mobile_Deura
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Mobile_Deura.Controllers" }
            );
            routes.MapRoute(
                name: "Home1",
                url: "",
                defaults: new
                {
                    controller = "Login",
                    action = "Index"
                }
            );
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { string.Format("{0}.Controllers", BuildManager.GetGlobalAsaxType().BaseType.Assembly.GetName().Name) }
            //);
        }
    }
}
