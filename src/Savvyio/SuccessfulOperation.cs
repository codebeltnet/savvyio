namespace Savvyio
{
    public sealed class SuccessfulOperation : ConditionalOperation
    {
        public SuccessfulOperation() : base(true)
        {
        }
    }

    public sealed class SuccessfulOperation<TResult> : ConditionalOperation<TResult>
    {
        public SuccessfulOperation(TResult result) : base(true, result)
        {
        }
    }
}
