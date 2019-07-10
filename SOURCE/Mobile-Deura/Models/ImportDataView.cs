using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mobile_Deura.Models
{
    public partial class ImportDataView
    {
        public Guid KeyId { get; set; }
        public int? Id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string mobile { get; set; }

        public string center { get; set; }

        public string Sent_log { get; set; } 
    }
}