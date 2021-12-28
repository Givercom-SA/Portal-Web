using System.Collections.Generic;

namespace Domain.Common.Contracts
{
    public interface IRepository<T> where T : Entity
    {
        T Get(long id);

        void Add(T entity);

        void Delete(T entity);

        IList<T> GetAll();

        IList<T> ExcuteStoredProcedure(string name, params object[] param);

        void Commit();
    }
}
