using LeaveManagmentAPI.DTO;
using LeaveManagmentAPI.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Data;

namespace LeaveManagmentAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [EnableCors("myCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DataCont _db;

        public AdminController(DataCont db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("getUserInfor/{id}")]
        public async Task<IActionResult> GetUserInfo(int id)
        {
            try
            {
                if (id == null || id <= 0) { return BadRequest("User does not exist"); }
                var userInfo = _db.Users.Where(u => u.Id == id)
                        .Select(n => new UserViewDTO()
                        {
                            name = n.Name,
                            email = n.Email,

                            ListOfLeaves = n.LeaveRequests.Select(u => new leaves()
                            {
                                Reason = u.Reason,
                                StartDate = u.StartDate,
                                EndDate = u.EndDate,
                                Status = u.Status,

                            }).ToList()
                        }).FirstOrDefault();


                return Ok(userInfo);

            }
            catch (Exception erro)
            {
                return Ok(erro.Message);
            }
        }

        //update  status 
        [HttpPut]
        [Route("UpadteStatus/{id}")]
        public async Task<IActionResult> UpdateUserWishes(int id, AdminUpdateStatusDTO adminUpdateStatusDTO)
        {
            try
            {
                var dbstatus = _db.LeaveRequest.FirstOrDefault(n => n.Id == id);
                if (dbstatus == null)
                {
                    return BadRequest("can not be update");
                }

                dbstatus.Status = adminUpdateStatusDTO.Status;

                //check if our entity state has ben modified
                _db.Entry(dbstatus).State = EntityState.Modified;

                //save changes 
                await _db.SaveChangesAsync();

                return Ok("Wish has been succesfully updated");
            }
            catch (Exception error)
            {
                return Ok(error.Message);
            }
        }



        [HttpGet] 
        [Route("getAllLeaveRequest")]
        public async Task<IActionResult> getAllLeaveRequest()
        {
            try
            {

                var ListOfRequest = _db.LeaveRequest.Select(w => new
                {
                    w.Id,
                    w.UserId,
                    w.Reason,
                    w.StartDate,
                    w.EndDate,
                    w.Status,
                }).ToList();


                return Ok(ListOfRequest);

            }
            catch (Exception erro)
            {
                return Ok(erro.Message);
            }
        }


        [HttpGet]
        [Route("Statistics")]
        public async Task<IActionResult> Statistics()
        {
            try
            {

                var ListOfRequest =  new statsDTO
                {
                    TotalApprovedReq = _db.LeaveRequest.Count(l => l.Status == "Approved"),
                    TotalPendingReq = _db.LeaveRequest.Count(l => l.Status == "Pending"),
                    TotalRejectedReq = _db.LeaveRequest.Count(l => l.Status == "Rejected"),

                };


                return Ok(ListOfRequest);

            }
            catch (Exception erro)
            {
                return Ok(erro.Message);
            }
        }








    }
}
