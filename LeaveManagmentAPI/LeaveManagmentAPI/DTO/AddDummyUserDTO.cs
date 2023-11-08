using LeaveManagmentAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagmentAPI.DTO
{
    public class AddDummyUserDTO
    {
        [Required(ErrorMessage = "Enter Name")]
        public string Name { get; set; }

        [Required, EmailAddress(ErrorMessage = "Please Enter a valid Email Addres")]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z0-9@*#]{8,15})$", ErrorMessage = "Password must contain \n " +
            "Minimum 8  characters atleast 1 UpperCase Alphbet \n 1 lowerCase Alphabet \n 1 Number and 1 Special Character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public AddDummyUserDTO(User user)
        {
            Name = user.Name;
            Email = user.Email;
            Role = user.Role;
        }
        public AddDummyUserDTO()
        {

        }

    }
}
