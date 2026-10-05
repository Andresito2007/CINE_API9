using CINE_API.Data;
using CINE_API.Dtos;
using CINE_API.Exceptions;
using CINE_API.Models;

namespace CINE_API.Services;

// Lógica de negocio de los tickets. Aquí se trabaja con la lista en memoria.
public class TicketService : ITicketService
{
    private readonly List<Ticket> _tickets = DatosIniciales.Tickets;

    public List<Ticket> ObtenerTodos()
    {
        return _tickets;
    }

    public Ticket ObtenerPorId(int id)
    {
        Ticket? ticket = _tickets.FirstOrDefault(t => t.Id == id);

        if (ticket == null)
        {
            throw new NoEncontradoException($"No existe el ticket con Id {id}");
        }

        return ticket;
    }

    public Ticket Crear(TicketRequestDto dto)
    {
        ValidarReglas(dto, idActual: 0);

        Ticket nuevo = new Ticket
        {
            // El Id y el Estado los pone el sistema, no el cliente.
            Id = _tickets.Count == 0 ? 1 : _tickets.Max(t => t.Id) + 1,
            Estado = "Activo"
        };
        CopiarDatos(dto, nuevo);

        _tickets.Add(nuevo);
        return nuevo;
    }

    public Ticket Actualizar(int id, TicketRequestDto dto)
    {
        Ticket ticket = ObtenerPorId(id); // si no existe, lanza 404

        ValidarReglas(dto, idActual: id);
        CopiarDatos(dto, ticket);

        return ticket;
    }

    public void Eliminar(int id)
    {
        Ticket ticket = ObtenerPorId(id); // si no existe, lanza 404
        _tickets.Remove(ticket);
    }

    // Reglas del cine que no se pueden expresar con anotaciones.
    // idActual sirve para que, al actualizar, el ticket no choque consigo mismo.
    private void ValidarReglas(TicketRequestDto dto, int idActual)
    {
        if (!DatosIniciales.Clientes.Any(c => c.Id == dto.ClienteId))
        {
            throw new ReglaNegocioException($"No existe el cliente con Id {dto.ClienteId}");
        }

        if (!DatosIniciales.Peliculas.Any(p => p.Id == dto.PeliculaId))
        {
            throw new ReglaNegocioException($"No existe la película con Id {dto.PeliculaId}");
        }

        if (dto.FechaFuncion <= DateTime.Now)
        {
            throw new ReglaNegocioException("No se pueden vender tickets para una función que ya pasó.");
        }

        bool asientoOcupado = _tickets.Any(t =>
            t.Id != idActual &&
            t.Estado == "Activo" &&
            t.PeliculaId == dto.PeliculaId &&
            t.FechaFuncion == dto.FechaFuncion &&
            t.Sala.Equals(dto.Sala, StringComparison.OrdinalIgnoreCase) &&
            t.Asiento.Equals(dto.Asiento, StringComparison.OrdinalIgnoreCase));

        if (asientoOcupado)
        {
            throw new ConflictoException($"El asiento {dto.Asiento.ToUpper()} ya está vendido para esa función.");
        }
    }

    // Pasa los datos del DTO al ticket.
    private static void CopiarDatos(TicketRequestDto dto, Ticket ticket)
    {
        ticket.ClienteId = dto.ClienteId;
        ticket.PeliculaId = dto.PeliculaId;
        ticket.FechaFuncion = dto.FechaFuncion!.Value;
        ticket.Sala = dto.Sala;
        ticket.Asiento = dto.Asiento.ToUpper();
        ticket.Precio = dto.Precio;
    }
}
