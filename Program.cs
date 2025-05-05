using API_Reclutamiento.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// 1) Servicios de la aplicación
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(opt =>
        opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Scheme = "ApiKey",
        Description = "Ingrese su API Key en el campo para autenticar."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new List<string>()
        }
    });
});

// 2) Configuración CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
          .WithOrigins("http://localhost:4200", "https://formulariopostulantes.com.ar")  
          .AllowAnyMethod()
          .AllowAnyHeader();
    });
});

var app = builder.Build();

// 3) Pipeline de middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANTE: aplica CORS antes que tu middleware de API Key
app.UseCors("AllowAll");

// 4) Middleware de API Key
var apiKeyName = "X-API-KEY";
var apiKeyValue = app.Configuration["ApiSettings:ApiKey"];

app.Use(async (context, next) =>
{
    if (!context.Request.Headers.TryGetValue(apiKeyName, out var extractedApiKey)
        || extractedApiKey != apiKeyValue)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("API Key inválida o no proporcionada.");
        return;
    }

    await next();
});

app.UseAuthorization();
app.MapControllers();
app.Run();
