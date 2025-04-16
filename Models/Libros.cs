using System.ComponentModel.DataAnnotations;

namespace Biblioteca2.Models
{
    public class Libro
    {
        [Key]
        [Required(ErrorMessage = "El ISBN es obligatorio.")]
        [StringLength(17, MinimumLength = 10, ErrorMessage = "El ISBN debe tener entre 10 y 17 caracteres.")]
        [RegularExpression(@"^(?:\d{9}[\dXx]|\d{13})$", ErrorMessage = "Formato de ISBN inválido.")]
        public string ISBN { get; set; } = "";

        [Required(ErrorMessage = "El título es obligatorio.")]
        [MaxLength(200, ErrorMessage = "El título no puede exceder 200 caracteres.")]
        public string Titulo { get; set; } = "";

        [Required(ErrorMessage = "El autor es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El autor no puede exceder 100 caracteres.")]
        public string Autor { get; set; } = "";

        [MaxLength(50, ErrorMessage = "El género no puede exceder 50 caracteres.")]
        public string Genero { get; set; } = "";

        public int CantidadDisponible { get; set; }

        public Libro() { }

        public Libro(string isbn, string titulo, string autor, string genero, int cantidad)
        {
            ISBN = isbn;
            Titulo = titulo;
            Autor = autor;
            Genero = genero;
            CantidadDisponible = cantidad;
        }
    }
}