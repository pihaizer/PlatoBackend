using System.Reflection;

using Azure.Storage.Blobs;

using Backend.Context;
using Backend.Services;

using FirebaseAdmin;

using Google.Apis.Auth.OAuth2;

using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<PlatoContext>(opt => {
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

FirebaseApp.Create(new AppOptions {
    Credential = GoogleCredential.FromJson(builder.Configuration["GOOGLE_CREDENTIAL"])
});

WebApplication app = builder.Build();

// if (app.Environment.IsDevelopment()) {
app.UseSwagger();
app.UseSwaggerUI();
// }

Console.WriteLine(FirebaseApp.DefaultInstance.Name);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();