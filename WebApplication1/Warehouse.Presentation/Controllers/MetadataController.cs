using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Warehouse.Presentation.Controllers;


[ApiController]
[Route("api/metadata")]
public class MetadataController : ControllerBase
{
    [HttpGet("validation/{dtoName}")]
    public IActionResult GetValidation(string dtoName)
    {
        var assembly = Assembly.GetExecutingAssembly();


        var dtoType = assembly
            .GetTypes()
            .FirstOrDefault(t => t.Name == dtoName);


        if (dtoType == null)
        {
            return NotFound(
                $"DTO '{dtoName}' was not found."
            );
        }


        var properties = dtoType
            .GetProperties()
            .Select(property => new
            {
                Name = property.Name,

                Type = property.PropertyType.Name,

                Required = property
                    .GetCustomAttribute<RequiredAttribute>() != null,

                MaxLength = property
                    .GetCustomAttribute<MaxLengthAttribute>()
                    ?.Length,

                MinLength = property
                    .GetCustomAttribute<MinLengthAttribute>()
                    ?.Length
            });


        return Ok(new
        {
            DtoName = dtoName,
            Properties = properties
        });
    }
}