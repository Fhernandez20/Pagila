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

            Console.WriteLine("---------------------------------------------\n");


            // 2. Inner Join
            InnerJoinFilmLanguage();

            Console.WriteLine("---------------------------------------------\n");

            // 3. Interseccion
            InterseccionActoresClientes();

            Console.WriteLine("---------------------------------------------\n");

            // 4. Agrupamiento
            AgrupamientoYAgregacion();

            Console.WriteLine("---------------------------------------------\n");

            // 5. Left Outer Join
            LeftOuterJoin();

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

                //Select 
                var productoCartesiano = categories
                    .SelectMany(c => staff, (c, s) => new
                    {
                        Categoria = c.Name,
                        StaffNombre = $"{s.FirstName} {s.LastName}",
                    })
                    .ToList();

                Console.WriteLine("10 combinaciones:");
                foreach (var item in productoCartesiano.Take(10))
                {
                    Console.WriteLine($"  • {item.Categoria} - {item.StaffNombre} ");
                }
            }
        }

        // Inner Join
        static void InnerJoinFilmLanguage()
        {

            Console.WriteLine("2. Inner Join");

            using (var context = new PagilaContext())
            {
                var filmConIdioma = context.Films
            .Join(
                context.Languages,
                film => film.LanguageId,
                language => language.LanguageId,
                (film, language) => new
                {
                    Titulo = film.Title,
                    Idioma = language.Name
                }
            )
            .Take(15)
            .ToList();

                Console.WriteLine($"Peliculas Encontradas Con idioma original: {filmConIdioma.Count}\n");

                foreach (var pelicula in filmConIdioma)
                {
                    Console.WriteLine($"{pelicula.Titulo} Idioma: {pelicula.Idioma}");
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

                Console.WriteLine("APARECEN EN AMBAS TABLAS:");
                foreach (var nombre in nombresComunes)
                {
                    Console.WriteLine($"{nombre}");
                }
            }
        }

        static void AgrupamientoYAgregacion()
        {
            Console.WriteLine("4. AGRUPAMIENTO Y AGREGACION");
            Console.WriteLine("Analizar pagos agrupados por cliente\n");

            using (var context = new PagilaContext())
            {
                var mejoresClientes = context.PaymentP202201s
                    .GroupBy(p => p.CustomerId)
                    .Select(g => new
                    {
                        ClienteId = g.Key,
                        TotalGastado = g.Sum(x => x.Amount),
                        PromedioPago = g.Average(x => x.Amount),
                        CantidadRentas = g.Count()
                    })
                    .OrderByDescending(x => x.TotalGastado)
                    .Take(10)
                    .ToList();

                Console.WriteLine("Top 10:\n");

                foreach (var c in mejoresClientes)
                {
                    Console.WriteLine($"Cliente {c.ClienteId}: " +
                                      $"Total={c.TotalGastado:F2}, " +
                                      $"Promedio={c.PromedioPago:F2}, " +
                                      $"Rentas={c.CantidadRentas}");
                }
            }
        }

        static void LeftOuterJoin()
        {
            Console.WriteLine("5. LEFT OUTER JOIN\n");

            using (var context = new PagilaContext())
            {
                var peliculasSinCopias = context.Films
                    .GroupJoin(
                        context.Inventories,
                        f => f.FilmId,
                        i => i.FilmId,
                        (f, invs) => new { Film = f, Invs = invs }
                    )
                    .SelectMany(
                        //REVISA SI ES NULLLLL
                        x => x.Invs.DefaultIfEmpty(),
                        (x, inv) => new { x.Film, Inv = inv }
                    )
                    //REVISA SI NO HAY COPIA
                    .Where(x => x.Inv == null)
                    .Select(x => new { x.Film.FilmId, x.Film.Title })
                    .ToList();

                foreach (var p in peliculasSinCopias)
                    Console.WriteLine($"{p.FilmId} - {p.Title}");
            }
        }
    }
}