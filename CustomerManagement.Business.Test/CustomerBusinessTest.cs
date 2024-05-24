using AutoMapper;
using CustomerManagement.Infra.Core.Exceptions;
using CustomerManagement.Models.Entities;
using CustomerManagement.Repository;
using CustomerManagement.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;

namespace CustomerManagement.Business.Test
{
    public class CustomerBusinessTest
    {
        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CustomerBusiness>> _loggerMock;
        private readonly CustomerBusiness _customerBusiness;

        public CustomerBusinessTest()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CustomerBusiness>>();
            _customerBusiness = new CustomerBusiness(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        #region Get

        [Fact]
        public async Task CustomerBusiness_Get_Customer_ThrowArgumentException_When_Email_Is_Empty()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _customerBusiness.GetCustomerByEmailAsync(null));
            await Assert.ThrowsAsync<ArgumentException>(() => _customerBusiness.GetCustomerByEmailAsync(string.Empty));
        }


        [Fact]
        public async Task CustomerBusiness_Get_Customer_ThrowArgumentException_When_Phone_Is_Empt()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _customerBusiness.GetCustomersByPhoneAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => _customerBusiness.GetCustomersByPhoneAsync(string.Empty));
        }

        [Fact]
        public async Task CustomerBusiness_Get_ByEmail_Should_Return_Data_When_Customer_Exists()
        {
            var email = "john@test.com";
            var customer = new Customer { Id = Guid.NewGuid(), Email = email };
            var customerViewModel = new CustomerViewModel { Id = customer.Id, Email = customer.Email };

            _repositoryMock.Setup(r => r.GetCustomerByEmailAsync(email)).ReturnsAsync(customer);
            _mapperMock.Setup(m => m.Map<CustomerViewModel>(customer)).Returns(customerViewModel);

            // Act
            var result = await _customerBusiness.GetCustomerByEmailAsync(email);

            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
            Assert.Equal(customer.Id, result.Id);
        }


        [Fact]
        public async Task CustomerBusiness_Get_ByPhone_Should_Return_Data_When_Phone_Exists()
        {
            var phone = "4134567890";
            var customers = new List<Customer>
            {
                new Customer 
                { 
                    Id = Guid.NewGuid(),
                    PhoneNumbers = new List<Phones> 
                    {
                        new Phones { DDD = "41", PhoneNumber = "34567890", PhoneType = Models.Entities.PhoneType.Fixo },
                        new Phones { DDD = "41", PhoneNumber = "987654321", PhoneType = Models.Entities.PhoneType.Movel }
                    } 
                }
            };
            
            var customerViewModels = new List<CustomerViewModel>
            {
                new CustomerViewModel 
                { 
                    Id = customers.First().Id, 
                    PhoneNumbers = new List<PhoneViewModel> 
                    { 
                        new PhoneViewModel
                        { 
                            DDD = "41", 
                            PhoneNumber = "34567890",
                            PhoneType = ViewModels.PhoneType.Fixo
                        },
                        new PhoneViewModel { DDD = "41", PhoneNumber = "987654321", PhoneType = ViewModels.PhoneType.Movel }
                    } 
                }
            };

            _repositoryMock.Setup(r => r.GetCustomersByPhoneAsync("41", "34567890")).ReturnsAsync(customers);
            _mapperMock.Setup(m => m.Map<IEnumerable<CustomerViewModel>>(customers)).Returns(customerViewModels);

            var result = await _customerBusiness.GetCustomersByPhoneAsync(phone);

            Assert.NotNull(result.FirstOrDefault());
            Assert.Equal(2, result.FirstOrDefault()!.PhoneNumbers.Count);
        }

        #endregion Get

        #region

        [Fact]
        public async Task CustomerBusiness_Create_Should_Throw_AppException_When_Model_Is_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _customerBusiness.AddAsync((CustomerViewModel?)null));
        }

        [Fact]
        public async Task CustomerBusiness_Create_Should_Throw_InvalidOperationException_When_Name_Is_Empty()
        {
            var model = new CustomerViewModel { Name = "", Email = "john@test.com" };
            await Assert.ThrowsAsync<InvalidOperationException>(() => _customerBusiness.AddAsync(model));
        }


        [Fact]
        public async Task CustomerBusiness_Delete_By_Email_Should_ThrowNotFoundException_When_CustomerDoesNotExist()
        {
            var email = "test@example.com";
            _repositoryMock.Setup(r => r.GetCustomerByEmailAsync(email)).ReturnsAsync((Customer)null);

            await Assert.ThrowsAsync<AppNotFoundException>(() => _customerBusiness.DeleteCustomerByEmailAsync(email));
        }

        [Fact]
        public void CustomerBusiness_ClearPhones_Should_ThrowArgumentNullException_When_CustomerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _customerBusiness.ClearPhoneNumbers(null));
        }

        [Fact]
        public void CustomerBusiness_ClearPhones_Should_ThrowArgumentNullException_When_Phone_Or_DDD_IsNull()
        {
            var model = new CustomerViewModel
            {
                PhoneNumbers = new List<PhoneViewModel>
                {
                    new PhoneViewModel { DDD = null, PhoneNumber = "3-456-7890" },
                    new PhoneViewModel { DDD = "0(41)", PhoneNumber = "3456-7890" }
                }
            };

            var model1 = new CustomerViewModel
            {
                PhoneNumbers = new List<PhoneViewModel>
                {
                    new PhoneViewModel { DDD = "55", PhoneNumber = "3-456-7890" },
                    new PhoneViewModel { DDD = "0(41)", PhoneNumber = null }
                }
            };

            Assert.Throws<ArgumentNullException>(() => _customerBusiness.ClearPhoneNumbers(model));
            Assert.Throws<ArgumentNullException>(() => _customerBusiness.ClearPhoneNumbers(model1));
        }

        [Fact]
        public void CustomerBusiness_ClearPhones_Should_Clear_Phones_Successfull()
        {
            var model = new CustomerViewModel
            {
                PhoneNumbers = new List<PhoneViewModel>
                {
                    new PhoneViewModel { DDD = "(51)", PhoneNumber = "3-456-7890" },
                    new PhoneViewModel { DDD = "0(41)", PhoneNumber = "3456-7890" }
                }
            };
            
            var result = _customerBusiness.ClearPhoneNumbers(model);

            Assert.NotNull(result);
            Assert.True(result.PhoneNumbers.Any());
            Assert.Equal(2, result.PhoneNumbers.Count);

            var phone1 = result.PhoneNumbers.First();
            var phone2 = result.PhoneNumbers.ElementAt(1);

            Assert.Equal("51", phone1.DDD);
            Assert.Equal("34567890", phone1.PhoneNumber);

            Assert.Equal("41", phone2.DDD);
            Assert.Equal("34567890", phone2.PhoneNumber);

        }
        #endregion
    }
}