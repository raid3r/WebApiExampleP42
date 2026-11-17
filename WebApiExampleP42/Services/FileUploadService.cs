using WebApiExampleP42.Models;

namespace WebApiExampleP42.Services;

public class FileUploadService(ApiDbContext dbContext, IWebHostEnvironment webHostEnvironment)
{

    public async Task DeleteImageFileAsync(ImageFile imageFile)
    {
        // видаляємо файл з диску
        var filePath = Path.Combine(webHostEnvironment.WebRootPath, "uploads", imageFile.FileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        // видаляємо запис з бази даних
        dbContext.Images.Remove(imageFile);
        await dbContext.SaveChangesAsync();
    }


    public async Task<ImageFile> UploadFileAsync(IFormFile file)
    {
        // перевірки...
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new InvalidOperationException("Invalid file type. Only image files are allowed.");
        }

        var maxFileSize = 5 * 1024 * 1024; // 5 MB        
        if (file.Length > maxFileSize)
        {
            throw new InvalidOperationException("File size exceeds the maximum limit of 5 MB.");
        }

        if (file.Length == 0)
        {
            throw new InvalidOperationException("File is empty.");
        }



        // визначаємо шлях до папки для збереження файлів
        var uploadDirPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads");

        // з якою назвою зберігати файл?
        var uniqueFileName = GenerateUniqueFileName(file.FileName);
        var filePath = Path.Combine(uploadDirPath, uniqueFileName); // С:\Users\kvvkv\source\repos\WebApiExampleP42\WebApiExampleP42\wwwroot\uploads\123e4567-e89b-12d3-a456-426614174000.jpg

        // https://site.com/uploads/123e4567-e89b-12d3-a456-426614174000.exe

        // зберігаємо файл
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // зберігаємо інформацію про файл у базі даних
        var imageFile = new ImageFile
        {
            OriginalFileName = file.FileName,
            FileName = uniqueFileName,
            UploadedAt = DateTime.UtcNow
        };
        await dbContext.Images.AddAsync(imageFile);
        await dbContext.SaveChangesAsync();

        return imageFile;
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
