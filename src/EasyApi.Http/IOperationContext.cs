using System.Security.Principal;

namespace EasyApi.Http
{
    // TODO: Need to be move to the core project.
    public interface IOperationContext
    {
        IPrincipal User { get; }
    }
}