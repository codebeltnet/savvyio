namespace Savvyio
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy(T entity);
    }
}
