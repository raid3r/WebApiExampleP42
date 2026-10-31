namespace WebApiExampleP42.Models.DTO;

public class UserViewDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public int RoleId { get; set; }
}
