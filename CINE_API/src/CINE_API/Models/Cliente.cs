namespace CINE_API.Models;

// Persona que compra tickets en el cine.
public class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Dni { get; set; } = "";
    public string Email { get; set; } = "";

    public Cliente() { }

    public Cliente(int id, string nombre, string dni, string email)
    {
        Id = id;
        Nombre = nombre;
        Dni = dni;
        Email = email;
    }
}
