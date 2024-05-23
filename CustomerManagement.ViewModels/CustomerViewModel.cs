using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public ICollection<PhoneViewModel> PhoneNumbers { get; set; } = new List<PhoneViewModel>();
    }
}
