using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Technicians.Models;
using static Technicians.ViewModels.MembershipType;
using static Technicians.ViewModels.PropertyType;

namespace Technicians.ViewModels
{
    public class RegisterViewModel
    {
        public string RegisterId { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
        public string LastName { get; set; }

        public string  CustomerName { get; set; }
        //public string CustomerName { get { return FirstName + " " + LastName; } set { } }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "IsEmailInUse", controller:"Home")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords must match!")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        public MembershipType Membership { get; set; }

        [DataType(DataType.MultilineText)]
        public string Features { get; set; }

        public DateTime DateRegistered { get; set; }




    }

}
