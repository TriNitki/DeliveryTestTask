using Delivery.Audit.Logger;
using Delivery.Contracts;
using Delivery.Service.Infrastructure;
using Delivery.UseCases.District.Commands.Create;
using Delivery.UseCases.District.Commands.Upload;
using Delivery.UseCases.Utils.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.Service.Controllers;

/// <summary>
/// District management
/// </summary>
[Route("api")]
[ApiController]
public class DistrictsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAuditLogger _logger;

    public DistrictsController(IMediator mediator, IAuditLogger logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Create district
    /// </summary>
    /// <param name="request">Request</param>
    /// <response code="201"> District successfully created </response>
    /// <response code="400"> Invalid request passed </response>
    [HttpPost("district")]
    [ProducesResponseType(typeof(DistrictModel), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> CreateDistrict(CreateDistrictCommand request)
    {
        var result = await _mediator.Send(request);
        _logger.Log(result.ToLog("CreateDistrict"));
        return result.ToActionResult();
    }

    /// <summary>
    /// Upload districts from file
    /// </summary>
    /// <param name="file">File</param>
    /// <response code="200"> Districts successfully uploaded </response>
    /// <response code="400"> Invalid file </response>
    [HttpPost("districts/upload")]
    [ProducesResponseType(typeof(UploadModel<DistrictModel>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> UploadDistrictsFromFile(IFormFile file)
    {
        var result = await _mediator.Send(new UploadDistrictsCommand(file.OpenReadStream()));
        _logger.Log(result.ToLog("UploadDistricts"));
        return result.ToActionResult();
    }
}