using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mobile_Deura.Untils
{
    public class CheckLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["admin"] == null)
            {
                filterContext.Result = new RedirectResult("/ADMIN/Login/Index");
            }
        }
    }

    public class EmpCheckLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
//            if (filterContext.HttpContext.Session["Emp"] == null)
//            {
//                filterContext.Result = new RedirectResult("/Login/Index");
//            }

            if (string.IsNullOrEmpty(AccountUntils.Cookies_Get("id")))
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }
        }
    }
}