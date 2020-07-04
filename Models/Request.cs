using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Technicians.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Technicians.Models
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }
      
        public int myRef { get { return RequestId + 100; } set { } }
        [Required]
        public string RefNo { get { return "REF" + myRef.ToString(); } set { } }
        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Needed Service")]
        public NeededService NeededService { get; set; }
        [Required]
        [Display(Name = "Service Type")]
        public string ServiceType { get; set; }
        [Required]
        [Display(Name = "Membership Type")]
        public MembershipType MembershipType { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Schedule Time")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime ScheduleTime { get; set; }
        public string MyService { get; set; }
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        [Display(Name = "Photo")]
        public string PhotoPath { get; set; }
        [Display(Name = "Video")]
        public string VideoPath { get; set; }
        public string NairaCost { get { return "₦" + Cost.ToString("N0"); } }
        [Required]
        public RequestStatus RequestStatus { get; set; }
        public int Cost { get; set; }
        [Display(Name = "Admin Comment")]
        [DataType(DataType.MultilineText)]
        public string AdminComment { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Cost Details")]
        public string CostDetails { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Reason(s) for Cancellation")]
        public string CancellationRemark { get; set; }


        [Display(Name = "Payment Status")]
        public PaymentStatus PaymentStatus { get; set; }
    }
}
