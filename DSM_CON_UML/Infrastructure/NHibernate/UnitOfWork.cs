using NHibernate;
using System;
using ApplicationCore.Domain.Repositories;

namespace Infrastructure.NHibernate
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISession _session;
        private ITransaction? _transaction;

        public UnitOfWork(ISession session)
        {
            _session = session;
        }

        public void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        public void CommitTransaction()
        {
            try
            {
                if (_transaction?.IsActive == true)
                {
                    _transaction.Commit();
                }
            }
            catch
            {
                if (_transaction?.IsActive == true)
                {
                    _transaction.Rollback();
                }
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                if (_transaction?.IsActive == true)
                {
                    _transaction.Rollback();
                }
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void SaveChanges()
        {
            var wasActive = _transaction?.IsActive == true;
            if (!wasActive)
            {
                BeginTransaction();
            }

            try
            {
                _session.Flush();
                if (!wasActive)
                {
                    CommitTransaction();
                }
            }
            catch
            {
                if (!wasActive && _transaction?.IsActive == true)
                {
                    RollbackTransaction();
                }
                throw;
            }
        }

        public void Dispose()
        {
            if (_transaction?.IsActive == true)
            {
                RollbackTransaction();
            }
            _transaction?.Dispose();
            _session.Dispose();
        }
    }
}