using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace Quanli.Models
{
    public class SubjectToCourse
    {
        [Key]
        public int Id { get; set; }

        public Subject Subject { get; set; }
        [Require]
        public int SubjectId { get; set; }
        public Course Course { get; set; }
        [Require]
        public int CourseId { get; set; }
    }
}