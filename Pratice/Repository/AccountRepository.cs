
using Devart.Data.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pratice.Model;
using PraticelinqDatabseContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Pratice.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _configuration;

        public AccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable> SignUPAsync(userModel u1)
        {
            PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext();
            User1 user1 = new User1();
            Role role = new Role();var nm = u1.RName;
            var query = (from user in context.User1s 
                         join r1 in context.Roles 
                         on user.RoleID  equals r1.RoleID 
                         where user.UserName == u1.UserName && user.Password ==u1.Password && r1.RName == nm 
                         select new
                         {
                            r1.RName 
                         }).Count ();
            if (query != 0)
            {
                return "ALready Exist";
            }
            else
            {
                if (nm == "Admin")
                {
                    user1.UserName = u1.UserName;
                    user1.Password = u1.Password;
                    user1.RoleID = 1;
                    context.User1s.InsertOnSubmit(user1);
                    context.SubmitChanges();
                    return $"{u1.UserName}  Register as Admin Successfully";
                }
                else if (nm == "User")
                {
                    user1.UserName = u1.UserName;
                    user1.Password = u1.Password;
                    user1.RoleID = 2;
                    context.User1s.InsertOnSubmit(user1);
                    context.SubmitChanges();
                    return $"{u1.UserName}  Register as User Successfully";
                }
                else
                {
                    return "Invalid Role Name Enter valid RoleName";
                }
            }
        }

        public async Task<string> LoginAsync(userModel u1)
        {
            
            PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext();
            User1 user1 = new User1();
            var res1 = (from i in context.User1s
                        where i.UserName == u1.UserName && i.Password == u1.Password
                        select new
                        {
                            RoleId = i.RoleID,
                            UserId = i.UserID
                        }).FirstOrDefault();

            if (res1 != null)
            {
                var authclaims = new List<Claim>
                {
                    new Claim (ClaimTypes .Name ,u1.UserName),
                    new Claim (ClaimTypes .Name  ,u1.Password),
                    new Claim (ClaimTypes .Role ,res1 .RoleId.ToString()),
                    new Claim (ClaimTypes .Sid  ,res1 .UserId.ToString()),
                new Claim (JwtRegisteredClaimNames.Jti,Guid .NewGuid().ToString()),
                };
                var authSignKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authclaims,
                    signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256Signature)
                  );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                return "Please Enter valid login detail";
            }

        }

        public async Task<IEnumerable> GetAllUserDetail()
        {
            PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext();
            User1 user1 = new User1();
            var r2 = (from r3 in context.User1s
                      select r3);
            for (int i = 0; i < r2.Count(); i++)
            {
                var result = (from r1 in context.User1s
                              select new
                              {
                                  UserID = r1.UserID,
                                  UserName = r1.UserName,
                                  RoleID = r1.RoleID
                              }
                         ).ToList();
                return result;
            }
            return "Not Found Data";
        }
    } 
    



}
