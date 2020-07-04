using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technicians.ViewModels
{
    public class RequestCancelViewModel : RequestEditViewModel
    {
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Reason(s) for Cancellation*" )]
        public string CancellationRemark { get; set; }
    }
}
