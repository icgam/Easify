using System;

namespace EasyApi.Extensions
{
    // TODO: Should be moved to Core project. Also Add Today as well.
    public interface IDateTimeProvider
    {
        DateTime Now();
    }
}
