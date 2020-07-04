using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technicians.Models
{
    public class Technician
    {
        public string TechnicianId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Date Registered")]
        public DateTime DateRegistered { get; set; }

        [Display(Name = "Certificate")]
        public string CertificatePath { get; set; }
    }
}
