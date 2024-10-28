using Delivery.Audit.Logger;
using Delivery.Service.Infrastructure;
using Delivery.UseCases.District.Commands.Create;
using Delivery.UseCases.District.Commands.Upload;
using Delivery.UseCases.Utils.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.Service.Controllers;

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

    [HttpPost("district")]
    public async Task<IActionResult> CreateDistrict(CreateDistrictCommand request)
    {
        var result = await _mediator.Send(request);
        _logger.Log(result.ToLog("CreateDistrict"));
        return result.ToActionResult();
    }

    [HttpPost("districts/upload")]
    public async Task<IActionResult> UploadDistrictsInJson(IFormFile file)
    {
        var result = await _mediator.Send(new UploadDistrictsCommand(file.OpenReadStream()));
        _logger.Log(result.ToLog("UploadDistricts"));
        return result.ToActionResult();
    }
}