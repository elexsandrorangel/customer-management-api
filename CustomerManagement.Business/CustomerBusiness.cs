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

        public CustomerViewModel ClearPhoneNumbers(CustomerViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            // clear non-numeric characters from phone numbers            
            if (model.PhoneNumbers.Any())
            {
                foreach (var item in model.PhoneNumbers)
                {
                    item.DDD = Regex.Replace(item.DDD, "[^0-9]", "").TrimStart('0');
                    item.PhoneNumber = Regex.Replace(item.PhoneNumber, "[^0-9]", "");
                }
            }

            return model;
        }

        public override Task<CustomerViewModel> AddAsync(CustomerViewModel t)
        {
            t = ClearPhoneNumbers(t);
            return base.AddAsync(t);
        }

        public override Task<CustomerViewModel> UpdateAsync(CustomerViewModel updated)
        {
            updated = ClearPhoneNumbers(updated);
            return base.UpdateAsync(updated);
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

        public async Task<CustomerViewModel?> UpdatePhoneAndEmailAsync(Guid id, CustomerUpdateViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var customer = await GetAsync(id);

            if (customer == null)
            {
                throw new ArgumentNullException($"{nameof(customer)} is null");
            }

            if (!string.IsNullOrEmpty(model.Email))
            {
                customer.Email = model.Email;
            }

            if (model.PhoneNumbers != null && model.PhoneNumbers.Any())
            {
                foreach (var item in model.PhoneNumbers)
                {
                    if (item.Id == Guid.Empty)
                    {
                        customer.PhoneNumbers.Add(item);
                    }
                    else
                    {
                        var existingPhone = customer.PhoneNumbers.FirstOrDefault(p => p.Id == item.Id);
                        if (existingPhone != null)
                        {
                            existingPhone.DDD = item.DDD;
                            existingPhone.PhoneNumber = item.PhoneNumber;
                            existingPhone.PhoneType = item.PhoneType;
                        }
                        else
                        {
                            // Remove provided ID to avoid colisions
                            item.Id = Guid.Empty;
                            customer.PhoneNumbers.Add(item);
                        }
                    }
                }
            }

            return await UpdateAsync(customer);
        }

        protected override async Task ValidateInsert(CustomerViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
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

        public async Task DeletePhoneAsync(Guid customerId, Guid phoneId)
        {
            await Repository.DeletePhoneAsync(customerId, phoneId);
        }
    }
}
