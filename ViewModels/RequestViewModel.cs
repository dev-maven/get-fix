using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Technicians.ViewModels
{
    public class RequestViewModel
    {

        [Key]
        public int RequestId { get; set; }

        public int myRef { get { return RequestId + 100; } set { } }
        [Required]
        public string RefNo { get { return "REF" + myRef.ToString(); } set { } }

        [Required]
        [Display(Name = "Service Needed*")]

        public NeededService NeededService { get; set; }
        //[Required]
        [Display(Name = "What is the problem?*")]
        public string ServiceType { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:mm/dd/yyyy hh:mm}", ApplyFormatInEditMode = false)]
        [Display(Name = "When can we come?*")]
        public DateTime ScheduleTime { get; set; }
        [Required]
        public RequestStatus RequestStatus { get; set; }
        public string MyService { get; set; }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Additional info")]
        public string Comment { get; set; }
        [Display(Name = "Upload Photo")]
        public IFormFile Photo { get; set; }
        [Display(Name = "Upload Video (max size 10Mb)")]
        public IFormFile Video { get; set; }
        [Display(Name = "Payment Status")]
        public PaymentStatus PaymentStatus { get; set; }


    }
}
