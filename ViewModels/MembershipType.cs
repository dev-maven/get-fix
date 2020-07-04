using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technicians.ViewModels
{
        public enum MembershipType
        {
            [Display(Name = "PAYG")] PAYG,
            [Display(Name = "Subscriber")] SUB,

        }

    }

