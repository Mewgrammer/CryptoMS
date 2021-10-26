using AutoMapper;
using CsrStorage.Services;
using MediatR;
using Shared.Communication.Commands;
using Shared.Communication.Responses;

namespace CsrStorage.Communication;

public class StoreCsrCommandHandler : IRequestHandler<StoreCsrCommand, CsrResponse>
{
    private readonly CsrStorageService _storageService;
    private readonly IMapper _mapper;

    public StoreCsrCommandHandler(CsrStorageService storageService, IMapper mapper)
    {
        _storageService = storageService;
        _mapper = mapper;
    }

    public async Task<CsrResponse> Handle(StoreCsrCommand request, CancellationToken cancellationToken)
    {
        var storedCsr = await _storageService.AddCsr(request.Csr);
        return _mapper.Map<CsrResponse>(storedCsr);
    }
}