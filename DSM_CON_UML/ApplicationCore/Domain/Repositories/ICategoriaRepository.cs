using ApplicationCore.Domain.EN;
using System.Collections.Generic;

namespace ApplicationCore.Domain.Repositories
{
    public interface ICategoriaRepository
    {
        Categoria? DamePorOID(long id);
        IEnumerable<Categoria> DameTodos();
        void New(Categoria entity);
        void Modify(Categoria entity);
        void Destroy(Categoria entity);
        void ModifyAll(IEnumerable<Categoria> entities);
    }
}
