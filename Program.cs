using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

app.MapPost("/api/upload", async (HttpRequest request) =>
{
    if (!request.HasFormContentType)
        return Results.BadRequest("Expected a form");

    var form = await request.ReadFormAsync();
    var file = form.Files["image"];

    if (file == null)
        return Results.BadRequest("Image file missing");

    using var ms = new MemoryStream();
    await file.CopyToAsync(ms);
    var imageBytes = ms.ToArray();

    using var image = SixLabors.ImageSharp.Image.Load(imageBytes);
    return Results.Json(new
    {
        message = "Image received successfully",
       
    });
});

app.Run();
