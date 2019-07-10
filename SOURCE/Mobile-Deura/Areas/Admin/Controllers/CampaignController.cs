using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Mobile_Deura.Models;
using Mobile_Deura.Untils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Mobile_Deura.Areas.Admin.Controllers
{
    public class CampaignController : Controller
    {
        // GET: Admin/Campaign
        public ActionResult Index()
        {
            return null;
        }

        public ActionResult ListCampaign()
        {
            var entity = new MobilePREntities();
            ViewBag.lsCampaigntype = entity.CampaignType.Where(x => x.IsActive == true);
            ViewBag.lsDomainType = entity.Domain.ToList();

            return PartialView();
        }


        public ActionResult _AddCampaignWithDomain(string name , string starttime, string endtime , int? number, int? budget , int type, string domain)
        {
            SystemMessage systemMessage = new SystemMessage();
            Guid uid = Guid.NewGuid();
            var result = "";
            try
            {
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(starttime) && !string.IsNullOrWhiteSpace(endtime) && type != 0)
                {
                    using (var entity = new MobilePREntities())
                    {
                        var cam = entity.Set<Campaign>();
                        cam.Add(new Campaign
                        {

                            Name = name,
                            Type = type,
                            StartTime = Convert.ToDateTime(starttime),
                            EndTime = Convert.ToDateTime(endtime),
                            NumberContact = number,
                            Budgets = budget,
                            CampaignCode = Convert.ToString(uid),
                            Domain = domain

                        });

                        entity.SaveChanges();

                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Cập nhập thành công";
                    }

                }
                else
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Không cập nhập được vui lòng kiêm tra các trường thông tin";
                }

                result = systemMessage.Message;
                return Json(new { result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {

                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult _ListCampaign()
        {

            var entity = new MobilePREntities();
            User user = AccountUntils.GetUser();

            var result = entity.Database
                .SqlQuery<ViewModelCampaign>(@"SELECT c.Id , c.Name, ct.TypeCode , c.StartTime , c.EndTime , c.isActive , c.Domain ,  
               ( SELECT COUNT(pd.id) FROM pg_data pd WHERE pd.id_campaign = c.Id ) AS PgDataCount  
                    FROM Campaign c INNER JOIN CampaignType ct ON ct.Id = c.Type ORDER BY c.Id DESC")
                .ToList();

            return PartialView(result);


        }


        public ActionResult ListOptionCampaign()
        {           
            return View();
        }

        public ActionResult _ListOptionCampaign()
        {
            var entity = new MobilePREntities();

            var result = entity.Database.SqlQuery<ViewModelInListCampain>(
                @"SELECT DISTINCT co.Id, c.Name, c.StartTime, c.EndTime, ss.Script, c1.City_Name, l.name AS local_name  , c1.Id AS City_id , l.local_id  , ss.Id AS script_id  
                  , c.id AS campaign_id
                  , ( SELECT COUNT(*) FROM CampaignStore cs WHERE cs.Campaign_Op_Code =  co.Code )  AS numberUser ,  c.Type ,  c.CampaignCode   FROM Campaign c 
                      INNER JOIN CampaignOption co ON c.Id = co.Campaign_Id   
                      LEFT JOIN CampaignStore cs ON co.Code = cs.Campaign_Op_Code 
                      LEFT JOIN City c1 ON c1.Id = co.City_Id
                      LEFT JOIN Local l ON co.Local_Id = l.local_id  
                      LEFT JOIN ScritpStore ss ON ss.Id = co.Script_id WHERE c.isActive = 1").ToList();

            return PartialView(result);
        }


        public ActionResult AddCampaign()
        {
            var entity = new MobilePREntities();
            ViewBag.lsCampaigntype = entity.CampaignType.Where(x => x.IsActive == true);

            return PartialView();
        }


        public ActionResult _deleteCampaign(int id)
        {
            SystemMessage systemMessage = new SystemMessage();
            var result = "";

            try
            {
                using (var entity = new MobilePREntities())
                {
                    var itemRemove = entity.Campaign.SingleOrDefault(x => x.Id == id);
                    if (itemRemove != null)
                    {
                        entity.Campaign.Remove(itemRemove);
                        entity.SaveChanges();

                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Cập nhập thành công";
                    }
                }

                result = systemMessage.Message;
                return Json(new { result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult _AddCampaign(string name,string starttime, string endtime , int? number ,int? budget , int type)
        {
            SystemMessage systemMessage = new SystemMessage();
            Guid uid = Guid.NewGuid();

            var result = "";
            try
            {
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(starttime) && !string.IsNullOrWhiteSpace(endtime) && type != 0 )
                {
                    using (var entity = new MobilePREntities())
                    {
                        var cam = entity.Set<Campaign>();
                        cam.Add(new Campaign
                        {

                            Name = name,
                            Type = type,
                            StartTime = Convert.ToDateTime(starttime),
                            EndTime = Convert.ToDateTime(endtime),
                            NumberContact = number,
                            Budgets = budget,
                            CampaignCode = Convert.ToString(uid)

                        });

                        entity.SaveChanges();

                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Cập nhập thành công";
                    }

                }

                result = systemMessage.Message;
                return Json(new { result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {

                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult AddOptionCampaign()
        {
            var entity = new MobilePREntities();
            var db = new Business.Business();

            ViewBag.User = entity.User.Where(x => x.IsActive == true && x.UserName != "admin");
            //   ViewBag.lsCampaign = entity.Campaign.Where(x => x.isActive == true );
            ViewBag.lsCampaign = db.ListCampaignNotOption();
            ViewBag.isScript = entity.ScritpStore.Where( x => x.isActive == true);
            ViewBag.lsCenter = entity.center.ToList();

            ViewBag.lsDomainType = entity.Domain.ToList();

            return PartialView();
        }

        public ActionResult ListScript()
        {
            return PartialView();
        }

        public ActionResult _ListScript()
        {
            var entity = new MobilePREntities();
            var result = entity.Database
                .SqlQuery<ViewModelListScript>(@"SELECT ss.Id , ss.Script  , ss.CreatAt , ss.UpdateAt ,  ss.NameScript  , ss.isActive FROM ScritpStore ss")
                .ToList();
            return PartialView(result);
        }


        public ActionResult _DeleteScript(int id)
        {
            SystemMessage systemMessage = new SystemMessage();
            var result = "";

            try
            {
                using (var entity = new MobilePREntities())
                {
                    var itemRemove = entity.ScritpStore.SingleOrDefault(x => x.Id == id);
                    if (itemRemove != null)
                    {
                        entity.ScritpStore.Remove(itemRemove);
                        entity.SaveChanges();

                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Cập nhập thành công";
                    }
                }

                result = systemMessage.Message;
                return Json(new { result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult _DeleteOptionCampaign(int id)
        {
            SystemMessage systemMessage = new SystemMessage();
            var result = "";
            try
            {
                using (var entity = new MobilePREntities())
                {
                    var itemRemove = entity.CampaignOption.SingleOrDefault(x => x.Id == id);
                    if (itemRemove != null)
                    {
                        entity.CampaignOption.Remove(itemRemove);
                        entity.SaveChanges();

                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Cập nhập thành công";

                    }
                }

                result = systemMessage.Message;
                return Json(new { result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult EditOptionCampaign(int id)
        {

            var db = new Business.Business();
            var result = db.GetCampaignStore(id);
            ViewBag.UserInStore = db.GetUserInStore(result.code);
            ViewBag.User = db.GetAllUsers();
            // list script 
            ViewBag.listScript = db.GetScriptList();
            return PartialView(result);

        }

        public ActionResult EditOptionCampaign2(int id)
        {

            var db = new Business.Business();
            var result = db.GetCampaignStore(id);

            ViewBag.CenterInStore = db.GetUserInStore(result.code);
            ViewBag.Center = db.GetAllCenter();
           
            // list script 
            ViewBag.listScript = db.GetScriptList();
            return PartialView(result);

        }



        public ActionResult _EditOptionCampaign2(string data, int? compaign_id, int? script_id, string code)
        {
            var result = "";
            SystemMessage systemMessage = new SystemMessage();
            if (!string.IsNullOrWhiteSpace(data) && !string.IsNullOrWhiteSpace(code) && compaign_id != null && script_id != null)
            {

                using (var entity = new MobilePREntities())
                {
                    SqlParameter spCid = new SqlParameter(@"compaign_id", compaign_id);
                    SqlParameter spSid = new SqlParameter(@"script_id", script_id);
                    SqlParameter spCode = new SqlParameter(@"code", code);

                    try
                    {

                        entity.Database.ExecuteSqlCommand("UPDATE CampaignOption SET Script_id = @script_id , UpdateAt = GETDATE() WHERE Code = @code;DELETE CampaignStore WHERE Campaign_Op_Code = @code", spSid, spCode);

                        var cdata = JsonConvert.DeserializeObject<dynamic>(data);
                        var comstore = entity.Set<CampaignStore>();

                        foreach (var item in cdata)
                        {
                            int centerId = Convert.ToInt16(item["Id"]);
                            var isExit = entity.CampaignStore.FirstOrDefault(o => o.Center_Id == centerId && o.Campaign_Op_Code == code && o.Campaign_Id == compaign_id);

                            if (isExit == null)
                            {

                                comstore.Add(new CampaignStore
                                {
                                    Campaign_Id = compaign_id,
                                    Center_Id = centerId,
                                    Campaign_Op_Code = code
                                });

                                entity.SaveChanges();
                            }

                        }


                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Cập nhập thành công";
                        result = systemMessage.Message;

                        return Json(new { result }, JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = e.ToString();
                        return Json(new { result }, JsonRequestBehavior.AllowGet);
                    }


                }

            }
            else
            {
                systemMessage.IsSuccess = true;
                systemMessage.Message = "Không thể cập nhập";
                result = systemMessage.Message;

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }         
        }




        public ActionResult _EditOptionCampaign(string data , int? compaign_id, int? script_id , string code )
        {
            var result = "";
            SystemMessage systemMessage = new SystemMessage();
            if (!string.IsNullOrWhiteSpace(data) && !string.IsNullOrWhiteSpace(code) && compaign_id != null && script_id != null)
            {

                using (var entity = new MobilePREntities())
                {
                    SqlParameter spCid = new SqlParameter(@"compaign_id" , compaign_id);
                    SqlParameter spSid = new SqlParameter(@"script_id" , script_id);
                    SqlParameter spCode = new SqlParameter(@"code" , code);

                    try
                    {

                        entity.Database.ExecuteSqlCommand("UPDATE CampaignOption SET Script_id = @script_id , UpdateAt = GETDATE() WHERE Code = @code;DELETE CampaignStore WHERE Campaign_Op_Code = @code", spSid ,spCode);
                        var cdata = JsonConvert.DeserializeObject<dynamic>(data);
                        var comstore = entity.Set<CampaignStore>();          
                        foreach (var item in cdata)
                        {
                            int userId = Convert.ToInt16(item["Id"]);
                            var isExit = entity.CampaignStore.FirstOrDefault(o => o.User_Id == userId && o.Campaign_Op_Code == code && o.Campaign_Id == compaign_id);
                            if (isExit == null)
                            {                                
                                comstore.Add(new CampaignStore
                                {
                                    Campaign_Id = compaign_id,
                                    User_Id = userId,
                                    Campaign_Op_Code = code
                                });
                                entity.SaveChanges();
                            }                        
                        }

                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Cập nhập thành công";
                        result = systemMessage.Message;

                        return Json(new { result }, JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = e.ToString();
                        return Json(new { result }, JsonRequestBehavior.AllowGet);
                    }             
                }

            }

            else
            {
                systemMessage.IsSuccess = true;
                systemMessage.Message = "Không thể cập nhập";
                result = systemMessage.Message;

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult _AddOptionCampaignFacebook(int? compaign_id, int? script_id)
        {
            Guid uid = Guid.NewGuid();
            string result = "";
            SystemMessage systemMessage = new SystemMessage();
            var db = new Business.Business();

            try
            {
                string code = Convert.ToString(uid);
                if (compaign_id != null)
                {
                    Campaign _campaign = db.GetOneCampaign(compaign_id);
                    string contentscript = db.GetScript(script_id);

                    SystemMessage sysme = SendToCCSI(_campaign.Name, _campaign.CampaignCode, contentscript);

                    using (var entity = new MobilePREntities())
                    {
                        var compa = entity.Set<CampaignOption>();
                        compa.Add(new CampaignOption
                        {
                            Campaign_Id = compaign_id,
                            Code = code,
                            Script_id = script_id,
                            CreatAt = DateTime.Now,
                            UpdateAt = DateTime.Now

                        });
                        entity.SaveChanges();

                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Cập nhập thành công";

                    }
                }
                result = systemMessage.Message;

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }

        }

        // add PG SHOP
        public ActionResult _AddOptionCampaignPGShop(string data, int? compaign_id, int? script_id)
        {
            Guid uid = Guid.NewGuid();

            SystemMessage systemMessage = new SystemMessage();
            var db = new Business.Business();
            Campaign cp = db.GetOneCampaign(compaign_id);
            string result = "";

            try
            {
                string code = Convert.ToString(uid);

                if (!string.IsNullOrEmpty(data) && compaign_id != null && script_id != null)
                {
                    Campaign _campaign = db.GetOneCampaign(compaign_id);
                    string contentscript = db.GetScript(script_id);

                    SystemMessage sysme = SendToCCSI(_campaign.Name, _campaign.CampaignCode, contentscript);

                    if (sysme.IsSuccess == true)
                    {

                        using (var entity = new MobilePREntities())
                        {
                            var compa = entity.Set<CampaignOption>();
                            compa.Add(new CampaignOption
                            {
                                Campaign_Id = compaign_id,
                                Code = code,
                                Script_id = script_id,
                                CreatAt = DateTime.Now,
                                UpdateAt = DateTime.Now
                            });

                            entity.SaveChanges();

                            var cdata = JsonConvert.DeserializeObject<dynamic>(data);
                            var comstore = entity.Set<CampaignStore>();

                            foreach (var item in cdata)
                            {
                                string domain = Convert.ToString(item["name"]);

                                var isExit = entity.CampaignStore.FirstOrDefault(o => o.DomainSub == domain && o.Campaign_Op_Code == code);
                                if (isExit == null)
                                {
                                    comstore.Add(new CampaignStore
                                    {
                                        Campaign_Id = compaign_id,
                                        DomainSub = domain,
                                        Campaign_Op_Code = code,
                                        CampaignCode = cp.CampaignCode
                                    });

                                    entity.SaveChanges();
                                }

                            }

                            systemMessage.IsSuccess = true;
                            systemMessage.Message = "Cập nhập thành công bên ccsi và PGWeb";
                        }

                    }
                    else
                    {

                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "Không tạo được option qua ccsi";
                    }

                }

                result = systemMessage.Message;


                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return Json(new { result }, JsonRequestBehavior.AllowGet);

            }
        }


        public ActionResult _AddOptionCampaignCenter(string data, int? compaign_id, int? script_id)
        {
            string result = "";

            Guid uid = Guid.NewGuid();
            SystemMessage systemMessage = new SystemMessage();
            var db = new Business.Business();

            try
            {
                string code = Convert.ToString(uid);
                if(!string.IsNullOrWhiteSpace(data) && compaign_id != null)
                {
                    using (var entity = new MobilePREntities())
                    {
                        var compa = entity.Set<CampaignOption>();
                        compa.Add(new CampaignOption
                        {
                            Campaign_Id = compaign_id,
                            Code = code,
                            Script_id = script_id,
                            CreatAt = DateTime.Now,
                            UpdateAt = DateTime.Now
                        });

                        entity.SaveChanges();

                        var cdata = JsonConvert.DeserializeObject<dynamic>(data);
                        var comstore = entity.Set<CampaignStore>();

                        foreach (var item in cdata)
                        {
                            int CenterId = Convert.ToInt16(item["Id"]);
                            var isExit = entity.CampaignStore.FirstOrDefault(o => o.Center_Id == CenterId && o.Campaign_Op_Code == code);
                            if (isExit == null)
                            {
                                comstore.Add(new CampaignStore
                                {
                                    Campaign_Id = compaign_id,
                                    Center_Id = CenterId,
                                    Campaign_Op_Code = code
                                });

                                entity.SaveChanges();
                            }

                        }

                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Cập nhập thành công";
                    }           
                }
                result = systemMessage.Message;
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }  
        }


        // add optione campaign and sent to ccsi 
        public ActionResult _AddOptionCampaign(string data , int? compaign_id , int? script_id)
        {
            Guid uid = Guid.NewGuid();
            SystemMessage systemMessage = new SystemMessage();
            var db = new Business.Business();
            string result = "";

            try
            {              
                string code = Convert.ToString(uid);
                
                if (!string.IsNullOrEmpty(data) && compaign_id != null && script_id != null)
                {
                    Campaign _campaign = db.GetOneCampaign(compaign_id);
                    string contentscript = db.GetScript(script_id);

                    SystemMessage sysme = SendToCCSI(_campaign.Name, _campaign.CampaignCode, contentscript);

                    if (sysme.IsSuccess == true )
                    {

                        using (var entity = new MobilePREntities())
                        {
                            var compa = entity.Set<CampaignOption>();
                            compa.Add(new CampaignOption
                            {
                                Campaign_Id = compaign_id,
                                Code = code,
                                Script_id = script_id,
                                CreatAt = DateTime.Now,
                                UpdateAt = DateTime.Now
                            });

                            entity.SaveChanges();

                            var cdata = JsonConvert.DeserializeObject<dynamic>(data);
                            var comstore = entity.Set<CampaignStore>();

                            foreach (var item in cdata)
                            {
                                int userId = Convert.ToInt16(item["Id"]);
                                var isExit = entity.CampaignStore.FirstOrDefault(o => o.User_Id == userId && o.Campaign_Op_Code == code);
                                if (isExit == null)
                                {
                                    comstore.Add(new CampaignStore
                                    {
                                        Campaign_Id = compaign_id,
                                        User_Id = userId,
                                        Campaign_Op_Code = code
                                    });

                                    entity.SaveChanges();
                                }

                            }

                            systemMessage.IsSuccess = true;
                            systemMessage.Message = "Cập nhập thành công bên ccsi và PGWeb";
                        }

                    }
                    else
                    {

                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "Không tạo được option qua ccsi";
                    }
                    
                }

                result = systemMessage.Message;


                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return Json(new { result }, JsonRequestBehavior.AllowGet);

            }

        }

        public int GetCampaignOptionSequence()
        {
            var entity = new MobilePREntities();
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            entity.Database.ExecuteSqlCommand("SELECT @result = (NEXT VALUE FOR CampaignOptionSequence)", result);

            return (int)result.Value;
        }

        public ActionResult _OnOffCampaign(int? id , int _isActive)
        {
            var db = new Business.Business();
            var result = "";
            SystemMessage systemMessage = new SystemMessage();
            Campaign campaign = db.GetCampaign(id);
            var data = db.GetToCampaign(campaign.CampaignCode);

            if(data != null)
            {
                SystemMessage sysme = PutUpdateSendToCCSI(data.Name, data.CampaignCode, data.Script, data.isActive);
                if (sysme.IsSuccess == true)
                {
                    using (var entity = new MobilePREntities())
                    {
                        var item = entity.Campaign.SingleOrDefault(x => x.Id == id);
                        SqlParameter sp = new SqlParameter(@"id", id);
                        SqlParameter isp = new SqlParameter(@"isActive", _isActive);
                        try
                        {
                            entity.Database.ExecuteSqlCommand("UPDATE Campaign SET isActive = @isActive WHERE Id = @id", isp, sp);
                            systemMessage.IsSuccess = true;
                            systemMessage.Message = "Cập nhập thành công sang CCSI và PGWeb";
                            result = systemMessage.Message;
                            return Json(new { result }, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception e)
                        {
                            systemMessage.IsSuccess = false;
                            systemMessage.Message = e.ToString();
                            return Json(new { result }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    systemMessage.Message = "Cập nhập Không thành công sang CCSI";
                    result = systemMessage.Message;
                    return Json(new { result }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                using (var entity = new MobilePREntities())
                {
                    var item = entity.Campaign.SingleOrDefault(x => x.Id == id);
                    SqlParameter sp = new SqlParameter(@"id", id);
                    SqlParameter isp = new SqlParameter(@"isActive", _isActive);
                    try
                    {
                        entity.Database.ExecuteSqlCommand("UPDATE Campaign SET isActive = @isActive WHERE Id = @id", isp, sp);
                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Cập nhập thành công";
                        result = systemMessage.Message;
                        return Json(new { result }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = e.ToString();
                        return Json(new { result }, JsonRequestBehavior.AllowGet);
                    }
                }

            }

        }

        public ActionResult _OnOffScript(int? id, int _isActive)
        {

            SystemMessage systemMessage = new SystemMessage();
            string result = "";
            using (var entity = new MobilePREntities())
            {
                var item = entity.ScritpStore.SingleOrDefault(x => x.Id == id);

                if (item != null)
                {
                    SqlParameter sp = new SqlParameter(@"id", id);
                    SqlParameter isp = new SqlParameter(@"isActive", _isActive);
                    try
                    {
                        entity.Database.ExecuteSqlCommand("UPDATE ScritpStore SET isActive = @isActive WHERE Id = @id", isp, sp);
                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Cập nhập thành công";
                        result = systemMessage.Message;
                        return Json(new { result }, JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        systemMessage.IsSuccess = false;
                        result = e.ToString();

                        return Json(new { result }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    systemMessage.IsSuccess = false;
                    result = "Không tòn tại";
                    return Json(new { result }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult _EditScript(int id)
        {
            var db = new Business.Business();
            var result = db.GetScritpStore(id);
            return PartialView(result);
        }

        [HttpPost]
        public ActionResult _AddScript(string namescript, string script)
        {
            var result = "";
            SystemMessage systemMessage = new SystemMessage();
          
                try
                {
                    using (var entity = new MobilePREntities())
                    {

                        SqlParameter pnamescript = new SqlParameter(@"namscript", namescript);
                        SqlParameter pscript = new SqlParameter(@"script", script);

                        entity.Database.ExecuteSqlCommand("INSERT ScritpStore ( NameScript, Script , CreatAt , UpdateAt , isActive ) VALUES ( @namscript  , @script , GETDATE() , GETDATE() , 1  ) ", pnamescript, pscript);

                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Cập nhập thành công";
                        result = systemMessage.Message;
                        return Json(new { result }, JsonRequestBehavior.AllowGet);

                    }
                }
                catch (Exception e)
                {
                    systemMessage.IsSuccess = false;
                    result = e.ToString();
                    return Json(new { result }, JsonRequestBehavior.AllowGet);
                }              
           
        }

        [HttpPost]
        public ActionResult EditScript(int id , string NameScript , string Script)
        {
            var result = "";
            SystemMessage systemMessage = new SystemMessage();
            using (var entity = new MobilePREntities())
            {
                try
                {

                    SqlParameter pid = new SqlParameter(@"id", id);
                    SqlParameter pnamescript = new SqlParameter(@"namscript", NameScript);
                    SqlParameter pscript = new SqlParameter(@"script", Script);
                    entity.Database.ExecuteSqlCommand("UPDATE ScritpStore SET NameScript = @namscript , Script = @script WHERE Id = @id", pnamescript, pscript, pid);

                    systemMessage.IsSuccess = true;
                    systemMessage.Message = "Cập nhập thành công";
                    result = systemMessage.Message;
                    return Json(new { result }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception e)
                {
                    systemMessage.IsSuccess = false;
                    result = e.ToString();
                    return Json(new { result }, JsonRequestBehavior.AllowGet);
                }                
            }
        }

        public ActionResult EditCampaign(int id)
        {
            var db = new Business.Business();
            var result = db.GetCampaign(id);

            var entity = new MobilePREntities();
            ViewBag.lsCampaigntype = entity.CampaignType.Where(x => x.IsActive == true);

            return PartialView(result);

        }

        public ActionResult _EditCampaign(int id , string name , string starttime , string endtime , int? number , int? budget , int type  )
        {
            var result = "";
            SystemMessage systemMessage = new SystemMessage();
            
            using (var entity = new MobilePREntities())
            {
                try
                {
                    SqlParameter spId = new SqlParameter(@"id",id);
                    SqlParameter spName = new SqlParameter(@"name" , name);
                    SqlParameter spstarttime = new SqlParameter(@"starttime" , Convert.ToDateTime(starttime));
                    SqlParameter spendtime = new SqlParameter(@"endtime" , Convert.ToDateTime(endtime));
                    SqlParameter spNumber = new SqlParameter(@"number" , number);
                    SqlParameter spBudget = new SqlParameter(@"budget" , budget);
                    SqlParameter sptype = new SqlParameter(@"type" , type);

                    if (number == null && budget != null)
                    {
                        entity.Database.ExecuteSqlCommand(@"UPDATE Campaign SET StartTime = @starttime, Name = @name, Budgets = @budget ,
                                                         EndTime = @endtime , Type = @type WHERE Id = @id", spstarttime, spName, spendtime, spId, spBudget , sptype);
                    }
                    else if(number != null && budget == null)
                    {
                        entity.Database.ExecuteSqlCommand(@"UPDATE Campaign SET StartTime = @starttime, Name = @name, NumberContact = @number,
                                                         EndTime = @endtime , Type = @type WHERE Id = @id", spstarttime, spName, spendtime, spId, spNumber , sptype);
                    }
                    else if(number == null && budget == null)
                    {
                        entity.Database.ExecuteSqlCommand(@"UPDATE Campaign SET StartTime = @starttime, Name = @name, 
                                                         EndTime = @endtime , Type = @type WHERE Id = @id", spstarttime, spName, spendtime, spId , sptype);
                    }
                    else 
                    {
                        entity.Database.ExecuteSqlCommand(@"UPDATE Campaign SET StartTime = @starttime, Name = @name, NumberContact = @number , Budgets = @budget,
                                                         EndTime = @endtime , Type = @type WHERE Id = @id", spstarttime, spName, spendtime, spId, spNumber, spBudget , sptype);
                    }

                    
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = "Cập nhập thành công";
                    result = systemMessage.Message;

                    return Json(new { result }, JsonRequestBehavior.AllowGet);
 
                }
                catch (Exception e)
                {
                    systemMessage.IsSuccess = false;
                    result = e.ToString();
                    return Json(new { result }, JsonRequestBehavior.AllowGet);
                }
            }

        }


        private SystemMessage PutUpdateSendToCCSI(string name, string code, string script , bool _status)
        {
            SystemMessage systemMessage = new SystemMessage();
           
            try
            {
                string sStatus = "";
                if (_status == true)
                {
                    sStatus = "off";
                }
                else if (_status == false)
                {
                    sStatus = "on";
                }

                var client = new RestClient("http://192.168.1.222/campaign");
                var request = new RestRequest();
                request.Method = Method.PUT;

                request.AddHeader("API-KEY-AUTH", "1qazxsw@#!$");
                request.AddParameter("name", name);
                request.AddParameter("code", code);
                request.AddParameter("script", string.Empty);
                request.AddParameter("status", sStatus);

                var response = client.ExecuteTaskAsync(request);
                ObjectReturn jsonObject = JsonConvert.DeserializeObject<ObjectReturn>(response.Result.Content);

                var status = jsonObject.status;
                systemMessage.IsSuccess = status == "FAIL" ? false : true;
                systemMessage.Message = jsonObject.msg;

            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
            }
            return systemMessage;
        }


        private SystemMessage SendToCCSI(string name, string code, string script)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var client = new RestClient("http://192.168.1.222/campaign");
                var request = new RestRequest();
                request.Method = Method.POST;
                request.AddHeader("API-KEY-AUTH", "1qazxsw@#!$");
                request.AddParameter("name", name);
                request.AddParameter("code", code);
                request.AddParameter("script", string.Empty);

                var response = client.ExecuteTaskAsync(request);
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(response.Result.Content);

                var status = jsonObject.status;
                systemMessage.IsSuccess = status == "FAIL" ? false : true;
                systemMessage.Message = jsonObject.msg;
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
 