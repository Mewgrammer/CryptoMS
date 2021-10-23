using AutoMapper;
using Contracts.Communication.Contracts;
using Contracts.Communication.Queries;
using CsrStorage.Services;
using MediatR;

namespace CsrStorage.Communication;

public class
    GetAllCsrsQueryHandler : IRequestHandler<CsrCollectionQuery,
        IEnumerable<CsrResponse>>
{
    private readonly CsrStorageService _storageService;
    private readonly IMapper _mapper;

    public GetAllCsrsQueryHandler(CsrStorageService storageService, IMapper mapper)
    {
        _storageService = storageService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CsrResponse>> Handle(CsrCollectionQuery request,
        CancellationToken cancellationToken)
    {
        var csrs = await _storageService.Find();
        return csrs.Select(csr => _mapper.Map<CsrResponse>(csr));
    }
}