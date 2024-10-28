using Delivery.Contracts;
using Delivery.UseCases.Utils.Result;
using MediatR;
using System.Text.Json;
using Delivery.UseCases.District.Commands.Create;

namespace Delivery.UseCases.District.Commands.Upload;

public class UploadDistrictsCommand(Stream fileStream) : IRequest<Result<UploadModel<DistrictModel>>>
{
    public Stream FileStream { get; set; } = fileStream;
}

public class UploadDistrictsCommandHandler : IRequestHandler<UploadDistrictsCommand, Result<UploadModel<DistrictModel>>>
{
    private readonly IMediator _mediator;

    public UploadDistrictsCommandHandler(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<Result<UploadModel<DistrictModel>>> Handle(UploadDistrictsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var jsonObject = await JsonSerializer.DeserializeAsync<List<CreateDistrictCommand>>(
                request.FileStream, cancellationToken: cancellationToken);

            request.FileStream.Close();

            if (jsonObject == null)
                return Result<UploadModel<DistrictModel>>.Invalid("Invalid file");

            var results = jsonObject.Select(x => _mediator.Send(x, cancellationToken).Result).ToList();

            var successes = results.Where(x => x.IsSuccess);
            var failures = results.Where(x => !x.IsSuccess);

            var result = new UploadModel<DistrictModel>
            {
                UploadedModels = successes.Select(x => x.GetValue()).ToList(),
                Errors = failures.SelectMany(x => x.Errors!).ToList()
            };

            return Result<UploadModel<DistrictModel>>.Success(result);
        }
        catch (JsonException)
        {
            return Result<UploadModel<DistrictModel>>.Invalid("Invalid file");
        }
    }
}