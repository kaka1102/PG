using System.Web.Mvc;

namespace Mobile_Deura.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Admin"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //context.MapRoute(
            //    "Admin_default",
            //    "Admin/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional},
                new[] {"Mobile_Deura.Areas.Admin.Controllers"}
            );

            context.MapRoute(
                name: "Home2",
                url: "admin",
                defaults: new
                {
                    controller = "Login",
                    action = "Index"
                }
            );
        }
    }
}