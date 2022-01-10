using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pratice.Model
{
    public class AuthorModel
    {
        [Required]
        public string  Authorname { get; set; }
    }
}
