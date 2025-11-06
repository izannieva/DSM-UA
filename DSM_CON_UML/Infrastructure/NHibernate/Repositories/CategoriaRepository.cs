using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using Infrastructure.NHibernate.Repositories;

namespace Infrastructure.NHibernate.Repositories
{
    public class CategoriaRepository : NHibernateRepositoryBase<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ISession session) : base(session) { }
    }
}
