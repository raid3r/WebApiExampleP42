using System.ComponentModel.DataAnnotations;

namespace WebApiExampleP42.Models.DTO;

public class AddMessageRequest
{
    [Required(ErrorMessage = "The To field is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(100, ErrorMessage = "The To field must be a maximum of 100 characters.")]
    public string To { get; set; }
    [Required(ErrorMessage = "The From field is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string From { get; set; }
    [Required(ErrorMessage = "The Text field is required.")]
    [StringLength(500, ErrorMessage = "The Text field must be a maximum of 500 characters.")]
    public string Text { get; set; }
}



