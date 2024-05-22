using CustomerManagement.Models.Entities;
using CustomerManagement.Repository.Contexts;

namespace CustomerManagement.Repository
{
    public class CustomerRepository : BaseRepository<Customer, CustomerManagementContext>, ICustomerRepository
    {
        public CustomerRepository(CustomerManagementContext context) : base(context)
        {
        }
    }
}
