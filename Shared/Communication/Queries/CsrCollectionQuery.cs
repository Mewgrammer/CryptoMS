using MediatR;
using Shared.Communication.Responses;

namespace Shared.Communication.Queries;

public class CsrCollectionQuery  : IRequest<IEnumerable<CsrResponse>>
{ }