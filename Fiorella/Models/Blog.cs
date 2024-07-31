using System.ComponentModel.DataAnnotations.Schema;

namespace Fiorello.Models
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public string ImgUrl { get; set; }
        [NotMapped]
        public string ShortDesc => Desc.Length > 50 ? Desc.Substring(0, 50) : Desc;
    }
}
