using System.ComponentModel.DataAnnotations;

namespace LeaveManagmentAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Role { get; set; }
        public string Password { get; set; }

        public ICollection<Leave> LeaveRequests { get; set; }
    }
}
