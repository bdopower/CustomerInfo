using CustomerInfoApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CustomerInfoApp.DataEntities
{
    //This is the Customer table
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string OtherPhone { get; set; }
        public string Certificate { get; set; }
        public int ReportNumber { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd/MM/yyyy")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode =true, DataFormatString = "dd/MM/yyyy")]
        public DateTime? EndDate { get; set; }
        public bool isActive { get; set; }
        public string Notes { get; set; }

        [ForeignKey("RegisteredBy")]
        public string RegisteredById { get; set; }
        public virtual ApplicationUser RegisteredBy { get; set; }
    }
}