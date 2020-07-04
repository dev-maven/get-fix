using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Technicians.Models;

namespace Technicians.ViewModels
{
    public class AdminEditViewModel : RequestEditViewModel 
    {
        [DataType(DataType.MultilineText)]
        [Required]
        [Display(Name = "Admin Comment*")]
        public string AdminComment { get; set; }
        [Required]
        [Display(Name = "Request Status*")]
        public RequestStatus RequestStatus { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Cost Details*")]
        public string CostDetails { get; set; }
        [Required]
        [Display(Name = "Cost*")]
        public int Cost { get; set; }
        public string NairaCost { get { return "₦" + Cost.ToString("N0"); } }


    }
}
