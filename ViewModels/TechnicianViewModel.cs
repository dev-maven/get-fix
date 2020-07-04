using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technicians.ViewModels
{
    public class TechnicianViewModel
    {
        public string TechnicianId { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
        [Display(Name = "First Name*")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
        [Display(Name = "Last Name*")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number*")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "IsEmailInUse", controller: "Home")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Address*")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "Date Registered")]
        public DateTime DateRegistered { get; set; }

        [Display(Name = "Upload Certificate")]
        public IFormFile Certificate { get; set; }
    }
}
