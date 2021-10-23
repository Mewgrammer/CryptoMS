using AutoMapper;
using Contracts.Communication.Contracts;
using Contracts.Communication.Messaging;
using CsrStorage.Data.Entities;

namespace CsrStorage.Mapping;

public class CertificateRequestEntityProfile : Profile
{
    public CertificateRequestEntityProfile()
    {
        CreateMap<CsrEntity, CsrResponse>();
        CreateMap<CsrEntity, CertificateRequestMessageBody>();
    }
}