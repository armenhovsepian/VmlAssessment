
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Vml.Core.Dtos;
using Vml.Core.Enums;
using Vml.Core.Interfaces.V1;

namespace Vml.Api.Controllers.V1
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    public class DataProcessorController : ControllerBase
    {
        private readonly ILogger<DataProcessorController> _logger;
        private readonly IDataProcesserService _dataProcessorService;

        public DataProcessorController(ILogger<DataProcessorController> logger,
            IDataProcesserService dataProcessorService)
        {
            _logger = logger;
            _dataProcessorService = dataProcessorService;
        }

        /// <summary>
        /// Get all data jobs
        /// </summary>
        /// <param name="pageNumber">Current page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of Data jobs</returns>
        /// <response code="200">List of dataJob was successfully retrieved.</response>
        [ProducesResponseType(typeof(IEnumerable<DataJobDTO>), 200)]
        [HttpGet("/GetAllDataJobs")]
        public async Task<ActionResult<IEnumerable<DataJobDTO>>> GetAllDataJobs([FromQuery] int? pageNumber, [FromQuery] int? pageSize, CancellationToken cancellationToken)
        {
            if (pageNumber == null || pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize == null || pageSize < 10)
            {
                pageSize = 10;
            }

            return Ok(await _dataProcessorService.GetAllDataJobs(pageNumber.GetValueOrDefault(), pageSize.GetValueOrDefault(), cancellationToken));
        }

        /// <summary>
        /// Get all data jobs filtered by status
        /// </summary>
        /// <param name="pageNumber">Current page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="status">The status of the datajob (new = 0, processing = 1, Completed = 2)</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of Data jobs</returns>
        /// <response code="200">List of dataJob was successfully retrieved.</response>
        [ProducesResponseType(typeof(IEnumerable<DataJobDTO>), 200)]
        [HttpGet("/GetDataJobsByStatus")]
        public async Task<ActionResult<IEnumerable<DataJobDTO>>> GetDataJobsByStatus([FromQuery] int? pageNumber, [FromQuery] int? pageSize, DataJobStatus status, CancellationToken cancellationToken)
        {
            if (pageNumber == null || pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize == null || pageSize < 10)
            {
                pageSize = 10;
            }

            return Ok(await _dataProcessorService.GetDataJobsByStatus(pageNumber.GetValueOrDefault(), pageSize.GetValueOrDefault(), status, cancellationToken));
        }

        /// <summary>
        /// Gets a single dataJob.
        /// </summary>
        /// <param name="id">The requested dataJob identifier (e.g. 3fa85f64-5717-4562-b3fc-2c963f66afa6)</param>
        /// <returns>The requested dataJob.</returns>
        /// <response code="200">The dataJob was successfully retrieved.</response>
        /// <response code="404">The dataJob does not exist.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataJobDTO), 200)]
        [ProducesResponseType(404)]
        [HttpGet("/GetDataJob/{id:guid}")]
        public async Task<ActionResult<DataJobDTO>> GetDataJob(Guid id, CancellationToken cancellationToken)
        {
            var result = await _dataProcessorService.GetDataJob(id, cancellationToken);
            if (result.IsNotFound())
            {
                _logger.LogInformation($"The dataJob does not exist Id: {id}, {result.Errors.FirstOrDefault()}");

                return NotFound();
            }

            return Ok(result.Value);
        }


        /// <summary>
        /// Create a new DataJob
        /// </summary>
        /// <param name="dataJob">DataJob object</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="200">The dataJob was successfully created.</response>
        /// <response code="404">Bad request/Ivalid parameters</response>
        [ProducesResponseType(typeof(DataJobDTO), 200)]
        [ProducesResponseType(400)]
        [HttpPost("/Create")]
        public async Task<ActionResult<DataJobDTO>> Create(DataJobDTO dataJob, CancellationToken cancellationToken)
        {
            var result = await _dataProcessorService.Create(dataJob, cancellationToken);
            if (result.IsInvalid())
            {
                _logger.LogInformation($"Invalid request, {result.Errors.FirstOrDefault()}");
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Update dataJob by GUID
        /// </summary>
        /// <param name="id">GUID of the dataJob</param>
        /// <param name="dataJob">DataJob object to be updated</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="204">The dataJob was successfully updated.</response>
        /// <response code="404">The dataJob does not exist.</response>
        [Produces("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [HttpPut("/Update/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, DataJobDTO dataJob, CancellationToken cancellationToken)
        {
            var result = await _dataProcessorService.Update(dataJob, cancellationToken);
            if (result.IsNotFound())
            {
                return NotFound();
            }

            if (result.IsInvalid())
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete dataJob by GUID
        /// </summary>
        /// <param name="dataJobID">GUID of the dataJob</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="204">The dataJob was successfully deleted.</response>
        /// <response code="404">The dataJob does not exist.</response>
        [Produces("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [HttpDelete("/Delete/{dataJobID:guid}")]
        public async Task<ActionResult> Delete(Guid dataJobID, CancellationToken cancellationToken)
        {
            var result = await _dataProcessorService.Delete(dataJobID, cancellationToken);
            if (result.IsNotFound())
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Start DataJob by GUID
        /// </summary>
        /// <param name="dataJobId">GUID of the dataJob</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="202">The dataJob was successfully started.</response>
        /// <response code="404">The dataJob does not exist.</response>
        /// <response code="400">Invali request.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [HttpPatch("/StartBackgroundProcess/{dataJobId:guid}")]
        public async Task<ActionResult<bool>> StartBackgroundProcess(Guid dataJobId, CancellationToken cancellationToken)
        {
            var result = await _dataProcessorService.StartBackgroundProcess(dataJobId, cancellationToken);
            if (result.IsNotFound())
            {
                return NotFound();
            }

            if (result.IsForbidden())
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get dataJob status By GUID
        /// </summary>
        /// <param name="dataJobId">GUID of the dataJob</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="200">The dataJob status was successfully retrieved.</response>
        /// <response code="404">The dataJob does not exist.</response>
        [ProducesResponseType(typeof(DataJobStatus), 200)]
        [ProducesResponseType(404)]
        [HttpGet("/GetBackgroundProcessStatus/{dataJobId:guid}")]
        public async Task<ActionResult<DataJobStatus>> GetBackgroundProcessStatus(Guid dataJobId, CancellationToken cancellationToken)
        {
            var result = await _dataProcessorService.GetBackgroundProcessStatus(dataJobId, cancellationToken);

            if (result.IsNotFound())
            {
                return NotFound();
            }

            return Ok(result.Value);
        }

        [HttpGet("/GetBackgroundProcessResults/{dataJobId:guid}")]
        public async Task<ActionResult<List<string>>> GetBackgroundProcessResults(Guid dataJobId, CancellationToken cancellationToken)
        {
            throw new Exception("To Test Error handler");
        }
    }

}
