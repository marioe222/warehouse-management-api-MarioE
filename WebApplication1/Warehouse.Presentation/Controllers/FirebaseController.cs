using Microsoft.AspNetCore.Mvc;
using Warehouse.Infrastructure.Firebase;

namespace Warehouse.Presentation.Controllers;

[ApiController]
[Route("api/firebase")]
public class FirebaseController : ControllerBase
{
    private readonly FirebaseAdminService _firebaseAdminService;

    public FirebaseController(
        FirebaseAdminService firebaseAdminService)
    {
        _firebaseAdminService = firebaseAdminService;
    }


    [HttpPost("set-user-role")]
    public async Task<IActionResult> SetUserRole()
    {
        await _firebaseAdminService.SetUserRoleAsync(
            "LW2h2HY6cTdlbOpgsqVXApkpr403",
            "user"
        );
        return Ok("User role assigned");
    }
    [HttpPost("set-admin-role")]
    public async Task<IActionResult> SetAdminRole()
    {
        await _firebaseAdminService.SetUserRoleAsync(
            "FpRC7x6TFPXZZAiKBBJ7Y1Tjxgo2",
            "admin"
        );

        return Ok("Admin role assigned");
    }
}