using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebAPI.DataBase;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("authenticate")]
        public IActionResult GetToken(UserInfo userInfo)
        {
            var user = ExistingUsers.RegisteredUsers.Where(x => x.username == userInfo.username).SingleOrDefault();
            bool authorized = user != null && string.Equals(ExistingUsers.UserPasswords.GetValueOrDefault(user.username), userInfo.password);

            if (user != null && authorized)
            {
                Random jollyGenerator = new Random();
                int jolly = jollyGenerator.Next(1, 31);

                var claims = new[]
                {
                      new Claim(JwtRegisteredClaimNames.Sub, ""),
                      new Claim(JwtRegisteredClaimNames.Email, ""),
                      new Claim(JwtRegisteredClaimNames.Birthdate, DateTime.Now.ToString()),
                      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                      new Claim("Jolly", jolly.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                  _config["Jwt:Issuer"],
                  _config["Jwt:Issuer"],
                  claims,
                  expires: DateTime.Now.AddMinutes(30),
                  signingCredentials: creds);

                
                user.token = new JwtSecurityTokenHandler().WriteToken(token);
                user.jolly = jolly;

                return Ok(user);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}