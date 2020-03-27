using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using DesktopApp.Models;
using DataApp.Data;
using Avalonia.Controls.Templates;
using Avalonia;
using Avalonia.Controls;
using Desktop.ViewModels;
using ReactiveUI;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace DesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ObservableAsPropertyHelper<IEnumerable<ProductModel>> _searchResults;
        public IEnumerable<ProductModel> SearchResults => _searchResults.Value;
        public string Term
        {
            get => searchTextBox;
            set 
            {
                Console.WriteLine(value);
                this.RaiseAndSetIfChanged(ref searchTextBox, value);
            }
        }
        private readonly AppData ctx;
        private string searchTextBox;

        public MainWindowViewModel()
        {
            ctx = DataApp.Data.AppData.InitDb();
            _searchResults = this
           .WhenAnyValue(x => x.Term)
           .Throttle(TimeSpan.FromMilliseconds(800))
           .Select(term => term?.Trim())
           .DistinctUntilChanged()
           .Where(term => !string.IsNullOrWhiteSpace(term))
           .SelectMany(SearchProducts)
           .ObserveOn(RxApp.MainThreadScheduler)
           .ToProperty(this, x => x.SearchResults);
        }

        private async Task<IEnumerable<ProductModel>> SearchProducts(string term, CancellationToken token)
        {
            var products = ctx.Products.ByName(term);
            var categories = ctx.Categories.All();
            return products.Select(p => new ProductModel { Id = p.Id, Name = p.Name, CategoryId = p.CategoryId, CategoryName = categories.SingleOrDefault(c => c.Id == p.Id)?.Name });
        }

        public async void AddProduct(Window parent)
        {
            var locator = Application.Current.DataTemplates[0] as IDataTemplate;
            var child = new AddProductViewModel(ctx);
            var dialog = (Window)locator.Build(child);
            dialog.DataContext = child;
            await dialog.ShowDialog(parent);

        }
    }
}
