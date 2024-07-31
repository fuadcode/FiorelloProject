using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.AdminArea.ViewModels.Category
{
    public class CategoryCreateVM
    {
        [Required, MaxLength(10)]
        [DisplayName("CategoryName")]
        public string Name { get; set; }
        [MaxLength(1000)]
        [DisplayName("CategoryDesc")]

        public string Desc { get; set; }
    }
}
