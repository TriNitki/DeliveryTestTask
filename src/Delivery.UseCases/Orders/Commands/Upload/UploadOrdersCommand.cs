using System.Text.Json;
using Delivery.Contracts;
using Delivery.UseCases.Orders.Commands.Create;
using Delivery.UseCases.Utils.Result;
using MediatR;

namespace Delivery.UseCases.Orders.Commands.Upload;

public class UploadOrdersCommand(Stream fileStream) : IRequest<Result<UploadModel<OrderModel>>>
{
    public Stream FileStream { get; set; } = fileStream;
}

public class UploadOrdersCommandHandler : IRequestHandler<UploadOrdersCommand, Result<UploadModel<OrderModel>>>
{
    private readonly IMediator _mediator;

    public UploadOrdersCommandHandler(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<Result<UploadModel<OrderModel>>> Handle(UploadOrdersCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var jsonObject = await JsonSerializer.DeserializeAsync<List<CreateOrderCommand>>(
                request.FileStream, cancellationToken: cancellationToken);

            request.FileStream.Close();

            if (jsonObject == null)
                return Result<UploadModel<OrderModel>>.Invalid("Invalid file");

            var results = jsonObject.Select(x => _mediator.Send(x, cancellationToken).Result).ToList();

            var successes = results.Where(x => x.IsSuccess);
            var failures = results.Where(x => !x.IsSuccess);

            var result = new UploadModel<OrderModel>
            {
                UploadedModels = successes.Select(x => x.GetValue()).ToList(),
                Errors = failures.SelectMany(x => x.Errors!).ToList()
            };

            return Result<UploadModel<OrderModel>>.Success(result);
        }
        catch (JsonException)
        {
            return Result<UploadModel<OrderModel>>.Invalid("Invalid file");
        }
    }
}