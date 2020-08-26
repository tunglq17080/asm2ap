using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Quanli.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Quanli.Controllers
{
    public class ViewCourseController : Controller
    {
        private ApplicationDbContext _context;
        

        public ViewCourseController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: ViewCourse
        public ActionResult Index()
        {
            var httpContext = System.Web.HttpContext.Current;
            var user = httpContext.User.Identity.Name;
            if (User.IsInRole("Trainer"))
            {
                var takeInfor = _context.Trainers.SingleOrDefault(c => c.Email == user);
                if (takeInfor == null)
                    return RedirectToAction("InformationForm", new RouteValueDictionary(new { Controller = "ManangerAccount", Action = "InformationForm", email = user }));
                else
                {
                    int id = takeInfor.Id;
                    var courseInforList = _context.CourseToTrainers.Include(c => c.SubjectToCourse).Where(c => c.TrainerId == id);
                    ArrayList getId = new ArrayList();
                    foreach (var i in courseInforList)
                    {
                        getId.Add(i.SubjectToCourse.Id);
                    }
                    ViewBag.subjectToCourseId = getId;
                    ViewBag.trainerId = id;
                    var list = _context.SubjectToCourses.Include(c => c.Course).Include(c => c.Subject).ToList();
                    return View("TrainerView", list);
                }
                    
            }
            if (User.IsInRole("Trainee"))
            {
                var takeInfor = _context.Trainees.SingleOrDefault(c => c.Email == user);
                if (takeInfor == null)
                    return RedirectToAction("InformationForm", new RouteValueDictionary(new { Controller = "TraineeAccount", Action = "InformationForm", email = user }));
                else
                {
                    var id = takeInfor.Id;
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
            }
            return View();
        }
    }
}