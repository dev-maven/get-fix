using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technicians.ViewModels
{
    public enum ServiceType
    {
        [Display(Name = "Repair")]Repair,
        [Display(Name = "Part Replacement")] PReplacement,
        [Display(Name = "I don't know")]IDK,
    }
}
