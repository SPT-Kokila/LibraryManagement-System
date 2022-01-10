using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pratice.Model
{
    public class ReportModel
    {
        [Required] public string BookName { get; set; }
    }
}
