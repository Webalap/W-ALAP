using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Common.ModelsEx.Shopping.Discounts
{
    public class PromoCodes
    {
        public PromoCodes()
        {
            DiscountAmount = 0.0m;
        }
        [Required]
        [Display(Name = "Code")]
        public string PromoCode { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Required]
        [Display(Name = "Discount Type")]
        public int DiscountType { get; set; }
        [Required]
        [Display(Name = "Discount Amount")]
        public decimal DiscountAmount { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Use Count")]
        public int UsedCount { get; set; }
        [Display(Name = "Eligible")]
        public int EligibleCount { get; set; }

    }
}