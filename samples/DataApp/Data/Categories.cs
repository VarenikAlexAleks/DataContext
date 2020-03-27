using PetaPoco;
using DataContext;

namespace DataApp.Data
{
    public class Categories : DataQuery<Category>
    {
        public Categories(IDatabase database) : base(database)
        {
        }
    }
}
