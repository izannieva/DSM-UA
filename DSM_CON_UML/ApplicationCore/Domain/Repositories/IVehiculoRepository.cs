using ApplicationCore.Domain.EN;
using System.Collections.Generic;

namespace ApplicationCore.Domain.Repositories
{
    public interface IVehiculoRepository
    {
        Vehiculo? DamePorOID(long id);
        IEnumerable<Vehiculo> DameTodos();
        void New(Vehiculo entity);
        void Modify(Vehiculo entity);
        void Destroy(Vehiculo entity);
        void ModifyAll(IEnumerable<Vehiculo> entities);
    }
}
