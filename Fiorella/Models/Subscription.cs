using System.ComponentModel.DataAnnotations;

namespace Fiorello.Models
{
    public class Subscription : BaseEntity
    {

        [Required]
        public string SubscriptionName { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public string Email { get; set; }
    }
}
