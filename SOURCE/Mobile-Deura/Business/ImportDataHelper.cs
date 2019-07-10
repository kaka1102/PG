using LinqToExcel;
using Mobile_Deura.Infrastructure.Helpers;
using Mobile_Deura.Untils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Mobile_Deura.Business
{
    public class ImportDataHelper
    {
        public CheckResult CheckImportData(string fileName, List<pg_data_import> importData)
        {
            var result = new CheckResult();
            var tagertFile = new FileInfo(fileName);

            if (!tagertFile.Exists)
            {
                result.ID = Guid.NewGuid();
                result.Success = false;
                result.ErrorCount = 0;
                result.ErrorMessage = "Tệp dữ liệu đã nhập không tồn tại";
                return result;
            }

            var excelFile = new ExcelQueryFactory(fileName);
          
            excelFile.AddMapping<pg_data_import>(x => x.mobile, "mobile");
            excelFile.AddMapping<pg_data_import>(x => x.name, "name");
            excelFile.AddMapping<pg_data_import>(x => x.address, "address");
            excelFile.AddMapping<pg_data_import>(x => x.center, "center");
            excelFile.AddMapping<pg_data_import>(x => x.CampaignCode, "CampaignCode");

            var excelContent = excelFile.Worksheet<pg_data_import>("data");

            int errorCount = 0;
            int rowIndex = 1;
            var importErrorMessages = new List<string>();

            foreach (var row in excelContent)
            {       
                var errorMessage = new StringBuilder();
                var dtImport = new pg_data_import();

                dtImport.mobile = row.mobile;
                

                if (string.IsNullOrWhiteSpace(row.mobile))
                {
                    errorMessage.Append("Ko co so dienthoai");
                }
                dtImport.name = row.name;

                if(string.IsNullOrWhiteSpace(row.name))
                {
                    errorMessage.Append("Ko co so ten");
                }

                dtImport.address = row.address;
                dtImport.center = row.center;
                dtImport.CampaignCode = row.CampaignCode;
                if(errorMessage.Length > 0)
                {
                    errorCount += 1;                    
                    importErrorMessages.Add(string.Format("{0} -- {1} {2}" , rowIndex  , errorMessage, "<br/>"));               
                }
                importData.Add(dtImport);
                rowIndex += 1;
            }

            try
            {
                result.ID = Guid.NewGuid();
                result.Success = errorCount.Equals(0);
                result.RowCount = importData.Count;
                result.ErrorCount = errorCount;

                string allErrorMessage = string.Empty;

                foreach (var message in importErrorMessages)
                {
                    allErrorMessage += message;
                }

                result.ErrorMessage = allErrorMessage;

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public void SaveImportData(IEnumerable<pg_data_import> importData , string _center , string campaigncode)
        {
            try
            {
                //using (var db = new MobilePREntities())
                //{
                //    foreach (var item in db.pg_data_import.OrderBy(x => x.Id))
                //    {
                //        db.pg_data_import.Remove(item);
                //    }
                //    db.SaveChanges();
                //}

                using (var db = new MobilePREntities())
                {
                    foreach (var item in importData)
                    {

                        var t = new pg_data_import
                        {
                            name = item.name,
                            address = item.address,
                            mobile = item.mobile,
                            created_at = DateTime.Now,
                            center = _center,
                            CampaignCode = campaigncode
                        };

                        db.pg_data_import.Add(t);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}


 