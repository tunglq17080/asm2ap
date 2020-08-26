using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Quanli.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Quanli.Controllers
{
    [Authorize(Roles = "Training")]
    public class TrainingController : Controller
    {
        private ApplicationDbContext _context;
      

        public TrainingController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Training
        public ActionResult Index()
        {
            var httpContext = System.Web.HttpContext.Current;
            var user = httpContext.User.Identity.Name;
            var takeTraining = _context.Trainings.SingleOrDefault(c =>c.Email == user);
            if (takeTraining == null)
                return RedirectToAction("TrainingForm", new RouteValueDictionary(new { Controller = "Training", Action = "TrainingForm", email = user }));
            else
            
            return View(takeTraining);
        }
        public ActionResult TrainingForm(string email)
        {
            if (email == null)
            {
                return HttpNotFound();
            }
                var traingInfor = new Training() {Email = email};
                return View(traingInfor);
            
        }
        [HttpPost]
        public ActionResult Save(Training training)
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
            return RedirectToAction("Index","Training");
        }

    }
}