namespace CINE_API.Exceptions;

// Se lanza cuando se busca algo que no existe (ej. ticket con Id 99). → 404
public class NoEncontradoException : Exception
{
    public NoEncontradoException(string mensaje) : base(mensaje) { }
}
