using System.ComponentModel.DataAnnotations;

namespace LeaveManagmentAPI.DTO
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; } = String.Empty;
        [Required]
        public string Password { get; set; } = String.Empty;

    }
}
