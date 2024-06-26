﻿using CustomerManagement.Infra.Core.Exceptions;
using CustomerManagement.Models.Entities;
using CustomerManagement.Repository.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CustomerManagement.Repository
{
    public class CustomerRepository : BaseRepository<Customer, CustomerManagementContext>, ICustomerRepository
    {
        public CustomerRepository(CustomerManagementContext context) : base(context)
        {
        }

        #region Overrides

        public override async Task<IEnumerable<Customer>> GetAsync(int page = 0, int qty = int.MaxValue, bool track = false)
        {
            if (track)
            {
                return await Context.Customers.Include(c => c.PhoneNumbers)
                    .OrderBy(a => a.CreatedAt).Skip(page * qty)
                    .Take(qty).ToListAsync();
            }
            return await Context.Customers.Include(c => c.PhoneNumbers)
                .AsNoTracking()
                .OrderBy(a => a.CreatedAt).Skip(page * qty)
                .Take(qty).ToListAsync();
        }

        public override async Task<IEnumerable<Customer>> GetAsync(Expression<Func<Customer, bool>> match, int page = 0, int qty = int.MaxValue, bool track = false)
        {
            if (track)
            {
                return await Context.Customers.Include(c => c.PhoneNumbers)
                    .Where(match)
                    .OrderBy(a => a.CreatedAt)
                    .Skip(page * qty).Take(qty).ToListAsync();
            }
            return await Context.Customers.Include(c => c.PhoneNumbers)
                .Where(match)
                .AsNoTracking()
                .OrderBy(a => a.CreatedAt)
                .Skip(page * qty).Take(qty).ToListAsync();
        }

        public override async Task<Customer?> GetSingleOrDefaultAsync(Expression<Func<Customer, bool>> match, bool track = false)
        {
            if (track)
            {
                return await Context.Customers.Include(c => c.PhoneNumbers).FirstOrDefaultAsync(match);
            }
            return await Context.Customers.Include(c => c.PhoneNumbers).AsNoTracking().FirstOrDefaultAsync(match);
        }

        #endregion Overrides
        
        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await Context.Customers.Include(c => c.PhoneNumbers).AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IEnumerable<Customer>> GetCustomersByPhoneAsync(string ddd, string number)
        {
            var customers = await Context.Phones.Include(p => p.Customer).AsNoTracking()
                .Where(p => p.DDD == ddd && p.PhoneNumber == number)
                .Select(p => p.Customer).ToListAsync();

            return customers.DistinctBy(c => c.Id).ToList();
        }

        public async Task<bool> IsEmailRegiseredToAnotherUserAsync(Guid id, string email)
        {
            return await Context.Customers.AsNoTracking().AnyAsync(c => c.Email == email && c.Id != id);
        }

        public async Task DeletePhoneAsync(Guid customerId, Guid phoneId)
        {
            var customer = await Context.Customers.Include(p => p.PhoneNumbers)
                .FirstOrDefaultAsync(c => c.Id == customerId) ?? throw new AppNotFoundException();

            var existingPhone = customer.PhoneNumbers.FirstOrDefault(p => p.Id == phoneId);
            if (existingPhone != null)
            {
                customer.PhoneNumbers.Remove(existingPhone);
            }

            await UpdateAsync(customer);
        }
    }
}
