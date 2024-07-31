using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.AdminArea.ViewModels.SliderVMs
{
    public class SliderListVM
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}
