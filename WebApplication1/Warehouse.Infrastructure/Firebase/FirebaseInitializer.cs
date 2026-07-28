using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Warehouse.Infrastructure.Firebase;

public static class FirebaseInitializer
{
    private static readonly object LockObject = new();


    public static void Initialize()
    {
        lock (LockObject)
        {
            if (FirebaseApp.DefaultInstance != null)
            {
                return;
            }


            var credentialPath = Path.Combine(
                AppContext.BaseDirectory,
                "Firebase",
                "serviceAccount.json"
            );


            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(
                    credentialPath)
            });
        }
    }
}