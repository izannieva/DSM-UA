using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using System;

namespace ApplicationCore.Domain.CP
{
    public class AnuncioCP
    {
        private readonly IAnuncioRepository _anuncioRepo;
        private readonly IVehiculoRepository _vehiculoRepo;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IPagoRepository _pagoRepo;
        private readonly IUnitOfWork _uow;

        public AnuncioCP(
            IAnuncioRepository anuncioRepo, 
            IVehiculoRepository vehiculoRepo, 
            IUsuarioRepository usuarioRepo,
            IPagoRepository pagoRepo,
            IUnitOfWork uow)
        {
            _anuncioRepo = anuncioRepo;
            _vehiculoRepo = vehiculoRepo;
            _usuarioRepo = usuarioRepo;
            _pagoRepo = pagoRepo;
            _uow = uow;
        }

        /// <summary>
        /// Proceso completo de publicación de un anuncio con su vehículo asociado
        /// </summary>
        public long PublicarAnuncioConVehiculoCP(
            // Datos del vehículo
            string marca, string modelo, int anio, int kilometros, 
            string? descripcion, decimal precioBase, long categoriaId,
            // Datos del anuncio
            string titulo, long usuarioId)
        {
            try
            {
                // 1. Crear el vehículo
                var vehiculo = new Vehiculo
                {
                    Marca = marca,
                    Modelo = modelo,
                    Anio = anio,
                    Kilometros = kilometros,
                    Descripcion = descripcion,
                    PrecioBase = precioBase,
                    CategoriaId = categoriaId
                };

                _vehiculoRepo.New(vehiculo);

                // 2. Crear el anuncio asociado
                var anuncio = new Anuncio
                {
                    Titulo = titulo,
                    FechaPublicacion = DateTime.Now,
                    Estado = "Activo",
                    PrecioVenta = precioBase, // Inicialmente igual al precio base
                    UsuarioId = usuarioId,
                    VehiculoId = vehiculo.IdVehiculo
                };

                _anuncioRepo.New(anuncio);

                // 3. Guardar todos los cambios en una única transacción
                _uow.SaveChanges();

                return anuncio.IdAnuncio;
            }
            catch (Exception)
            {
                // Si algo falla, la transacción se revierte automáticamente
                throw;
            }
        }

        /// <summary>
        /// Proceso completo de finalización de venta de un vehículo
        /// </summary>
        public void CompletarVentaCP(long anuncioId, long compradorId, decimal precioFinal, string tipoPago)
        {
            try
            {
                // 1. Verificar que el anuncio existe y está activo
                var anuncio = _anuncioRepo.DamePorOID(anuncioId);
                if (anuncio == null)
                    throw new Exception("Anuncio no encontrado");
                if (anuncio.Estado == "Vendido")
                    throw new Exception("El anuncio ya está marcado como vendido");

                // 2. Verificar que el comprador existe
                var comprador = _usuarioRepo.DamePorOID(compradorId);
                if (comprador == null)
                    throw new Exception("Comprador no encontrado");

                // 3. Crear el registro de pago
                var pago = new Pago
                {
                    FechaPago = DateTime.Now,
                    Cantidad = precioFinal,
                    TipoPago = tipoPago,
                    UsuarioId = compradorId,
                    AnuncioId = anuncioId
                };

                _pagoRepo.New(pago);

                // 4. Actualizar el estado del anuncio
                anuncio.Estado = "Vendido";
                anuncio.PrecioVenta = precioFinal;
                _anuncioRepo.Modify(anuncio);

                // 5. Guardar todos los cambios en una única transacción
                _uow.SaveChanges();
            }
            catch (Exception)
            {
                // Si algo falla, la transacción se revierte automáticamente
                throw;
            }
        }
    }
}
