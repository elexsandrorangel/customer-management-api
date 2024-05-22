namespace CustomerManagement.ViewModels
{
    public class PhoneViewModel : BaseViewModel
    {
        public string DDD { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public PhoneType PhoneType { get; set; }

    }

    public enum PhoneType
    {
        Fixo,
        Movel
    }
}
