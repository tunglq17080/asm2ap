using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Quanli.Models;

namespace Quanli.Controllers.Api
{
    public class SubjectController : ApiController
    {
        private ApplicationDbContext _context;

        public SubjectController()
        {
            _context = new ApplicationDbContext();
        }
        //get/Api/Subject
        [HttpGet]
        public IEnumerable<Subject> Subjects()
        {
            return _context.Subjects.ToList();
        }

        public Subject GetSubject(int id)
        {
            var subject = _context.Subjects.SingleOrDefault(c => c.Id == id);
            if (subject == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return subject;
        }
        //post /api/subjects
        [HttpPost]
        public Subject CreateSubject(Subject subject)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            _context.Subjects.Add(subject);
            _context.SaveChanges();
            return subject;
        }        

        //pust /api/subject/1
        [HttpPut]
        public void UpdateSubject(int id, Subject subject)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            var subInDb = _context.Subjects.SingleOrDefault(c => c.Id == id);

            if (subInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            subInDb.name = subject.name;
            subInDb.Information = subject.Information;
            _context.SaveChanges();
        }

        //Delelte /api/subject/1
        [HttpDelete]
        public void DeleteSubject(int id)
        {
            var subInDb = _context.Subjects.SingleOrDefault(c => c.Id == id);
            if (subInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            _context.Subjects.Remove(subInDb);
            _context.SaveChanges();
        }
    }
}
