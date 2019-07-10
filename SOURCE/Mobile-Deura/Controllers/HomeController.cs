using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime;
using Mobile_Deura.Const;
using Mobile_Deura.Models;
using Mobile_Deura.Untils;
using Newtonsoft.Json;
using RestSharp;

namespace Mobile_Deura.Controllers
{
    [EmpCheckLogin]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult Campaign()
        {
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public ActionResult CampaignCenter(int id, string name, string endtime, int Script_id, string campaigncode)
        {
            var db = new Business.Business();

            int idLogin = 0;
            if (Request.Cookies["id"] != null)
            {
                var value = Request.Cookies["id"].Value;
                idLogin = int.Parse(value);
            }

            var result = db.CheckCenterCampaign(idLogin);
            if (result == null || result.Count == 0)
                return RedirectToAction("Index");

            try
            {
                ViewBag.name = name;
                ViewBag.endtime = endtime;
                ViewBag.campaign_id = id;
                ViewBag.Script_id = Script_id;
                ViewBag.Campaigncode = campaigncode;
           
                return PartialView();
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }      
        }

        
        [HttpGet]
        public ActionResult Campaign(int id , string name , string endtime , int Script_id , string campaigncode)
        {
            var db = new Business.Business();

          
            int idLogin = 0;
            if (Request.Cookies["id"] != null)
            {
                var value = Request.Cookies["id"].Value;
                idLogin = int.Parse(value);
            }

            var result = db.CheckAlertCampaign(idLogin);
            if(result == null || result.Count == 0)
                return RedirectToAction("Index");     
            try
            {
                ViewBag.name = name;
                ViewBag.endtime = endtime;
                ViewBag.campaign_id = id;
                ViewBag.Script_id = Script_id;
                ViewBag.Campaigncode = campaigncode;
                
                return PartialView();

            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }


   
        private string SetSource()
        {
            string source;
            int idLogin;
            try
            {
                if (Request.Cookies["id"] != null)
                {
                    var value = Request.Cookies["id"].Value;
                    idLogin = int.Parse(value);

                    var db = new Business.Business();

                    User us = db.getInfoUser(idLogin);
                    if (us.idCity == 1)
                    {
                        return source = "HPG.PG"; // HP
                    }
                    else if (us.idCity == 5)
                    {
                        return source = "CTO.PG"; // can tho
                    }
                    else
                    {
                        return source = "HNI.PG";
                    }
                }
                else
                {
                    return "NullID";
                }
            }
            catch(Exception e)
            {
                return "";
            }
           
        }


        public ActionResult ListDataCurrentday()
        {
            var db = new Business.Business();
            var result = db.GetListPgDataByEmp();
            var lstSuccess = result.Where(ob => ob.sent_log.Contains("True"));
            var lstFalse = result.Where(ob => ob.sent_log.Contains("False"));
            ViewBag.success = lstSuccess.Count();
            ViewBag.lstFalse = lstFalse.Count();
            return PartialView(result);
        }

        // add campaign cho PG va trung tam
        public ActionResult _AddDataCampaign(string fullname, string phone, string center, string note, string localid, int? id_campaign, int? id_script, string campaigncode)
        {
            var db = new Business.Business();
            pg_data data = new pg_data();
            data.name = fullname.Trim().ToLower();
            data.mobile = phone.Trim().ToLower();
            data.center = center;
            data.note = note;
            data.id_campaign = id_campaign;
            data.id_script = id_script;

 
            string city_code = null;

            if (!String.IsNullOrEmpty(localid))
            {
                data.local_id = Convert.ToInt32(localid);
                city_code = db.GetCityLocal(Convert.ToInt32(localid));
            }
            var agent = AccountUntils.GetEmp().agent;

            var resultLog = CallServiceCcsiCampaign(fullname , null, null , center + "#" + note , phone , null , localid, campaigncode, null , agent  , city_code , data);
           
            var log = resultLog.IsSuccess.ToString() + "#" + resultLog.Message;

            data.sent_log = log;

            var result = db.AddData(data);

            //  db.UpInfoSendSMS(center , fullname , phone);    // sua lai noi dung gui sms

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult AddData(string fullname, string phone, string address, string center, string note , string localid ,bool isCampaign)
        {
            var db = new Business.Business();
            pg_data data = new pg_data();
            data.name = fullname.Trim().ToLower();
            data.mobile = phone.Trim().ToLower();
            data.address = address;
            data.center = center;
            data.note = note;
            data.isCampaign = isCampaign;

            string city_code = null;

            if (!String.IsNullOrEmpty(localid))
            {
                data.local_id = Convert.ToInt32(localid);
                city_code = db.GetCityLocal(Convert.ToInt32(localid));
            }
           
            var agent = AccountUntils.GetEmp().agent;
            var resultLog = CallBackgroudJob(fullname, phone, agent, center + "#" + note , localid , city_code  , isCampaign , data );
           
//           if (resultLog.IsSuccess)
//            {
            var log = resultLog.IsSuccess.ToString() + "#" + resultLog.Message;
            var strReport = resultLog.Message;
            data.sent_log = log;
            var result = db.AddData(data);
//            }

            if(resultLog.IsSuccess == false)
            {
                resultLog.Message = "Số điện thoại vi phạm điều kiện (đã có trên hệ thống,trùng demo...)#" + strReport.ToString();
            }

            return Json(new { result = resultLog }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult LocalComboboxLocal()
        {
            int idLogin = 0;
            if (Request.Cookies["id"] != null)
            {
                var value = Request.Cookies["id"].Value;
                idLogin = int.Parse(value);
            }

            var db = new Business.Business();
            var result = db.ListLocalById(idLogin);
            return PartialView(result);
        }


        public ActionResult LoadComboboxCenter()
        {
            var db = new Business.Business();
            

            if (Request.Cookies["idCity"] != null)
            {
                var value = Request.Cookies["idCity"].Value;          


                var result = db.ListCenterById(int.Parse(value));

                return PartialView(result);
            }
            else
            {
                var result = db.ListCenters();
                return PartialView(result);
            }          
        }

        public int Cookies_Get()
        {
            int Cid = 0;
            if (Request.Cookies["idCity"] != null)
            {
                var value = Request.Cookies["idCity"].Value;
                Cid = int.Parse(value);
            }
            return Cid;
        }
   


        //public ActionResult LoadComboboxCenter(int idCity)
        //{
        //    var db = new Business.Business();
        //    var result = db.ListCenterById(idCity);
        //    return PartialView(result);
        //}

        public ActionResult AlertCampaign()
        {
            var db = new Business.Business();
            int idLogin = 0;
            if (Request.Cookies["id"] != null)
            {
                var value = Request.Cookies["id"].Value;
                idLogin = int.Parse(value);
            }
            var result = db.CheckAlertCampaign(idLogin);
            // kiem tra loai campagin
            if(result.Count == 0)
            {
                ViewBag.checkcenter = db.CheckCenterCampaign(idLogin);
            }
            
            return PartialView(result);
        }


         

        private SystemMessage CallServiceCcsiCampaign(string fullname,string gender,string age,string dataNote,string mobile,string voucherCode,string locationId,string campaignCode,string appoinmentType , string staffCode , string cityCode, pg_data data)
        {
            SystemMessage systemMessage = new SystemMessage();

            // test
            //campaignCode = "TESSSSSSS";
            //cityCode = "HN";
            if (cityCode.Trim() == "HCM")
            {
                cityCode = "HN";
            }


            try
            {
                //  var client = new RestClient("http://192.168.1.222/data");

                var client = new RestClient("http://192.168.1.222/data/pg");

                var request = new RestRequest();

                request.Method = Method.POST;
                request.AddHeader("API-KEY-AUTH", "1qazxsw@#!$");
                request.AddParameter("fullname", fullname);
                request.AddParameter("gender", null);
                request.AddParameter("age", age);
                request.AddParameter("dataNote", dataNote);
                request.AddParameter("mobile", mobile);
                request.AddParameter("locationId", locationId);
                request.AddParameter("campaignCode", campaignCode);
                request.AddParameter("staffCode", staffCode); //staffCode
                request.AddParameter("cityCode", cityCode);

                var response = client.ExecuteTaskAsync(request);
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(response.Result.Content);

                var status = jsonObject.status;
                systemMessage.IsSuccess = status == "FAIL" ? false : true;
                systemMessage.Message = jsonObject.msg;

                //var dt = JsonConvert.DeserializeObject<dynamic>(jsonObject);
                //var status = dt["status"];
                //systemMessage.IsSuccess = status == "FAIL" ? false : true;
                //systemMessage.Message = dt["msg"];

            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                var db = new Business.Business();
                data.sent_log = "false#" + e.ToString();
                db.AddData(data);

            }

            return systemMessage;
        }

        private SystemMessage CallBackgroudJob(string fullname, string phone, string agent, string centerNote, string local_id , string city_code , bool? isCampaign, pg_data data)
        {
            SystemMessage systemMessage = new SystemMessage();

            string source = SetSource();

            try
            {
                //  Console.WriteLine(source);
                var client = new RestClient("http://192.168.1.222/lead/lead");
                var request = new RestRequest();

                request.Method = Method.POST;
                request.AddHeader("API-KEY-AUTH", "1qazxsw@#!$");
                request.AddParameter("fullname", fullname);
                request.AddParameter("mobile", phone);
                request.AddParameter("source", source);
                request.AddParameter("agent", agent);
                request.AddParameter("centernote", centerNote);
                request.AddParameter("city_code", city_code);
                request.AddParameter("local_id", local_id);
                request.AddParameter("isCampaign", isCampaign);

                var response = client.ExecuteTaskAsync(request);
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(response.Result.Content);
                var dt = JsonConvert.DeserializeObject<dynamic>(jsonObject);
                var status = dt["status"];
                systemMessage.IsSuccess = status == "FAIL" ? false : true;

                systemMessage.Message = dt["msg"];
                //if(systemMessage.IsSuccess == false && systemMessage.Message == null)
                //{
                //    systemMessage.Message = "Số điện thoại vi phạm điều kiện ( trùng demo ... )";
                //}

            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                var db = new Business.Business();
                data.sent_log = "false#" + e.ToString();
                db.AddData(data);
            }

            return systemMessage;
        }

    }
}