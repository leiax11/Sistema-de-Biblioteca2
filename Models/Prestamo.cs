using System;

namespace Biblioteca2.Models
{
    public class Prestamo
    {
        public string ISBN { get; set; }
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }

        public Prestamo() { }

        public Prestamo(string isbn, string usuario, DateTime fecha)
        {
            ISBN = isbn;
            Usuario = usuario;
            Fecha = fecha;
        }
    }
}