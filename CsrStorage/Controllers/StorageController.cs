using AutoMapper;
using Contracts.CsrStorage;
using CsrStorage.Services;
using Microsoft.AspNetCore.Mvc;

namespace CsrStorage.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/csr/storage")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly CsrStorageService _storage;
        private readonly IMapper _mapper;

        public StorageController(CsrStorageService storage, IMapper mapper)
        {
            _storage = storage;
            _mapper = mapper;
        }

        /// <summary>
        ///  Gets all stored CSRs
        /// </summary>
        /// <returns>All stored CSRs</returns>
        /// <response code="200">Returns all stored CSRs</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<CertificateRequestDto>> Get()
        {
            var entities = await _storage.Find();
            return entities.Select(csr => _mapper.Map<CertificateRequestDto>(csr));
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
        public async Task<CertificateRequestDto?> GetById(Guid id)
        {
            try
            {
                return _mapper.Map<CertificateRequestDto>(await _storage.FindOne(id));
            }
            catch (InvalidOperationException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return null;
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
        public async Task<CertificateRequestDto> Post([FromBody] string csr)
        {
            return _mapper.Map<CertificateRequestDto>(await _storage.AddCsr(csr));
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
                await _storage.DeleteCsr(id);
            }
            catch (InvalidOperationException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }
    }
}