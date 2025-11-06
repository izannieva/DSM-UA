using ApplicationCore.Domain.EN;
using System.Collections.Generic;

namespace ApplicationCore.Domain.Repositories
{
    public interface IMensajeRepository
    {
        Mensaje? DamePorOID(long id);
        IEnumerable<Mensaje> DameTodos();
        void New(Mensaje entity);
        void Modify(Mensaje entity);
        void Destroy(Mensaje entity);
        void ModifyAll(IEnumerable<Mensaje> entities);
    }
}
