using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

using Warehouse.Presentation.Resources;


namespace Warehouse.Presentation.Controllers;


[ApiController]
[Route("api/metadata")]
public class MetadataController : ControllerBase
{
    private readonly IStringLocalizer<SharedResources> _localizer;


    public MetadataController(
        IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer;
    }



    // GET: api/metadata/validation/{dtoName}
    [HttpGet("validation/{dtoName}")]
    public IActionResult GetValidation(string dtoName)
    {
        var assembly = Assembly.GetExecutingAssembly();


        var dtoType = assembly
            .GetTypes()
            .FirstOrDefault(t => t.Name == dtoName);



        if (dtoType == null)
        {
            return NotFound(new
            {
                message = _localizer["DtoNotFound"],
                dtoName
            });
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
            message = _localizer["ValidationMetadataRetrieved"],
            DtoName = dtoName,
            Properties = properties
        });
    }
}