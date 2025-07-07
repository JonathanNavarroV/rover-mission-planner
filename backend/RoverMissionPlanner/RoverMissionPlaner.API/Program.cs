using FluentValidation;
using FluentValidation.AspNetCore;
using RoverMissionPlanner.API.Validators;
using RoverMissionPlanner.Application.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Agregar el servicio en memoria
builder.Services.AddSingleton<IRoverTaskService, RoverTaskService>();

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Permite deserializar enums desde strings en JSON
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Habilitar la validación automatica en el pipeline
builder.Services.AddFluentValidationAutoValidation();
// Integración con validación del lado del cliente
builder.Services.AddFluentValidationClientsideAdapters();
// Registra todos los validadores de RoverTaskValidator
builder.Services.AddValidatorsFromAssemblyContaining<RoverTaskValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Usar ejemplo en Swagger
    options.SchemaFilter<RoverMissionPlanner.API.Swagger.RoverTaskExample>();
});

var app = builder.Build();

// Se agrega el middleware para manejo global de excepciones
app.UseMiddleware<RoverMissionPlanner.API.Middleware.ExceptionMiddleware>();

app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
