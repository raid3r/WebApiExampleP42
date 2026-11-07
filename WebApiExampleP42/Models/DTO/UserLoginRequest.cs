using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApiExampleP42.Models.DTO;

[Description("DTO for user login request containing email and password.")]
public class UserLoginRequest
{
    [Description("The email address of the user attempting to log in. Example: 'user@example.com'")]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }
    
    [Description("The password of the user attempting to log in. Example: 'P@ssw0rd'")]
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}
