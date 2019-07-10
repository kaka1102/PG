using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mobile_Deura.Untils;

namespace Mobile_Deura.Areas.Admin.Controllers
{
    public class DataManagerController : Controller
    {
        // GET: Admin/DataManager
        public ActionResult ListDataManager()
        {
            var db = new Business.Business();
            ViewBag.city = db.GetAllCity();

            return PartialView();
        }

        public ActionResult _ListDataManager(string key,int page = 1)
        {
            var db = new Business.Business();
            User user = AccountUntils.GetUser();

            var result = db.GetProducts(page, 10, key);
            if (result.Total % 10 == 0)
            {
                ViewBag.totalPage = result.Total / 10;
            }
            else
            {
                ViewBag.totalPage = result.Total / 10 + 1;
            }

            ViewBag.pageSelected = page;
            ViewBag.LoaiTK = user.LoaiTK;
 
            return PartialView(result.DatasList);

        }



        public ActionResult _ListDataManagerByCity(int? idcity , string key, int page = 1 )
        {
            var db = new Business.Business();
            User user = AccountUntils.GetUser();


            var result = db.GetProductsByCity(page, 10, key , idcity);

            if (result.Total % 10 == 0)
            {
                ViewBag.totalPage = result.Total / 10;
            }
            else
            {
                ViewBag.totalPage = result.Total / 10 + 1;
            }
            ViewBag.pageSelected = page;
            ViewBag.LoaiTK = user.LoaiTK;

            return PartialView(result.DatasList);
        }

        //        public ActionResult DeleteData(int id)
        //        {
        //            var db = new Business.Business();
        //            var result = db.DeleteDataById(id);
        //            return Json(new { result }, JsonRequestBehavior.AllowGet);
        //        }


        public ActionResult ResendData(int id)
        {
            var db = new Business.Business();
//                    var result = db.DeleteDataById(id);
            return Json(new {  }, JsonRequestBehavior.AllowGet);
        }
    }
}