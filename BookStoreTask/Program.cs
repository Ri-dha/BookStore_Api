using System.Globalization;
using BookStoreTask.Data;
using BookStoreTask.FilesMod;
using BookStoreTask.Utli;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddDbContext<ProjectContext>(options =>
        options.UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }))
            .EnableSensitiveDataLogging()
            .UseNpgsql(builder.Configuration.GetConnectionString("local")).EnableSensitiveDataLogging(),
    ServiceLifetime.Scoped);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder => builder.WithOrigins(
                "http://localhost:63342", "http://localhost:3000",
                "http://localhost:3001", "http://localhost:3002",
                "http://localhost:3003", "http://localhost:3004",
                "http://localhost:3005", "http://localhost:3006"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("status", "Content-Disposition",
                "Access-Control-Allow-Origin"));
});
builder.Services.AddControllers(options =>
    {
        options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        options.SerializerSettings.Converters.Add(new IsoDateTimeConverter
            { DateTimeStyles = DateTimeStyles.AssumeUniversal });
    })
    .AddViewLocalization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add JWT Authentication to Swagger
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token.",
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    };

    options.AddSecurityRequirement(securityRequirement);

    // Explicitly handle file uploads in Swagger
    // options.OperationFilter<SwaggerFileOperationFilter>();
});


builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger";
        c.DocExpansion(DocExpansion.None);
    });
}

app.UseCors("AllowLocalhost");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();