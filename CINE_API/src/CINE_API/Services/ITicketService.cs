using CINE_API.Dtos;
using CINE_API.Models;

namespace CINE_API.Services;

// Contrato: dice QUÉ operaciones hay con los tickets, no CÓMO se hacen.
public interface ITicketService
{
    List<Ticket> ObtenerTodos();
    Ticket ObtenerPorId(int id);
    Ticket Crear(TicketRequestDto dto);
    Ticket Actualizar(int id, TicketRequestDto dto);
    void Eliminar(int id);
}
