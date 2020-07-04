using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technicians.ViewModels
{
    public enum NeededService
    {
        [Display(Name = "Electrical")] Electrical,
        [Display(Name = "Plumbing")] Plumbing,
        [Display(Name = "Air Conditioning")] AirConditioning,
        [Display(Name = "Sewage Removal")] SewageRemoval,
        [Display(Name = "Cleaning")] Cleaning
    }
}
