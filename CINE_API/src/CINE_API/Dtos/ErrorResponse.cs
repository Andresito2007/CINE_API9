namespace CINE_API.Dtos;

// Formato ÚNICO para todas las respuestas de error de la API.
// Así el que consume la API siempre recibe la misma estructura.
public class ErrorResponse
{
    public int Estado { get; set; }            // Código HTTP: 400, 404, 409, 500
    public string Error { get; set; } = "";    // Nombre corto del error
    public string Mensaje { get; set; } = "";  // Explicación para el usuario
    public string Ruta { get; set; } = "";     // Endpoint que se llamó
    public DateTime Fecha { get; set; } = DateTime.Now;

    // Solo se llena cuando fallan las validaciones de los campos.
    public Dictionary<string, string[]>? Errores { get; set; }
}
