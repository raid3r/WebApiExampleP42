namespace WebApiExampleP42.Models;

public class ImageFile
{
    public int Id { get; set; }
    public string OriginalFileName { get; set; }
    public string FileName { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
