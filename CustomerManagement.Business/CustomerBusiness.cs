using AutoMapper;
using CustomerManagement.Models.Entities;
using CustomerManagement.Repository;
using CustomerManagement.ViewModels;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Business
{
    public class CustomerBusiness : BaseBusiness<Customer, CustomerViewModel, ICustomerRepository>, ICustomerBusiness
    {
        public CustomerBusiness(ICustomerRepository repository, 
            IMapper mapper, 
            ILogger logger) 
            : base(repository, mapper, logger)
        {
        }

        protected override void ValidateInsert(CustomerViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
