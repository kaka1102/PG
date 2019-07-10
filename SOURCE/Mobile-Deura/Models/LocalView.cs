using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mobile_Deura.Models
{ 
    public partial class LocalView
    {
        public Nullable<int> local_id { get; set; }
        public string name { get; set; }
        public string City_Code { get; set; }
        public string City_Name { get; set; }
        public bool? isActive { get; set; }
        public string parent_id { get; set; }
        public string shortcode { get; set; }
        public int Id { get; set; }   

    }

}