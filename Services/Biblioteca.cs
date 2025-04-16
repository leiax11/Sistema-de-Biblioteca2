using Biblioteca2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Biblioteca2.Services
{
    public class Biblioteca
    {
        private Dictionary<string, Libro> _libros = new Dictionary<string, Libro>();
        private List<Prestamo> _prestamos = new List<Prestamo>();
        private readonly string _librosArchivo;
        private readonly string _prestamosArchivo;

        public Biblioteca()
        {
            var dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(dataDirectory);
            
            _librosArchivo = Path.Combine(dataDirectory, "libros.json");
            _prestamosArchivo = Path.Combine(dataDirectory, "prestamos.json");

            CargarDatos();
        }

        private void CargarDatos()
        {
            CargarLibros();
            CargarPrestamos();
        }

        private void CargarLibros()
        {
            try
            {
                if (!File.Exists(_librosArchivo)) return;

                string json = File.ReadAllText(_librosArchivo);
                var jObject = JObject.Parse(json);

                _libros.Clear();
                foreach (var item in jObject)
                {
                    _libros[item.Key] = new Libro(
                        isbn: item.Key,
                        titulo: item.Value["titulo"]?.Value<string>() ?? string.Empty,
                        autor: item.Value["autor"]?.Value<string>() ?? string.Empty,
                        genero: item.Value["genero"]?.Value<string>() ?? string.Empty,
                        cantidad: item.Value["cantidad"]?.Value<int>() ?? 0
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar libros: {ex.Message}");
            }
        }

        private void GuardarLibros()
        {
            try
            {
                var jObject = new JObject();
                foreach (var libro in _libros.Values)
                {
                    jObject[libro.ISBN] = JObject.FromObject(new
                    {
                        titulo = libro.Titulo,
                        autor = libro.Autor,
                        genero = libro.Genero,
                        cantidad = libro.CantidadDisponible
                    });
                }
                File.WriteAllText(_librosArchivo, jObject.ToString(Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar libros: {ex.Message}");
            }
        }

        private void CargarPrestamos()
        {
            try
            {
                if (!File.Exists(_prestamosArchivo)) return;

                string json = File.ReadAllText(_prestamosArchivo);
                _prestamos = JsonConvert.DeserializeObject<List<Prestamo>>(json) ?? new List<Prestamo>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar préstamos: {ex.Message}");
            }
        }

        private void GuardarPrestamos()
        {
            try
            {
                File.WriteAllText(_prestamosArchivo, JsonConvert.SerializeObject(_prestamos, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar préstamos: {ex.Message}");
            }
        }

        public void AgregarLibro(Libro libro)
        {
            if (_libros.ContainsKey(libro.ISBN))
            {
                throw new InvalidOperationException("El libro ya existe en la biblioteca.");
            }

            _libros[libro.ISBN] = libro;
            GuardarLibros();
        }

        public bool EliminarLibro(string isbn)
        {
            if (_libros.Remove(isbn))
            {
                GuardarLibros();
                return true;
            }
            return false;
        }

        public Libro ObtenerLibro(string isbn)
        {
            return _libros.TryGetValue(isbn, out var libro) ? libro : null;
        }

        public IEnumerable<Libro> ObtenerTodosLibros()
        {
            return _libros.Values.OrderBy(l => l.Titulo);
        }

        public void RegistrarPrestamo(Prestamo prestamo)
        {
            if (!_libros.TryGetValue(prestamo.ISBN, out var libro))
            {
                throw new KeyNotFoundException("El libro no existe en la biblioteca.");
            }

            if (libro.CantidadDisponible <= 0)
            {
                throw new InvalidOperationException("No hay ejemplares disponibles de este libro.");
            }

            libro.CantidadDisponible--;
            _prestamos.Add(prestamo);
            
            GuardarLibros();
            GuardarPrestamos();
        }

        public void DevolverLibro(string isbn, string usuario)
        {
            var prestamo = _prestamos.FirstOrDefault(p => 
                p.ISBN == isbn && 
                string.Equals(p.Usuario, usuario, StringComparison.OrdinalIgnoreCase));

            if (prestamo == null)
            {
                throw new KeyNotFoundException("No se encontró el préstamo especificado.");
            }

            if (_libros.TryGetValue(isbn, out var libro))
            {
                libro.CantidadDisponible++;
            }

            _prestamos.Remove(prestamo);
            GuardarLibros();
            GuardarPrestamos();
        }

        public IEnumerable<Prestamo> ObtenerTodosPrestamos()
        {
            return _prestamos.OrderBy(p => p.Fecha);
        }

        public IEnumerable<Libro> BuscarLibrosPorTitulo(string titulo)
        {
            return _libros.Values
                .Where(l => l.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase))
                .OrderBy(l => l.Titulo);
        }

        public IEnumerable<Libro> BuscarLibrosPorAutor(string autor)
        {
            return _libros.Values
                .Where(l => l.Autor.Contains(autor, StringComparison.OrdinalIgnoreCase))
                .OrderBy(l => l.Autor);
        }

         public IEnumerable<Libro> BuscarLibrosPorGenero(string genero)
        {
            return _libros.Values
                .Where(l => l.Genero.Contains(genero, StringComparison.OrdinalIgnoreCase))
                .OrderBy(l => l.Genero);
        }
    }
}