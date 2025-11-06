using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Domain.CEN
{
    public class AnuncioCEN
    {
        private readonly IAnuncioRepository _anuncioRepo;
        private readonly IUnitOfWork _uow;

        public AnuncioCEN(IAnuncioRepository anuncioRepo, IUnitOfWork uow)
        {
            _anuncioRepo = anuncioRepo;
            _uow = uow;
        }

        // CRUD Operations

        public long Crear(string titulo, System.DateTime fechaPublicacion, string estado, 
            decimal precioVenta, long usuarioId, long vehiculoId)
        {
            var anuncio = new Anuncio
            {
                Titulo = titulo,
                FechaPublicacion = fechaPublicacion,
                Estado = estado,
                PrecioVenta = precioVenta,
                UsuarioId = usuarioId,
                VehiculoId = vehiculoId
            };

            _anuncioRepo.New(anuncio);
            _uow.SaveChanges();
            return anuncio.IdAnuncio;
        }

        public Anuncio? ReadOID(long id)
        {
            return _anuncioRepo.DamePorOID(id);
        }

        public IEnumerable<Anuncio> ReadAll()
        {
            return _anuncioRepo.DameTodos();
        }

        public void Modify(long id, string titulo, string estado, decimal precioVenta)
        {
            var anuncio = _anuncioRepo.DamePorOID(id);
            if (anuncio == null)
                throw new System.Exception("Anuncio no encontrado");

            anuncio.Titulo = titulo;
            anuncio.Estado = estado;
            anuncio.PrecioVenta = precioVenta;

            _anuncioRepo.Modify(anuncio);
            _uow.SaveChanges();
        }

        public void Destroy(long id)
        {
            var anuncio = _anuncioRepo.DamePorOID(id);
            if (anuncio == null)
                throw new System.Exception("Anuncio no encontrado");

            _anuncioRepo.Destroy(anuncio);
            _uow.SaveChanges();
        }

        // Filter Operations

        /// <summary>
        /// Filtra anuncios por usuario
        /// </summary>
        /// <param name="usuarioId">ID del usuario</param>
        /// <returns>Lista de anuncios del usuario</returns>
        public IEnumerable<Anuncio> ReadFilterByUsuario(int usuarioId)
        {
            return _anuncioRepo.DameTodos()
                .Where(a => a.UsuarioId == usuarioId);
        }

        /// <summary>
        /// Filtra anuncios por estado
        /// </summary>
        /// <param name="estado">Estado del anuncio (ej: "Activo", "Vendido", "Destacado")</param>
        /// <returns>Lista de anuncios en ese estado</returns>
        public IEnumerable<Anuncio> ReadFilterByEstado(string estado)
        {
            return _anuncioRepo.DameTodos()
                .Where(a => a.Estado.ToLower() == estado.ToLower());
        }

        /// <summary>
        /// Filtra anuncios por marca del vehículo
        /// </summary>
        /// <param name="marca">Marca del vehículo</param>
        /// <returns>Lista de anuncios de vehículos de esa marca</returns>
        public IEnumerable<Anuncio> ReadFilterByMarca(string marca)
        {
            return _anuncioRepo.DameTodos()
                .Where(a => a.Vehiculo != null && 
                           a.Vehiculo.Marca.ToLower() == marca.ToLower());
        }

        /// <summary>
        /// Filtra anuncios por categoría del vehículo
        /// </summary>
        /// <param name="categoriaId">ID de la categoría</param>
        /// <returns>Lista de anuncios de vehículos en esa categoría</returns>
        public IEnumerable<Anuncio> ReadFilterByCategoria(int categoriaId)
        {
            return _anuncioRepo.DameTodos()
                .Where(a => a.Vehiculo != null && 
                           a.Vehiculo.CategoriaId == categoriaId);
        }

        // Custom Operations (no CRUD)

        /// <summary>
        /// Renueva un anuncio actualizando su fecha de publicación
        /// </summary>
        public void RenovarAnuncio(long anuncioId)
        {
            var anuncio = _anuncioRepo.DamePorOID(anuncioId);
            if (anuncio == null)
                throw new System.Exception("Anuncio no encontrado");

            anuncio.FechaPublicacion = System.DateTime.Now;
            _anuncioRepo.Modify(anuncio);
            _uow.SaveChanges();
        }

        /// <summary>
        /// Marca un anuncio como vendido y registra el precio final de venta
        /// </summary>
        public void MarcarComoVendido(long anuncioId, decimal precioFinalVenta)
        {
            var anuncio = _anuncioRepo.DamePorOID(anuncioId);
            if (anuncio == null)
                throw new System.Exception("Anuncio no encontrado");

            anuncio.Estado = "Vendido";
            anuncio.PrecioVenta = precioFinalVenta;
            _anuncioRepo.Modify(anuncio);
            _uow.SaveChanges();
        }

        /// <summary>
        /// Actualiza el precio de un anuncio con un descuento específico
        /// </summary>
        public void AplicarDescuento(long anuncioId, decimal porcentajeDescuento)
        {
            if (porcentajeDescuento < 0 || porcentajeDescuento > 100)
                throw new System.ArgumentException("El porcentaje de descuento debe estar entre 0 y 100");

            var anuncio = _anuncioRepo.DamePorOID(anuncioId);
            if (anuncio == null)
                throw new System.Exception("Anuncio no encontrado");

            decimal factor = 1 - (porcentajeDescuento / 100);
            anuncio.PrecioVenta = anuncio.PrecioVenta * factor;
            _anuncioRepo.Modify(anuncio);
            _uow.SaveChanges();
        }
    }
}