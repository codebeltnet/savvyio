namespace Savvyio
{
    public abstract class ConditionalOperation
    {
        protected ConditionalOperation(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public bool Succeeded { get; }
    }

    public abstract class ConditionalOperation<TResult> : ConditionalOperation
    {
        protected ConditionalOperation(bool succeeded, TResult result) : base(succeeded)
        {
            Result = result;
        }

        public TResult Result { get; }
    }
}
