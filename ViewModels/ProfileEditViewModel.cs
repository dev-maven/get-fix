using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Technicians.Models;

namespace Technicians.ViewModels
{
    public class ProfileEditViewModel
    {
        public int Balance { get; set; }
        [DataType(DataType.MultilineText)]
        public string Features { get; set; }

        public MembershipType Membership { get; set; }

        public string RegisterId { get; set; }
        [Display(Name = "First Name")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
        public string LastName { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Property Type")]
        public PropertyType PropertyType { get; set; }
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public int newBal { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string NairaBalance { get { return "₦" + Balance.ToString("N0"); } }
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        [Display(Name = "Date Registered")]
        public DateTime DateRegistered { get; set; }
    }
}
