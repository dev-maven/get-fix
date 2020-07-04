using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technicians.ViewModels
{
    public enum PaymentStatus
    {
        [Display(Name = "Unpaid")] unpaid,
        [Display(Name = "Paid")] paid

    }
}
