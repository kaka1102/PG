using Mobile_Deura.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Mobile_Deura.Business;
using Mobile_Deura.Untils;
using System.Data.SqlClient;
using System.Web.Hosting;

namespace Mobile_Deura.Areas.Admin.Controllers
{

    public class ImportDataController : Controller
    {
        private string fileSavedPath = WebConfigurationManager.AppSettings["UploadPath"];
        private string fileDownloadPath = WebConfigurationManager.AppSettings["DownloadPath"];
        
        // GET: Admin/ImportData
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListImportData()
        {
            var db = new Business.Business();
            ViewBag.center = db.GetAllCenter();
            ViewBag.campaign = db.GetAllCampaign();
            
            return View();
        }

        public ActionResult _ListImportData()
        {
            var entity = new MobilePREntities();
            var db = new Business.Business();
            User user = AccountUntils.GetUser();
            string centername = db.GetCenterEnglishNameById(user.idCenter);
            List<ImportDataView> result = null;
            if(user.LoaiTK == 0)
            {
                result = entity.Database.SqlQuery<ImportDataView>("SELECT Id, name , address , mobile , center  , Sent_log FROM pg_data_import").ToList();
            }
            else
            {
                if(centername != null)
                {
                    SqlParameter sCenter = new SqlParameter(@"center", centername);
                    result = entity.Database.SqlQuery<ImportDataView>(@"SELECT Id, name , address , mobile , center  , Sent_log FROM pg_data_import WHERE center = @center", sCenter).ToList();
                }
                
              
            }

            return PartialView(result);
        }

        public ActionResult AddList()
        {
            return PartialView();
        }


        public ActionResult ToImport(string savedFileName)
        {
             return Json("success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadFile(string file = "")
        {

            file = string.Concat(HostingEnvironment.MapPath(fileDownloadPath), "/", file);

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = Path.GetFileName(file);

            return File(file, contentType, fileName);

        }
        // fileDownloadPath
        [HttpPost]
        public ActionResult Import(string savedFileName , string center , string campaign)
        {
            var jo = new JObject();
            string result;
            try
            {
                var fileName = string.Concat(Server.MapPath(fileSavedPath), "/", savedFileName);
                var importData = new List<pg_data_import>();
                var helper = new ImportDataHelper();
                var checkResult = helper.CheckImportData(fileName, importData);

                jo.Add("Result", checkResult.Success);
                jo.Add("Msg", checkResult.Success ? string.Empty : checkResult.ErrorMessage);

                if (checkResult.Success)
                {                  
                    helper.SaveImportData(importData , center , campaign);
                }
                result = JsonConvert.SerializeObject(jo);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Content(result, "application/json");
        }


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase FileUpload)
        {
            JObject jo = new JObject();
            string result = string.Empty;

            if(FileUpload == null)
            {
                jo.Add("Result", false);
                jo.Add("Msg", "không upload được file");
                result = JsonConvert.SerializeObject(jo);
               // return RedirectToAction("ListImportData", "ImportData");
            }
            if(FileUpload.ContentLength <= 0)
            {
                jo.Add("Result", true);
                jo.Add("Msg", "Vui lòng tải lên tập tin chính xác");
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }

            string fileExtName = Path.GetExtension(FileUpload.FileName).ToLower();
            if (!fileExtName.Equals(".xls", StringComparison.OrdinalIgnoreCase)
               &&
               !fileExtName.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                jo.Add("Result", false);
                jo.Add("Msg", "  .xls   .xlsx   ");
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }

            try
            {
                var uploadResult = this.FileUploadHandler(FileUpload);

                jo.Add("Result", !string.IsNullOrWhiteSpace(uploadResult));
                jo.Add("Msg", !string.IsNullOrWhiteSpace(uploadResult) ? uploadResult : "");

                result = JsonConvert.SerializeObject(jo);
            }
            catch (Exception ex)
            {
                jo.Add("Result", false);
                jo.Add("Msg", ex.Message);
                result = JsonConvert.SerializeObject(jo);
            }
            return Content(result, "application/json");
        }

        
        private string FileUploadHandler(HttpPostedFileBase file)
        {
            string result;

            if (file == null)
            {
                 throw new ArgumentNullException("file", "Tải lên thất bại: không có tập tin！");
            }
            if (file.ContentLength <= 0)
            {
                 throw new InvalidOperationException("Tải lên thất bại: tập tin không có nội dung!");
            }

            try
            {
                string virtualBaseFilePath = Url.Content(fileSavedPath);
                string filePath = HttpContext.Server.MapPath(virtualBaseFilePath);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string newFileName = string.Concat(
                    DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    Path.GetExtension(file.FileName).ToLower());

                string fullFilePath = Path.Combine(Server.MapPath(fileSavedPath), newFileName);
                file.SaveAs(fullFilePath);

                result = newFileName;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

    }
}