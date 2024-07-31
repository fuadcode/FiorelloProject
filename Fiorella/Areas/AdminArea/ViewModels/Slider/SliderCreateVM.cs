using System.ComponentModel.DataAnnotations;

namespace Fiorella.Areas.AdminArea.ViewModels.Slider
{
    public class SliderCreateVM
    {
        [Required]
        public IFormFile Photo { get; set; }
    }
}
