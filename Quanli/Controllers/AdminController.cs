using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Quanli.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Routing;

namespace Quanli.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        //public string RoleStype;
        public AdminController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            AdminUserManager = userManager;
            AdminSignInManager = signInManager;
        }
        public ApplicationSignInManager AdminSignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager AdminUserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult AdminRegister()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AdminRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Role = model.Role };
                var result = await AdminUserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var roleStore = new RoleStore<IdentityRole>();
                    var roleManager = new RoleManager<IdentityRole>(roleStore);
                    await roleManager.CreateAsync(new IdentityRole(model.Role));
                    await AdminUserManager.AddToRolesAsync(user.Id, model.Role);

                    //await AdminSignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    

                    return RedirectToAction("InformationForm", new RouteValueDictionary(new {Controller ="Admin",Action ="InformationForm", email = model.Email }));
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            throw new NotImplementedException();
        }
        // GET: Admin
        public ActionResult Index()
        {
            var context = new IdentityDbContext();
            var user = context.Users.ToList();
            return View(user);
        }
        [AllowAnonymous]
        public ActionResult InformationForm(string email)
        {
            if(email == null)
            {
                return HttpNotFound();
            }

            var traingInfor = new Training() {Email = email };

            return View(traingInfor);
        }
        [HttpPost]
        public ActionResult Save (Training training)
        {
            if (training.Id == 0)
                _context.Trainings.Add(training);
            else
            {
                var customerInDb = _context.Trainings.Single(c => c.Id == training.Id);
                customerInDb.Name = training.Name;
                customerInDb.BirthDate = training.BirthDate;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}