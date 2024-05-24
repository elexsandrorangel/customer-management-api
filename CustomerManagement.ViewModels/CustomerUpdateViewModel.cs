using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.ViewModels
{
    public class CustomerUpdateViewModel
    {

        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public ICollection<PhoneViewModel> PhoneNumbers { get; set; } = new List<PhoneViewModel>();
    }
}
