using System;
using System.Linq.Expressions;

namespace EasyApi.Extensions.Specifications
{
    // TODO: Should be moved to Core project or separate assembly
    public abstract class Specification<T>
    {
        public static readonly Specification<T> All = new IdentitySpecification<T>();

        public static Specification<T> From(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return All.And(new PredicateSpecification<T>(predicate));
        }

        public bool IsSatisfiedBy(T entity)
        {
            var predicate = ToExpression().Compile();
            return predicate(entity);
        }

        public abstract Expression<Func<T, bool>> ToExpression();

        public Specification<T> And(Specification<T> specification)
        {
            if (this == All)
                return specification;

            return specification == All ? this : new AndSpecification<T>(this, specification);
        }

        public Specification<T> Or(Specification<T> specification)
        {
            if (this == All || specification == All)
                return All;

            return new OrSpecification<T>(this, specification);
        }

        public Specification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }
}