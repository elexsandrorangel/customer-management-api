using CustomerManagement.Business;
using CustomerManagement.Controllers;
using CustomerManagement.Infra.Core.Exceptions;
using CustomerManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CustomerManagement.Test
{
    public class CustomersControllerTest
    {
        #region Get

        [Fact]
        public async Task CustomersController_Get_All_NoResult()
        {
            var mockBusiness = new Mock<ICustomerBusiness>();

            mockBusiness.Setup(x => x.GetAsync(1, int.MaxValue))
                .Returns(Task.FromResult(new List<CustomerViewModel>().AsEnumerable()));

            var controller = new CustomersController(mockBusiness.Object);

            var result = await controller.Get(page: 1, qty: int.MaxValue);

            Assert.IsAssignableFrom<OkObjectResult>(result);
            var responseObj = (result as OkObjectResult)?.Value as IEnumerable<CustomerViewModel>;

            Assert.NotNull(responseObj);
            Assert.Empty(responseObj);
        }

        [Fact]
        public async Task CustomersController_Get_All_NoResult_NullList()
        {
            var mockBusiness = new Mock<ICustomerBusiness>();

            mockBusiness.Setup(x => x.GetAsync(1, int.MaxValue))
                .Returns(Task.FromResult((IEnumerable<CustomerViewModel>)null));

            var controller = new CustomersController(mockBusiness.Object);

            var result = await controller.Get(page: 100, qty: int.MaxValue);

            Assert.IsAssignableFrom<OkObjectResult>(result);
            var responseObj = (result as OkObjectResult)?.Value as IEnumerable<CustomerViewModel>;

            Assert.NotNull(responseObj);
            Assert.Empty(responseObj);
        }

        [Fact]
        public async Task CustomersController_Get_All_Customers()
        {
            var mockBusiness = new Mock<ICustomerBusiness>();

            var fakeList = new List<CustomerViewModel>()
            {
                new CustomerViewModel(),
                new CustomerViewModel()
            };

            mockBusiness.Setup(x => x.GetAsync(1, int.MaxValue))
                .Returns(Task.FromResult(fakeList.AsEnumerable()));

            var controller = new CustomersController(mockBusiness.Object);

            var result = await controller.Get(page: 1, qty: int.MaxValue);

            Assert.IsAssignableFrom<OkObjectResult>(result);
            var responseObj = (result as OkObjectResult)?.Value as IEnumerable<CustomerViewModel>;
            Assert.NotNull(responseObj);
            Assert.NotEmpty(responseObj);
            Assert.Equal(2, responseObj.Count());
        }


        [Fact]
        public async Task CustomersController_Get_ById_ReturnsNotFound()
        {
            var mockBusiness = new Mock<ICustomerBusiness>();

            mockBusiness.Setup(x => x.GetAsync(It.IsAny<Guid>(), false))
                .Returns(Task.FromResult((CustomerViewModel?)null));

            var controller = new CustomersController(mockBusiness.Object);

            var result = await controller.Get(id: Guid.NewGuid());

            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Fact]
        public async Task CustomersController_Get_ById_ReturnsCustomer()
        {
            var mockBusiness = new Mock<ICustomerBusiness>();

            mockBusiness.Setup(x => x.GetAsync(It.IsAny<Guid>(), false))
                .Returns(Task.FromResult(new CustomerViewModel()));

            var controller = new CustomersController(mockBusiness.Object);

            var result = await controller.Get(id: Guid.NewGuid());

            Assert.IsAssignableFrom<OkObjectResult>(result);
            var responseObj = (result as OkObjectResult)?.Value as CustomerViewModel;

            Assert.NotNull(responseObj);
        }

        #endregion Get

        #region Create

        [Fact]
        public void CustomersController_Create_NullObjectPassed_ShoudThrowException()
        {
            var mockBusiness = new Mock<ICustomerBusiness>();

            CustomerViewModel? fakeObj = null;
            mockBusiness.Setup(x => x.AddAsync(fakeObj)).Throws<ArgumentNullException>();

            var controller = new CustomersController(mockBusiness.Object);

            var ex = Assert.Throws<AggregateException>(() => controller.CreateAsync(null).Result);
            Assert.NotNull(ex.InnerException);
            Assert.IsType<ArgumentNullException>(ex.InnerException);
        }

        [Fact]
        public async Task CustomersController_Create_InvalidObjectPassed_Shoud_Return_BadRequest()
        {
            // Arrange
            var emptyCustomer = new CustomerViewModel();

            var mockBusiness = new Mock<ICustomerBusiness>();

            mockBusiness.Setup(x => x.AddAsync(new CustomerViewModel())).Throws<Exception>();
            var controller = new CustomersController(mockBusiness.Object);

            var result = await controller.CreateAsync(emptyCustomer);
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        #endregion Create

        #region Delete

        [Fact]
        public async Task CustomersController_Delete_When_Email_Is_Empty_Returns_BadRequest()
        {
            var mockBusiness = new Mock<ICustomerBusiness>();
            var controller = new CustomersController(mockBusiness.Object);

            string? email = null;

            var result = await controller.DeleteByEmailAsync(email);
            Assert.IsAssignableFrom<BadRequestResult>(result);

            string email2 = "";
            result = await controller.DeleteByEmailAsync(email2);
            Assert.IsAssignableFrom<BadRequestResult>(result);

        }

        [Fact]
        public async Task CustomersController_Delete_Customer_When_Email_Is_Valid()
        {
            var mockBusiness = new Mock<ICustomerBusiness>();
            var controller = new CustomersController(mockBusiness.Object);

            string? email = "jhon@doe.com";

            var result = await controller.DeleteByEmailAsync(email);
            mockBusiness.Verify(b => b.DeleteCustomerByEmailAsync(email), Times.Once());

            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public void CustomersController_Delete_Customer_When_NotFound_Error()
        {
            var mockBusiness = new Mock<ICustomerBusiness>();

            string email = "jhon2@doe.com";
            mockBusiness.Setup(x => x.DeleteCustomerByEmailAsync(email)).Throws<AppNotFoundException>();
            var controller = new CustomersController(mockBusiness.Object);

            var ex = Assert.Throws<AggregateException>(() => controller.DeleteByEmailAsync(email).Result);

            Assert.NotNull(ex.InnerException);
            Assert.IsType<AppNotFoundException>(ex.InnerException);
        }

        #endregion Delete
    }
}