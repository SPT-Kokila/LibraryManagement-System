
using Devart.Data.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pratice.Model;
using Pratice.Repository;
using PraticelinqDatabseContext;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;



namespace Pratice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _account;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountRepository account, IConfiguration configuration)
        {
            _account = account;
            _configuration = configuration;
        }

        //Register Detail
        [HttpPost("signup")]
        public async Task <IActionResult> SignUP(userModel u1)
        {
            var r1 = await _account.SignUPAsync(u1);
            return Ok(r1);
        }

        //GetAllUserDetail
        [Authorize(Roles = "1")]
        [HttpGet("fetch")]
        public async Task <IActionResult > Getall()
        {
            var r1 = await _account.GetAllUserDetail();
            return Ok(r1);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(userModel u1)
        {
            var result = await _account.LoginAsync(u1);
            if(string .IsNullOrEmpty (result))
            {
                return Unauthorized();
            }
            return Ok(result);
        }

        //Code to decode token
        [HttpGet("gettoken")]
        public ActionResult<string> GetToken()
        {
            var isClaim = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals(ClaimTypes.Name, StringComparison.InvariantCultureIgnoreCase));
            if (isClaim != null)
            {
                var id = isClaim.Value;
                var name1 = User.Identity.Name;

                return Ok(name1);
            }
            else
            {
                return BadRequest("no");
            }
        }
    }
}

