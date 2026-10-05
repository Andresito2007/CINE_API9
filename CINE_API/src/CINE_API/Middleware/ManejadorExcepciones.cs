using CINE_API.Dtos;
using CINE_API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CINE_API.Middleware;

// Manejo CENTRALIZADO de excepciones.
// Cualquier excepción que ocurra en un controlador o servicio llega aquí,
// así no repetimos try/catch en cada método.
public class ManejadorExcepciones : IExceptionHandler
{
    private readonly ILogger<ManejadorExcepciones> _logger;

    public ManejadorExcepciones(ILogger<ManejadorExcepciones> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        // Según el tipo de excepción elegimos el código HTTP.
        int estado;
        string error;
        string mensaje = exception.Message;

        switch (exception)
        {
            case NoEncontradoException:
                estado = StatusCodes.Status404NotFound;
                error = "No encontrado";
                break;
            case ReglaNegocioException:
                estado = StatusCodes.Status400BadRequest;
                error = "Solicitud inválida";
                break;
            case ConflictoException:
                estado = StatusCodes.Status409Conflict;
                error = "Conflicto";
                break;
            default:
                // Error no esperado: no mostramos detalles internos al cliente.
                _logger.LogError(exception, "Error no controlado");
                estado = StatusCodes.Status500InternalServerError;
                error = "Error interno";
                mensaje = "Ocurrió un error inesperado. Intente más tarde.";
                break;
        }

        ErrorResponse respuesta = new ErrorResponse
        {
            Estado = estado,
            Error = error,
            Mensaje = mensaje,
            Ruta = context.Request.Path
        };

        context.Response.StatusCode = estado;
        await context.Response.WriteAsJsonAsync(respuesta, cancellationToken);

        return true; // true = "ya manejé el error"
    }
}
