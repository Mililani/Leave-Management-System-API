using LeaveManagmentAPI.DTO;
using LeaveManagmentAPI.Models.Data;
using LeaveManagmentAPI.PasHashs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace LeaveManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly DataCont _db;
        private IConfiguration _config;

        public LoginController(DataCont db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }





        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> UserLogin([FromBody] LoginDTO user)
        {

            String password = MyPasswordHash.hashPassword(user.Password);
            var log = await _db.Users.Where(i => i.Email == user.Email && i.Password == password).Select(u => new
            {
                u.Id,
                u.Email,
                u.Name,
                u.Role,
            }).FirstOrDefaultAsync();

            if (log == null)
            {
                return BadRequest("user email does not exsist");
            }

            //add Claims
            var tohan = new JwtSecurityTokenHandler();
            var secKey = Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]);
            var tokendand = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                 new Claim[]{
                     new Claim("userID",log.Id.ToString()),
                     new Claim(ClaimTypes.Name,log.Name),
                     new Claim(ClaimTypes.Email,log.Email),
                     new Claim(ClaimTypes.Role,log.Role)

                }),
                Expires = DateTime.Now.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secKey), SecurityAlgorithms.HmacSha256),
                Issuer = _config["JWT:ValidIssuer"],
                Audience = _config["JWT:ValidAudience"],

            };
            var token = tohan.CreateToken(tokendand);
            string finltoken;
            return Ok(new
            {
                finltoken = new JwtSecurityTokenHandler().WriteToken(token),
            });
        }



    }
}
