using CustomerManagement.Models.Entities;
using CustomerManagement.ViewModels;

namespace CustomerManagement.Business
{
    public interface ICustomerBusiness : IBaseBusiness<Customer, CustomerViewModel>
    {
    }
}
