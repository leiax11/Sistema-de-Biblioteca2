using Biblioteca2.Models;
using Biblioteca2.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca2
{
    class Program
    {
        private static Biblioteca _biblioteca = new Biblioteca();

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            while (true)
            {
                MostrarMenuPrincipal();
                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        MenuLibros();
                        break;
                    case "2":
                        MenuPrestamos();
                        break;
                    case "3":
                        BuscarLibros();
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intente nuevamente.");
                        break;
                }
            }
        }

        private static void MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("📚 SISTEMA DE BIBLIOTECA 📚");
            Console.WriteLine("1. Gestión de Libros");
            Console.WriteLine("2. Gestión de Préstamos");
            Console.WriteLine("3. Buscar Libros");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opción: ");
        }

        private static void MenuLibros()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("📖 GESTIÓN DE LIBROS");
                Console.WriteLine("1. Agregar libro");
                Console.WriteLine("2. Listar todos los libros");
                Console.WriteLine("3. Eliminar libro");
                Console.WriteLine("4. Volver al menú principal");
                Console.Write("Seleccione una opción: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AgregarLibro();
                        break;
                    case "2":
                        ListarLibros();
                        break;
                    case "3":
                        EliminarLibro();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }

        private static void AgregarLibro()
        {
            Console.WriteLine("\n📗 NUEVO LIBRO");
            try
            {
                var libro = new Libro
                {
                    ISBN = LeerCadena("ISBN (10-17 caracteres): "),
                    Titulo = LeerCadena("Título: "),
                    Autor = LeerCadena("Autor: "),
                    Genero = LeerCadena("Género (opcional): ", obligatorio: false),
                    CantidadDisponible = LeerEntero("Cantidad disponible: ")
                };

                _biblioteca.AgregarLibro(libro);
                Console.WriteLine("✅ Libro agregado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        private static void ListarLibros()
        {
            Console.WriteLine("\n📚 CATÁLOGO COMPLETO");
            foreach (var libro in _biblioteca.ObtenerTodosLibros())
            {
                Console.WriteLine($"- {libro.ISBN}: {libro.Titulo} ({libro.Autor}) | " +
                                $"Género: {libro.Genero} | Disponibles: {libro.CantidadDisponible}");
            }
        }

        private static void EliminarLibro()
        {
            Console.Write("\nIngrese ISBN del libro a eliminar: ");
            var isbn = Console.ReadLine();

            if (_biblioteca.EliminarLibro(isbn))
                Console.WriteLine("✅ Libro eliminado");
            else
                Console.WriteLine("❌ Libro no encontrado");
        }

        private static void MenuPrestamos()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("📝 GESTIÓN DE PRÉSTAMOS");
                Console.WriteLine("1. Registrar préstamo");
                Console.WriteLine("2. Registrar devolución");
                Console.WriteLine("3. Listar préstamos activos");
                Console.WriteLine("4. Volver al menú principal");
                Console.Write("Seleccione una opción: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        RegistrarPrestamo();
                        break;
                    case "2":
                        RegistrarDevolucion();
                        break;
                    case "3":
                        ListarPrestamos();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }

        private static void RegistrarPrestamo()
        {
            Console.WriteLine("\n📝 NUEVO PRÉSTAMO");
            try
            {
                var prestamo = new Prestamo
                {
                    ISBN = LeerCadena("ISBN del libro: "),
                    Usuario = LeerCadena("Nombre del usuario: "),
                    Fecha = DateTime.Today
                };

                _biblioteca.RegistrarPrestamo(prestamo);
                Console.WriteLine("✅ Préstamo registrado");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        private static void RegistrarDevolucion()
        {
            Console.WriteLine("\n📌 REGISTRAR DEVOLUCIÓN");
            try
            {
                var isbn = LeerCadena("ISBN del libro: ");
                var usuario = LeerCadena("Nombre del usuario: ");

                _biblioteca.DevolverLibro(isbn, usuario);
                Console.WriteLine("✅ Devolución registrada");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        private static void ListarPrestamos()
        {
            Console.WriteLine("\n📋 PRÉSTAMOS ACTIVOS");
            foreach (var p in _biblioteca.ObtenerTodosPrestamos())
            {
                Console.WriteLine($"- {p.Fecha:dd/MM/yyyy}: {p.Usuario} | ISBN: {p.ISBN}");
            }
        }

        private static void BuscarLibros()
        {
            Console.Clear();
            Console.WriteLine("🔍 BUSCAR LIBROS");
            Console.WriteLine("1. Por título");
            Console.WriteLine("2. Por autor");
            Console.WriteLine("3. Por género");
            Console.Write("Seleccione opción: ");

            var termino = LeerCadena("\nIngrese término de búsqueda: ", obligatorio: false);

            var resultados = Console.ReadLine() switch
            {
                "1" => _biblioteca.BuscarLibrosPorTitulo(termino),
                "2" => _biblioteca.BuscarLibrosPorAutor(termino),
                "3" => _biblioteca.BuscarLibrosPorGenero(termino),
                _ => Enumerable.Empty<Libro>()
            };

            Console.WriteLine("\n🔎 RESULTADOS:");
            foreach (var libro in resultados)
            {
                Console.WriteLine($"- {libro.Titulo} ({libro.Autor}) | " +
                                $"Disponibles: {libro.CantidadDisponible}");
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static string LeerCadena(string mensaje, bool obligatorio = true)
        {
            while (true)
            {
                Console.Write(mensaje);
                var input = Console.ReadLine()?.Trim() ?? "";

                if (!obligatorio || !string.IsNullOrWhiteSpace(input))
                    return input;

                Console.WriteLine("Este campo es obligatorio");
            }
        }

        private static int LeerEntero(string mensaje)
        {
            while (true)
            {
                if (int.TryParse(LeerCadena(mensaje), out int resultado))
                    return resultado;

                Console.WriteLine("Debe ingresar un número válido");
            }
        }
    }
}