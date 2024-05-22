using CustomerManagement.Business;
using CustomerManagement.Controllers.Base;
using CustomerManagement.Models.Entities;
using CustomerManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : BaseApiController<ICustomerBusiness, Customer, CustomerViewModel>
    {
        public CustomersController(ICustomerBusiness business) : base(business)
        {
        }
    }
}
