using Quanli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace Quanli.Controllers
{
    public class CourseController : Controller
    {
        private ApplicationDbContext _context;
       

        public CourseController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Course
        public ActionResult Index()
        {
            //var course = _context.SubjectToCourses.Include(c => c.Course).Include(c => c.Subject).ToList();
            var course = _context.Courses.ToList();
            return View(course);
        }
        public ActionResult CourseForm()
        {
            //var course = _context.SubjectToCourses.Find()
            return View();
        }
        public ActionResult Save(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
            var courseId = _context.Courses.ToList();
            int max = 0;
            foreach(var i in courseId)
            {
                if (max < i.Id)
                    max = i.Id;
            }
            ViewBag.CourseId = max;
            var subject = _context.Subjects.ToList();
            return View("SubjectToCourseForm", subject); //RedirectToAction("SubjectToCourseForm", new System.Web.Routing.RouteValueDictionary(new { Controller = "Course", Action = "SubjectToCourseForm", Id = max }));
        }
        public ActionResult subjectList(int id)
        {
            ViewBag.CourseId = id;
            var subList = _context.SubjectToCourses.Include(c =>c.Course).Include(c => c.Subject).Where(c => c.CourseId == id);
            return View(subList);
        }
        public ActionResult SubjectToCourseForm(int id)
        {
            ViewBag.CourseId = id;
            //var course = new SubjectToCourse() { CourseId = id };
            var subject = _context.Subjects.ToList();
            //ViewBag.without = _context.SubjectToCourses.Include(c => c.Subject).Include(c => c.Course).Where(c => c.CourseId == id);
            return View(subject);
        }

        public ActionResult Add(int courseId, int subjectId)
        {

            //
            var add = new SubjectToCourse()
            {
                CourseId = courseId,
                SubjectId = subjectId,
            };
            _context.SubjectToCourses.Add(add);
            _context.SaveChanges();
            return RedirectToAction("Index", "Course");
        }
    }
}