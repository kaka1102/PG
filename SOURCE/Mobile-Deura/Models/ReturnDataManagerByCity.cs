using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mobile_Deura.Models
{
   
    public class ReturnDataManagerByCity
    {
        public List<Data_Manager_PageList_By_City_Result> DatasList { get; set; }


        public int Total { get; set; }
    }
}