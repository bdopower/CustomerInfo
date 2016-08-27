using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerInfoApp.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Certificate { get; set; }
        public string RegisteredBy { get; set; }
        public string ReportNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ActiveStatus { get; set; }
    }
}