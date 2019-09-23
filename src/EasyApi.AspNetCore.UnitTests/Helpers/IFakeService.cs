using System.Threading.Tasks;

namespace EasyApi.AspNetCore.UnitTests.Helpers
{
    public interface IFakeService
    {
        string State { get; }
        Task<TArg> CallAndReturnResultAsync<TArg>(TArg input);
        TArg CallAndReturnResult<TArg>(TArg input);
        void Call();
        Task CallAsync();
        Task CallAndThrowAsync();
        void CallAndThrow();
        Task<TArg> CallAndThrowInsteadOfReturnResultAsync<TArg>(TArg input);
    }
}