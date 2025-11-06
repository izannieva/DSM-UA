using ApplicationCore.Domain.EN;
using System.Collections.Generic;

namespace ApplicationCore.Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Usuario? DamePorOID(long id);
        IEnumerable<Usuario> DameTodos();
        void New(Usuario entity);
        void Modify(Usuario entity);
        void Destroy(Usuario entity);
        void ModifyAll(IEnumerable<Usuario> entities);
    }
}
