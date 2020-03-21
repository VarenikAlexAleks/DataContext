using System.Collections.Generic;
using PetaPoco;

namespace DataContext
{
    public abstract class DataQuery<T>
    {
        protected readonly IDatabase Database;
        public DataQuery(IDatabase database)
        {
            this.Database = database;
        }
        
        public virtual List<T> All() => Database.Fetch<T>();
        public virtual T Get(object id) => Database.FirstOrDefault<T>("where id = @0", id);
        public virtual void Save(T item) => Database.Save(item);
        public virtual int Delete(T item) => Database.Delete(item);
    }
}
