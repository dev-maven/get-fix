using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Technicians.Models
{
    public class Account : Controller
    {
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
