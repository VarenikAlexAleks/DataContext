using System;
using PetaPoco;
using Xunit;
using DataContext;
namespace DataContext.UnitTests
{
    public class UnitTestCRUD
    {
        private const string connectionString = "Data Source=test.db";
        [Fact]
        public void Test_ExecuteSql()
        {
            using var data = new DbContext(connectionString, "Microsoft.Data.Sqlite");
            var ret = data.ExecuteSql("select 1");
            Assert.Equal(-1, ret);
        }

        [Fact]
        public void Test_Atomic_CRUD()
        {
            const string name = "Alice";
            using var data = new SqliteContext();
            var person = new Person { Name = name };
            data.Persons.Save(person);
            Assert.True(person.Id > 0);
            var fromDb = data.Persons.All()[0];
            Assert.True(fromDb.Name == name);
            Assert.Equal(1, data.Persons.Delete(person));
        }

        [Fact]
        public void Test_Tx_CRUD()
        {
            const string name = "Alice";
            using var data = new SqliteContext();
            using var tx = data.OpenTx();
            var person = new Person { Name = name };
            data.Persons.Save(person);
            Assert.True(person.Id > 0);
            var fromDb = data.Persons.Get(person.Id);
            Assert.True(fromDb.Name == name);
            Assert.Equal(1, data.Persons.Delete(person));
            tx.Complete();
        }
        
        [Fact]
        public void Test_Tx_SaveOrUpdate()
        {
            const string name = "Alice";
            const string bob = "Bob";
            using var data = new SqliteContext();
            using var tx = data.OpenTx();
            var person = new Person { Name = name };
            data.Persons.Save(person);
            Assert.True(person.Name == name);
            person.Name = bob;
            data.Persons.Save(person);
            var fromDb = data.Persons.Get(person.Id);
            Assert.True(fromDb.Name == bob);
            Assert.Equal(1, data.Persons.Delete(fromDb));
            tx.Complete();
        }
        private class SqliteContext : DbContext
        {
            private Persons persons;
            public Persons Persons => persons ?? new Persons(Db);

            public SqliteContext() : base(connectionString, "Microsoft.Data.Sqlite")
            {
            }
        }

        private class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private class Persons : DataQuery<Person>
        {
            public Persons(IDatabase database) : base(database)
            {
                database.Execute("create table if not exists person (id integer primary key autoincrement, name text)");
            }
        }
    }
}