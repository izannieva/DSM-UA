using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using Infrastructure.NHibernate.Repositories;

namespace Infrastructure.NHibernate.Repositories
{
    public class VehiculoRepository : NHibernateRepositoryBase<Vehiculo>, IVehiculoRepository
    {
        public VehiculoRepository(ISession session) : base(session) { }
    }
}
