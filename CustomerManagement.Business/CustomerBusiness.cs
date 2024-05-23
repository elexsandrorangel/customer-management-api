using AutoMapper;
using CustomerManagement.Infra.Core.Exceptions;
using CustomerManagement.Infra.Core.Utils;
using CustomerManagement.Models.Entities;
using CustomerManagement.Repository;
using CustomerManagement.ViewModels;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace CustomerManagement.Business
{
    public class CustomerBusiness : BaseBusiness<Customer, CustomerViewModel, ICustomerRepository>, ICustomerBusiness
    {
        public CustomerBusiness(ICustomerRepository repository, 
            IMapper mapper, 
            ILogger<CustomerBusiness> logger) 
            : base(repository, mapper, logger)
        {
        }

        public override Task<CustomerViewModel> AddAsync(CustomerViewModel t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }

            // clear non-numeric characters from phone numbers            
            if (t.PhoneNumbers.Any())
            {
                foreach (var item in t.PhoneNumbers)
                {
                    item.DDD = Regex.Replace(item.DDD, "[^0-9]", "");
                    item.PhoneNumber = Regex.Replace(item.PhoneNumber, "[^0-9]", "");
                }
            }

            return base.AddAsync(t);
        }


        public async Task DeleteCustomerByEmailAsync(string email)
        {
            var customer = await GetCustomerByEmailAsync(email) ?? throw new AppNotFoundException();

            await Repository.DeleteAsync(customer.Id);
        }

        public async Task<CustomerViewModel?> GetCustomerByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException(nameof(email));
            }

            var data = await Repository.GetCustomerByEmailAsync(email);
            return ModelFromEntity(data);
        }

        public async Task<IEnumerable<CustomerViewModel>> GetCustomersByPhoneAsync(string phone)
        {
            PhoneUtils.GetDddAndNumber(phone, out string ddd, out string number);

            IEnumerable<Customer> data = await Repository.GetCustomersByPhoneAsync(ddd, number);

            return ModelFromEntity(data)!;
        }

        protected override async Task ValidateInsert(CustomerViewModel model)
        {
            if (model == null)
            {
                throw new AppException();
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                throw new InvalidOperationException("Nome requerido");
            }    
            if (string.IsNullOrEmpty(model.Email))
            {
                throw new InvalidOperationException("Email inválido");
            }

            if (await IsEmailRegiseredToAnotherUserAsync(model.Id, model.Email))
            {
                throw new InvalidOperationException("Email já cadastrado");
            }
        }

        private async Task<bool> IsEmailRegiseredToAnotherUserAsync(Guid id, string email)
        {
            return await Repository.IsEmailRegiseredToAnotherUserAsync(id, email);
        }
    }
}
