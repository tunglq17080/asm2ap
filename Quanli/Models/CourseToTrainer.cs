using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Quanli.Models
{
    public class CourseToTrainer
    {
        public int Id { get; set; }
        public Trainer Trainer { get; set; }
        [Require]//[Column(Order = 0)]
        public int TrainerId { get; set; }
        public SubjectToCourse SubjectToCourse { get; set; }
        [Require]//[Column(Order =1)]
        public int SubjectToCourseId { get; set; }
    }
}