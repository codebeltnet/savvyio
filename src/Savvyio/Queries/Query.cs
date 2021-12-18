namespace Savvyio.Queries
{
    public abstract class Query<TResult> : Model, IQuery<TResult>
    {
        protected Query(IMetadata metadata = null)
        {
            this.MergeMetadata(metadata);
        }
    }
}
