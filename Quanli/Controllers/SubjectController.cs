using Quanli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quanli.Controllers
{
    [Authorize(Roles = "Training")]
    public class SubjectController : Controller
    {        
        private ApplicationDbContext _context;
        public SubjectController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Subject

        public ActionResult ManageSubject()
        {
            return View();
        }
        public ActionResult SubjectForm()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Save(Subject subject)
        {
            if (subject.Id == 0)
            {
                _context.Subjects.Add(subject);
            }
            else
            {
                var subInDb = _context.Subjects.Single(c => c.Id == subject.Id);
                subInDb.name = subject.name;
                subInDb.Information = subject.Information;
            }
            _context.SaveChanges();
            return RedirectToAction("ManageSubject", "Subject");
        }
        public ActionResult Edit(int id)
        {
            var sub = _context.Subjects.SingleOrDefault(c => c.Id == id);
            if(sub == null)
            {
                return HttpNotFound();
            }
            return View("SubjectForm", sub);
        }

        public ActionResult Delete(int id)
        {
            var sub = _context.Subjects.SingleOrDefault(c => c.Id == id);
            if (sub == null)
                return HttpNotFound();
            else
            {
                _context.Subjects.Remove(sub);
                _context.SaveChanges();
                return RedirectToAction("ManageSubject", "Subject");
            }           
        }
    }
}