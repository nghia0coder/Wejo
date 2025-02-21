using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;

namespace Wejo.Identity.Application.Services;

public class FirebaseService
{
    public FirebaseService(IConfiguration configuration)
    {
        var serviceAccountPath = configuration["Firebase:ServiceAccountFile"];

        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(serviceAccountPath)
            });
        }
    }
}

