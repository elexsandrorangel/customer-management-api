using CustomerManagement.Business;
using CustomerManagement.Models;
using CustomerManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Controllers.Base
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {

    }

    public abstract class BaseApiController<T> : BaseApiController
        where T : IBaseBusiness
    {
        /// <summary>
        /// Application business implementation
        /// </summary>
        protected readonly T Business;

        protected BaseApiController(T business)
        {
            Business = business;
        }
    }

    public abstract class BaseApiController<TBusiness, TEntity, TModel> : BaseApiController<TBusiness>
        where TEntity : BaseEntity
        where TModel : BaseViewModel
        where TBusiness : IBaseBusiness<TEntity, TModel>
    {
        protected BaseApiController(TBusiness business)
            : base(business)
        {
        }

        /// <summary>
        /// Retrieves a list of records
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="page">Page number</param>
        /// <param name="qty">Itens per page</param>
        /// <response code="200">Request successfull</response>
        /// <response code="400">Request has missing/invalid values</response>
        /// <response code="500">Oops! An error occurred</response>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:guid?}")]
        [Route("page/{page:int?}/items/{qty:int}")]
        [Route("{id:int?}/page/{page:int}/items/{qty:int}")]
        [ProducesResponseType(typeof(IEnumerable<BaseViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Get(Guid? id = null, int page = 0, int qty = int.MaxValue)
        {
            if (id.HasValue)
            {
                TModel? model = await Business.GetAsync(id.Value, false);

                if (model == null)
                {
                    return NotFound();
                }

                return Ok(model);
            }

            IEnumerable<TModel> list = await Business.GetAsync(page, qty);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="model">Model to persist</param>
        /// <response code="201">Record created successfully</response>
        /// <response code="400">Request has missing/invalid values</response>
        /// <response code="500">Oops! An error occurred</response>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(BaseViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> CreateAsync([FromBody] TModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TModel data = await Business.AddAsync(model);
            
            if (data == null)
            {
                return BadRequest();
            }

            string url = $"{Request.Scheme}://{Request.Host}{Request.Path}/{data.Id}";
            return Created(url, data);
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
        [ProducesResponseType(typeof(BaseViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Edit(Guid id, [FromBody] TModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TModel? data = await Business.UpdateAsync(model);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data!);
        }

        /// <summary>
        /// Remove the record from database
        /// </summary>
        /// <param name="id">Record identifier</param>
        /// <response code="204">Request successfull</response>
        /// <response code="400">Request has missing/invalid values</response>
        /// <response code="404">Record not found</response>
        /// <response code="500">Oops! An error occurred</response>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            await Business.DeleteAsync(id);
            return NoContent();
        }
    }

}
