using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Quanli.Models;

namespace Quanli.Controllers.Api
{
    public class CourseController : ApiController
    {
        /*private ApplicationDbContext _context;
        private bool c;

        public CourseController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        [HttpGet]
        public IEnumerable<Course> Courses()
        {
            return _context.Courses.ToList();
        }
        [HttpPost]
        public SubjectToCourse CreateNewCoures(SubjectToCourse newCourse)
        {
            var course = _context.Courses.Single(c => c.Id == newCourse.CourseId);
            var subjects = _context.Subjects.Where(m => newCourse.SubjectId.Contains(m.Id));
            foreach(var subject in subjects)
            {
                var add = new SubjectToCourse
                {
                    Course = course,
                    Subject = subject,
                };
                _context.SubjectToCourses.Add(add);
            }
            _context.SaveChanges();
            return newCourse;
        }*/
    }
}
