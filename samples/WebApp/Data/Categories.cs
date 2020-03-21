using PetaPoco;
using DataContext;

namespace WebApp.Data
{
    public class Categories : DataQuery<Category>
    {
        public Categories(IDatabase database) : base(database)
        {
        }
    }
}
