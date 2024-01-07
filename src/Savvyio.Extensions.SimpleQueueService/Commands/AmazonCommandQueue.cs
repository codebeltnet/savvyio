using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.Collections.Generic;
using Cuemon.Extensions.IO;
using Cuemon.Threading;
using Savvyio.Commands;
using Savvyio.Messaging;

namespace Savvyio.Extensions.SimpleQueueService.Commands
{
	/// <summary>
	/// Provides a default implementation of the <see cref="AmazonQueue{TRequest}"/> class tailored for messages holding an <see cref="ICommand"/> implementation.
	/// </summary>
	/// <seealso cref="AmazonQueue{TRequest}" />
	public class AmazonCommandQueue : AmazonQueue<ICommand>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AmazonCommandQueue"/> class.
		/// </summary>
		/// <param name="marshaller">The <see cref="IMarshaller"/> that is used when converting <see cref="ICommand"/> implementations to messages.</param>
		/// <param name="options">The <see cref="AmazonCommandQueueOptions"/> used to configure this instance.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="marshaller"/> cannot be null - or -
		/// <paramref name="options"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="options"/> are not in a valid state.
		/// </exception>
		public AmazonCommandQueue(IMarshaller marshaller, AmazonCommandQueueOptions options) : base(marshaller, options)
		{
		}

		/// <summary>
		/// Sends the specified <paramref name="messages" /> whose generic type argument is <see cref="ICommand"/> asynchronous using Point-to-Point Channel/P2P MEP.
		/// </summary>
		/// <param name="messages">The <see cref="ICommand"/> enclosed messages to send.</param>
		/// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="messages"/> cannot be null.
		/// </exception>
		public override async Task SendAsync(IEnumerable<IMessage<ICommand>> messages, Action<AsyncOptions> setup = null)
		{
			Validator.ThrowIfNull(messages);
			var options = setup.Configure();
			var batches = messages.ToPartitioner(AmazonMessageOptions.MaxNumberOfMessages);
			var tasks = new List<Task>();
			while (batches.HasPartitions)
			{
				var sqs = new AmazonSQSClient(Options.Credentials, Options.Endpoint);
				var batchRequest = new SendMessageBatchRequest
				{
					QueueUrl = Options.SourceQueue.OriginalString,
					Entries = new List<SendMessageBatchRequestEntry>(batches.Select(message => new SendMessageBatchRequestEntry
					{
						Id = message.Id,
						MessageGroupId = UseFirstInFirstOut ? message.Source : null,
						MessageDeduplicationId = UseFirstInFirstOut ? message.Id : null,
						MessageBody = Marshaller.Serialize(message).ToEncodedString(),
						MessageAttributes = new Dictionary<string, MessageAttributeValue>
							{
								{
									MessageAttributeTypeKey, new MessageAttributeValue
									{
										DataType = "String",
										StringValue = message.Data.GetMemberType()
									}
								}
							}
					}))
				};
				tasks.Add(sqs.SendMessageBatchAsync(batchRequest, options.CancellationToken));
			}
			await Task.WhenAll(tasks).ConfigureAwait(false);
		}

		/// <summary>
		/// Receive one or more command(s) asynchronous using Point-to-Point Channel/P2P MEP.
		/// </summary>
		/// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a sequence of <see cref="IMessage{T}"/> whose generic type argument is <see cref="ICommand"/>.</returns>
		public override IAsyncEnumerable<IMessage<ICommand>> ReceiveAsync(Action<AsyncOptions> setup = null)
		{
			var options = setup.Configure();
			return RetrieveMessagesAsync(options.CancellationToken);
		}
	}
}
