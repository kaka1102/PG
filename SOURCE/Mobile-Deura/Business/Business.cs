using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using Mobile_Deura.Models;
using Mobile_Deura.Untils;
using Mobile_Deura;
using System.Configuration;

namespace Mobile_Deura.Business
{
    public class Business
    {
        public string GetScript(int? id)
        {
            if(id != null)
            {
                using (var entity = new MobilePREntities())
                {
                    var result = entity.ScritpStore.FirstOrDefault(x => x.Id == id);
                    return result?.Script;
                }
            }
            else
            {
                return null;
            }
            
        }

        public SystemMessage AdminCheckLogin(string username, string password)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                using (var entity = new MobilePREntities())
                {
                    // loaiTK : 100 quan ly / 1  lead / 2 nhan vien /  3 nhan vien bc / 4 user import 

                    var user = entity.User.FirstOrDefault(ob => ob.UserName == username && ob.IsActive == true && (ob.isAdmin == true || ob.LoaiTK == 100 || ob.LoaiTK == 1 || ob.LoaiTK == 4));
                    //if (user == null || user.isAdmin == false )
                    if (user == null)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "Tài khoản không tồn tại !!";
                        return systemMessage;
                    }
                    if (password == user.Password)
                    {
                        HttpContext.Current.Session["admin"] = user;
                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Đăng nhập thành công";
                    
                        if(user.LoaiTK == 4)
                            systemMessage.Message = "import";

                        return systemMessage;
                    }
                   
