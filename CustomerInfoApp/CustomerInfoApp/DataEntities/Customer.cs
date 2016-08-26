using CustomerInfoApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CustomerInfoApp.DataEntities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Certificate { get; set; }
        public int ReportNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool isActive { get; set; }

        [ForeignKey("RegisteredBy")]
        public string RegisteredById { get; set; }
        public virtual ApplicationUser RegisteredBy { get; set; }
    }
}