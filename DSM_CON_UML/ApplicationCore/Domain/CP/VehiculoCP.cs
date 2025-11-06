using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using System;

namespace ApplicationCore.Domain.CP
{
    public class VehiculoCP
    {
        private readonly IVehiculoRepository _vehiculoRepo;
        private readonly ICategoriaRepository _categoriaRepo;
        private readonly IUnitOfWork _uow;

        public VehiculoCP(
            IVehiculoRepository vehiculoRepo,
            ICategoriaRepository categoriaRepo,
            IUnitOfWork uow)
        {
            _vehiculoRepo = vehiculoRepo;
            _categoriaRepo = categoriaRepo;
            _uow = uow;
        }

        /// <summary>
        /// Proceso de creación de un vehículo con validación de categoría
        /// </summary>
        public long CrearVehiculoCP(
            string marca, 
            string modelo, 
            int anio, 
            int kilometros,
            string? descripcion,
            decimal precioBase,
            long categoriaId)
        {
            try
            {
                // 1. Verificar que la categoría existe
                var categoria = _categoriaRepo.DamePorOID(categoriaId);
                if (categoria == null)
                    throw new Exception("La categoría especificada no existe");

                // 2. Validar datos del vehículo
                if (anio < 1900 || anio > DateTime.Now.Year + 1)
                    throw new Exception("Año del vehículo inválido");

                if (kilometros < 0)
                    throw new Exception("Kilometraje no puede ser negativo");

                if (precioBase <= 0)
                    throw new Exception("El precio base debe ser mayor que 0");

                // 3. Crear el vehículo
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
                _uow.SaveChanges();

                return vehiculo.IdVehiculo;
            }
            catch (Exception)
            {
                // Si algo falla, la transacción se revierte automáticamente
                throw;
            }
        }

        /// <summary>
        /// Actualiza los kilómetros y el precio base de un vehículo
        /// </summary>
        public void ActualizarInformacionVehiculoCP(
            long vehiculoId,
            int nuevosKilometros,
            decimal nuevoPrecioBase)
        {
            try
            {
                // 1. Verificar que el vehículo existe
                var vehiculo = _vehiculoRepo.DamePorOID(vehiculoId);
                if (vehiculo == null)
                    throw new Exception("Vehículo no encontrado");

                // 2. Validar datos
                if (nuevosKilometros < vehiculo.Kilometros)
                    throw new Exception("Los nuevos kilómetros no pueden ser menores que los actuales");

                if (nuevoPrecioBase <= 0)
                    throw new Exception("El nuevo precio base debe ser mayor que 0");

                // 3. Actualizar información
                vehiculo.Kilometros = nuevosKilometros;
                vehiculo.PrecioBase = nuevoPrecioBase;

                _vehiculoRepo.Modify(vehiculo);
                _uow.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}