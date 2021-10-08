using AutoMapper;
using Contracts.CsrStorage;
using Contracts.Messages;
using CsrStorage.Data.Entities;

namespace CsrStorage.Mapping;

public class CertificateRequestEntityProfile : Profile
{
    public CertificateRequestEntityProfile()
    {
        CreateMap<CertificateRequestEntity, CertificateRequestDto>();
        CreateMap<CertificateRequestEntity, CertificateRequestMessageBody>();
    }
}