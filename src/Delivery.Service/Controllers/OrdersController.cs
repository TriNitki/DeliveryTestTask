using Delivery.Audit.Logger;
using Delivery.Contracts;
using Delivery.Service.Infrastructure;
using Delivery.UseCases.Orders.Commands.Create;
using Delivery.UseCases.Orders.Commands.Upload;
using Delivery.UseCases.Orders.Queries;
using Delivery.UseCases.Utils.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.Service.Controllers;

/// <summary>
/// Order management
/// </summary>
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

    /// <summary>
    /// Get delivery orders
    /// </summary>
    /// <param name="districtId">District id</param>
    /// <param name="firstDeliveryDateTime">First delivery date time</param>
    /// <param name="lastDeliveryDateTime">Last delivery date time</param>
    /// <response code="200"> Success </response>
    /// <response code="400"> Invalid query values passed </response>
    [HttpGet("orders")]
    [ProducesResponseType(typeof(DeliveryOrderModel), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> GetDeliveryOrders(
        [FromQuery] Guid districtId, [FromQuery] DateTime? firstDeliveryDateTime = null, [FromQuery] DateTime? lastDeliveryDateTime = null)
    {
        var result = await _mediator.Send(new GetDeliveryOrderQuery(districtId, firstDeliveryDateTime, lastDeliveryDateTime));
        _logger.Log(result.ToLog("GetOrders"));
        return result.ToActionResult();
    }


    /// <summary>
    /// Create order
    /// </summary>
    /// <param name="request"> Request </param>
    /// <response code="201"> Order successfully created </response>
    /// <response code="400"> Invalid request passed </response>
    [HttpPost("order")]
    [ProducesResponseType(typeof(OrderModel), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand request)
    {
        var result = await _mediator.Send(request);
        _logger.Log(result.ToLog("CreateOrder"));
        return result.ToActionResult();
    }

    /// <summary>
    /// Upload orders from file
    /// </summary>
    /// <param name="file"> File </param>
    /// <response code="200"> Orders successfully uploaded </response>
    /// <response code="400"> Invalid file </response>
    [HttpPost("orders/upload")]
    [ProducesResponseType(typeof(UploadModel<OrderModel>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> UploadOrdersFromFile(IFormFile file)
    {
        var result = await _mediator.Send(new UploadOrdersCommand(file.OpenReadStream()));
        _logger.Log(result.ToLog("UploadOrders"));
        return result.ToActionResult();
    }
}