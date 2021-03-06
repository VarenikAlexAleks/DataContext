using PetaPoco;
using System;
using System.Collections.Generic;
using DataContext;

namespace DataApp.Data
{
    public class Products : DataQuery<Product>
    {
        public Products(IDatabase database) : base(database)
        {
        }

        public  override List<Product> All() => base.All();
      
        public  Page<Product> Page(long page, long size, string orderBy)
        {
          return Database.Page<Product>(page, size, "order by " + orderBy);
        }

        internal List<Product> ByCategoryId(int id) => Database.Fetch<Product>("where categoryId = @0", id);

        public IEnumerable<Product> ByName(string term)
        {
            return Database.Query<Product>("where  lower(name) like @0", term?.ToLowerInvariant());
        }
    }
}
