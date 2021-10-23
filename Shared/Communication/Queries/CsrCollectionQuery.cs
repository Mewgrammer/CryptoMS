using System.ComponentModel.DataAnnotations;
using Contracts.Communication.Contracts;
using MediatR;

namespace Contracts.Communication.Queries;

public class CsrCollectionQuery  : IRequest<IEnumerable<CsrResponse>>
{ }