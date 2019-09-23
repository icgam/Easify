using System;
using System.Collections.Generic;

namespace EasyApi.Extensions
{
    // TODO: Should be moved to Core project. Also Add Today as well.
    public interface IComponentResolver
    {
        bool IsRegistered<TComponent>() where TComponent : class;
        bool IsRegistered(Type type);
        IEnumerable<TComponent> Resolve<TComponent>() where TComponent : class;
        IEnumerable<object> Resolve(Type type);
    }
}
