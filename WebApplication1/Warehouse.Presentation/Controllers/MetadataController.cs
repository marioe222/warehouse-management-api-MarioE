using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Warehouse.Presentation.Resources;

namespace Warehouse.Presentation.Controllers;

[ApiController]
[Route("api/metadata")]
public class MetadataController : ControllerBase
{
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly ILogger<MetadataController> _logger;


    public MetadataController(
        IStringLocalizer<SharedResources> localizer,
        ILogger<MetadataController> logger)
    {
        _localizer = localizer;
        _logger = logger;
    }


    // GET: api/metadata/validation/{dtoName}
    [HttpGet("validation/{dtoName}")]
    public IActionResult GetValidation(string dtoName)
    {
        _logger.LogInformation(
            "Validation metadata requested for DTO {DtoName}",
            dtoName);


        var assembly = Assembly.GetExecutingAssembly();


        var dtoType = assembly
            .GetTypes()
            .FirstOrDefault(t => t.Name == dtoName);


        if (dtoType == null)
        {
            _logger.LogWarning(
                "DTO {DtoName} was not found",
                dtoName);


            return NotFound(new
            {
                message = _localizer["DtoNotFound"].Value,
                dtoName
            });
        }


        var properties = dtoType
            .GetProperties()
            .Select(property => new
            {
                property.Name,

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


        _logger.LogInformation(
            "Validation metadata retrieved successfully for DTO {DtoName}",
            dtoName);


        return Ok(new
        {
            message = _localizer["ValidationMetadataRetrieved"].Value,
            DtoName = dtoName,
            Properties = properties
        });
    }
}