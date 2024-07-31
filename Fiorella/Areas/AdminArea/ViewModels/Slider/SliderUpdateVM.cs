using System.ComponentModel.DataAnnotations;

namespace Fiorella.Areas.AdminArea.ViewModels.Slider
{
    public class SliderUpdateVM
    {

        [Required]
        public IFormFile Photo { get; set; }
        public string ImageUrl { get; set; }
    }
}
