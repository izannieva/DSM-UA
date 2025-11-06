using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Tool.hbm2ddl;
using Infrastructure.NHibernate;
using NHibernate.Cfg;
using NHibernate;
using Infrastructure.NHibernate.Repositories;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.CP;
using ApplicationCore.Domain.Repositories;

namespace InitializeDb
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("InitializeDb starting...");

            try
            {
                // 1. Configuración inicial
                var dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
                Directory.CreateDirectory(dataDir);
                AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);

                // 2. Configurar NHibernate y crear schema
                var sessionFactory = ConfigureNHibernate();

                // 3. Crear repositorios y servicios
                using (var session = sessionFactory.OpenSession())
                {
                    var uow = new UnitOfWork(session);
                    var usuarioRepo = new UsuarioRepository(session);
                    var anuncioRepo = new AnuncioRepository(session);
                    var vehiculoRepo = new VehiculoRepository(session);
                    var categoriaRepo = new CategoriaRepository(session);
                    var pagoRepo = new PagoRepository(session);

                    var usuarioCEN = new UsuarioCEN(usuarioRepo, uow);
                    var anuncioCEN = new AnuncioCEN(anuncioRepo, uow);
                    var anuncioCP = new AnuncioCP(anuncioRepo, vehiculoRepo, usuarioRepo, pagoRepo, uow);

                    // 4. Crear datos de prueba
                    Console.WriteLine("Creando datos de prueba...");

                    // 4.1 Crear usuarios
                    var userId1 = usuarioCEN.Crear("Juan Pérez", "juan@test.com", "password123", "666111222");
                    var userId2 = usuarioCEN.Crear("Ana García", "ana@test.com", "password456", "666333444");
                    Console.WriteLine($"Usuarios creados con IDs: {userId1}, {userId2}");

                    // 4.2 Probar login
                    var usuarioLogin = usuarioCEN.Login("juan@test.com", "password123");
                    Console.WriteLine($"Login exitoso: {usuarioLogin?.Nombre ?? "Login fallido"}");

                    // 4.3 Crear categoría
                    var categoria = new Categoria { Nombre = "Deportivos" };
                    categoriaRepo.New(categoria);
                    uow.SaveChanges();
                    
                    // Asegurarnos de que la categoría se ha creado correctamente
                    var categoriaCreada = categoriaRepo.DamePorOID(categoria.IdCategoria);
                    if (categoriaCreada == null)
                        throw new Exception("Error al crear la categoría");

                    // 4.4 Publicar anuncio con vehículo (CustomTransaction)
                    var anuncioId = anuncioCP.PublicarAnuncioConVehiculoCP(
                        "Ferrari", "F40", 1992, 50000, 
                        "Clásico en perfecto estado", 250000M, categoria.IdCategoria,
                        "Ferrari F40 - Único en España", userId1);
                    Console.WriteLine($"Anuncio creado con ID: {anuncioId}");

                    // 4.5 Probar operaciones custom CEN
                    anuncioCEN.AplicarDescuento(anuncioId, 10);
                    Console.WriteLine("Descuento aplicado al anuncio");

                    anuncioCEN.RenovarAnuncio(anuncioId);
                    Console.WriteLine("Anuncio renovado");

                    // 4.6 Probar filtros
                    var anunciosUsuario = anuncioCEN.ReadFilterByUsuario((int)userId1);
                    var anunciosActivos = anuncioCEN.ReadFilterByEstado("Activo");
                    var anunciosFerrari = anuncioCEN.ReadFilterByMarca("Ferrari");
                    var anunciosCategoria = anuncioCEN.ReadFilterByCategoria((int)categoria.IdCategoria);

                    Console.WriteLine($"Filtros probados - Resultados: " +
                        $"Por usuario: {anunciosUsuario.Count()}, " +
                        $"Activos: {anunciosActivos.Count()}, " +
                        $"Ferrari: {anunciosFerrari.Count()}, " +
                        $"Por categoría: {anunciosCategoria.Count()}");

                    // 4.7 Completar una venta (CustomTransaction)
                    anuncioCP.CompletarVentaCP(anuncioId, userId2, 225000M, "Transferencia");
                    Console.WriteLine("Venta completada");

                    Console.WriteLine("InitializeDb completado exitosamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en InitializeDb: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        private static ISessionFactory ConfigureNHibernate()
        {
            var cfg = new Configuration();
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "hibernate.cfg.xml");
            if (File.Exists(configPath)) 
                cfg.Configure(configPath);
            else 
                cfg.Configure();

            // Añadir mappings
            var mapDir = Path.Combine(Directory.GetCurrentDirectory(), "..", "Infrastructure", "NHibernate", "Mappings");
            if (Directory.Exists(mapDir))
            {
                foreach (var f in Directory.GetFiles(mapDir, "*.hbm.xml")) 
                    cfg.AddFile(f);
            }

            // Crear schema
            var export = new SchemaExport(cfg);
            export.Execute(false, true, false);

            return cfg.BuildSessionFactory();
        }
    }
}
