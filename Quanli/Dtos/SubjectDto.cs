﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Quanli.Dtos
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string Information { get; set; }
    }
}