namespace OprawaObrazow.Shared.Services;

public interface IImageService
{
    Task<string> UploadImageAsync(IFormFile file, string folderName);
    void DeleteImage(string imagePath);
    
}

public class ImageService(ILogger<ImageService> logger, IWebHostEnvironment environment) : IImageService
{
    public async Task<string> UploadImageAsync(IFormFile file, string folderName)
    {
        if (file is null || file.Length == 0)
            throw new ArgumentException("File is empty");

        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        
        var uploadPath = Path.Combine(environment.WebRootPath, folderName);
        
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);
        
        var filePath = Path.Combine(uploadPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        
        return Path.Combine(folderName, fileName).Replace('\\', '/');
    }

    public void DeleteImage(string imagePath)
    {
        if ( string.IsNullOrEmpty(imagePath) )
            return;
        
        var fullPath = Path.Combine(environment.WebRootPath, imagePath);

        if (File.Exists(fullPath))
        {
            try
            {
                File.Delete(fullPath);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while deleting image at {Path}", fullPath);
            }
        }
    }
}