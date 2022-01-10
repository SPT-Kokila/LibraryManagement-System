using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pratice.Model
{
    public class userModel
    {
        [Required(ErrorMessage = "UserName required")]
        public string  UserName { get; set; }
      
        [RegularExpression (@"[A-Z][a-z0-9@#_]{6,}[a-z0-9]$",ErrorMessage = "Minimum 8 character," +
            "Start first letter uppercase,atleast one lower case,one number and one special symbol")]
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
        public string RName { get; set; }
    }

    
}
