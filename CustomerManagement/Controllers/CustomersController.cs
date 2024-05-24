using CustomerManagement.Business;
using CustomerManagement.Controllers.Base;
using CustomerManagement.Models.Entities;
using CustomerManagement.ViewModels;
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

        [HttpGet]
        [Route("phone/{phone}")]
        [ProducesResponseType(typeof(IEnumerable<CustomerViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> GetByPhoneAsync(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return BadRequest();
            }

            var customers = await Business.GetCustomersByPhoneAsync(phone);

            return Ok(customers);
        }

        [HttpGet]
        [Route("email/{email}")]
        [ProducesResponseType(typeof(CustomerViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }

            var customer = await Business.GetCustomerByEmailAsync(email);

            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpDelete]
        [Route("email/{email}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }

            await Business.DeleteCustomerByEmailAsync(email);

            return NoContent();
        }


        /// <summary>
        /// Update the specified record
        /// </summary>
        /// <param name="id">Record identifier</param>
        /// <param name="model">Record data</param>
        /// <response code="200">Request successfull</response>
        /// <response code="400">Request has missing/invalid values</response>
        /// <response code="404">Record not found</response>
        /// <response code="500">Oops! An error occurred</response>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:guid}")]
        [ProducesResponseType(typeof(CustomerViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Edit(Guid id, [FromBody] CustomerUpdateViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustomerViewModel? data = await Business.UpdatePhoneAndEmailAsync(id, model);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data!);
        }


        [HttpDelete]
        [Route("{id:guid}/phone/{phoneId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> DeletePhone(Guid id, Guid phoneId)
        {
            if (id == Guid.Empty || phoneId == Guid.Empty)
            {
                return BadRequest();
            }

            await Business.DeletePhoneAsync(id, phoneId);

            return NoContent();
        }
    }
}
