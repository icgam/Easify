using System.Threading.Tasks;

namespace EasyApi.AspNetCore.UnitTests.Helpers
{
    public sealed class FakeService : IFakeService
    {
        private const int TaskDelayInMs = 1000;

        public string State { get; private set; } = string.Empty;

        public async Task<TArg> CallAndReturnResultAsync<TArg>(TArg input)
        {
            await Task.Delay(TaskDelayInMs);
            return await Task.Run(async () =>
            {
                State = "Changed";
                await Task.Delay(TaskDelayInMs);
                return input;
            });
        }

        public TArg CallAndReturnResult<TArg>(TArg input)
        {
            State = "Changed";
            return input;
        }

        public void Call()
        {
            State = "Changed";
        }

        public async Task CallAsync()
        {
            await Task.Delay(TaskDelayInMs).ContinueWith(t =>
            {
                State = "Changed";
            });
        }

        public async Task CallAndThrowAsync()
        {
            await Task.Delay(TaskDelayInMs);
            await Task.Run(() =>
            {
                State = "Changed";
                throw new FakeException();
            });
        }

        public void CallAndThrow()
        {
            State = "Changed";
            throw new FakeException();
        }

        public async Task<TArg> CallAndThrowInsteadOfReturnResultAsync<TArg>(TArg input)
        {
            await Task.Delay(TaskDelayInMs);
            await Task.Run(async () =>
            {
                State = "Changed";
                await Task.Delay(TaskDelayInMs);
                await Task.Run(() => throw new FakeException());
            });

            return input;
        }
    }
}