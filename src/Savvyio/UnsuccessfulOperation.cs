namespace Savvyio
{
    public sealed class UnsuccessfulOperation : ConditionalOperation
    {
        public UnsuccessfulOperation() : base(false)
        {
        }
    }

    public sealed class UnsuccessfulOperation<TResult> : ConditionalOperation<TResult>
    {
        public UnsuccessfulOperation(TResult result = default) : base(false, result)
        {
        }
    }
}
