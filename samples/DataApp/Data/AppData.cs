using PetaPoco;
using System;
using DataContext;
namespace DataApp.Data
{

    
    public class AppData : DbContext
    {
        private Products products;
        public Products Products => products ?? (products = new Products(Db));
        private Categories  categories;
        public Categories Categories  => categories ?? (categories = new Categories(Db));

        public AppData(string connectionString, string dataProvider) 
        : base(connectionString, dataProvider)
        {
        }

        public static AppData InitDb()
        {
            var db = new AppData("DataSource=test.db", "Microsoft.Data.Sqlite");

            using( var tx = db.OpenTx())
            {
                db.ExecuteSql("create table if not exists Category (ID INTEGER PRIMARY KEY  AUTOINCREMENT, Name text)");    
                db.ExecuteSql("create table if not exists Product (id integer primary key autoincrement, name text, categoryid int)"); 

                var food = new Category{ Name= "Еда"};
                var closes = new Category{ Name= "Одежда"};

                db.Categories.Save(food);
                db.Categories.Save(closes);

                db.Products.Save( new Product {Name = "Колбаса", CategoryId = food.Id });
                db.Products.Save( new Product {Name = "Шапка", CategoryId = closes.Id });
                db.Products.Save( new Product {Name = "Хлеб", CategoryId = food.Id });
                db.Products.Save( new Product {Name = "Штаны", CategoryId = closes.Id });

                tx.Complete();
            }
            return db;
        }
    }
}
