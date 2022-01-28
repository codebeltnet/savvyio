using System;
using System.Linq.Expressions;

namespace Savvyio
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public bool IsSatisfiedBy(T entity)
        {
            var predicate = ToExpression()?.Compile();
            return predicate != null && predicate(entity);
        }

        public abstract Expression<Func<T, bool>> ToExpression();
    }
}
