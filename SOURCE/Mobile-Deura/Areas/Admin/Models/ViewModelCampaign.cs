using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mobile_Deura
{

    public class ObjectReturn
    {
        public string status { get; set; }
        public string msg { get; set; }
    }

    public class ModelCamapingToCCSI
    {
        public string Name { get; set; } 
        public string CampaignCode { get; set; }
        public string Script { get; set; }
        public bool isActive { get; set; }
    }
    public class ViewModelCampaign
    {
        public   int Id { get; set; }
        public   string Name { get; set; }
        public   int? Type { get; set; }
        public   DateTime? StartTime { get; set; }
        public   DateTime? EndTime { get; set; }
        public bool isActive { get; set; }
        public int? NumberContact { get; set; }
        public int? Budgets { get; set; }
        public string TypeCode { get; set; }
        public string Domain { get; set; }
        public int PgDataCount { get; set; }

    }

    public class ViewModelInListCampain
    {
        public int Id { get; set; }  // id option
        public string Name { get; set; } // name campaign
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Script { get; set; }
        public string City_Name { get; set; }
        public string Local_Name { get; set; }
        public int? City_id { get; set; }
        public int? local_id { get; set; }
        public int? script_id { get; set; }
        public int campaign_id { get; set; }
        public int numberUser { get; set; }
        public int Type { get; set; }
        public string CampaignCode { get; set; }
    }

    public class ViewModelListScript
    {
        public int Id { get; set; }
        public string Script { get; set; }
        public DateTime? CreatAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string NameScript { get; set; }
        public bool isActive { get; set; }
    }


    public class Person
    {
        public string name { get; set; }
        public int Id { get; set; }
    }

    public class RootObject
    {
        public List<Person> People { get; set; }
    }


    public class CampaignSettiong
    {
        public int campaign_id { get; set; }
        public int script_id { get; set; }
        public string code { get; set; }
        public int option_id { get; set; }             
    }

}