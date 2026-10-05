using CINE_API.Dtos;
using CINE_API.Middleware;
using CINE_API.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Cuando fallan las anotaciones del DTO ([Required], [Range], etc.)
        // respondemos con el mismo formato ErrorResponse que el resto de errores.
        options.InvalidModelStateResponseFactory = context =>
        {
            Dictionary<string, string[]> errores = context.ModelState
                .Where(campo => campo.Value!.Errors.Count > 0)
                .ToDictionary(
                    campo => campo.Key,
                    campo => campo.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

            ErrorResponse respuesta = new ErrorResponse
            {
                Estado = StatusCodes.Status400BadRequest,
                Error = "Validación fallida",
                Mensaje = "Uno o más campos no son válidos.",
                Ruta = context.HttpContext.Request.Path,
                Errores = errores
            };

            return new BadRequestObjectResult(respuesta);
        };
    });

builder.Services.AddOpenApi();

// Registramos el servicio de tickets.
// Singleton = una sola instancia para toda la app, así la lista en memoria
// no se pierde entre una petición y otra.
builder.Services.AddSingleton<ITicketService, TicketService>();

// Registramos el manejador centralizado de excepciones.
builder.Services.AddExceptionHandler<ManejadorExcepciones>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Debe ir al inicio para poder atrapar los errores de todo lo que viene después.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
