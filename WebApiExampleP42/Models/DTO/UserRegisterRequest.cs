using System.ComponentModel.DataAnnotations;

namespace WebApiExampleP42.Models.DTO;

/// <summary>
/// DTO for user registration request containing name, email, and password.
/// </summary>
public class UserRegisterRequest
{
    /// <summary>
    /// User name. Example: User Name
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    //[StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; }


    /// <summary>
    /// User email
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    /// <summary>
    /// User password. Min 6 chars.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } // In a real application, passwords should be hashed and salted for security
}
