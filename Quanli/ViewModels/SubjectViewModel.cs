using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quanli.Models;

namespace Quanli.ViewModels
{
    public class SubjectViewModel
    {
        public Subject Subject { get; set; }
        public Course Course { get; set; }
        public SubjectToCourse SubjectToCourse { get; set; }
    }
}