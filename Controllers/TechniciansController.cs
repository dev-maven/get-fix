using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Technicians.Models;
using Technicians.ViewModels;

namespace Technicians.Controllers
{
    public class TechniciansController : Controller
    {
        private readonly ProfileContext _context;
        private readonly IHostingEnvironment hostingEnvironment;

        public TechniciansController(ProfileContext context, IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        public IActionResult Index()
        {

            return RedirectToAction(nameof(TechniciansIndex));
        }

        [Authorize(Roles = "Admin, SuperAdmin")]


        // GET: Technicians
        public async Task<IActionResult> TechniciansIndex(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)

            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var technician = from s in _context.Technician
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                technician = technician.Where(s => s.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    technician = technician.OrderByDescending(s => s.FirstName);
                    break;
                case "Date":
                    technician = technician.OrderBy(s => s.DateRegistered);
                    break;
                case "date_desc":
                    technician = technician.OrderByDescending(s => s.DateRegistered);
                    break;
                default:  // Name ascending 
                    technician = technician.OrderBy(s => s.FirstName);
                    break;
            }


            int pageSize = 8;

            return View(await PaginatedList<Technician>.CreateAsync(technician.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        [Authorize(Roles = "Admin, SuperAdmin")]

        // GET: Technicians/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technician = await _context.Technician
                .FirstOrDefaultAsync(m => m.TechnicianId == id);
            if (technician == null)
            {
                return NotFound();
            }

            return View(technician);
        }

        // GET: Technicians/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Completed()
        {
            return View();
        }

        // POST: Technicians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TechnicianViewModel model)

        {
           
                if (ModelState.IsValid)
                {
                string imageFileName = FirstImageUploadedFile(model);


                Technician technician = new Technician
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    DateRegistered = DateTime.Now,
                    EmailAddress = model.EmailAddress,
                    CertificatePath = imageFileName,
                    PhoneNumber = model.PhoneNumber
                };
                    _context.Add(technician);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Completed));
                }
                      return View(model);
        }

        private string FirstImageUploadedFile(TechnicianViewModel model)
        {
            string imageFileName = null;
            if (model.Certificate != null)
            {
                string uploadsfolder = Path.Combine(hostingEnvironment.WebRootPath, "certificates");
                imageFileName = Guid.NewGuid().ToString() + " _" + model.Certificate.FileName;
                string filePath = Path.Combine(uploadsfolder, imageFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Certificate.CopyTo(fileStream);
                }

            }

            return imageFileName;
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        // GET: Technicians/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technician = await _context.Technician
                .FirstOrDefaultAsync(m => m.TechnicianId == id);
            if (technician == null)
            {
                return NotFound();
            }

            return View(technician);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        // POST: Technicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var technician = await _context.Technician.FindAsync(id);
            _context.Technician.Remove(technician);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(TechniciansIndex));
        }

        private bool TechnicianExists(string id)
        {
            return _context.Technician.Any(e => e.TechnicianId == id);
        }
    }
}
