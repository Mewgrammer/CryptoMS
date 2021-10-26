using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Opw.HttpExceptions;
using Shared.Communication.Commands;
using Shared.Communication.Queries;
using Shared.Communication.Responses;

namespace CsrStorage.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/csr/storage")]
    [Authorize]
    public class StorageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StorageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///  Gets all stored CSRs
        /// </summary>
        /// <returns>All stored CSRs</returns>
        /// <response code="200">Returns all stored CSRs</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<CsrResponse>> Get()
        {
            return await _mediator.Send(new CsrCollectionQuery());
        }

        /// <summary>
        ///  Gets a stored CSR by ID
        /// </summary>
        /// <param name="id">The Id of the stored CSR</param>
        /// <returns>A stored CSR</returns>
        /// <response code="200">Returns the stored CSR</response>
        /// <response code="404">If no stored CSR with the id exists</response>            
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<CsrResponse> GetById(Guid id)
        {
            try
            {
                return await _mediator.Send(new CsrByIdQuery {Id = id});
            }
            catch (InvalidOperationException e)
            {
                throw new NotFoundException("Csr with id does not exist", e);
            }
        }

        /// <summary>
        ///  Gets a stored CSR by ID
        /// </summary>
        /// <param name="csr">The CSR to be stored in base64 pem format</param>
        /// <returns>A stored CSR</returns>
        /// <response code="201">Returns the created CSR</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<CsrResponse> Post([FromBody] string csr)
        {
            return await _mediator.Send(new StoreCsrCommand {Csr = csr});
        }

        /// <summary>
        ///  Deletes a stored CSR by ID
        /// </summary>
        /// <param name="id">The Id of the stored CSR</param>
        /// <response code="200">If the CSR was deleted</response>
        /// <response code="404">If no stored CSR with the id exists</response>            
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteCsrCommand {Id = id});
            }
            catch (InvalidOperationException e)
            {
                throw new NotFoundException("Csr with id does not exist", e);
            }
        }
    }
}