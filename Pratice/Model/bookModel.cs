using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pratice.Model
{
    public class bookModel
    {
            
        [Required] public string BookName { get; set; }
        [Required] public string Publisher { get; set; }
        [Required] public string Edition { get; set; }
        [Required] public int noofcopies { get; set; }
        [Required] public int price { get; set; }
        [Required] public string[]  nm { get; set; }
    }
}
