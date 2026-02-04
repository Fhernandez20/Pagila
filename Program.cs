using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using PagilaDemo.Models;

namespace PagilaDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            Seleccion();
            Console.WriteLine("\n" + new string('=', 60) + "\n");

            Proyeccion();
            Console.WriteLine("\n" + new string('=', 60) + "\n");

            UnionYDiferencia();
            Console.WriteLine("\n" + new string('=', 60) + "\n");

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


        static void Seleccion()
        {
            Console.WriteLine("Seleccion");

            using (var context = new PagilaContext())
            {
                var seleccion = context.Films
                .Where(f=> f.Rating == "PG-13" && f.Length >120)
                .Select(f=> new { f.Title, f.Rating, f.Length})
                .OrderBy(f=>f.Title)
                .ToList();

                Console.WriteLine($"Numero de registros con una clasificacion PG-13 y duracion mayor a 120 minutos: {seleccion.Count} \n");

                if (seleccion.Count > 0)
                {
                    foreach(var s in seleccion)
                    {
                        Console.WriteLine($"- {s.Title} | {s.Rating} | {s.Length} mins");
                    }
                }
                else
                {
                    Console.WriteLine("No se encontraron registros que cumplan los requisitos");
                }
            }
        }

        static void Proyeccion()
        {
            Console.WriteLine("Proyeccion\n");
            using(var context = new PagilaContext())
            {
                //Tipo anonimo
                var proyeccionAnonima = context.Customers
                .Select(c => new
                {
                    Nombre = c.FirstName,
                    Email = c.Email,
                    Activo= c.Active
                })
                .OrderBy(c=>c.FirstName)
                .ToList();

                foreach(var p in proyeccionAnonima)
                {
                    Console.WriteLine($"- {p.Nombre} | {p.Email} | Activo: {p.Activo}");
                }

                //Con clase DTO
                var proyeccionDTO = context.Customers
                .Select(c=> new ClienteProyeccionDTO
                {
                    Nombre = c.FirstName,
                    Email = c.Email,
                    EstadoActivo = c.Active? "Activo" : "Inactivo"
                })
                .OrderBy(c=>c.FirstName)
                .ToList();

                foreach(var pr in proyeccionDTO)
                {
                    Console.WriteLine($"- {pr.Nombre} | {pr.Email} | {pr.EstadoActivo}");
                }
            }
        }

        //Union
        static void UnionYDiferencia()
        {
            Console.WriteLine("Union y diferencia");

            using (var context = new PagilaContext())
            {
                //spain tiene id de 87
                var ciudadesEspana = context.Cities
                .Where(c=>c.CountryId==87)
                .Select(c=>c.CityName)
                .ToList();

                var ciudadesM = context.Cities
                .Where(c=>c.CityName.StartsWith("M"))
                .Select(c=>c.CityName)
                .ToList();

                var union = ciudadesEspana.
                Union(ciudadesM)
                .OrderBy(c=>c)
                .ToList();

                var diferencia=ciudadesEspana
                .Except(ciudadesM)
                .OrderBy(c=>c)
                .ToList();

                Console.WriteLine($"Numero de ciudades de Spain:  {ciudadesEspana.Count}");
                Console.WriteLine($"Numero de ciudades que empiezan con M: {ciudadesM.Count}");

                Console.WriteLine("Union");
                foreach(var ciudad in union)
                {
                    Console.WriteLine($"- {ciudad}");
                }

                Console.WriteLine("Diferencia");
                foreach(var ciudad in diferencia)
                {
                    Console.WriteLine($"- {ciudad}");
                }

            }
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


    public class ClienteProyeccionDTO
{
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string EstadoActivo { get; set; }  
}
}