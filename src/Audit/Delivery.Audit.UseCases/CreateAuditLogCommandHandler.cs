using Delivery.Audit.Contracts;
using Delivery.Audit.Core;
using Delivery.Audit.UseCases.Abstractions;
using MediatR;

namespace Delivery.Audit.UseCases;

public class CreateAuditLogCommandHandler: IRequestHandler<CreateAuditLogCommand, Unit>
{
    private readonly IAuditLogRepository _repository;

    public CreateAuditLogCommandHandler(IAuditLogRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Unit> Handle(CreateAuditLogCommand request, CancellationToken cancellationToken)
    {
        await _repository.Add(new AuditLog
        {
            DateTime = request.DateTime,
            ClientIp = request.ClientIp,
            Action = request.Action,
            IsSuccessful = request.IsSuccessful,
            Message = request.Message,
            ExtraData = request.ExtraData,
            RequestId = request.RequestId
        });

        return Unit.Value;
    }
}