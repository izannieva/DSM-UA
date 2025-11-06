using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using Infrastructure.NHibernate.Repositories;

namespace Infrastructure.NHibernate.Repositories
{
    public class PagoRepository : NHibernateRepositoryBase<Pago>, IPagoRepository
    {
        public PagoRepository(ISession session) : base(session) { }
    }
}
