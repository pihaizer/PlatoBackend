using System.Reflection;

using Backend.Context;

using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

if (builder.Environment.IsEnvironment("Development")) {
    // This reads the configuration keys from the secret store.
    // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
    builder.Configuration.AddUserSecrets(Assembly.GetCallingAssembly());
}

builder.Services.AddDbContext<ClimbingRouteContext>(opt => {
    string? connectionString = builder.Environment.IsEnvironment("Development")
        ? builder.Configuration.GetConnectionString("Plato")
        : Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_Plato");
    if (connectionString != null) opt.UseNpgsql(connectionString);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();