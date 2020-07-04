using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technicians.ViewModels
{
    public class CompletedEditViewModel : RequestEditViewModel
    {
        [DataType(DataType.MultilineText)]
        [Display(Name = "Admin Comment")]
        public string AdminComment { get; set; }
        [Display(Name = "Request Status")]
        public RequestStatus RequestStatus { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Cost Details")]
        public string CostDetails { get; set; }
        public int Cost { get; set; }
        public string NairaCost { get { return "₦" + Cost.ToString("N0"); } }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Reason(s) for Cancellation")]
        public string CancellationRemark { get; set; }
    }
}
