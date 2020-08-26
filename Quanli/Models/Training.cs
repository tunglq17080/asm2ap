using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Quanli.Models;

namespace Quanli.Models
{
    public class Training
    {
        [Require]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }

        //[Require]
        
        /*[Require]
        [StringLength(128)]
        public string Email { get; set; }

        [ForeignKey("Email")]
        public RegisterViewModel RegisterViewModel { get; set; }*/

        
    }
}