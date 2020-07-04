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
    [Authorize(Roles = "User, SuperAdmin")]
    public class UserRequestController : Controller
    {
        private readonly ProfileContext _context;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSend _emailSend;

        public UserRequestController(ProfileContext context, IHostingEnvironment hostingEnvironment,
            UserManager<ApplicationUser> userManager, IEmailSend emailSend)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _emailSend = emailSend;

        }

        

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            var id = _userManager.GetUserAsync(User).Result.Id;

            var valueGeneratedOnAdd = _context.Request.Where(u => u.ApplicationUserId.Equals(id));
            var myrequests = valueGeneratedOnAdd.Count();

            if (myrequests != 0)
            {

                var myRequests = valueGeneratedOnAdd.Count();
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

                var customer = from s in valueGeneratedOnAdd
                               select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    customer = customer.Where(s => s.MyService.Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        customer = customer.OrderByDescending(s => s.NeededService.ToString());
                        break;
                    case "Date":
                        customer = customer.OrderBy(s => s.ScheduleTime);
                        break;
                    case "date_desc":
                        customer = customer.OrderByDescending(s => s.ScheduleTime);
                        break;
                    default:  // Name ascending 
                        customer = customer.OrderBy(s => s.NeededService.ToString());
                        break;
                }


                int pageSize = 6;

                return View(await PaginatedList<Request>.CreateAsync(customer.AsNoTracking(), pageNumber ?? 1, pageSize));
            }

            return RedirectToAction(nameof(NoRequest));
        }

               
        public async Task<IActionResult> NoRequest()
        {
            return View();
        }


        public IActionResult Create()
        {
            return View();
        }
        // POST: Request/Create
        // To protect from overposting attacks, please enable the specific features you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (CreateRequestViewModel model)

        {
            if (model.ScheduleTime > DateTime.Now)
            {
                if (ModelState.IsValid)
                {
                    var firstName = _userManager.GetUserAsync(User).Result.FirstName;
                    var lastName = _userManager.GetUserAsync(User).Result.LastName;
                    var phone = _userManager.GetUserAsync(User).Result.PhoneNumber;
                    var FullName = firstName + " " + lastName;
                    //var FullName = "Akin Ishola";
                    var address = _userManager.GetUserAsync(User).Result.Address;
                    string imageFileName = FirstImageUploadedFile(model);
                    var id = _userManager.GetUserAsync(User).Result.Id;
                    var membership = _userManager.GetUserAsync(User).Result.Membership;
                    string firstVideoFileName = FirstVideoUploadedFile(model);
                    var email = _userManager.GetUserAsync(User).Result.Email;
                    var admin = "admin@webvestlimited.com";





                    Request newRequest = new Request
                    {

                        Address = address,
                        PhoneNumber = phone,
                        CustomerName = FullName,
                        NeededService = model.NeededService,
                        ServiceType = model.ServiceType,
                        ScheduleTime = model.ScheduleTime,
                        Comment = model.Comment,
                        PhotoPath = imageFileName,
                        ApplicationUserId = id,
                        MembershipType = membership,
                        RequestStatus = RequestStatus.pending,
                        RefNo = model.RefNo,
                        MyService = model.NeededService.ToString(),
                        VideoPath = firstVideoFileName,
                        PaymentStatus = PaymentStatus.unpaid


                    };
                    _context.Add(newRequest);
                    await _context.SaveChangesAsync();
                    var myid = newRequest.RequestId;
                    var callbackUrl = Url.Action("Details", "UserRequest", new { id = newRequest.RequestId },
                    protocol: HttpContext.Request.Scheme);
                    await _emailSend.SendEmailAsync(email, "Request Successful",
                        $"Dear " + firstName +
                        $" <br />" +
                        $" Your Request has been created" +
                        $" <br />" +
                        $"we will contact you shortly " +
                        $"click the button below to check the details and status of your request" +
                        $" <br /> " +
                        $" <br /> " +
                        $" <br /> " +
                        $" <a style=\"  text-decoration: none; color: #fff; background: #101522; padding: 7px 22px; margin-right: 10px; border-radius: 50px; border: 2px solid #f82249;font-weight: 600; margin-left: 8px; margin-top: 2px; margin-bottom: 2px; line-height: 1.5;font-size: 13px;\" href ='{callbackUrl}'>Check My Request</a>" +
                        $" <br />" +
                        $" " +
                        $" <br />"
                                        );
                    var admincallbackUrl = Url.Action("Edit", "Request", new { id = newRequest.RequestId },
                    protocol: HttpContext.Request.Scheme);
                    await _emailSend.SendEmailAsync(admin, "New Request",
               
                 $" New Request has been created by " + FullName + 
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


                    return RedirectToAction(nameof(Index));
                    //return RedirectToAction("details", new { id = newRequest.RequestId });
                }

            }

            ModelState.AddModelError(string.Empty, "Scheduled Time Invalid! ");
            return View(model);
        }

        public async Task<IActionResult> Profile(string id)
        {

            id = _userManager.GetUserAsync(User).Result.Id;

           var bal = _userManager.GetUserAsync(User).Result.MyBalance;

            if (id == null)
            {
                return NotFound();
            }


            var profile = await _context.Users.FindAsync(id);
            ProfileEditViewModel profileEditViewModel = new ProfileEditViewModel
            {
                RegisterId = profile.Id,
                Balance = profile.MyBalance,
                FirstName = profile.FirstName,
                Password = profile.PasswordHash,
                EmailAddress = profile.Email,
                LastName = profile.LastName,
                Membership = profile.Membership,
                PropertyType = profile.PropertyType,
                Address = profile.Address,
                Features = profile.Features,
                DateRegistered = profile.DateRegistered,
                PhoneNumber = profile.PhoneNumber,
                CustomerName = profile.CustomerName,
            };



            return View(profileEditViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(string id, ProfileEditViewModel model)
        {


            if (ModelState.IsValid)
            {

                var balance = _userManager.GetUserAsync(User).Result.MyBalance;

                var phone = _userManager.GetUserAsync(User).Result.PhoneNumber;
                var name = _userManager.GetUserAsync(User).Result.CustomerName;
                var address = _userManager.GetUserAsync(User).Result.Address;
                var date = _userManager.GetUserAsync(User).Result.DateRegistered;
                var email = _userManager.GetUserAsync(User).Result.Email;
                var fName = _userManager.GetUserAsync(User).Result.FirstName;
                var lName = _userManager.GetUserAsync(User).Result.LastName;
                var myId = _userManager.GetUserAsync(User).Result.Id;
                var password = _userManager.GetUserAsync(User).Result.PasswordHash;
                var features = _userManager.GetUserAsync(User).Result.Features;
                var property = _userManager.GetUserAsync(User).Result.PropertyType;
                var membership = _userManager.GetUserAsync(User).Result.Membership;
                var admin = "admin@webvestlimited.com";

                var profile = await _userManager.GetUserAsync(User);
                profile.Id = myId;
                profile.CustomerName = name;
                profile.FirstName = fName;
                profile.LastName = lName;
                profile.Address = address;
                profile.MyBalance = balance;
                profile.PhoneNumber = phone;
                profile.Features = features;
                profile.PropertyType = model.PropertyType;
                profile.PasswordHash = password;
                profile.Membership = membership;
                profile.Email = email;
                profile.DateRegistered = date;

                if (model.PropertyType != PropertyType.None)
                {
                    _context.Update(profile);
                    await _context.SaveChangesAsync();

                    var admincallbackUrl = Url.Action("Edit", "Home", new { id = profile.Id },
                     protocol: HttpContext.Request.Scheme);
                    await _emailSend.SendEmailAsync(admin, "User Details Updated",

                   profile.CustomerName +
                   $" Updated thier details" +
                 $" <br />" +
                 $"click the button below to check the users profile" +
                 $" <br /> " +
                 $" <br /> " +
                 $" <br /> " +
                 $" <a style=\"  text-decoration: none; color: #fff; background: #101522; padding: 7px 22px; margin-right: 10px; border-radius: 50px; border: 2px solid #f82249;font-weight: 600; margin-left: 8px; margin-top: 2px; margin-bottom: 2px; line-height: 1.5;font-size: 13px;\" href ='{admincallbackUrl}'>Check Profile</a>" +
                 $" <br />" +
                 $" " +
                 $" <br />"
                         );

                    return RedirectToAction("Profile");
                }
                ModelState.AddModelError(string.Empty, "PropertyType is required");
                return View(model);

            }

            return View(model);

        }


        public async Task<IActionResult> CancelRequest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var request = await _context.Request
                   .FirstOrDefaultAsync(m => m.RequestId == id);
            if (request == null)
            {
                return NotFound();
            }
            if (request.RequestStatus == RequestStatus.pending)
            {
               
                return View("CancelRequest");

            }
            else return View("Completed");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelRequest(int? id, RequestCancelViewModel model)
        {
            var myUser = _userManager.GetUserAsync(User).Result.Id;
            var email = _userManager.GetUserAsync(User).Result.Email;
            var admin = "admin@webvestlimited.com";
            var request = await _context.Request.FindAsync(id);
            if (myUser == request.ApplicationUserId)
            {
                if (ModelState.IsValid)
                {
                    var Userid = _context.Request.FindAsync(id).Result.ApplicationUserId;

                    var profile = _context.Users.FindAsync(Userid).Result;
                    request.CustomerName = profile.CustomerName;
                    request.Address = profile.Address;
                    request.PhoneNumber = profile.PhoneNumber;
                    request.MembershipType = profile.Membership;
                    request.RequestId = _context.Request.FindAsync(id).Result.RequestId;
                    request.ServiceType = _context.Request.FindAsync(id).Result.ServiceType;
                    request.ScheduleTime = _context.Request.FindAsync(id).Result.ScheduleTime;
                    request.NeededService = _context.Request.FindAsync(id).Result.NeededService;
                    request.Comment = _context.Request.FindAsync(id).Result.Comment;
                    request.ApplicationUserId = Userid;
                    request.RefNo = _context.Request.FindAsync(id).Result.RefNo;
                    request.PaymentStatus = _context.Request.FindAsync(id).Result.PaymentStatus;
                    request.PhotoPath = _context.Request.FindAsync(id).Result.PhotoPath;
                    request.VideoPath = _context.Request.FindAsync(id).Result.VideoPath;
                    request.RequestStatus = RequestStatus.cancelled;
                    request.CancellationRemark = model.CancellationRemark;


                    _context.Update(request);
                    await _context.SaveChangesAsync();


                    var myid = request.RequestId;
                    var callbackUrl = Url.Action("Details", "UserRequest", new { id = request.RequestId },
                    protocol: HttpContext.Request.Scheme);
                    await _emailSend.SendEmailAsync(email, "Request Cancelled",
                        $"Dear " + _userManager.GetUserAsync(User).Result.FirstName +
                        $" <br />" +
                        $" Your Request has been cancelled" +
                        $" <br />" +
                        $"we will contact you shortly " +
                        $"click the button below to check the details and status of your request" +
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
                    await _emailSend.SendEmailAsync(admin, "Request Cancelled",

                 $"  Request from  " + request.CustomerName +
                 $" has been cancelled " +
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
                return View(model);
            }
            return NotFound();

        }



        // GET: Request/Details/5
        public async Task<IActionResult> Details(int? id)
        {
                if (id == null)
                {
                    return NotFound();
                }

                var request = await _context.Request
                    .FirstOrDefaultAsync(m => m.RequestId == id);
                if (request == null)
                {
                    return NotFound();
                }
                var Userid = _userManager.GetUserAsync(User).Result.Id;

            if (request.ApplicationUserId == Userid)
            {
                if (request.RequestStatus == RequestStatus.pending)



                    return View(request);

                else return View("CompletedDetails", request);
            }

            return NotFound();
           
        }

        // GET: Request/Create

        public IActionResult Completed()
        {
            return View();
        }

        private string FirstImageUploadedFile(CreateRequestViewModel model)
        {
            string imageFileName = null;
            if (model.Photo != null)
            {
                string uploadsfolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                imageFileName = Guid.NewGuid().ToString() + " _" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsfolder, imageFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }

            }

            return imageFileName;
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

        private string FirstVideoUploadedFile(CreateRequestViewModel model)
        {
            string firstVideoFileName = null;
            if (model.Video != null)
            {
                string uploadsfolder = Path.Combine(hostingEnvironment.WebRootPath, "videos");
                firstVideoFileName = Guid.NewGuid().ToString() + " _" + model.Video.FileName;
                string filePath = Path.Combine(uploadsfolder, firstVideoFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Video.CopyTo(fileStream);
                }

            }

            return firstVideoFileName;
        }

        private string VideoUploadedFile(RequestViewModel model)
        {
            string videoFileName = null;
            if (model.Video != null)
            {
                string uploadsfolder = Path.Combine(hostingEnvironment.WebRootPath, "videos");
                videoFileName = Guid.NewGuid().ToString() + " _" + model.Video.FileName;
                string filePath = Path.Combine(uploadsfolder, videoFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Video.CopyTo(fileStream);
                }

            }

            return videoFileName;
        }

        // GET: Request/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Userid = _userManager.GetUserAsync(User).Result.Id;
            var firstName = _userManager.GetUserAsync(User).Result.FirstName;
            var lastName = _userManager.GetUserAsync(User).Result.LastName;
            var phone = _userManager.GetUserAsync(User).Result.PhoneNumber;
            var FullName = firstName + " " + lastName;
            var address = _userManager.GetUserAsync(User).Result.Address;
            var request = await _context.Request.FindAsync(id);
            var membership = _userManager.GetUserAsync(User).Result.Membership.ToString();

            if (request.ApplicationUserId == Userid)
            {
                if (request.RequestStatus == RequestStatus.pending)
                {
                    RequestEditViewModel requestEditViewModel = new RequestEditViewModel
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
                        ApplicationUserId = Userid,
                        RefNo = request.RefNo,
                        MembershipType = request.MembershipType,
                        RequestStatus = request.RequestStatus,
                        MyService = request.MyService,
                        ExistingVideoPath = request.VideoPath,
                        PaymentStatus = request.PaymentStatus



                    };



                    return View(requestEditViewModel);

                }
                return RedirectToAction(nameof(Completed));

            }
            return NotFound();

        


            if (request == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: Request/Edit/5
        // To protect from overposting attacks, please enable the specific features you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RequestEditViewModel model)
        {
            var firstName = _userManager.GetUserAsync(User).Result.FirstName;
            var lastName = _userManager.GetUserAsync(User).Result.LastName;
            var phone = _userManager.GetUserAsync(User).Result.PhoneNumber;
            var FullName = firstName + " " + lastName;
            var address = _userManager.GetUserAsync(User).Result.Address;
            var Userid = _userManager.GetUserAsync(User).Result.Id;
            var email = _userManager.GetUserAsync(User).Result.Email;
            var membership = _userManager.GetUserAsync(User).Result.Membership;
            var admin = "admin@webvestlimited.com";
            if (model.ScheduleTime > DateTime.Now)
            {
                if (ModelState.IsValid)
                {
                    var request = await _context.Request.FindAsync(id);
                    request.CustomerName = FullName;
                    request.Address = address;
                    request.PhoneNumber = phone;
                    request.RequestId = model.Id;
                    request.ServiceType = model.ServiceType;
                    request.ScheduleTime = model.ScheduleTime;
                    request.NeededService = model.NeededService;
                    request.Comment = model.Comment;
                    request.ApplicationUserId = Userid;
                    request.RefNo = model.RefNo;
                    request.MembershipType = membership;
                    request.RequestStatus = model.RequestStatus;
                    request.MyService = model.NeededService.ToString();
                    request.PaymentStatus = model.PaymentStatus;
                    if (model.Photo != null)
                    {
                        if (model.ExistingPhotoPath != null)
                        {
                            string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                                  "images", model.ExistingPhotoPath);
                            System.IO.File.Delete(filePath);
                        }
                        request.PhotoPath = ProcessUploadedFile(model);
                    }

                    if (model.Video != null)
                    {
                        if (model.ExistingVideoPath != null)
                        {
                            string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                                  "videos", model.ExistingVideoPath);
                            System.IO.File.Delete(filePath);
                        }
                        request.VideoPath = VideoUploadedFile(model);
                    }

                    if(model.DeleteMedia == true)
                    {
                        request.VideoPath = null;
                        request.PhotoPath = null;
                    }

                    _context.Update(request);
                    await _context.SaveChangesAsync();


                    var myid = request.RequestId;
                    var callbackUrl = Url.Action("Details", "UserRequest", new { id = request.RequestId },
                    protocol: HttpContext.Request.Scheme);
                    await _emailSend.SendEmailAsync(email, "Request Update",
                        $"Dear " + _userManager.GetUserAsync(User).Result.FirstName +
                        $" <br />" +
                        $" Your Request has been updated" +
                        $" <br />" +
                        $"we will contact you shortly " +
                        $"click the button below to check the details and status of your request" +
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
                    await _emailSend.SendEmailAsync(admin, "Request Edited",

                 $"  Request from  " + request.CustomerName +
                 $"has been edited " +
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


                    return RedirectToAction("details", new { id = request.RequestId });
                }

            }
            ModelState.AddModelError(string.Empty, "Scheduled Time Invalid! ");
            return View(model);
        }
       

        private bool RequestExists(int id)
        {
            return _context.Request.Any(e => e.RequestId == id);
        }
    }
}
