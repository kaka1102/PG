using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Mobile_Deura.Models;
using Mobile_Deura.Untils;
using Newtonsoft.Json;
using RestSharp;
using shortid;
 

namespace Mobile_Deura.Areas.Admin.Controllers
{
    public class LocalManagerController : Controller
    {

        // GET: Admin/LocalManager
        public ActionResult ListLocalManager()
        {
            return PartialView();
        }

        public ActionResult _ListLocalManager()
        {
           
            var entity = new MobilePREntities();
            User user = AccountUntils.GetUser();

            var idCity = user.idCity;

            List<LocalView> result = null;

            if (idCity != null)
            {
                SqlParameter cid = new SqlParameter("idCity" , idCity);
                result = entity.Database.SqlQuery<LocalView>("SELECT l.local_id, l.name, c.City_Code , c.City_Name, l.isActive, l.parent_id , l.shortcode , l.Id FROM Local l INNER JOIN City c ON l.city_id = c.Id where l.city_id = @idCity ", cid).ToList();
            }
            else
            {
                result = entity.Database.SqlQuery<LocalView>("SELECT l.local_id, l.name, c.City_Code , c.City_Name, l.isActive, l.parent_id , l.shortcode , l.Id FROM Local l INNER JOIN City c ON l.city_id = c.Id ").ToList();
                //  var result = entity.Local.ToList();
            }


            return PartialView(result);

        }

        //public ActionResult _ListLocalManager2()
        //{
        //    var entity = new MobilePREntities();
        //    // User user = AccountUntils.GetUser();

        //    var result = entity.Database.SqlQuery<LocalView>("SELECT l.local_id, l.name, c.City_Code , c.City_Name, l.isActive, l.parent_id , l.shortcode , l.Id FROM Local l INNER JOIN City c ON l.city_id = c.Id ").ToList();
        //    //  var result = entity.Local.ToList();     
        //    return PartialView(result);

        //}


        public ActionResult ResendData(int id)
        {
            return Json(new { }, JsonRequestBehavior.AllowGet);  
        }

        public ActionResult AddLocal()
        {

            var entity = new MobilePREntities();
            ViewBag.lstCity = entity.City;
            return PartialView();

        }


        public ActionResult _AddLocal(string localname , string cityname , string shortcode , int isActive = 1)
        {

            var db = new Business.Business();
            var entity = new MobilePREntities();

            var _localid  = GetMySequence();

            Local lc = new Local();
          
            lc.name = localname;    
            lc.shortcode = shortcode;
            lc.isActive = Convert.ToBoolean(isActive);
            lc.city_id = cityname;
            lc.local_id = _localid;
     
            var result = db.AddLocal(lc);

            if (result.IsSuccess)
            {
                int cityId = Convert.ToInt32(cityname);

                City ct = entity.City.SingleOrDefault(x => x.Id == cityId);         
                addApiLocal(  Convert.ToString(_localid), localname , ct.City_Code , "1" , shortcode );
                //string localid, string name, string city_code, string parent_id, string shortcode)

            }

            return Json(new { result }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult EditAccount(int id)
        {

            return null;
        }
        public ActionResult EditLocal(int id)
        {
            var entity = new MobilePREntities();
            ViewBag.lstCity = entity.City;
            ViewBag.infor = entity.Local.FirstOrDefault(m => m.local_id == id);

            return PartialView();
        }
        public ActionResult _EditLocal(int Id, string localname, string cityname, string shortcode, int isActive = 1)
        {
            var entity = new MobilePREntities();
            var db = new Business.Business();

            Local lc = new Local();
            lc.Id = Id;
            lc.name = localname;
            lc.shortcode = shortcode;
            lc.isActive = Convert.ToBoolean(isActive);
            lc.city_id = cityname;

            var result = db._EditLocal(lc);

            if (result.IsSuccess)
            {
                City ct = entity.City.SingleOrDefault(x => x.Id == Convert.ToInt32(cityname));
                upApiLocal( Convert.ToString(Id) , localname , ct.City_Code , "1" , shortcode);
            }

            return Json(new { result }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult _DeleteLocal(int id)
        {
            var db = new Business.Business();

            Local lc = new Local();
            lc.Id = id;

            var result = db.DeleteLocal(lc);

            if (result.IsSuccess)
            {
                delApiLocal(Convert.ToString(id));
            }

            return Json(new { result }, JsonRequestBehavior.AllowGet);

        }
        public int GetMySequence()
        {
            var entity = new MobilePREntities();
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            entity.Database.ExecuteSqlCommand("SELECT @result = (NEXT VALUE FOR LocalSequence)", result);
            return (int)result.Value;

        }

        private SystemMessage upApiLocal(string localid , string name, string city_code , string parent_id , string shortcode)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var client = new RestClient("http://192.168.1.222/location/location");
                var request = new RestRequest();
                request.Method = Method.PUT;

                request.AddHeader("API-KEY-AUTH", "1qazxsw@#!$");
                request.AddParameter("local_id", localid);
                request.AddParameter("name", name);
                request.AddParameter("city_code", city_code);
                request.AddParameter("parent_id", parent_id);
                request.AddParameter("shortcode", shortcode);

                var response = client.ExecuteTaskAsync(request);
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(response.Result.Content);
                var dt = JsonConvert.DeserializeObject<dynamic>(jsonObject);

                var status = dt["status"];
                systemMessage.IsSuccess = status == "FAIL" ? false : true;
                systemMessage.Message = dt["msg"];

            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();

            }
            return systemMessage;

        }


        private SystemMessage delApiLocal(string localid)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var client = new RestClient("http://192.168.1.222/location/location");
                var request = new RestRequest();

                request.Method = Method.DELETE;

                request.AddHeader("API-KEY-AUTH", "1qazxsw@#!$");
                request.AddParameter("local_id", localid);

                var response = client.ExecuteTaskAsync(request);

                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(response.Result.Content);

                var dt = JsonConvert.DeserializeObject<dynamic>(jsonObject);
                var status = dt["status"];
                systemMessage.IsSuccess = status == "FAIL" ? false : true;
                systemMessage.Message = dt["msg"];

            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();

            }
            return systemMessage;

        }


       
        private SystemMessage addApiLocal(string localid, string name, string city_code, string parent_id, string shortcode)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var client = new RestClient("http://192.168.1.222/location/location");
                var request = new RestRequest();
                request.Method = Method.POST;

                request.AddHeader("API-KEY-AUTH", "1qazxsw@#!$");

                request.AddParameter("local_id", localid);
                request.AddParameter("name", name);
                request.AddParameter("city_code", city_code);
                request.AddParameter("parent_id", parent_id);
                request.AddParameter("shortcode", shortcode);

                var response = client.ExecuteTaskAsync(request);

                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(response.Result.Content);

                var dt = JsonConvert.DeserializeObject<dynamic>(jsonObject);
                var status = dt["status"];
                systemMessage.IsSuccess = status == "FAIL" ? false : true;
                systemMessage.Message = dt["msg"];

            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                
            }
            return systemMessage;
        }
    }
}