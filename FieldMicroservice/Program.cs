using Application.Automapper;
using Application.DTOS.Request;
using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.IAvailabilityServices;
using Application.Interfaces.IServices.IFieldServices;
using Application.Interfaces.IServices.IFieldTypeServices;
using Application.Interfaces.IValidator;
using Application.Services.AvailabilityServices;
using Application.Services.FieldServices;
using Application.Services.FieldTypeServices;
using Application.Validators;
using FieldMicroservice.Binders;
using FluentValidation;
using Infrastructure.Command;
using Infrastructure.Persistence;
using Infrastructure.Query;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new TimeSpanModelBinderProvider());
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new FieldMicroservice.Binders.TimeSpanConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "FieldMicroservice", Version = "1.0" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var connectionString = builder.Configuration["ConnectionString"];
builder.Services.AddDbContext<FieldMSContext>(options => options.UseSqlServer(connectionString));

//custom
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddScoped<IFieldQuery, FieldQuery>();
builder.Services.AddScoped<IFieldTypeQuery, FieldTypeQuery>();
builder.Services.AddScoped<IAvailabilityQuery, AvailabilityQuery>();

builder.Services.AddScoped<IFieldCommand, FieldCommand>();
builder.Services.AddScoped<IAvailabilityCommand, AvailabilityCommand>();

builder.Services.AddScoped<IFieldPutServices, FieldPutServices>();
builder.Services.AddScoped<IFieldPostServices, FieldPostServices>();
builder.Services.AddScoped<IFieldGetServices, FieldGetServices>();

builder.Services.AddScoped<IAvailabilityGetServices, AvailabilityGetServices>();
builder.Services.AddScoped<IAvailabilityPostServices, AvailabilityPostServices>();
builder.Services.AddScoped<IAvailabilityPutServices, AvailabilityPutServices>();
builder.Services.AddScoped<IAvailabilityDeleteService, AvailabilityDeleteService>();

builder.Services.AddScoped<IFieldTypeGetServices, FieldTypeGetServices>();

// Register the validators
builder.Services.AddValidatorsFromAssemblyContaining<FieldRequestValidator>();
builder.Services.AddScoped<IValidatorHandler<FieldRequest>, ValidatorHandler<FieldRequest>>();
builder.Services.AddValidatorsFromAssemblyContaining<AvailabilityRequestValidator>();
builder.Services.AddScoped<IValidatorHandler<AvailabilityRequest>, ValidatorHandler<AvailabilityRequest>>();
builder.Services.AddValidatorsFromAssemblyContaining<GetFieldsRequestValidator>();
builder.Services.AddScoped<IValidatorHandler<GetFieldsRequest>, ValidatorHandler<GetFieldsRequest>>();

// Setting the culture at a global level
var defaultCulture = new CultureInfo("en-US");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new List<CultureInfo> { defaultCulture },
    SupportedUICultures = new List<CultureInfo> { defaultCulture }
};

var secretKey = builder.Configuration["Jwt:SecretKey"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new ArgumentNullException(nameof(secretKey), "Secret key JWT can´t be null or empty.");
}
var key = Encoding.ASCII.GetBytes(secretKey);

// Configurar la autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Solo si estás en desarrollo
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    // Continúa con la solicitud
    await next();

    // Si el estado de la respuesta es 401 (No autorizado), añade los encabezados CORS
    if (context.Response.StatusCode == 401)
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");  // O usa el dominio que prefieras
        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization, Content-Type");
    }
});

// Use culture settings in the application
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FieldMicroservice v1.0");
    });
}

app.UseHttpsRedirection();

//JWT
app.UseAuthentication();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
