using CustomerManagement.Models.Entities;
using CustomerManagement.ViewModels;

namespace CustomerManagement.Business
{
    public interface ICustomerBusiness : IBaseBusiness<Customer, CustomerViewModel>
    {
        Task<IEnumerable<CustomerViewModel>> GetCustomersByPhoneAsync(string phone);

        Task<CustomerViewModel?> GetCustomerByEmailAsync(string email);

        Task DeleteCustomerByEmailAsync(string email);
    }
}
