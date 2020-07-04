using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Technicians.ViewModels;
using static Technicians.ViewModels.MembershipType;
using static Technicians.ViewModels.PropertyType;

namespace Technicians.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name ="Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Property Type")]
        public PropertyType PropertyType { get; set; }
        public string Address { get; set; }
        [Display(Name = "Membership Type")]
        public MembershipType Membership { get; set; }
        [Display(Name = "Balance")]
        public int MyBalance { get; set; }

        public string NairaBalance { get { return "₦" + MyBalance.ToString("N0"); } }
   
        public string Features { get; set; }
        [Display(Name = "Date Registered")]
        public DateTime DateRegistered { get; set; }
    }
}
