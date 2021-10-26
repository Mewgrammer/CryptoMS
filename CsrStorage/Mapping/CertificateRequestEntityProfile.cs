using AutoMapper;
using CsrStorage.Data.Entities;
using Shared.Communication.Messaging;
using Shared.Communication.Responses;

namespace CsrStorage.Mapping;

public class CertificateRequestEntityProfile : Profile
{
    public CertificateRequestEntityProfile()
    {
        CreateMap<CsrEntity, CsrResponse>();
        CreateMap<CsrEntity, CertificateRequestMessageBody>();
    }
}