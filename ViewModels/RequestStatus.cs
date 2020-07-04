using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technicians.ViewModels
{
    public enum RequestStatus
    {
            [Display(Name = "Pending")] pending,
            [Display(Name = "Completed")] completed,
            [Display(Name = "Cancelled")] cancelled

    }
}
