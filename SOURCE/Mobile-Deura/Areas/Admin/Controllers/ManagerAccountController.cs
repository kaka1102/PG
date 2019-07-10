using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mobile_Deura.Models;
using Mobile_Deura.Untils;

namespace Mobile_Deura.Areas.Admin.Controllers
{
    [CheckLogin]
    public class ManagerAccountController : Controller
    {
        // GET: Admin/ManagerAccount
        public ActionResult ListAccount()
        {
            User user = AccountUntils.GetUser();
            ViewBag.TypeAccount = user.LoaiTK;
            return PartialView();
        }
        public ActionResult LoadCityCombobox(int idSelected=0)
        {
            var db = new Business.Business();
            ViewBag.idSelected = idSelected;
            var result = db.ListCity();
            return PartialView(result);
        }
        public ActionResult _ListAccount()
        {

            User user = AccountUntils.GetUser();
            ViewBag.TypeAccount = user.LoaiTK;
            var db = new Business.Business();
            var result = db.ListUser();
            ViewBag.center = db.GetAllCenter();

            return PartialView(result);

        }
        public ActionResult AddAccount()
        {
            var db = new Business.Business();
            User user = AccountUntils.GetUser();

            if (user.LoaiTK == 100)
            {
                ViewBag.lstTeamLead = db.GetTeamleadById(user.Id);
            }
            else
            {
                ViewBag.lstTeamLead = db.GetAllTeamLead();
            }

            ViewBag.lstUserManager = db.GetAllUserManager();     
            ViewBag.LoaiTK = user.LoaiTK;
            ViewBag.Id = user.Id;
            ViewBag.UserName = user.UserName;
            ViewBag.Group = db.GetAllGroups();

            return PartialView();


        }

        public ActionResult _AddAccount(string username, string password, string confirmPassword, string fullname, string birth, string phone, string email, string source, string teamname, int LoaiTK = 0, int idParent = 0, int idCity = 0 , int idManager = 0 , int idCenter = 0 )  
        {
            var db = new Business.Business();
            User user = new User();
            user.UserName = username;

            if (!password.Equals(confirmPassword))
            {
                SystemMessage systemMessage = new SystemMessage();
                systemMessage.IsSuccess = false;
                systemMessage.Message = "Xác nhận mật khẩu không chính xác";
                return Json(new { result = systemMessage }, JsonRequestBehavior.AllowGet);
            }

            user.Password = password;

            DateTime _birth = new DateTime(1, 1, 1);

            if (!string.IsNullOrEmpty(birth) && DateTime.TryParseExact(birth, "dd/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _birth) == false)
            {
                SystemMessage systemMessage = new SystemMessage();
                systemMessage.IsSuccess = false;
                systemMessage.Message = "Ngày tháng năm sinh không đúng định dạng";
                return Json(new { result = systemMessage }, JsonRequestBehavior.AllowGet);
            }

            if (DateTime.TryParseExact(birth, "dd/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _birth))
            {
                user.DateOfBirth = _birth;
            }

            user.Password = password;
            user.FullName = fullname;
            user.agent = phone;
            user.Email = email;
            user.source = source;
            user.LoaiTK = LoaiTK;
            user.idCity = idCity;
            user.idManager = idManager;
            user.idCenter = idCenter;
            user.TeamName = teamname;

            if (idParent != 0 )
            {
                user.idParent = idParent;
            }

            var result = db.AddUser(user);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditAccount(int id)
        {
            var db = new Business.Business();
            var result = db.GetUserById(id);
            ViewBag.id = id;

            User user = AccountUntils.GetUser();

            ViewBag.loaiTK = user.LoaiTK;
            ViewBag.UserName = user.UserName;
            ViewBag.TeamName = result.TeamName;

            if (result!=null)
            {
                int kq = 0;
                if (result.idParent !=null)
                {
                    kq = result.idParent.Value;
                }
                ViewBag.idParent = kq;
            }
            else
            {
                ViewBag.idParent = 0;
            }

            if (user.LoaiTK == 100)
            {
                ViewBag.lstTeamLead = db.GetTeamleadById(user.Id);
            }
            else
            {
                ViewBag.lstTeamLead = db.GetAllTeamLead();
            }
           
            ViewBag.lstUserManager = db.GetAllUserManager();
            ViewBag.lstCenter = db.GetAllCenter();
            ViewBag.Group = db.GetAllGroups();


            return PartialView(result);
        }

       
       
        public ActionResult _EditAccount(int id, string username, string password, string fullname, string birth, string phone, string email, string source , string teamname , int LoaiTK = 0, int idParent = 0, int idCity = 0 , int idManager = 0, int idCenter = 0 )
        {
            var db = new Business.Business();
            User user = new User();
            user.UserName = username;
            user.Password = password;
            user.Id = id;


            DateTime _birth = new DateTime(1, 1, 1);

            if (!string.IsNullOrEmpty(birth) && DateTime.TryParseExact(birth, "dd/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _birth) == false)
            {
                SystemMessage systemMessage = new SystemMessage();
                systemMessage.IsSuccess = false;
                systemMessage.Message = "Ngày tháng năm sinh không đúng định dạng";
                return Json(new { result = systemMessage }, JsonRequestBehavior.AllowGet);
            }

            if (DateTime.TryParseExact(birth, "dd/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _birth))
            {
                user.DateOfBirth = _birth;
            }

            user.Password = password;
            user.FullName = fullname;
            user.agent = phone;
            user.Email = email;
            user.source = source;
            user.LoaiTK = LoaiTK;
            user.idCity = idCity;
            user.idManager = idManager;
            user.idCenter = idCenter;
            user.TeamName = teamname;

            if (LoaiTK == 100)
            {
                user.idParent = 0;
            }
            else
            {
                user.idParent = idParent;
            }

            var result = db.EditUser(user);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteAccount(int id)
        {
            var db = new Business.Business();
            var result = db.DeleteUser(id);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadComboboxCenter()
        {
            var db = new Business.Business();
            var result = db.ListCenters();
            return PartialView(result);

        }

    }
}