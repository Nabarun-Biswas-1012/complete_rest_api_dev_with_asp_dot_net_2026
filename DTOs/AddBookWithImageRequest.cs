namespace LiveProject_WebAPI_Demo.DTOs;

public class AddBookWithImageRequest
{
    public string? Name { get; set; }

    public string? Author { get; set; }

    public int Price { get; set; }

    public IFormFile? Image { get; set; }
}