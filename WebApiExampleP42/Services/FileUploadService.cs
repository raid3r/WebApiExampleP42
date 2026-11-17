namespace WebApiExampleP42.Services;

public class FileUploadService(IWebHostEnvironment webHostEnvironment)
{

    
    public async Task<string> UploadFileAsync(IFormFile file)
    {
        // перевірки...

        // визначаємо шлях до папки для збереження файлів
        var uploadDirPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads");

        // з якою назвою зберігати файл?
        var uniqueFileName = GenerateUniqueFileName(file.FileName);
        var filePath = Path.Combine(uploadDirPath, uniqueFileName); // С:\Users\kvvkv\source\repos\WebApiExampleP42\WebApiExampleP42\wwwroot\uploads\123e4567-e89b-12d3-a456-426614174000.jpg

        // зберігаємо файл
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return uniqueFileName;
    }

    private string GenerateUniqueFileName(string originalFileName)
    {
        // <img src="/uploads/123e4567-e89b-12d3-a456-426614174000.jpg" />
        // dev.jpg -> 123e4567-e89b-12d3-a456-426614174000.jpg
        var extension = Path.GetExtension(originalFileName);
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        return uniqueFileName;
    }

}
