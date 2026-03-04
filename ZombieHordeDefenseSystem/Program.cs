
using ZombieHordeDefenseSystem.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddProjectServices()
    .AddCorsConfiguration()
    .AddSwaggerDocumentation();

WebApplication app = builder.Build();

app.UseProjectPipeline();

app.Run();