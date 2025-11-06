using ApplicationCore.Domain.EN;
using System.Collections.Generic;

namespace ApplicationCore.Domain.Repositories
{
    public interface IPagoRepository
    {
        Pago? DamePorOID(long id);
        IEnumerable<Pago> DameTodos();
        void New(Pago entity);
        void Modify(Pago entity);
        void Destroy(Pago entity);
        void ModifyAll(IEnumerable<Pago> entities);
    }
}
