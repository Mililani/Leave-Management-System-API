using LeaveManagmentAPI.DTO;
using LeaveManagmentAPI.Models;
using LeaveManagmentAPI.Models.Data;
using LeaveManagmentAPI.PasHashs;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;

namespace LeaveManagmentAPI.Controllers
{
    public class AddDummyUserController : Controller
    {
        private readonly DataCont _dBcontext;
        private IConfiguration _config;
        public AddDummyUserController(DataCont dBcontext, IConfiguration config)
        {
            _dBcontext = dBcontext;
            _config = config;
        }




        [HttpPost("CreateNew-User")]
        public async Task<IActionResult> CreateUser(AddDummyUserDTO addDummyUserDTO)
        {

            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse("576780@student.belgiumcampus.ac.za"));
            email.To.Add(MailboxAddress.Parse(addDummyUserDTO.Email));

            email.Subject = "Wishlist Password";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = addDummyUserDTO.Password };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.office365.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("576780@student.belgiumcampus.ac.za", "Koketso123");
            smtp.Send(email);
            smtp.Disconnect(true);



            if (_dBcontext.Users.Any(u => u.Email == addDummyUserDTO.Email))
            {
                return BadRequest("User already exists");
            }



            var user = new User
            {
                Email = addDummyUserDTO.Email,
                Name = addDummyUserDTO.Name,
                Role = addDummyUserDTO.Role,
                Password = MyPasswordHash.hashPassword(addDummyUserDTO.Password),

            };

            _dBcontext.Users.Add(user);
            await _dBcontext.SaveChangesAsync();
            return Ok("Creation Successfully mail sent");

        }
    }
}
