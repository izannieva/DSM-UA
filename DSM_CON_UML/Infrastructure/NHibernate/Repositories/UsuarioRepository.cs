using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using Infrastructure.NHibernate.Repositories;
using System.Collections.Generic;

namespace Infrastructure.NHibernate.Repositories
{
    public class UsuarioRepository : NHibernateRepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ISession session) : base(session) { }
    }
}
