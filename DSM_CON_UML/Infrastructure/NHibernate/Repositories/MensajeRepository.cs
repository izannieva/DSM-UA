using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using Infrastructure.NHibernate.Repositories;

namespace Infrastructure.NHibernate.Repositories
{
    public class MensajeRepository : NHibernateRepositoryBase<Mensaje>, IMensajeRepository
    {
        public MensajeRepository(ISession session) : base(session) { }
    }
}
