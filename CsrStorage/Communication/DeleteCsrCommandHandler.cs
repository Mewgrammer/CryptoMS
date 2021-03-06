using CsrStorage.Services;
using MediatR;
using Shared.Communication.Commands;

namespace CsrStorage.Communication;

public class DeleteCsrCommandHandler : AsyncRequestHandler<DeleteCsrCommand>
{
    private readonly CsrStorageService _storageService;

    public DeleteCsrCommandHandler(CsrStorageService storageService)
    {
        _storageService = storageService;
    }

    protected override async Task Handle(DeleteCsrCommand request, CancellationToken cancellationToken)
    {
        await _storageService.DeleteCsr(request.Id);
    }
}