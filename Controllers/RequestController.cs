using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Technicians.Models;
using Technicians.Services;
using Technicians.ViewModels;
using WebMatrix.WebData;

namespace Technicians.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class RequestController : Controller
    {
        private readonly ProfileContext _context;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSend _emailSend;

        public RequestController(ProfileContext context, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, IEmailSend emailSend)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _emailSend = emailSend;


        }


        // GET: Request
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
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

            var requests = from s in _context.Request
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                requests = requests.Where(s => s.CustomerName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    requests = requests.OrderByDescending(s => s.CustomerName);
                    break;
                case "Date":
                    requests = requests.OrderBy(s => s.ScheduleTime);
                    break;
                case "date_desc":
                    requests = requests.OrderByDescending(s => s.ScheduleTime);
                    break;
                default:  // Name ascending 
                    requests = requests.OrderBy(s => s.CustomerName);
                    break;
            }


            int pageSize = 8;

            return View(await PaginatedList<Request>.CreateAsync(requests.AsNoTracking(), pageNumber ?? 1, pageSize));

        }


        private string ProcessUploadedFile(RequestViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsfolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + " _" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsfolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }

            }

            return uniqueFileName;
        }

        // GET: Request/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var request = await _context.Request.FindAsync(id);

            if (request.RequestStatus == RequestStatus.pending || request.RequestStatus == RequestStatus.cancelled)
            {
                AdminEditViewModel adminEditViewModel = new AdminEditViewModel
                {
                    Id = request.RequestId,
                    NeededService = request.NeededService,
                    ServiceType = request.ServiceType,
                    Comment = request.Comment,
                    ExistingPhotoPath = request.PhotoPath,
                    CustomerName = request.CustomerName,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    ScheduleTime = request.ScheduleTime,
                    ApplicationUserId = request.ApplicationUserId,
                    RefNo = request.RefNo,
                    MembershipType = request.MembershipType,
                    RequestStatus = request.RequestStatus,
                    AdminComment = request.AdminComment,
                    Cost = request.Cost,
                    CostDetails = request.CostDetails,
                    ExistingVideoPath = request.VideoPath,
                    PaymentStatus = request.PaymentStatus
                };



                return View(adminEditViewModel);
            }
            CompletedEditViewModel completedEditViewModel = new CompletedEditViewModel
            {
                Id = request.RequestId,
                NeededService = request.NeededService,
                ServiceType = request.ServiceType,
                Comment = request.Comment,
                ExistingPhotoPath = request.PhotoPath,
                CustomerName = request.CustomerName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                ScheduleTime = request.ScheduleTime,
                ApplicationUserId = request.ApplicationUserId,
                RefNo = request.RefNo,
                MembershipType = request.MembershipType,
                RequestStatus = request.RequestStatus,
                AdminComment = request.AdminComment,
                Cost = request.Cost,
                CostDetails = request.CostDetails,
                ExistingVideoPath = request.VideoPath,
                CancellationRemark = request.CancellationRemark,
                PaymentStatus = request.PaymentStatus
            };



            return View("CompletedDetails", completedEditViewModel);
        }


        // POST: Request/Edit/5
        // To protect from overposting attacks, please enable the specific features you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminEditViewModel model, ProfileEditViewModel myprofile, CompletedEditViewModel admin)
        {

            var FullName = _context.Request.FindAsync(id).Result.CustomerName;
            var phone = _context.Request.FindAsync(id).Result.PhoneNumber;
            var address = _context.Request.FindAsync(id).Result.Address;
            var membership = _context.Request.FindAsync(id).Result.MembershipType;
            var requestStatus = _context.Request.FindAsync(id).Result.RequestStatus;
            var adminEmail = "admin@webvestlimited.com";






            var Userid = _context.Request.FindAsync(id).Result.ApplicationUserId;
            var prevBal = _context.Users.FindAsync(Userid).Result.MyBalance;
            var date = _context.Users.FindAsync(Userid).Result.DateRegistered;
            var email = _context.Users.FindAsync(Userid).Result.Email;
            var fName = _context.Users.FindAsync(Userid).Result.FirstName;
            var lName = _context.Users.FindAsync(Userid).Result.LastName;
            var password = _context.Users.FindAsync(Userid).Result.PasswordHash;
            var features = _context.Users.FindAsync(Userid).Result.Features;
            var property = _context.Users.FindAsync(Userid).Result.PropertyType;

            if (requestStatus == RequestStatus.cancelled || requestStatus == RequestStatus.pending)
            { 
            if (ModelState.IsValid && model.RequestStatus == RequestStatus.completed)
            {
                var request = await _context.Request.FindAsync(id);
                request.CustomerName = FullName;
                request.Address = address;
                request.PhoneNumber = phone;
                request.MembershipType = _context.Request.FindAsync(id).Result.MembershipType;
                request.RequestId = _context.Request.FindAsync(id).Result.RequestId;
                request.ServiceType = _context.Request.FindAsync(id).Result.ServiceType;
                request.ScheduleTime = _context.Request.FindAsync(id).Result.ScheduleTime;
                request.NeededService = _context.Request.FindAsync(id).Result.NeededService;
                request.Comment = _context.Request.FindAsync(id).Result.Comment;
                request.ApplicationUserId = Userid;
                request.RefNo = _context.Request.FindAsync(id).Result.RefNo;
                //request.RequestStatus = _context.Request.Find(request.RequestStatus)

                request.AdminComment = model.AdminComment;
                request.RequestStatus = model.RequestStatus;
                request.Cost = model.Cost;
                request.CostDetails = model.CostDetails;
                request.PhotoPath = _context.Request.FindAsync(id).Result.PhotoPath;
                request.VideoPath = _context.Request.FindAsync(id).Result.VideoPath;
                request.CancellationRemark = _context.Request.FindAsync(id).Result.CancellationRemark;
                //request.ApplicationUserId = _context.Request.FindAsync(Request).Result.ApplicationUserId;



                var profile = await _userManager.FindByIdAsync(request.ApplicationUserId);
                //profile.Id = request.ApplicationUserId;
                profile.CustomerName = FullName;
                profile.FirstName = fName;
                profile.LastName = lName;
                profile.Address = address;
                profile.PhoneNumber = phone;
                profile.Features = features;
                profile.PropertyType = property;
                profile.PasswordHash = password;
                profile.Membership = membership;
                profile.Email = email;
                profile.DateRegistered = date;
                if (profile.Membership == MembershipType.SUB)
                {
                    profile.MyBalance = prevBal - model.Cost;
                        request.PaymentStatus = PaymentStatus.paid;
                }
                else profile.MyBalance = prevBal;

                _context.Update(request);
                _context.Update(profile);
                await _context.SaveChangesAsync();

                    var callbackUrl = Url.Action("Details", "UserRequest", new { id = request.RequestId },
                   protocol: HttpContext.Request.Scheme);
                    await _emailSend.SendEmailAsync(email, "Request Completed",
                        $"Dear " + profile.FirstName +
                        $" <br />" +
                        $" Your Request has been completed" +
                        $" <br />" +
                        $"click the button below to check the details and cost" +
                        $" <br /> " +
                        $" <br /> " +
                        $" <br /> " +
                        $" <a style=\"  text-decoration: none; color: #fff; background: #101522; padding: 7px 22px; margin-right: 10px; border-radius: 50px; border: 2px solid #f82249;font-weight: 600; margin-left: 8px; margin-top: 2px; margin-bottom: 2px; line-height: 1.5;font-size: 13px;\" href ='{callbackUrl}'>Check My Request</a>" +
                        $" <br />" +
                        $" " +
                        $" <br />"
                                        );
                    var admincallbackUrl = Url.Action("Edit", "Request", new { id = request.RequestId },
                    protocol: HttpContext.Request.Scheme);
                    await _emailSend.SendEmailAsync(adminEmail, "Request Completed",

                 $"  Request from  " + request.CustomerName +
                 $" has been completed" +
                 $" <br />" +
                 $"click the button below to check the details of the request" +
                 $" <br /> " +
                 $" <br /> " +
                 $" <br /> " +
                 $" <a style=\"  text-decoration: none; color: #fff; background: #101522; padding: 7px 22px; margin-right: 10px; border-radius: 50px; border: 2px solid #f82249;font-weight: 600; margin-left: 8px; margin-top: 2px; margin-bottom: 2px; line-height: 1.5;font-size: 13px;\" href ='{admincallbackUrl}'>Check Request</a>" +
                 $" <br />" +
                 $" " +
                 $" <br />"
                         );


                    return RedirectToAction("Index", new { id = request.RequestId });
            }

            ModelState.AddModelError(string.Empty, "Please change Request Status to Completed and fill all fields ");
            return View(model);
        }
            if (ModelState.IsValid)
            {
                var request = await _context.Request.FindAsync(id);
                request.CustomerName = FullName;
                request.Address = address;
                request.PhoneNumber = phone;
                request.MembershipType = _context.Request.FindAsync(id).Result.MembershipType;
                request.RequestId = _context.Request.FindAsync(id).Result.RequestId;
                request.ServiceType = _context.Request.FindAsync(id).Result.ServiceType;
                request.ScheduleTime = _context.Request.FindAsync(id).Result.ScheduleTime;
                request.NeededService = _context.Request.FindAsync(id).Result.NeededService;
                request.Comment = _context.Request.FindAsync(id).Result.Comment;
                request.ApplicationUserId = Userid;
                request.RefNo = _context.Request.FindAsync(id).Result.RefNo;
                request.PaymentStatus = _context.Request.FindAsync(id).Result.PaymentStatus;
                //request.RequestStatus = _context.Request.Find(request.RequestStatus)

                request.AdminComment = admin.AdminComment;
                request.RequestStatus = _context.Request.FindAsync(id).Result.RequestStatus;
                request.Cost = _context.Request.FindAsync(id).Result.Cost;
                request.CostDetails = admin.CostDetails;
                request.PhotoPath = _context.Request.FindAsync(id).Result.PhotoPath;
                request.VideoPath = _context.Request.FindAsync(id).Result.VideoPath;
                request.CancellationRemark = _context.Request.FindAsync(id).Result.CancellationRemark;
                //request.ApplicationUserId = _context.Request.FindAsync(Request).Result.ApplicationUserId;



                var profile = await _userManager.FindByIdAsync(request.ApplicationUserId);
                //profile.Id = request.ApplicationUserId;
                profile.CustomerName = FullName;
                profile.FirstName = fName;
                profile.LastName = lName;
                profile.Address = address;
                profile.PhoneNumber = phone;
                profile.Features = features;
                profile.PropertyType = property;
                profile.PasswordHash = password;
                profile.Membership = membership;
                profile.Email = email;
                profile.DateRegistered = date;
                profile.MyBalance = prevBal;
               

                _context.Update(request);
                _context.Update(profile);
                await _context.SaveChangesAsync();

                var callbackUrl = Url.Action("Details", "UserRequest", new { id = request.RequestId },
       protocol: HttpContext.Request.Scheme);
                await _emailSend.SendEmailAsync(email, "Request Details updated",
                    $"Dear " + profile.FirstName +
                    $" <br />" +
                    $" Your Request details has been updated" +
                    $" <br />" +
                    $"click the button below to check the details and cost" +
                    $" <br /> " +
                    $" <br /> " +
                    $" <br /> " +
                    $" <a style=\"  text-decoration: none; color: #fff; background: #101522; padding: 7px 22px; margin-right: 10px; border-radius: 50px; border: 2px solid #f82249;font-weight: 600; margin-left: 8px; margin-top: 2px; margin-bottom: 2px; line-height: 1.5;font-size: 13px;\" href ='{callbackUrl}'>Check My Request</a>" +
                    $" <br />" +
                    $" " +
                    $" <br />"
                                    );
                var admincallbackUrl = Url.Action("Edit", "Request", new { id = request.RequestId },
                protocol: HttpContext.Request.Scheme);
                await _emailSend.SendEmailAsync(adminEmail, "Request Details Updated",

             $"  Request from  " + request.CustomerName +
             $"has been updated" +
             $" <br />" +
             $"click the button below to check the details of the request" +
             $" <br /> " +
             $" <br /> " +
             $" <br /> " +
             $" <a style=\"  text-decoration: none; color: #fff; background: #101522; padding: 7px 22px; margin-right: 10px; border-radius: 50px; border: 2px solid #f82249;font-weight: 600; margin-left: 8px; margin-top: 2px; margin-bottom: 2px; line-height: 1.5;font-size: 13px;\" href ='{admincallbackUrl}'>Check Request</a>" +
             $" <br />" +
             $" " +
             $" <br />"
                     );


                return RedirectToAction("Index", new { id = request.RequestId });
            }
            return View(admin);

        }


        public async Task<IActionResult> Paid(int id)
        {

            var request = await _context.Request.FindAsync(id);

            if (request.PaymentStatus == PaymentStatus.unpaid)
            {


                request.CustomerName = _context.Request.FindAsync(id).Result.CustomerName;
                request.Address = _context.Request.FindAsync(id).Result.Address;
                request.PhoneNumber = _context.Request.FindAsync(id).Result.PhoneNumber;
                request.MembershipType = _context.Request.FindAsync(id).Result.MembershipType;
                request.RequestId = _context.Request.FindAsync(id).Result.RequestId;
                request.ServiceType = _context.Request.FindAsync(id).Result.ServiceType;
                request.ScheduleTime = _context.Request.FindAsync(id).Result.ScheduleTime;
                request.NeededService = _context.Request.FindAsync(id).Result.NeededService;
                request.Comment = _context.Request.FindAsync(id).Result.Comment;
                request.ApplicationUserId = _context.Request.FindAsync(id).Result.ApplicationUserId;
                request.RefNo = _context.Request.FindAsync(id).Result.RefNo;
                request.PaymentStatus = PaymentStatus.paid;

                //request.RequestStatus = _context.Request.Find(request.RequestStatus)

                request.AdminComment = _context.Request.FindAsync(id).Result.AdminComment;
                request.RequestStatus = _context.Request.FindAsync(id).Result.RequestStatus;
                request.Cost = _context.Request.FindAsync(id).Result.Cost;
                request.CostDetails = _context.Request.FindAsync(id).Result.CostDetails;
                request.PhotoPath = _context.Request.FindAsync(id).Result.PhotoPath;
                request.VideoPath = _context.Request.FindAsync(id).Result.VideoPath;
                request.CancellationRemark = _context.Request.FindAsync(id).Result.CancellationRemark;
                var profile = _userManager.FindByIdAsync(request.ApplicationUserId).Result;
                var email = profile.Email;
                var adminEmail = "admin@webvestlimited.com";

                _context.Update(request);
                await _context.SaveChangesAsync();

                var callbackUrl = Url.Action("Details", "UserRequest", new { id = request.RequestId },
       protocol: HttpContext.Request.Scheme);
                await _emailSend.SendEmailAsync(email, "Payment Confirmation",
                    $"Dear " + profile.FirstName +
                    $" <br />" +
                    $" The service payemnt for your request has been confirmed" +
                    $" <br />" +
                    $"click the button below to check the details and cost" +
                    $" <br /> " +
                    $" <br /> " +
                    $" <br /> " +
                    $" <a style=\"  text-decoration: none; color: #fff; background: #101522; padding: 7px 22px; margin-right: 10px; border-radius: 50px; border: 2px solid #f82249;font-weight: 600; margin-left: 8px; margin-top: 2px; margin-bottom: 2px; line-height: 1.5;font-size: 13px;\" href ='{callbackUrl}'>Check My Request</a>" +
                    $" <br />" +
                    $" " +
                    $" <br />"
                                    );
                var admincallbackUrl = Url.Action("Edit", "Request", new { id = request.RequestId },
                protocol: HttpContext.Request.Scheme);
                await _emailSend.SendEmailAsync(adminEmail, "Payment Confirmation",

             $"  Request from  " + request.CustomerName +
             $"has been confirmed paid" +
             $" <br />" +
             $"click the button below to check the details of the request" +
             $" <br /> " +
             $" <br /> " +
             $" <br /> " +
             $" <a style=\"  text-decoration: none; color: #fff; background: #101522; padding: 7px 22px; margin-right: 10px; border-radius: 50px; border: 2px solid #f82249;font-weight: 600; margin-left: 8px; margin-top: 2px; margin-bottom: 2px; line-height: 1.5;font-size: 13px;\" href ='{admincallbackUrl}'>Check Request</a>" +
             $" <br />" +
             $" " +
             $" <br />"
                     );


                return RedirectToAction("Index");
            }
            else
            return RedirectToAction("Index");

        }
            public async Task<ActionResult> ReportByDate()
        {
            IQueryable<ReportByDateViewModel> data =
                from requests in _context.Request
                group requests by (requests.ScheduleTime.Date) into dateGroup
                select new ReportByDateViewModel()
                {
                    ScheduleTime = dateGroup.Key,
                    CustomerCount = dateGroup.Count()
                    
                };
            return View(await data.AsNoTracking().ToListAsync());
        }

        public async Task<ActionResult> ReportByService()
        {
            IQueryable<ReportByServiceViewModel> data =
                from requests in _context.Request
                group requests by (requests.NeededService) into dateGroup
                select new ReportByServiceViewModel()
                {
                    NeededService = dateGroup.Key.ToString(),
                    CustomerCount = dateGroup.Count()

                };
            return View(await data.AsNoTracking().ToListAsync());
        }

    }
}
