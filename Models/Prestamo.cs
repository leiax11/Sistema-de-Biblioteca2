using System;

namespace Biblioteca2.Models
{
    public class Prestamo
    {
        [Required(ErrorMessage = "El ISBN es obligatorio")]
        [StringLength(17, MinimumLength = 10)]
        public string ISBN { get; set; }

        [Required]
        [MaxLength(100)]
        public string Usuario { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Now; // Valor por defecto
        
        public Prestamo() { }

        public Prestamo(string isbn, string usuario, DateTime fecha)
        {
            ISBN = isbn;
            Usuario = usuario;
            Fecha = fecha;
        }
    }
}