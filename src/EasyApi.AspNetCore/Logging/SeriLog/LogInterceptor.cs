using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using EasyApi.Logging;
using Foil;
using Microsoft.Extensions.Logging;

namespace EasyApi.AspNetCore.Logging.SeriLog
{
    public sealed class LogInterceptor : AsyncInterceptor
    {
        private readonly IArgumentsFormatter _argumentsFormatter;

        public LogInterceptor(IArgumentsFormatter argumentsFormatter, ILogger<LogInterceptor> logger)
        {
            _argumentsFormatter = argumentsFormatter ?? throw new ArgumentNullException(nameof(argumentsFormatter));
            Log = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private ILogger<LogInterceptor> Log { get; }

        protected override async Task InterceptAsync(IInvocation invocation, Func<IInvocation, Task> proceed)
        {
            LogInvocationStarted(invocation);
            LogExecutionParameters(invocation);
            try
            {
                await proceed(invocation).ConfigureAwait(false);

                LogInvocationCompleted(invocation);
                LogReturnVoid(invocation);
            }
            catch (Exception e)
            {
                HandleException(invocation, e);
                throw;
            }
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation,
            Func<IInvocation, Task<TResult>> proceed)
        {
            LogInvocationStarted(invocation);
            LogExecutionParameters(invocation);
            try
            {
                var result = await proceed(invocation).ConfigureAwait(false);

                LogInvocationCompleted(invocation);
                LogInvocationResult(invocation, result);

                return result;
            }
            catch (Exception e)
            {
                HandleException(invocation, e);
                throw;
            }
        }

        private void HandleException(IInvocation invocation, Exception e)
        {
            if (e is AggregateException agex)
                LogInvocationThrown(invocation, agex.InnerExceptions.Single());
            else
                LogInvocationThrown(invocation, e);
        }

        #region Logs

        private void LogInvocationStarted(IInvocation invocation)
        {
            Log.LogDebug(
                $"Executing '{invocation.TargetType.FullName}.{invocation.Method.Name}' ...");
        }

        private void LogExecutionParameters(IInvocation invocation)
        {
            if (Log.IsEnabled(LogLevel.Debug) == false)
                return;

            if (invocation.Arguments == null || invocation.Arguments.Length == 0)
            {
                Log.LogTrace("Method called with NO PARAMETERS.");
            }
            else
            {
                var arguments = _argumentsFormatter.FormatArguments(invocation.Arguments);
                Log.LogTrace($"Execution parameters: {arguments}");
            }
        }

        private void LogInvocationThrown<TException>(IInvocation invocation, TException exception)
            where TException : Exception
        {
            Log.LogError($"Failed to execute '{invocation.TargetType.FullName}.{invocation.Method.Name}'!" +
                         $" '{exception.GetType().FullName}' has been thrown.");
        }

        private void LogInvocationCompleted(IInvocation invocation)
        {
            Log.LogDebug($"'{invocation.TargetType.FullName}.{invocation.Method.Name}' execution completed.");
        }

        private void LogReturnVoid(IInvocation invocation)
        {
            if (Log.IsEnabled(LogLevel.Trace) == false)
                return;

            Log.LogTrace($"'{invocation.TargetType.FullName}.{invocation.Method.Name}' " +
                         "is VOID and has no return value.");
        }

        private void LogInvocationResult<TReturnValue>(IInvocation invocation, TReturnValue returnValue)
        {
            if (Log.IsEnabled(LogLevel.Trace) == false)
                return;

            var result = _argumentsFormatter.FormatArgument(returnValue);
            Log.LogTrace($"Result for '{invocation.TargetType.FullName}.{invocation.Method.Name}' is: {result}.");
        }

        #endregion
    }
}