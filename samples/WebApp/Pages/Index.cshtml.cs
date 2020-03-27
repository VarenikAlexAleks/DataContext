using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using DataApp.Data;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly AppData data;
        public PaginatedList<ProductWithCategory> Products;
        public List<Category> Categories;

        [BindProperty]
        public Category AddCategory { get; set; }

        [BindProperty]
        public Product AddProduct { get; set; }
        public string NameSort { get; set; } //= "name";
        public IndexModel(ILogger<IndexModel> logger, AppData data)
        {
            _logger = logger;
            this.data = data;
        }

        public void OnGet(string sortOrder, long? pageIndex, int? changeOrder)
        {
            if (string.IsNullOrWhiteSpace(sortOrder) || changeOrder.HasValue)
                sortOrder = sortOrder == "name" ? "name desc" : "name";
            NameSort = sortOrder;
            Categories = data.Categories.All();
            var items = data.Products.Page(pageIndex ?? 1, 5, NameSort);
            var records = items.Items.Select(e => new ProductWithCategory
            {
                Id = e.Id,
                Name = e.Name,
                CategoryId = e.CategoryId,
                Category = Categories.Where(c => c.Id == e.CategoryId).Select(c => c.Name).SingleOrDefault()
            });

            Products = new PaginatedList<ProductWithCategory>(records, items.TotalPages, items.CurrentPage);
        }

        public IActionResult OnPostAddCategory()
        {
            data.Categories.Save(AddCategory);
            return RedirectToPage();
        }
        public IActionResult OnPostAddProduct()
        {
            data.Products.Save(AddProduct);
            return RedirectToPage();
        }

        public class ProductWithCategory : Product
        {
            public string Category { get; set; }
        }
    }
}
