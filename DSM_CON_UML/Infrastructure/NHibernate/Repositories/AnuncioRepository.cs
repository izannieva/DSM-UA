using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using Infrastructure.NHibernate.Repositories;

namespace Infrastructure.NHibernate.Repositories
{
    public class AnuncioRepository : NHibernateRepositoryBase<Anuncio>, IAnuncioRepository
    {
        public AnuncioRepository(ISession session) : base(session) { }
    }
}
