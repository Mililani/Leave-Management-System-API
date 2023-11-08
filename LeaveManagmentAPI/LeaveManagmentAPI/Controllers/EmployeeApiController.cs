using LeaveManagmentAPI.DTO;
using LeaveManagmentAPI.Models;
using LeaveManagmentAPI.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace LeaveManagmentAPI.Controllers
{
    [Authorize(Roles = "Employee")]
    [EnableCors("myCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {

        private readonly DataCont _db;

        public EmployeeApiController(DataCont db)
        {
            _db = db;
        }


        [HttpPost]
        [Route("RequestLeave")]
        public async Task<IActionResult> RequestLeave([FromBody] LeaveDTO leaveDTO)
        {
            try
            {

                //get the current user by ID using the claim
                int UserId = Convert.ToInt32(HttpContext.User.FindFirstValue("userID"));

                if (leaveDTO == null)
                {
                    return BadRequest("wish list can not return an empty wish");
                }
                if (UserId == null || UserId <= 0)
                {
                    return BadRequest("user not allowed to use this endpoint");
                }
                
                if (leaveDTO.StartDate >= leaveDTO.EndDate || leaveDTO.StartDate <DateTime.Now)
                {
                    return BadRequest("Invalid leave request date");
                }

                var overlap = _db.LeaveRequest.
                    Where(r => r.UserId == UserId && r.Status == "Pending" && r.StartDate <= leaveDTO.EndDate
                    && r.EndDate >= leaveDTO.StartDate).ToList();

                if(overlap.Count >0)
                {
                    return BadRequest("current leave request is overlapping with existing approved request(s)");
                }


                var Leave = new Leave
                {
                    UserId = UserId,
                    StartDate = leaveDTO.StartDate,
                    EndDate = leaveDTO.EndDate,
                    Reason = leaveDTO.Reason,
                    Status ="Pending"
                    
                };



                _db.LeaveRequest.Add(Leave);
                await _db.SaveChangesAsync();
                return Ok("Request was sent");

            }
            catch (Exception error)
            {
                return Ok(error.Message);
            }
        }

        [HttpGet]
        [Route("GetAllRequest")]
        public async Task<IActionResult> GetAllRequest()
        {
            try
            {

                int UserId = Convert.ToInt32(HttpContext.User.FindFirstValue("userID"));

                if (UserId == null || UserId <= 0)
                {
                    return BadRequest("user not allowed to use this endpoint");
                }
                var ListOfWishes = _db.LeaveRequest.Select(w => new
                {
                    w.Id,
                    w.UserId,
                    w.StartDate,
                    w.EndDate,
                    w.Reason,
                    w.Status,
                }).
                Where(w =>
                w.UserId == UserId).ToList();

                return Ok(ListOfWishes);

            }
            catch (Exception erro)
            {
                return Ok(erro.Message);
            }
        }





    }
}
