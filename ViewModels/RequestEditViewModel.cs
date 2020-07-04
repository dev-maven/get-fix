using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Technicians.ViewModels
{
    public class RequestEditViewModel : RequestViewModel
    {
        public int Id { get; set; }


        public string ExistingPhotoPath { get; set; }

        public string ExistingVideoPath { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string ApplicationUserId { get; set; }

        public MembershipType? MembershipType { get; set; }

        public bool DeleteMedia { get; set; }



    }
}
