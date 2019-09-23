using System;
using System.Linq.Expressions;

namespace EasyApi.Extensions.Specifications
{
    public sealed class PredicateSpecification<T> : Specification<T>
    {
        private readonly Expression<Func<T, bool>> _predicate;

        public PredicateSpecification(Expression<Func<T, bool>> predicate)
        {
            _predicate = predicate;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            return _predicate;
        }
    }
}