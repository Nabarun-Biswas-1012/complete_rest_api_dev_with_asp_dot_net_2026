using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace LiveProject_WebAPI_Demo.Services.Interfaces;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {
        var account = new Account(
            configuration["CloudinarySettings:CloudName"],
            configuration["CloudinarySettings:ApiKey"],
            configuration["CloudinarySettings:ApiSecret"]
            );

        _cloudinary = new Cloudinary(account);
    }


    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new Exception("No Image Selected!!");
        };

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, stream)
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return uploadResult.SecureUrl.ToString();
    }
}