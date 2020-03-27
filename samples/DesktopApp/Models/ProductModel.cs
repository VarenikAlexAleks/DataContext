using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataApp.Data;

namespace DesktopApp.Models
{
    public class ProductModel : Product
    {
        [Display( Name = "Категория")]
        public string CategoryName {get; set;}

    }
}