using System.ComponentModel.DataAnnotations;

namespace CINE_API.Dtos;

// Datos que el cliente envía para crear o actualizar un ticket.
// No incluye Id ni Estado: esos los define el sistema.
public class TicketRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = "El ClienteId es obligatorio y debe ser mayor a 0.")]
    public int ClienteId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El PeliculaId es obligatorio y debe ser mayor a 0.")]
    public int PeliculaId { get; set; }

    [Required(ErrorMessage = "La fecha de la función es obligatoria.")]
    public DateTime? FechaFuncion { get; set; }

    [Required(ErrorMessage = "La sala es obligatoria.")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "La sala debe tener entre 3 y 20 caracteres.")]
    public string Sala { get; set; } = "";

    // Fila de la A a la L y número del 1 al 20. Ejemplo: "F7".
    [Required(ErrorMessage = "El asiento es obligatorio.")]
    [RegularExpression("^[A-La-l]([1-9]|1[0-9]|20)$", ErrorMessage = "El asiento debe tener una fila (A-L) y un número (1-20). Ejemplo: F7.")]
    public string Asiento { get; set; } = "";

    [Range(5, 50, ErrorMessage = "El precio debe estar entre 5 y 50 soles.")]
    public decimal Precio { get; set; }
}
