using Microsoft.AspNetCore.Mvc;

namespace X509.RegistrationAuthority.Controller;


[ApiVersion("1")]
[Route("api/v{version:apiVersion}/ra")]
[ApiController]
public class RaController : Microsoft.AspNetCore.Mvc.Controller
{

}