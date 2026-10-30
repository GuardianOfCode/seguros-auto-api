using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SegurosAuto.Api.Data;
using SegurosAuto.Api.Middleware;
using SegurosAuto.Api.Repositories;
using SegurosAuto.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------- Base de datos ----------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------- Inyección de dependencias (Repository → Service → Controller) ----------
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ICoberturaRepository, CoberturaRepository>();
builder.Services.AddScoped<IPolizaRepository, PolizaRepository>();

builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ICoberturaService, CoberturaService>();
builder.Services.AddScoped<IPolizaService, PolizaService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ---------- Swagger / OpenAPI con documentación completa ----------
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Seguros LAFISE — API de Emisión de Seguros de Auto",
        Version = "v1",
        Description =
            "API REST para el módulo de emisión de pólizas de automóviles.\n\n" +
            "**Flujo recomendado:**\n" +
            "1. Consultar clientes (`GET /api/clientes`) y coberturas (`GET /api/coberturas`).\n" +
            "2. Emitir la póliza (`POST /api/polizas/emitir`). La prima se calcula en el servidor.\n" +
            "3. Consultar el detalle (`GET /api/polizas/{id}`) o el historial (`GET /api/polizas`).\n\n" +
            "**Errores:** se devuelven en formato `ProblemDetails` con códigos 400, 404 y 500.",
        Contact = new OpenApiContact
        {
            Name = "Tu Nombre",
            Email = "tu.correo@correo.com"
        }
    });

    // Lee el archivo XML generado a partir de los comentarios ///
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

var app = builder.Build();

// El middleware de errores va primero para capturar cualquier excepción
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Seguros Auto API v1");
    options.DocumentTitle = "Seguros Auto API";
    options.DisplayRequestDuration();
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();