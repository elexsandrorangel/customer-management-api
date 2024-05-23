using CustomerManagement.Models.Entities;

namespace CustomerManagement.Repository
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetCustomersByPhoneAsync(string ddd, string phone);

        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task<bool> IsEmailRegiseredToAnotherUserAsync(Guid id, string email);
    }
}
