using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrar controllers y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Prueba API", Version = "v1" });
});

var app = builder.Build();

// 2. Pipeline: Swagger + Controllers
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Prueba API V1");
});

app.MapControllers();

app.Run();