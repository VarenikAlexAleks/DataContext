using System.Collections.ObjectModel;
using DataApp.Data;
namespace Desktop.ViewModels
{
    public class AddProductViewModel
    {
        public string Name {get; set; } = "Edit to do";
        public ObservableCollection<Category> CategoryList { get; }
        public Category SelectedCategory { get; set; }
        private readonly AppData ctx;
        public AddProductViewModel(AppData ctx)
        {
            this.ctx = ctx;
            CategoryList = new ObservableCollection<Category>(ctx.Categories.All());
        }

        public void Save()
        {
            if (SelectedCategory == null)
                return;
            
            ctx.Products.Save(new Product{Name = this.Name, CategoryId = SelectedCategory.Id});
        }
    }
}