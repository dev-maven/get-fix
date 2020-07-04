using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technicians.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name ="Email Address")]
        public string EmailAddress { get; set; }
    }
}
