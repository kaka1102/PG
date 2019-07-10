using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Mobile_Deura.Models
{
  

    public partial class AlertCampaign
    {

        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime EndTime { get; set; }
        public string Name { get; set; }
        public int Campaign_Id { get; set; }
        public int Script_id { get; set; }
        public string CampaignCode { get; set; }

    }

    public partial class AlertCenterCampaign
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime EndTime { get; set; }
        public string Name { get; set; }
        public int Campaign_Id { get; set; }
        public int Script_id { get; set; }
        public string CampaignCode { get; set; }
    }
}