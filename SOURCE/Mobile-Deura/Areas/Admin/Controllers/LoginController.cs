using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mobile_Deura.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult CheckLogin(string username,string password)
        {
            var db = new Business.Business();
            var result = db.AdminCheckLogin(username, password);


            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session["admin"] = null;
            return RedirectToAction("Index","Login");
        }
    }
}