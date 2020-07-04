using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technicians.ViewModels
{
    public class UserReportViewModel
    {

        [DataType(DataType.Date)]
        public DateTime DateRegistered { get; set; }

        public int CustomerCount { get; set; }
    }



}

