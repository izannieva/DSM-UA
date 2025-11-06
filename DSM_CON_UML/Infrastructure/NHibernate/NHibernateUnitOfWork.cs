using System;
using NHibernate;
using ApplicationCore.Domain.Repositories;

namespace Infrastructure.NHibernate
{
    public class NHibernateUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ISession _session;
        private ITransaction? _transaction;

        public NHibernateUnitOfWork(ISession session)
        {
            _session = session;
            _transaction = _session.BeginTransaction();
        }

        public void SaveChanges()
        {
            if (_transaction != null && !_transaction.WasCommitted && !_transaction.WasRolledBack)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = _session.BeginTransaction();
            }
        }

        public void Dispose()
        {
            try
            {
                if (_transaction != null)
                {
                    if (!_transaction.WasCommitted && !_transaction.WasRolledBack)
                        _transaction.Rollback();
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
            finally
            {
                _session?.Dispose();
            }
        }
    }
}