                    else
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "sai mật khẩu";
                        return systemMessage;
                    }
                }
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }
        }
        public SystemMessage EmpCheckLogin(string username, string password)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                using (var entity = new MobilePREntities())
                {
                    var user = entity.User.FirstOrDefault(ob => ob.UserName == username && ob.IsActive == true);

                   

                    if (user == null)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "Tài khoản không tồn tại !!";
                        return systemMessage;
                    }
                    if (password == user.Password)
                    {

                        var idCity = user.idCity;
                        if (idCity == null)
                        {
                            idCity = 0;
                        }

                        //      HttpContext.Current.Session["Emp"] = user;                    
                        Cookies_Set("id", user.Id.ToString(), DateTime.Now.AddYears(1));
                        Cookies_Set("FullName", user.FullName, DateTime.Now.AddYears(1));
                        Cookies_Set("UserName", user.UserName, DateTime.Now.AddYears(1));
                        Cookies_Set("agent", user.agent, DateTime.Now.AddYears(1));
                        Cookies_Set("idCity", idCity.ToString(), DateTime.Now.AddYears(1));
                     //   Cookies_Set("LoaiTK" , user.LoaiTK.ToString() , DateTime.Now.AddYears(1));
                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Đăng nhập thành công";
                        return systemMessage;
                    }
                    else
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "sai mật khẩu";
                        return systemMessage;
                    }
                }
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }
        }
        public static void Cookies_Set(string key, string value, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(key, HttpUtility.UrlEncode(value));
            cookie.Expires = expires;
            HttpContext.Current.Response.SetCookie(cookie);
        }

        public SystemMessage ChangePassword(string oldPass, string newPass, string confirmPass)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                using (var entity = new MobilePREntities())
                {
                    var id = AccountUntils.GetEmp().Id;
                    var check = entity.User.SingleOrDefault(ob => ob.Id == id);

                    if (check.Password != oldPass)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "Mật khẩu cũ không chính xác !!!";
                        return systemMessage;
                    }

                    var vld = ValidateChangePassword(newPass, confirmPass);
                    if (vld != null)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = vld;
                        return systemMessage;
                    }

                    check.Password = newPass.Trim();
                    entity.SaveChanges();
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = "Đổi mật khẩu thành công";
                    return systemMessage;
                }
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }
        }


        public User getInfoUser(int id)
        {
            using (var entity = new MobilePREntities())
            {
                return entity.User.FirstOrDefault(x => x.Id == id);
            }
        }

        public List<pg_data> GetListPgDataByEmp()
        {
            using (var entity = new MobilePREntities())
            {
                var userName = AccountUntils.GetEmp().UserName;
                var result = entity.pg_data.Where(ob =>
                    ob.created_by == userName && ob.created_date.Value.Day == DateTime.Now.Day && ob.created_date.Value.Month == DateTime.Now.Month && ob.created_date.Value.Year == DateTime.Now.Year).ToList();
                return result;
            }
        }
        public string ValidateChangePassword(string newPass, string confirmPass)
        {
            string rs = null;
            var vld = new ValidateUtils();
            rs = vld.CheckRequiredField(newPass, "Mật khẩu mới", 5, 20);
            if (rs != null)
            {
                return rs;
            }
            if (newPass != confirmPass)
            {
                return "xác nhận mật khẩu không chính xác !!!";
            }
            return null;
        }


        public SystemMessage AddLocal(Local lc)
        {
            SystemMessage systemMessage = new SystemMessage();
            using (var entity = new MobilePREntities())
            {
                entity.Local.Add(lc);
                entity.SaveChanges();

                systemMessage.IsSuccess = true;
                systemMessage.Message = "Thêm thành công";
                return systemMessage;
            }
        }
        public SystemMessage _EditLocal(Local lc)
        {
            SystemMessage systemMessage = new SystemMessage();
            using (var entity = new MobilePREntities())
            {

                var check = entity.Local.FirstOrDefault(m => m.Id == lc.Id);
                if (check != null)
                {
                    check.name = lc.name;
                    check.city_id = lc.city_id;
                    check.isActive = lc.isActive;
                    check.shortcode = lc.shortcode;
                    entity.SaveChanges();
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = "Cập nhật thành công";
                }

                else
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Có lỗi trong quá trình thao tác dữ liệu";
                }

                return systemMessage;
            }
        }
        public SystemMessage DeleteLocal(Local lc)
        {
            SystemMessage systemMessage = new SystemMessage();
            using (var entity = new MobilePREntities())
            {
                var check = entity.Local.FirstOrDefault(m => m.Id == lc.Id);
                if (check != null)
                {

                    check.isActive = false;
                    entity.SaveChanges();

                    systemMessage.IsSuccess = true;
                    systemMessage.Message = "Xóa thành công";
                }

                else
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Có lỗi trong quá trình thao tác dữ liệu";
                }
                return systemMessage;
            }
        }
        public SystemMessage AddUser(User myuser)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var check = ValidateAddUser(myuser);
                if (check != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = check;
                    return systemMessage;
                }

                using (var entity = new MobilePREntities())
                {
                    var checkExisted = entity.User.FirstOrDefault(ob => ob.UserName == myuser.UserName);
                    if (checkExisted != null)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "Tài khoản đã tồn tại";
                        return systemMessage;
                    }

                    var checkAgentExisted = entity.User.FirstOrDefault(ob => ob.agent == myuser.agent);
                    if(checkAgentExisted != null)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "Mã nhân viên đã tồn tại";
                        return systemMessage;
                    }

                    myuser.DateCreated = DateTime.Now;
                    myuser.isAdmin = false;
                    myuser.IsActive = true;
                    entity.User.Add(myuser);
                    entity.SaveChanges();
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = "Thêm tài khoản thành công";

                    return systemMessage;

                }
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }
        }
        public SystemMessage AddData(pg_data data)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                //                var check = ValidateAddData(data);
                //                if (check != null)
                //                {
                //                    systemMessage.IsSuccess = false;
                //                    systemMessage.Message = check;
                //                    return systemMessage;
                //                }

                using (var entity = new MobilePREntities())
                {
                    //                    var checkExisted =
                    //                        entity.pg_data.FirstOrDefault(ob => ob.mobile == data.mobile);
                    //                    if (checkExisted != null)
                    //                    {
                    //                        systemMessage.IsSuccess = false;
                    //                        systemMessage.Message = "Dữ liệu này đã tồn tại trong hệ thống !!!";
                    //                        return systemMessage;
                    //                    }
                    data.created_date = DateTime.Now;
                    data.created_by = AccountUntils.GetEmp().UserName;
                    data.agent = AccountUntils.GetEmp().agent;
                    entity.pg_data.Add(data);
                    entity.SaveChanges();
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = "Thêm dữ liệu thành công";
                    return systemMessage;
                }
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }
        }
        public SystemMessage EditUser(User myuser)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var check = ValidateAddUser(myuser);
                if (check != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = check;
                    return systemMessage;
                }

                using (var entity = new MobilePREntities())
                {
                    var checkExisted = entity.User.FirstOrDefault(ob => ob.UserName == myuser.UserName && ob.Id == myuser.Id && ob.isAdmin != true && ob.IsActive == true);
                    if (checkExisted == null)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "tài khoản đã không tồn tại";
                        return systemMessage;
                    }

                    if (myuser.idParent != 0)
                    {
                        if (myuser.idParent == myuser.Id)
                        {
                            systemMessage.IsSuccess = false;
                            systemMessage.Message = "Tài khoản teamlead và tài khoản chỉnh sửa trùng nhau";
                            return systemMessage;
                        }
                    }

                    if (myuser.LoaiTK == 2 && checkExisted.LoaiTK == 1)  /// tk đang là teamlead hạ xuống user thì cập nhật các con của nó về null
                    {
                        var lstChil = entity.User.Where(m => m.idParent == checkExisted.Id).ToList();
                        foreach (var itemchil in lstChil)
                        {
                            itemchil.idParent = null;
                            entity.SaveChanges();
                        }
                    }

                    checkExisted.DateModify = DateTime.Now;
                    checkExisted.Password = myuser.Password;
                    checkExisted.FullName = myuser.FullName;
                    checkExisted.Email = myuser.Email;
                    checkExisted.agent = myuser.agent;
                    checkExisted.source = myuser.source;
                    checkExisted.DateOfBirth = myuser.DateOfBirth;
                    checkExisted.LoaiTK = myuser.LoaiTK;
                    checkExisted.idParent = myuser.idParent;
                    checkExisted.idCity = myuser.idCity;
                    checkExisted.idManager = myuser.idManager;
                    checkExisted.idCenter = myuser.idCenter;
                    checkExisted.TeamName = myuser.TeamName;

                    entity.SaveChanges();
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = "Cập nhật tài khoản thành công";
                    return systemMessage;
                }
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }
        }


        public Campaign GetOneCampaign(int? id_campaign)
        {
            if (id_campaign != null)
            {
                using (var enity = new MobilePREntities())
                {
                    var result = enity.Campaign.FirstOrDefault(x => x.Id == id_campaign);
                    return result;
                }
            }
            else
            {
                return null;
            }

            
        }

        

        public List<User> ListUser()
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                using (var entity = new MobilePREntities())
                {
                    User user = AccountUntils.GetUser();
                    if (user.isAdmin == true && user.LoaiTK == 0)
                    {
                        var result = entity.User.Where(ob => ob.isAdmin == false && ob.IsActive != null).ToList();
                        return result;
                    }
                    else if (user.LoaiTK == 1 && user.isAdmin == false)
                    {
                        var result = entity.User.Where(ob => ob.isAdmin == false && ob.IsActive != null && ob.idParent == user.Id).ToList();
                        return result;
                    }
                    else if(user.LoaiTK == 100 && user.isAdmin == false)
                    {
                        // tai khoan manager 
                        var result = entity.User.Where(ob => ob.isAdmin == false && ob.IsActive != null && ob.idManager == user.Id && ob.LoaiTK < 100).ToList();
                        return result;
                    }                 
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<center> ListCenters()
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                using (var entity = new MobilePREntities())
                {
                    var result = entity.center.ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<center> ListCenterById(int idcity)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                using (var entity = new MobilePREntities())
                {
                    var result = entity.center.Where( x => x.idCity == idcity ).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public List<City> ListCity()
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                using (var entity = new MobilePREntities())
                {
                    var result = entity.City.ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Local> ListLocal()
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                using (var entity = new MobilePREntities())
                {
                    var result = entity.Local.Where(x => x.isActive == true).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<AlertCenterCampaign> CheckCenterCampaign(int id)
        {
            try {
                using (var entity = new MobilePREntities())
                {
                    var result = entity.User.FirstOrDefault(x => x.Id == id);
                    // kiem tra trong store campaign
                    SqlParameter spCenterId = new SqlParameter(@"center_id", result.idCenter);

                    List<AlertCenterCampaign> dt = entity.Database.SqlQuery<AlertCenterCampaign>(@"SELECT u.Id , u.UserName , c.EndTime , c.Name  , cs.Campaign_Id , co.Script_id , c.CampaignCode FROM [User] u 
                                         INNER JOIN CampaignStore cs ON u.idCenter = cs.Center_Id 
                                         INNER JOIN Campaign c ON c.Id = cs.Campaign_Id
                                         LEFT JOIN CampaignOption co ON co.Code = cs.Campaign_Op_Code     
                                      WHERE  CONVERT(DATE, GETDATE()) <= CONVERT(DATE, c.EndTime) AND  CONVERT(DATE, GETDATE()) >=  CONVERT(DATE, c.StartTime) AND c.isActive = 1 AND cs.Center_Id = @center_id", spCenterId).ToList();
                    return dt;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            
               
        }


        public string  GetCityCode(int id)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                using (var entity = new MobilePREntities())
                {

                    SqlParameter spId = new SqlParameter(@"id", id);
                    var result = entity.Database.SqlQuery<string>(@"SELECT top 1 c.City_Code FROM [User] u INNER JOIN City c ON c.Id = u.idCity  WHERE u.Id = id", spId).ToString();
               
                    return result;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }


        public List<AlertCampaign> CheckAlertCampaign(int id)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                using (var entity = new MobilePREntities())
                {
                    SqlParameter spId = new SqlParameter(@"id", id);
                    var result = entity.Database.SqlQuery<AlertCampaign>(@"SELECT u.Id , u.UserName , c.EndTime , c.Name  , cs.Campaign_Id , co.Script_id , c.CampaignCode FROM [User] u 
                                         INNER JOIN CampaignStore cs ON u.Id = cs.User_Id 
                                         INNER JOIN Campaign c ON c.Id = cs.Campaign_Id
                                         LEFT JOIN CampaignOption co ON co.Code = cs.Campaign_Op_Code                                 
                                    WHERE  CONVERT(DATE, GETDATE()) <= CONVERT(DATE, c.EndTime) AND CONVERT(DATE, GETDATE()) >= CONVERT(DATE, c.StartTime) AND c.isActive = 1 AND u.Id = @id ", spId).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


      

        public List<Local> ListLocalById(int idLogin)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                using (var entity = new MobilePREntities())
                {

                    if (idLogin > 0)
                    {
                        var checkUser = entity.User.FirstOrDefault(m => m.Id == idLogin);
                        if (checkUser != null && checkUser.idCity !=null)
                        {
                            string idCT = checkUser.idCity.ToString();
                            var result = entity.Local.Where(x => x.isActive == true && x.city_id == idCT).ToList();
                            return result;
                        }
                        else
                        {
                            var result = entity.Local.Where(x => x.isActive == true).ToList();
                            return result;
                        }
                    }
                    else
                    {
                        var result = entity.Local.Where(x => x.isActive == true).ToList();
                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public string GetCityLocal(int id)
        {

            try
            {
                using (var entity = new MobilePREntities())
                {
                    Local local = entity.Local.FirstOrDefault(x => x.local_id == id);

                    int city_id = Convert.ToInt32(local.city_id);

                    var result = entity.City.FirstOrDefault(x => x.Id == city_id);

                    return result.City_Code;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public User GetUserById(int id)
        {
            try
            {
                using (var entity = new MobilePREntities())
                {
                    var result =
                        entity.User.FirstOrDefault(ob => ob.isAdmin == false && ob.IsActive != null && ob.Id == id);
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

 

        public CampaignOption GetCampaignOption(int id)
        {
            try
            {
                using (var entity = new MobilePREntities())
                {
                    return entity.CampaignOption.FirstOrDefault(x => x.Id == id);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<CampaignStore> GetUserInStore(string code)
        {
            try
            {
                using (var entity = new MobilePREntities())
                {
                    return entity.CampaignStore.Where(x => x.Campaign_Op_Code == code).ToList();
                }
            }
            catch (Exception e)
            {

                return null;
            }

        }

        public List<City> GetAllCity()
        {
            try
            {
                using (var entity = new MobilePREntities())
                {
                    return entity.City.ToList();
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public string GetCenterEnglishNameById(int? idcenter)
        {
            using (var entity = new MobilePREntities())
            {
                if (idcenter != null)
                {
                    center center = entity.center.FirstOrDefault(x => x.id == idcenter);
                    return center.EnglishName;
                }
                else
                {
                    return null;
                }
            }
        }

        
        public string insertLog(string mobile,string name, string content , string status )
        {
            SMS_logs sms = new SMS_logs();
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                using (var entity = new MobilePREntities())
                {
                    sms.Mobile = mobile;
                    sms.Name = name;
                    sms.Content = content;
                    sms.Status = status;
                    entity.SMS_logs.Add(sms);
                    entity.SaveChangesAsync();

                    return "Lưu thông tin";

                }
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public void UpInfoSendSMS(string center_name, string cus_name , string mobile)
        {
            string message = string.Empty;
            string response = string.Empty;
            string content = string.Empty;
            string status = string.Empty;
            message = ConfigurationManager.AppSettings["Message"];
            // {0}.Thong bao lich hen chi {1} luc {2} ,ngay {3}.Chi vui long mang CMND.Rat han hanh duoc don tiep.LH CSKH: {4}
            content = string.Format(message, center_name, cus_name, Convert.ToDateTime(DateTime.Now).ToString("HH:mm"), Convert.ToDateTime(DateTime.Now).ToString("dd-MM-yyyy"), "1800588871");
            response = SentSMS(mobile, content);
            if (response.Contains("error"))
            {
                 status = response;
            }
            else
            {
                 status = "success";
            }

            using (var enity = new MobilePREntities())
            {
                var smslog = new SMS_logs
                {
                    Mobile = mobile,
                    Status = status,
                    Content = content,
                    Name = cus_name,
                    Created_At = DateTime.Now
                };
                enity.SMS_logs.Add(smslog);
                enity.SaveChanges();
            }
        }

        public string SentSMS(string phone, string message)
        {
            try
            {
                Dictionary<string, string> param = new Dictionary<string, string>
                {
                    {"BrandName", "VENESA" },
                    {"Phone", phone },
                    {"Message", message },
                };

                var objGrantType = SmsConfigs.getTechAuthorization();
                // set data
                TechSDK.Api.SendBrandNameOTP api = new TechSDK.Api.SendBrandNameOTP(param);
                // get data api response
                string response = objGrantType.execute(api);

                string err = string.Empty;

                //err = a[
                return response;

            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
        }

        public List<Campaign> GetAllCampaign()
        {
            try {

                using (var entity = new MobilePREntities())
                {
                    return entity.Campaign.Where(x => x.isActive == true && x.EndTime > DateTime.Now).ToList();
                }
            }

            catch(Exception e) {

                return null;
            }

        }

        public List<center> GetAllCenter()
        {
            try
            {
                using (var entity = new MobilePREntities())
                {
                    return entity.center.ToList();
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                using (var entity = new MobilePREntities())
                {
                    return entity.User.Where(x => x.IsActive == true && x.isAdmin == false).ToList();
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<ScritpStore> GetScriptList()
        {
            try
            {
                using (var entity = new MobilePREntities())
                {
                    return entity.ScritpStore.Where(x=>x.isActive == true).ToList();
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public CampaignSettiong GetCampaignStore(int id)
        {
            try
            {
                using (var entity = new MobilePREntities())
                {
                    SqlParameter spId = new SqlParameter(@"id" , id);
                    var result = entity.Database.SqlQuery<CampaignSettiong>(
                            @"SELECT DISTINCT co.Id AS option_id , ss.Id AS script_id ,  c.id AS campaign_id , co.Code AS code FROM Campaign c 
                                                INNER JOIN CampaignOption co ON c.Id = co.Campaign_Id
                                                INNER JOIN CampaignStore cs ON co.Code = cs.Campaign_Op_Code
                                                LEFT JOIN City c1 ON c1.Id = co.City_Id
                                                LEFT JOIN Local l ON co.Local_Id = l.local_id
                                                LEFT JOIN ScritpStore ss ON ss.Id = co.Script_id WHERE c.isActive = 1  AND co.Id = @id" , spId).FirstOrDefault();
                       

                    return result;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }



        public ScritpStore GetScritpStore(int id)
        {
            try
            {
                using (var entity = new MobilePREntities())
                {
                    var result = entity.ScritpStore.FirstOrDefault(x => x.Id == id );
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ModelCamapingToCCSI GetToCampaign(string code)
        {
            try
            {
                using (var entity = new MobilePREntities())
                {

                    SqlParameter psCode = new SqlParameter(@"CampaignCode", code);

                    ModelCamapingToCCSI result = entity.Database.SqlQuery<ModelCamapingToCCSI>(@"
                                SELECT TOP 1 c.Name  , c.CampaignCode , ss.Script ,  c.isActive FROM Campaign c 
                                INNER JOIN CampaignOption co ON c.Id = co.Campaign_Id   
                                INNER JOIN CampaignStore cs ON co.Code = cs.Campaign_Op_Code 
                                INNER JOIN ScritpStore ss ON ss.Id = co.Script_id 
                                WHERE c.CampaignCode = @CampaignCode", psCode).FirstOrDefault();

                    return result;
                }
            }
            catch(Exception e)
            {
                return null;
            }
            
        }

        public Campaign GetCampaign(int? id)
        {
            try
            {
                if(id != null)
                {
                    using (var entity = new MobilePREntities())
                    {
                        var result = entity.Campaign.FirstOrDefault(x => x.Id == id);
                        return result;
                    }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }


        public Campaign GetCampaign(int id)
        {
            try
            {
                using (var entity = new MobilePREntities())
                {
                    var result = entity.Campaign.FirstOrDefault(x => x.Id == id);
                    return result;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }


        public Local GetLocalById(int id)
        {
            try
            {
                using (var entity = new MobilePREntities())
                {
                    var result = entity.Local.FirstOrDefault(ob => ob.isActive == false && ob.isActive != null && ob.Id == id);
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;

            }

        }



        public SystemMessage DeleteUser(int id)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                using (var entity = new MobilePREntities())
                {
                    var result =
                        entity.User.FirstOrDefault(ob => ob.isAdmin == false && ob.IsActive != null && ob.Id == id);
                    if (result == null)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "tài khoản không tồn tại";
                        return systemMessage;
                    }

                    result.IsActive = null;
                    entity.SaveChanges();
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = "xóa thành công";
                    return systemMessage;
                }
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }
        }

        public SystemMessage DeleteDataById(int id)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                using (var entity = new MobilePREntities())
                {
                    var result = entity.pg_data.FirstOrDefault(ob => ob.id == id);
                    if (result == null)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "dữ liệu không tồn tại";
                        return systemMessage;
                    }
                    entity.pg_data.Remove(result);
                    entity.SaveChanges();
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = "xóa thành công";
                    return systemMessage;
                }
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }
        }

        public string ValidateAddUser(User myuser)
        {
            string rs = null;
            var vld = new ValidateUtils();

            rs = vld.CheckRequiredField(myuser.UserName, "Tên tài khoản", 2, 55);
            if (rs != null)
            {
                return rs;
            }

            rs = vld.CheckRequiredField(myuser.FullName, "Tên chủ tài khoản", 2, 55);
            if (rs != null)
            {
                return rs;
            }

            rs = vld.CheckRequiredField(myuser.Password, "Mật khẩu", 5, 25);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(myuser.Email, "Email", 5, 25);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckEmail(myuser.Email);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(myuser.agent, "Mã nhân viên", 3, 11);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(myuser.source, "source", 3, 20);
            if (rs != null)
            {
                return rs;
            }
            if (myuser.idCity == 0)
            {
                return rs = "Mời bạn chọn thành phố làm việc";
            }



            return null;
        }

        public string ValidateAddData(pg_data data)
        {
            string rs = null;
            var vld = new ValidateUtils();

            rs = vld.CheckRequiredField(data.name, "Tên khách hàng", 2, 55);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(data.mobile, "Số điện thoại", 9, 11);
            if (rs != null)
            {
                return rs;
            }
            var checkPhoneFormat = vld.PhoneService(data.mobile);
            if (checkPhoneFormat == false)
            {
                return "Số điện thoại không đúng định dạng";
            }
            rs = vld.CheckNonRequiredField(data.center, "trung tâm", 100);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckNonRequiredField(data.address, "địa chỉ", 100);
            if (rs != null)
            {
                return rs;
            }

            rs = vld.CheckNonRequiredField(data.note, "ghi chú", 50);
            if (rs != null)
            {
                return rs;
            }
            return null;
        }


        //public DataManager GetProducts(int currPage, int recodperpage, string key)
        //{
        //    DataManager dataManager = new DataManager();
        //    try
        //    {
        //        using (var entities = new MobilePREntities())
        //        {


        //            ObjectParameter parameter = new ObjectParameter("totalCount", typeof(int));
        //            var result = entities.Data_Manager_PageList(key, currPage, recodperpage, parameter).ToList();
        //            dataManager.DatasList = result;
        //            dataManager.Total = Int32.Parse(parameter.Value.ToString());
        //            return dataManager;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return dataManager;
        //    }
        //}

        public ReturnDataManagerByTeamLead GetProducts(int currPage, int recodperpage, string key)
        {
            ReturnDataManagerByTeamLead dataManager = new ReturnDataManagerByTeamLead();
            try
            {
                using (var entities = new MobilePREntities())
                {
                    User user = AccountUntils.GetUser();
                    int id_teamlead = 0;

                    if (user.LoaiTK == 1 || user.LoaiTK == 100)  // nếu là tài khoản teamlead thì gán id nếu ko thì để là 0 để select all
                    {
                        id_teamlead = user.Id;
                    }
                    

                    ObjectParameter parameter = new ObjectParameter("totalCount", typeof(int));

                    var result = entities.Data_Manager_PageList_By_TeamLead(id_teamlead, key, currPage, recodperpage, parameter).ToList();
                    dataManager.DatasList = result;
                    dataManager.Total = Int32.Parse(parameter.Value.ToString());

                    return dataManager;

                }
            }
            catch (Exception ex)
            {
                return dataManager;
            }
        }


        public List<Campaign> ListCampaignNotOption()
        {
            //SELECT * FROM Campaign c WHERE NOT EXISTS ( SELECT 1 FROM CampaignOption co WHERE co.Campaign_Id = c.Id ) 
            using (var entity = new MobilePREntities())
            {
                List<Campaign> lsCampaign = entity.Database.SqlQuery<Campaign>(@"SELECT * FROM Campaign c WHERE c.isActive = 1 AND NOT EXISTS ( SELECT 1 FROM CampaignOption co WHERE co.Campaign_Id = c.Id )").ToList();
                return lsCampaign;
            }
        }

        public ReturnDataManagerByCity GetProductsByCity(int currPage, int recodperpage, string key , int? idCity)
        {
            ReturnDataManagerByCity dataManager = new ReturnDataManagerByCity();
            try
            {
                using (var entities = new MobilePREntities())
                {
                    User user = AccountUntils.GetUser();
                    int id_teamlead = 0;

                    if (user.LoaiTK == 1 || user.LoaiTK == 100)  // nếu là tài khoản teamlead thì gán id nếu ko thì để là 0 để select all
                    {
                        id_teamlead = user.Id;
                    }

                    ObjectParameter parameter = new ObjectParameter("totalCount", typeof(int));

                    var result = entities.Data_Manager_PageList_By_City(idCity, key, currPage, recodperpage, parameter).ToList();

                    dataManager.DatasList = result;
                    dataManager.Total = Int32.Parse(parameter.Value.ToString());

                    return dataManager;
                }
            }
            catch (Exception ex)
            {
                return dataManager;
            }
        }

        public List<User> GetTeamleadById(int id)
        {
            using (var entity = new MobilePREntities())
            {
                var result = entity.User.Where(m => m.LoaiTK == 1 && m.idManager == id && m.IsActive == true).ToList();

                return result;
            }
        }

        public List<User> GetAllTeamLead()    
        {
            using (var entity = new MobilePREntities())
            {
                var result = entity.User.Where(m => m.LoaiTK == 1 && m.IsActive == true).ToList();

                return result;
            }
        }

        public List<User> GetAllUserManager()
        {
            using (var entity = new MobilePREntities())
            {
                var result = entity.User.Where(m => m.LoaiTK == 100 && m.IsActive == true).ToList();

                return result;
            }
        }

        public List<Group> GetAllGroups()
        {
            using (var entity = new MobilePREntities())
            {
                var result = entity.Group.ToList();
                return result;
            }
        }


    }
}