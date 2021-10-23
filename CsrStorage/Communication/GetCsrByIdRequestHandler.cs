using AutoMapper;
using Contracts.Communication.Contracts;
using Contracts.Communication.Queries;
using CsrStorage.Services;
using MediatR;
using Opw.HttpExceptions;

namespace CsrStorage.Communication;

public class GetCsrByIdQueryHandler : IRequestHandler<CsrByIdQuery, CsrResponse>
{
    private readonly CsrStorageService _storageService;
    private readonly IMapper _mapper;

    public GetCsrByIdQueryHandler(CsrStorageService storageService, IMapper mapper)
    {
        _storageService = storageService;
        _mapper = mapper;
    }

    public async Task<CsrResponse> Handle(CsrByIdQuery request, CancellationToken cancellationToken)
    {
        var storedCsr = await _storageService.FindOne(request.Id);
        return _mapper.Map<CsrResponse>(storedCsr);
    }
}