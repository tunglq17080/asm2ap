using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Quanli.Models
{
    public class CourseToTrainee
    {
        public int Id { get; set; }
        public Trainee Trainee { get; set; }
        [Require]
        public int TraineeId { get; set; }
        public SubjectToCourse SubjectToCourse { get; set; }
        [Require]
        public int SubjectToCourseId { get; set; }
    }
}