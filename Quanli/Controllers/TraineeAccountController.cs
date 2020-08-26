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
    public class TraineeAccountController : Controller
    {
        private ApplicationDbContext _context;
       
        //public string RoleStype;
        public TraineeAccountController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public TraineeAccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
                    await roleManager.CreateAsync(new IdentityRole("Trainee"));
                    await AdminUserManager.AddToRolesAsync(user.Id, "Trainee");

                    //await AdminSignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);



                    return RedirectToAction("InformationForm", new RouteValueDictionary(new { Controller = "TraineeAccount", Action = "InformationForm", email = model.Email }));
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
        // GET: TraineeAccount
        public ActionResult Index()
        {
            var trainee = _context.Trainees.ToList();
            return View(trainee);
        }
        public ActionResult InformationForm(string email)
        {
            if (email == null)
            {
                return HttpNotFound();
            }
            var traineeInfor = new Trainee() { Email = email };

            return View(traineeInfor);
        }
        [HttpPost]
        public ActionResult Save(Trainee trainee)
        {
            if (trainee.Id == 0)
                _context.Trainees.Add(trainee);
            else
            {
                var customerInDb = _context.Trainees.Single(c => c.Id == trainee.Id);
                customerInDb.Name = trainee.Name;
                customerInDb.DOB = trainee.DOB;
                customerInDb.TOEIC = trainee.TOEIC;
            }
            _context.SaveChanges();
            var getId = _context.Trainees.ToList();
            int max = 0;
            foreach (var i in getId)
            {
                if (max < i.Id)
                    max = i.Id;
            }
            return RedirectToAction("AddCourseForm", new RouteValueDictionary(new { Controller = "TraineeAccount", Action = "AddCourseForm", id = max }));
        }
        public ActionResult CourseInfor(int id)
        {
            var courseInforList = _context.CourseToTrainees.Include(c => c.SubjectToCourse).Where(c => c.TraineeId == id);
            ArrayList getId = new ArrayList();
            foreach (var i in courseInforList)
            {
                getId.Add(i.SubjectToCourse.Id);
            }
            ViewBag.subjectToCourseId = getId;
            ViewBag.traineeId = id;
            var list = _context.SubjectToCourses.Include(c => c.Course).Include(c => c.Subject).ToList();
            return View(list);
        }
        public ActionResult AddCourseForm(int id)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }
            ViewBag.traineeId = id;
            var list = _context.SubjectToCourses.Include(c => c.Course).Include(c => c.Subject).ToList();
            return View(list);
        }
        public ActionResult Add(int traineeId, int subjectToCourseId)
        {
            if (traineeId == 0 || subjectToCourseId == 0)
            {
                return HttpNotFound();
            }
            var add = new CourseToTrainee()
            {
                TraineeId = traineeId,
                SubjectToCourseId = subjectToCourseId,
            };
            _context.CourseToTrainees.Add(add);
            _context.SaveChanges();
            return RedirectToAction("Index", "TraineeAccount");
        }
    }
}