using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pratice.Model;
using Pratice.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdditionalController : ControllerBase
    {
        private readonly IAdditionalRepository _additionalRepository;

        public AdditionalController(IAdditionalRepository additionalRepository )
        {
            _additionalRepository = additionalRepository;
        }

        [HttpPost("Itotalbook")]
        [Authorize]
        public async Task <IActionResult> TotalIssue([FromBody] ReportModel reportModel)
        {
            int uid = (int)HttpContext.Items["UserId"];
            int rid = (int)HttpContext.Items["RoleId"];
            if(rid == 1)
            {
                var result = _additionalRepository.TotalIssueBook(reportModel, uid);
                return Ok(result.Result);
            }
            return Unauthorized("UnAuthorize aacess");
        }

        [HttpGet("searchbook")]
        [Authorize]
        public async Task<IActionResult> Searchdetail([FromBody] ReportModel reportModel)
        {
            int uid = (int)HttpContext.Items["UserId"];
            int rid = (int)HttpContext.Items["RoleId"];
            if(rid == 2)
            {
                var result = _additionalRepository.searchBook(reportModel, uid);
                return Ok(result.Result);
            }
            return Unauthorized("UnAuthorize aacess");
        }

        [HttpPost("getauthor")]
        public async Task<IActionResult> GetALlAuthor([FromBody] AuthorModel authorModel)
        {
            var result = await _additionalRepository.GetAllAUthorbookdetail(authorModel);
            return Ok(result);
        }
    }
}
