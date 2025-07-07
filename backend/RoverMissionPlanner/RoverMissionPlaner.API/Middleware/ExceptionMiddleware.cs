using System.Net;
using System.Text.Json;

namespace RoverMissionPlanner.API.Middleware
{
    // Middleware para capturar excepciones globalmente
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        // Constructor
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // Se Procesa la petición HTTP
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Ejecuta el siguiente middleware de la cadena
                await _next(context);
            }
            catch (Exception ex)
            {
                // Se imprime el error capturado
                _logger.LogError(ex, "Ocurió un error no controlado.");

                // Configurar la respuesta HTTP con JSON y código 500
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Crear el objeto respuesta con mensaje genérico
                var response = new { message = "Error interno del servidor." };

                // Serializa a JSON
                var json = JsonSerializer.Serialize(response);

                // Se envía la respuesta JSON
                await context.Response.WriteAsync(json);
            }
        }
    }
}
