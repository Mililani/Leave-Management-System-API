using LeaveManagmentAPI.Models;

namespace LeaveManagmentAPI.DTO
{
    public class UserViewDTO
    {
        public String name { get; set; } = string.Empty;
        public String email { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;

        public List<leaves> ListOfLeaves { get; set; }
        

    }

    public class leaves
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }
}
