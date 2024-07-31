using System.ComponentModel.DataAnnotations;

namespace Fiorello.Models
{
    public class SliderContent : BaseEntity
    {
        [Required, StringLength(50)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Desc { get; set; }
        public string ImgUrl { get; set; }
    }
}
