using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Mobile_Deura.Untils;

namespace Mobile_Deura.Controllers
{
    [EmpCheckLogin]
    public class AccountInfoController : Controller
    {
        // GET: AccountInfo
        public ActionResult ChangePassword()
        {
            return PartialView();
        }

        public ActionResult _ChangePassword(string oldPass,string newPass,string confirmPass)
        {
            var db = new Business.Business();
            var result = db.ChangePassword(oldPass, newPass, confirmPass);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}