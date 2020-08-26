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
using System.Data.Entity;
using System.Collections;


namespace Quanli.Controllers
{
    public class ManangerAccountController : Controller
    {
        private ApplicationDbContext _context;
        
        //public string RoleStype;
        public ManangerAccountController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ManangerAccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
                    await roleManager.CreateAsync(new IdentityRole("Trainer"));
                    await AdminUserManager.AddToRolesAsync(user.Id, "Trainer");

                    //await AdminSignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);



                    return RedirectToAction("InformationForm", new RouteValueDictionary(new { Controller = "ManangerAccount", Action = "InformationForm", email = model.Email }));
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
        // GET: ManangerAccount
        public ActionResult Index()
        {
            var trainer = _context.Trainers.ToList();
            return View(trainer);
        }

        public ActionResult InformationForm(string email)
        {
            if(email == null)
            {
                return HttpNotFound();
            }
            var trainerInfor = new Trainer() { Email = email };

            return View(trainerInfor);
        }

        [HttpPost]
        public ActionResult Save(Trainer trainer)
        {
            if (trainer.Id == 0)
                _context.Trainers.Add(trainer);
            else
            {
                var customerInDb = _context.Trainers.Single(c => c.Id == trainer.Id);
                customerInDb.Name = trainer.Name;
                customerInDb.BOD = trainer.BOD;
            }
            _context.SaveChanges();
            var getId = _context.Trainers.ToList();
            int max = 0;
            foreach (var i in getId)
            {
                if (max < i.Id)
                    max = i.Id;
            }
            return RedirectToAction("AddCourseForm", new RouteValueDictionary(new { Controller = "ManangerAccount", Action = "AddCourseForm", id = max }));
        }
        public ActionResult CourseInfor(int id)
        {
            var courseInforList = _context.CourseToTrainers.Include(c => c.SubjectToCourse).Where(c => c.TrainerId == id);
            ArrayList getId = new ArrayList();
            foreach (var i in courseInforList)
            {
                getId.Add(i.SubjectToCourse.Id);
            }
            ViewBag.subjectToCourseId = getId;
            ViewBag.trainerId = id;
            var list = _context.SubjectToCourses.Include(c => c.Course).Include(c => c.Subject).ToList();
            return View(list);
        }
        public ActionResult AddCourseForm(int id)
        {
            if(id == 0)
            {
                return HttpNotFound();
            }
            ViewBag.trainerId = id;
            var list = _context.SubjectToCourses.Include(c => c.Course).Include(c => c.Subject).ToList();
            return View(list);
        }
        public ActionResult Add (int trainerId, int subjectToCourseId)
        {
            if(trainerId == 0||subjectToCourseId == 0)
            {
                return HttpNotFound();
            }
            var add = new CourseToTrainer()
            {
                TrainerId = trainerId,
                SubjectToCourseId = subjectToCourseId,
            };
            _context.CourseToTrainers.Add(add);
            _context.SaveChanges();
            return RedirectToAction("Index", "ManangerAccount");
        }
    }
}