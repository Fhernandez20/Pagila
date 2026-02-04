using Microsoft.EntityFrameworkCore;
using PagilaDemo.Models;

namespace PagilaDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            // 1. Producto Cartesiano
            ProductoCartesiano();

            Console.WriteLine("\n" + new string('=', 60) + "\n");

            // 2. Inner Join
            InnerJoinFilmLanguage();

            Console.WriteLine("\n" + new string('=', 60) + "\n");

            // 3. Interseccion
            InterseccionActoresClientes();

            Console.WriteLine("\n\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }

        // Producto Cartesiano
        static void ProductoCartesiano()
        {
            Console.WriteLine("1. Producto Cartesiano");
            using (var context = new PagilaContext())
            {
                // 5 registros
                var categories = context.Categories.Take(5).ToList();
                var staff = context.Staff.ToList();

                Console.WriteLine($"Categorias {categories.Count}");
                Console.WriteLine($"Staff: {staff.Count}");
                Console.WriteLine($"Producto Cartesiano = {categories.Count} × {staff.Count} = {categories.Count * staff.Count} combinaciones\n");

                //Select 
                var productoCartesiano = categories
                    .SelectMany(c => staff, (c, s) => new
                    {
                        Categoria = c.Name,
                        StaffNombre = $"{s.FirstName} {s.LastName}",
                        StaffEmail = s.Email
                    })
                    .ToList();

                Console.WriteLine("Primeras 10 combinaciones:");
                foreach (var item in productoCartesiano.Take(10))
                {
                    Console.WriteLine($"  • {item.Categoria} - {item.StaffNombre} ({item.StaffEmail})");
                }
            }
        }

      // Inner Join
static void InnerJoinFilmLanguage()
{
    Console.WriteLine("2. Inner Join");

    using (var context = new PagilaContext())
    {
        // Join
        var filmConIdioma = context.Films
            .Join(
                context.Languages,                    
                film => film.LanguageId,              
                language => language.LanguageId,     
                (film, language) => new              
                {
                    Titulo = film.Title,
                    Idioma = language.Name,
                    Duracion = film.Length,
                    AñoLanzamiento = film.ReleaseYear
                }
            )
            .Take(15)
            .ToList();

        Console.WriteLine($"Peliculas Encontradas en idioma: {filmConIdioma.Count}\n");

        foreach (var pelicula in filmConIdioma)
        {
            Console.WriteLine($"{pelicula.Titulo}' Idioma: {pelicula.Idioma} - {pelicula.Duracion} minutos - Year: {pelicula.AñoLanzamiento}");
        }
    }
}

        // Interseccion
        static void InterseccionActoresClientes()
        {
            Console.WriteLine("3. Interseccion ");

            using (var context = new PagilaContext())
            {
                // Nombres Actores
                var nombresActores = context.Actors
                    .Select(a => a.FirstName)
                    .Distinct()
                    .ToList();

                // Nombres Clientes
                var nombresClientes = context.Customers
                    .Select(c => c.FirstName)
                    .Distinct()
                    .ToList();

                // Intersect
                var nombresComunes = nombresActores
                    .Intersect(nombresClientes)
                    .OrderBy(n => n)
                    .ToList();

                Console.WriteLine($"Nombres UNICOS en Actores: {nombresActores.Count}");
                Console.WriteLine($"Nombres UNICOS en Clientes: {nombresClientes.Count}");
                Console.WriteLine($"Nombres EN COMUN: {nombresComunes.Count}\n");

                Console.WriteLine("APARECEN EN AMBAS TABLAS:");
                foreach (var nombre in nombresComunes)
                {
                    Console.WriteLine($"  • {nombre}");
                }
            }
        }
    }
}