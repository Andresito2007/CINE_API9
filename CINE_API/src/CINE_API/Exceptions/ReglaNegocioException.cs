namespace CINE_API.Exceptions;

// Se lanza cuando los datos no cumplen una regla del cine (ej. función que ya pasó). → 400
public class ReglaNegocioException : Exception
{
    public ReglaNegocioException(string mensaje) : base(mensaje) { }
}
