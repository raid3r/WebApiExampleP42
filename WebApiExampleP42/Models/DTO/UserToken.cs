using System.ComponentModel;

namespace WebApiExampleP42.Models.DTO;

[Description("DTO for returning a user token after successful authentication or registration.")]
public class UserToken
{
    [Description("The token string used for authenticating the user. Example: '12345-abcdef-67890-12345-12435'")]
    public string Token { get; set; }
}
