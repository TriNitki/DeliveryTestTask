using Delivery.Audit.Logger;
using Delivery.Service.Infrastructure;
using Delivery.UseCases.Orders.Commands.Create;
using Delivery.UseCases.Orders.Commands.Upload;
using Delivery.UseCases.Orders.Queries;
using Delivery.UseCases.Utils.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.Service.Controllers;

[Route("api")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAuditLogger _logger;

    public OrdersController(IMediator mediator, IAuditLogger logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetDeliveryOrders(
        [FromQuery] Guid districtId, [FromQuery] DateTime? firstDeliveryDateTime = null, [FromQuery] DateTime? lastDeliveryDateTime = null)
    {
        var result = await _mediator.Send(new GetDeliveryOrderQuery(districtId, firstDeliveryDateTime, lastDeliveryDateTime));
        _logger.Log(result.ToLog("GetOrders"));
        return result.ToActionResult();
    }

    [HttpPost("order")]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand request)
    {
        var result = await _mediator.Send(request);
        _logger.Log(result.ToLog("CreateOrder"));
        return result.ToActionResult();
    }

    [HttpPost("orders/upload")]
    public async Task<IActionResult> UploadOrdersInJson(IFormFile file)
    {
        var result = await _mediator.Send(new UploadOrdersCommand(file.OpenReadStream()));
        _logger.Log(result.ToLog("UploadOrders"));
        return result.ToActionResult();
    }
}