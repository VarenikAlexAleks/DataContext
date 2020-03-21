using PetaPoco;
using System;

namespace DataContext
{
    public class DbContext : IDisposable
    {
        private readonly IDatabase db;
        protected IDatabase Db => db;
        
        public DbContext(string connectionString, string dataProvider)
        {
            db = new Database(connectionString, dataProvider);
        }

        public ITransaction OpenTx()
        {
            return db.GetTransaction();
        }

        public int ExecuteSql(string sql, params object[] args)
        {
            return db.Execute(sql, args);
        }
        
        public void Dispose()
        {
            db.Dispose();   
        }
    }
}
