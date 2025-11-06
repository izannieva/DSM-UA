using ApplicationCore.Domain.EN;
using System.Collections.Generic;

namespace ApplicationCore.Domain.Repositories
{
    public interface IAnuncioRepository
    {
        Anuncio? DamePorOID(long id);
        IEnumerable<Anuncio> DameTodos();
        void New(Anuncio entity);
        void Modify(Anuncio entity);
        void Destroy(Anuncio entity);
        void ModifyAll(IEnumerable<Anuncio> entities);
    }
}
