using PetaPoco;
using System;
using DataContext;
namespace WebApp.Data
{

    
    public class WebAppData : DbContext
    {
        private Products products;
        public Products Products => products ?? (products = new Products(Db));
        private Categories  categories;
        public Categories Categories  => categories ?? (categories = new Categories(Db));

        public WebAppData(string connectionString, string dataProvider) 
        : base(connectionString, dataProvider)
        {
        }
    }
}
