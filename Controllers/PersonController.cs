using AspNetCore5Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCore5Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PersonController :  ControllerBase
    {
        private readonly JwtSettings jwtSettings;
        public PersonController(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
        }
        private IEnumerable<Users> logins = new List<Users>() 
        {
            new Users() {
                    Id = Guid.NewGuid(),
                        EmailId = "adminakp@gmail.com",
                        UserName = "Admin",
                        Password = "Admin",
                },
                new Users() {
                    Id = Guid.NewGuid(),
                        EmailId = "adminakp@gmail.com",
                        UserName = "User1",
                        Password = "Admin",
                }
        };
        [HttpPost]
        public IActionResult GetToken(UserLogins userLogins)
        {
            try
            {
                var Token = new UserTokens();
                var Valid = logins.Any(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));
                if (Valid)
                {
                    var user = logins.FirstOrDefault(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));
                    Token = Helpers.JwtHelpers.GenTokenkey(new UserTokens()
                    {
                        EmailId = user.EmailId,
                        GuidId = Guid.NewGuid(),
                        UserName = user.UserName,
                        Id = user.Id,
                    }, jwtSettings);
                }
                else
                {
                    return BadRequest("wrong password");
                }
                return Ok(Token);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Get List of UserAccounts
        /// </summary>
        /// <returns>List Of UserAccounts</returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetList()
        {
            return Ok(logins);
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult List()
        {
            return new JsonResult (new []{ "张三", "李四", "王五" } );
        }
    }
}
