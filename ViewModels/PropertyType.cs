using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technicians.ViewModels
{
        public enum PropertyType
        {
            None,
            [Display(Name = "4 Beds")] Bed4,
            [Display(Name = "3 Beds")] Bed3,
            [Display(Name = "2 Beds")] Bed2,
            [Display(Name = "1 Bed")] Bed1,
            [Display(Name = "Mini Flat")] MFlat,
            [Display(Name = "Others")] Others,
        }
    }
