using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebApiExampleP42.Models;


public static class Roles
{
    public const int Admin = 1;
    public const int User = 2;
}

public class User
{
    //[ValidateNever]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; } // In a real application, passwords should be hashed and salted for security

     //[BindNever] // гарантує, що це поле не буде прив'язане з вхідними даними запиту
    public string Token { get; set; }
    
    public bool IsActive { get; set; }
    //[ValidateNever]
    //[Range(1, 2, ErrorMessage = "RoleId must be a positive integer.")]
    public int RoleId { get; set; } // Foreign key to Role

    //[Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    //public string ConfirmPassword { get; set; } // For registration, to confirm the password

    //[Url(ErrorMessage = "Invalid URL format.")]
    //public string SiteUrl { get; set; }


    //[RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid INN format.")]
    //public string INN { get; set; } // Tax Identification Number

}

/*
 * if (string.IsNullOrWhiteSpace(newUser.Name) || string.IsNullOrWhiteSpace(newUser.Email) || string.IsNullOrWhiteSpace(newUser.Password))
            {
                return BadRequest("Name, Email, and Password are required.");
            }



            if (!newUser.Email.Contains("@"))
            {
                return BadRequest("Invalid email format.");
            }
            if (newUser.Password.Length < 6)
            {
                return BadRequest("Password must be at least 6 characters long.");
            }
            if (newUser.Name.Length < 3)
            {
                return BadRequest("Name must be at least 3 characters long.");
            }
 */ 