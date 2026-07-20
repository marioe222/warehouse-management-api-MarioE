using FirebaseAdmin.Auth;

namespace Warehouse.Infrastructure.Firebase;

public class FirebaseAdminService
{
    public async Task SetUserRoleAsync(string uid, string role)
    {
        await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(
            uid,
            new Dictionary<string, object>
            {
                { "role", role }
            });
    }
}