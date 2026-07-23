using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Notifications.Api.Data;

namespace Warehouse.Notifications.Api.Controllers;


[ApiController]
[Route("notifications")]
public class NotificationsController : ControllerBase
{
    private readonly NotificationDbContext _context;


    public NotificationsController(NotificationDbContext context)
    {
        _context = context;
    }



    // GET /notifications
    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var notifications = await _context.Notifications
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();


        return Ok(notifications);
    }



    // PATCH /notifications/{id}/read
    [HttpPatch("{id}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(x => x.Id == id);


        if (notification == null)
            return NotFound();



        notification.IsRead = true;


        await _context.SaveChangesAsync();


        return Ok(notification);
    }
}