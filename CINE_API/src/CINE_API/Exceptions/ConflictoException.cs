namespace CINE_API.Exceptions;

// Se lanza cuando la operación choca con datos existentes (ej. asiento ya vendido). → 409
public class ConflictoException : Exception
{
    public ConflictoException(string mensaje) : base(mensaje) { }
}