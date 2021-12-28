using Domain.Common;
using Domain.Common.Contracts;
using Domain.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Data.Common
{
    public class Repository<T> : IRepository<T> where T : Entity, new()
    {
        protected readonly DbSet<T> Set;
        private readonly DbContext dbContext;

        public Repository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.Set = dbContext.Set<T>();
        }

        public T Get(long id)
        {
            return this.Set.SingleOrDefault(x => x.ID == id);
        }

        public void Add(T entity)
        {
            this.Set.Add(entity);
        }

        public void Delete(T entity)
        {
            this.Set.Remove(entity);
        }

        public IList<T> GetAll()
        {
            return this.Set.ToList();
        }

        public IList<T> ExcuteStoredProcedure(string name, params object[] param)
        {
            var result = new List<T>();

            var cmd = this.dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = name;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(param);

            using (var reader = cmd.ExecuteReader())
            {
                result = reader.MapToList<T>();
            }

            return result;
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }
    }
}
