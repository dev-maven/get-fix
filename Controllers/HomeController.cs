using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Technicians.Models;
using Technicians.Services;
using Technicians.ViewModels;

namespace Technicians.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ProfileContext _context;
        private readonly IEmailSend _emailSend;


        public HomeController(ProfileContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailSend emailSend)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.roleManager = roleManager;
            _context = context;
             _emailSend = emailSend;
        }

        //[Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        //[Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole (CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Home");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        //[Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        //[Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
            };

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        //[Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

        }


        //[Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);
        }

        //[Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("Not Found");
            }

            for (int i =0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;
                if(model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                    {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else return RedirectToAction("EditRole", new { Id = roleId });
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId});
        }



        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {

            if (_signInManager.IsSignedIn(User) && User.IsInRole("User")) 
            {
                return RedirectToAction("Index", "UserRequest");

            }

            else if (_signInManager.IsSignedIn(User) && (User.IsInRole("Admin") || User.IsInRole("SuperAdmin")))
            {
                return RedirectToAction("Index", "Request");

            }

            else return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Username);



            if (ModelState.IsValid)
            {
                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    var loginResults = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);


                    var errors = ModelState.Values.SelectMany(v => v.Errors);



                    if (loginResults.Succeeded)
                    {
                        return RedirectToAction("RoleRedirection");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Incorrect Email Address or Password");
                        return View(model);
                    }

                }
                return View("EmailConfirmation");
             
            }
            return View(model);
        }

        public IActionResult RoleRedirection()
        {
            if (_signInManager.IsSignedIn(User) && User.IsInRole("User"))
            {
                return RedirectToAction("Index", "UserRequest");

            }

            else
                return RedirectToAction("Index", "Request");

            
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Users(string sortOrder, string currentFilter, string searchString, int? pageNumber)
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

            var customer = from s in _context.Users
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                customer = customer.Where(s => s.CustomerName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    customer = customer.OrderByDescending(s => s.CustomerName);
                    break;
                case "Date":
                    customer = customer.OrderBy(s => s.DateRegistered);
                    break;
                case "date_desc":
                    customer = customer.OrderByDescending(s => s.DateRegistered);
                    break;
                default:  // Name ascending 
                    customer = customer.OrderBy(s => s.CustomerName);
                    break;
            }


            int pageSize = 8;

            return View(await PaginatedList<ApplicationUser>.CreateAsync(customer.AsNoTracking(), pageNumber ?? 1, pageSize));

        }



        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string EmailAddress)
        {
            var user = await _userManager.FindByEmailAsync(EmailAddress);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {EmailAddress} is already in use.");

            }
        }



            public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
       {
            model.Membership = MembershipType.PAYG;
           

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                var identityUser = new ApplicationUser
                {
                    UserName = model.EmailAddress,
                    Email = model.EmailAddress,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Membership = model.Membership,
                    MyBalance = 0,
                    DateRegistered = DateTime.Now,
                    CustomerName = model.FirstName + " " + model.LastName,                     
                };

                var identityResults = await _userManager.CreateAsync(identityUser, model.Password);
                if (identityResults.Succeeded)
                {

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    var callbackUrl = Url.Action("ConfirmEmail", "Home", new { userId = identityUser.Id, code = code },
                     protocol: HttpContext.Request.Scheme);
                    await _emailSend.SendEmailAsync(model.EmailAddress, "Confirm Account",
                        $"Welcome " + model.FirstName +
                        $" <br />" +
                        $" Thanks for Registering on Technicians.ng" +
                        $" <br />" +
                        $"Please Confirm your account by " +
                        $"clicking the button below" +
                        $" <br /> " +
                        $" <br /> " +
                        $" <br /> " +
                        $" <a style=\" text-decoration: none; color: #fff; background: #101522; padding: 7px 22px; margin-right: 10px; border-radius: 50px; border: 2px solid #f82249;font-weight: 600; margin-left: 8px; margin-top: 2px; margin-bottom: 2px; line-height: 1.5;font-size: 13px;\" href ='{callbackUrl}'>Confirm my account</a>" +
                        $" <br />" +
                        $" " +
                        $" <br />" 
                                        );


                    await _userManager.AddToRoleAsync(identityUser, "User");
                    await _signInManager.SignInAsync(identityUser, isPersistent: false);

                    return View("EmailConfirmation");
                }

                foreach (var error in identityResults.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error in creating user");
                return View(model);
            }

            return View(model);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult> ReportByDate()
        {
            IQueryable<UserReportViewModel> data =
                from users in _context.Users
                group users by (users.DateRegistered.Date) into dateGroup
                select new UserReportViewModel()
                {
                    DateRegistered = dateGroup.Key,
                    CustomerCount = dateGroup.Count()

                };
            return View(await data.AsNoTracking().ToListAsync());
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        // GET: Request/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
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

        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ProfileEditViewModel model)
        {


            if (ModelState.IsValid)
            {
                var prevBal = _context.Users.FindAsync(id).Result.MyBalance;
                var phone = _context.Users.FindAsync(id).Result.PhoneNumber;
                var name = _context.Users.FindAsync(id).Result.CustomerName;
                var address = _context.Users.FindAsync(id).Result.Address;
                var date = _context.Users.FindAsync(id).Result.DateRegistered;
                var email = _context.Users.FindAsync(id).Result.Email;
                var fName = _context.Users.FindAsync(id).Result.FirstName;
                var lName = _context.Users.FindAsync(id).Result.LastName;
                var myId = _context.Users.FindAsync(id).Result.Id;
                var password = _context.Users.FindAsync(id).Result.PasswordHash;
                var features = _context.Users.FindAsync(id).Result.Features;
                var property = _context.Users.FindAsync(id).Result.PropertyType;

                var profile = await _context.Users.FindAsync(id);
                profile.Id = myId;
                profile.CustomerName = name;
                profile.FirstName = fName;
                profile.LastName = lName;
                profile.Address = address;
                profile.MyBalance = model.newBal + prevBal;
                profile.PhoneNumber = phone;
                profile.Features = model.Features;
                profile.PropertyType = property;
                profile.PasswordHash = password;
                profile.Membership = model.Membership;
                profile.Email = email;
                profile.DateRegistered = date;

                var admin = "admin@webvestlimited.com";

                if (profile.Membership == MembershipType.SUB)
                {
                    _context.Update(profile);
                    await _context.SaveChangesAsync();

                    var callbackUrl = Url.Action("Profile", "UserRequest", new { id = profile.Id},
                    protocol: HttpContext.Request.Scheme);
                    await _emailSend.SendEmailAsync(email, "Profile Updated",
                        $"Dear " + profile.FirstName +
                        $" <br />" +
                        $" Your Profile and Balance has been updated" +
                        $" <br />" +
                        $" As a Subscriber you can" +
                        $" <br />" +
                        $"click the button below to check your details and balance" +
                        $" <br /> " +
                        $" <br /> " +
                        $" <br /> " +
                        $" <a style=\"  text-decoration: none; color: #fff; background: #101522; padding: 7px 22px; margin-right: 10px; border-radius: 50px; border: 2px solid #f82249;font-weight: 600; margin-left: 8px; margin-top: 2px; margin-bottom: 2px; line-height: 1.5;font-size: 13px;\" href ='{callbackUrl}'>Check My Profile</a>" +
                        $" <br />" +
                        $" " +
                        $" <br />"
                                        );
                    var admincallbackUrl = Url.Action("Edit", "Home", new { id = profile.Id },
                    protocol: HttpContext.Request.Scheme);
                    await _emailSend.SendEmailAsync(admin, "Profile Updated",
                    profile.CustomerName +
                 $"'s has been updated" +
                 $" <br />" +
                 $"click the button below to check the profile details" +
                 $" <br /> " +
                 $" <br /> " +
                 $" <br /> " +
                 $" <a style=\"  text-decoration: none; color: #fff; background: #101522; padding: 7px 22px; margin-right: 10px; border-radius: 50px; border: 2px solid #f82249;font-weight: 600; margin-left: 8px; margin-top: 2px; margin-bottom: 2px; line-height: 1.5;font-size: 13px;\" href ='{admincallbackUrl}'>Check Profile</a>" +
                 $" <br />" +
                 $" " +
                 $" <br />"
                         );



                    return RedirectToAction("Users");
                }
                ModelState.AddModelError(string.Empty, "Can't Top up Balance for PAYG");
                return View(model);

            }

            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View("ConfirmEmail");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.EmailAddress);
                if (user == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "UserRequest");
                }
            }
            return View(model);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.EmailAddress);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return View("ForgotPasswordConfirmation");
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Home", new { userId = user.Id, code = code },
                                    protocol: HttpContext.Request.Scheme);
                await _emailSend.SendEmailAsync(model.EmailAddress, "Reset Password",
                        $"Please Reset your password by " +
                        $"clicking the button below" +
                        $" <br /> " +
                        $" <br /> " +
                        $" <br /> " +
                        $" <a style=\" text-decoration: none; color: #fff; background: #101522; padding: 7px 22px; margin-right: 10px; border-radius: 50px; border: 2px solid #f82249;font-weight: 600; margin-left: 8px; margin-top: 2px; margin-bottom: 2px; line-height: 1.5;font-size: 13px;\" href ='{callbackUrl}'>Reset Password</a>" +
                        $" <br />" +
                        $" " +
                        $" <br />"
                                        );
                return View("PasswordReset");
            }
           else {
                ModelState.AddModelError(string.Empty, "Email Address required");
                return View(model);
            }


            return View(model);
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}