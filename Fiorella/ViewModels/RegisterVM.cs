using System.ComponentModel.DataAnnotations;

namespace Fiorello.ViewModels
{
    public class RegisterVM
    {
        [Required, MaxLength(100)]
        public string UserName { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, MaxLength(100), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, MaxLength(100), DataType(DataType.Password), Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}
