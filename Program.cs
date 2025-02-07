using ControlAcceso.Application.Interfaces;
using ControlAcceso.Application.Services;
using ControlAcceso.Domain.Entities;
using ControlAcceso.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder ( args );

// Configurar el cargador de configuración para soportar múltiples entornos
builder.Configuration
    .SetBasePath ( Directory.GetCurrentDirectory ( ) )
    .AddJsonFile ( "appsettings.json" , optional: false , reloadOnChange: true )
    .AddJsonFile ( $"appsettings.{builder.Environment.EnvironmentName}.json" , optional: true ) // Cargar configuraciones específicas del entorno
    .AddEnvironmentVariables ( );

// Add services to the container.
builder.Services.AddControllers ( );

// Configura MongoDBSettings
builder.Services.Configure<MongoDBSettings> ( builder.Configuration.GetSection ( "MongoDB" ) );

// Registra los servicios
builder.Services.AddScoped<IUsuarioService , UsuarioService> ( );
builder.Services.AddScoped<ILogAccesoService , LogAccesoService> ( );
builder.Services.AddScoped<ILoginRequestService , LoginRequestService> ( );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer ( );
builder.Services.AddSwaggerGen ( );

// Configurar CORS para permitir solicitudes de cualquier origen
builder.Services.AddCors ( options =>
{
    options.AddPolicy ( "AllowAllOrigins" ,
        builder => builder.AllowAnyOrigin ( )
                          .AllowAnyMethod ( )
                          .AllowAnyHeader ( ) );
} );

// Servicios
// Configurar JWT
var jwtSettings = builder.Configuration.GetSection ( "JwtSettings" );
var secretKeys = jwtSettings [ "SecretKey" ];

builder.Services.AddAuthentication ( options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
} )
.AddJwtBearer ( options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true ,
        ValidateAudience = true ,
        ValidateLifetime = true ,
        ValidateIssuerSigningKey = true ,
        ValidIssuer = jwtSettings [ "Issuer" ] , // URL local
        ValidAudience = jwtSettings [ "Audience" ] , // URL local
        IssuerSigningKey = new SymmetricSecurityKey ( Encoding.UTF8.GetBytes ( secretKeys ) )
    };

    // Configurar manejo de errores de autenticación
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            // Evitar la respuesta predeterminada para un error 401
            context.HandleResponse ( );

            // Crear el objeto ResponseData con el mensaje de error
            var response = new ResponseData
            {
                Data = null ,
                Message = "No autorizado: Token no válido o faltante." ,
                Status = 401
            };

            // Configurar el código de estado y devolver la respuesta en formato JSON
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 401; // Código de estado HTTP 401 (Unauthorized)
            await context.Response.WriteAsJsonAsync ( response );
        }
    };
} );

builder.Services.AddSwaggerGen ( c =>
{
    c.AddSecurityDefinition ( "Bearer" , new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header ,
        Description = "Por favor, ingrese 'Bearer' seguido de un espacio y su token JWT" ,
        Name = "Authorization" ,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    } );

    c.AddSecurityRequirement ( new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    } );
} );

var app = builder.Build ( );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment ( ))
{
    app.UseSwagger ( );
    app.UseSwaggerUI ( );
}

app.UseHttpsRedirection ( );

//app.UseAuthentication ( ); // Habilitar autenticación
app.UseAuthorization ( );

app.MapControllers ( );

app.Run ( );
