using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mobile_Deura.Infrastructure.Helpers
{
    public class CheckResult
    {
        public Guid ID { get; set; }

        public bool Success { get; set; }

        public int RowCount { get; set; }

        public int ErrorCount { get; set; }

        public string ErrorMessage { get; set; }

    }
}