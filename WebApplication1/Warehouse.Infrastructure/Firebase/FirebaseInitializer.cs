using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Warehouse.Infrastructure.Firebase;

public static class FirebaseInitializer
{
    public static void Initialize()
    {
        var credentialPath = Path.Combine(
            AppContext.BaseDirectory,
            "Firebase",
            "serviceAccount.json"
        );

        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile(credentialPath)
        });
    }
}