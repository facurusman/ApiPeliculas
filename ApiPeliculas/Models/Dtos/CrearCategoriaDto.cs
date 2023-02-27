using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos;

public class CrearCategoriaDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100, ErrorMessage = "Maximo 100 caracteres")]
    public string Nombre { get; set; }
}
