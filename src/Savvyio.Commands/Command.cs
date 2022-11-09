namespace Savvyio.Commands
{
    /// <summary>
    /// Provides a default implementation of the <see cref="ICommand"/> interface.
    /// </summary>
    /// <seealso cref="ICommand" />
    /// <seealso cref="Request"/>
    public abstract record Command : Request, ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="metadata">The optional metadata to merge with this instance.</param>
        protected Command(IMetadata metadata = null)
        {
            this.MergeMetadata(metadata);
        }
    }
}
