using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mobile_Deura.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return PartialView();
        }
        public ActionResult CheckLogin(string username, string password)
        {
            var db = new Business.Business();
            var result = db.EmpCheckLogin(username, password);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {
//            Session["Emp"] = null;
            Response.Cookies["id"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["FullName"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["agent"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["idCity"].Expires = DateTime.Now.AddDays(-1);
          //  Response.Cookies["LoaiTK"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index", "Login");

        }
    }
}