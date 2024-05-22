namespace CustomerManagement.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public ICollection<PhoneViewModel> PhoneNumbers { get; set; } = new List<PhoneViewModel>();
    }
}
