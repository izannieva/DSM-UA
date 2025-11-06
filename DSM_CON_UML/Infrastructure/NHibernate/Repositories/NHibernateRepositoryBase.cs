using System.Collections.Generic;
using NHibernate;

namespace Infrastructure.NHibernate.Repositories
{
    public abstract class NHibernateRepositoryBase<T> where T : class
    {
        protected readonly ISession Session;

        protected NHibernateRepositoryBase(ISession session)
        {
            Session = session;
        }

        public virtual T? DamePorOID(long id)
        {
            return Session.Get<T>(id);
        }

        public virtual IEnumerable<T> DameTodos()
        {
            return Session.Query<T>();
        }

        public virtual void New(T entity)
        {
            Session.Save(entity);
        }

        public virtual void Modify(T entity)
        {
            Session.Update(entity);
        }

        public virtual void Destroy(T entity)
        {
            Session.Delete(entity);
        }

        public virtual void ModifyAll(IEnumerable<T> entities)
        {
            foreach (var e in entities)
            {
                Session.Update(e);
            }
        }
    }
}
