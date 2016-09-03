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
        [Display(Name ="الاسم بالكامل")]
        public string FullName { get; set; }

        [Display(Name ="رقم الهاتف")]
        public string Phone { get; set; }

        [Display(Name = "ارقام أخرى")]
        public string OtherPhone { get; set; }

        [Display(Name = "الشهادة")]
        public string Certificate { get; set; }

        [Display(Name = "رقم التقرير")]
        public int ReportNumber { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd/MM/yyyy")]
        [Display(Name = "تاريخ الادخال")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode =true, DataFormatString = "dd/MM/yyyy")]
        [Display(Name = "تاريخ الانتهاء")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "الحالة")]
        public bool isActive { get; set; }

        [Display(Name = "الملاحظات")]
        public string Notes { get; set; }

        [ForeignKey("RegisteredBy")]
        [Display(Name = "اسم الحساب")]
        public string RegisteredById { get; set; }

        [Display(Name = "اسم الحساب")]
        public virtual ApplicationUser RegisteredBy { get; set; }
    }
}