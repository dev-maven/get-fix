using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Technicians.Models;

namespace Technicians.ViewModels
{
    public class ReportByDateViewModel
    {
       
            [DataType(DataType.Date)]
            public DateTime ScheduleTime { get; set; }

            public int CustomerCount { get; set; }
    }



}
