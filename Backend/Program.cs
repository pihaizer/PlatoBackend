using System.Security.Claims;

using AspNetCore.Firebase.Authentication.Extensions;

using Backend.Context;
using Backend.Filters;
using Backend.Services;
using Backend.Services.Impl;

using FirebaseAdmin;
using FirebaseAdmin.Auth;

using Google.Apis.Auth.OAuth2;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<PlatoContext>(opt => {
    opt.EnableSensitiveDataLogging();
    
    string? connectionString = builder.Environment.IsEnvironment("Development")
        ? builder.Configuration.GetConnectionString("Plato")
        : builder.Configuration["POSTGRESQLCONNSTR_Plato"];
    
    if (connectionString != null) {
        opt.UseNpgsql(connectionString);
    }
});

string azureStorageConnectionString = builder.Configuration["AZURE_STORAGE_CONNECTION_STRING"];

builder.Services.AddTransient<IClimbingRoutesService, ClimbingRoutesService>();
builder.Services.AddTransient<ICommentsService, CommentsService>();
builder.Services.AddTransient<IStorageService>(_ =>
    new AzureStorageService(azureStorageConnectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority = builder.Configuration["Jwt:Firebase:ValidIssuer"];
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Firebase:ValidIssuer"],
            ValidAudience = builder.Configuration["Jwt:Firebase:ValidAudience"]
        };
    });

FirebaseApp.Create(new AppOptions {
    Credential = GoogleCredential.FromJson(builder.Configuration["GOOGLE_CREDENTIAL"])
});

WebApplication app = builder.Build();

// if (app.Environment.IsDevelopment()) {
app.UseSwagger();
app.UseSwaggerUI();
// }

string superUserId = "rC8b1fYw97erLMXdKhS8J3OcdMZ2";
string token = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(superUserId);
Console.WriteLine(token.ReplaceLineEndings(""));
//
var claims = new Dictionary<string, object>
{
    { ClaimTypes.Role, ClaimRole.SuperUser }
};

await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(superUserId, claims);
UserRecord? superuser = await FirebaseAuth.DefaultInstance.GetUserAsync(superUserId);
Console.WriteLine(superuser.Uid);
Console.WriteLine(string.Join('\n', superuser.CustomClaims));

app.UseHttpsRedirection();
app.UseRouting();   
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();