using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Minio;

namespace Clients.API.Controllers;

[ApiController]
[Produces("application/json")]

[Route("minio")]
public class MinioController : ControllerBase
{
    private readonly IMinioClient _minioClient;

    public MinioController(IMinioClient minioClient)
    {
        _minioClient = minioClient ?? throw new ArgumentNullException(nameof(minioClient));
    }

    [HttpPut]
    [Route("add-image/{fileName}")]
    public async Task<ActionResult> AddClientImage([FromRoute, Required] string fileName, [FromForm] IFormFile image)
    {
        var putObjectArg = new PutObjectArgs()
            .WithBucket("clients-photos")
            .WithObject(fileName)
            .WithContentType("image/png")
            .WithStreamData(image.OpenReadStream())
            .WithObjectSize(image.Length);

        await _minioClient.PutObjectAsync(putObjectArg);

        return Ok();
    }

    [HttpGet]
    [Route("get-image/{fileName}")]
    public async Task<ActionResult> AddClientImage([Required] string fileName)
    {
        var memStream = new MemoryStream();

        var getObjectArgs = new GetObjectArgs()
            .WithBucket("clients-photos")
            .WithObject(fileName)
            .WithCallbackStream(stream => stream.CopyTo(memStream));

        try
        {
            var objectStat = await _minioClient.GetObjectAsync(getObjectArgs);

            return File(memStream.ToArray(), MediaTypeNames.Image.Jpeg, objectStat.ObjectName);

        }
        catch (Minio.Exceptions.ObjectNotFoundException)
        {
            return NoContent();
        }
    }
}