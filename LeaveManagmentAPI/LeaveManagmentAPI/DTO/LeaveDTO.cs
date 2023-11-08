using System.ComponentModel.DataAnnotations;

namespace LeaveManagmentAPI.DTO
{
    public class LeaveDTO
    {
        [Required(ErrorMessage = " StartDate is Required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is Required")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Reason for application of Leave is Required")]
        public string Reason { get; set; }

       
        public string Status { get; set; }
    }
}
