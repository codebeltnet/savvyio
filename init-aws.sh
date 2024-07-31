#!/bin/sh

# Create SQS queues
awslocal sqs create-queue --queue-name savvyio-commands
awslocal sqs create-queue --queue-name savvyio-commands.fifo --attributes FifoQueue=true,ContentBasedDeduplication=false
awslocal sqs create-queue --queue-name newtonsoft-savvyio-commands
awslocal sqs create-queue --queue-name newtonsoft-savvyio-commands.fifo --attributes FifoQueue=true,ContentBasedDeduplication=false
awslocal sqs create-queue --queue-name savvyio-events
awslocal sqs create-queue --queue-name savvyio-events.fifo --attributes FifoQueue=true,ContentBasedDeduplication=false
awslocal sqs create-queue --queue-name newtonsoft-savvyio-events
awslocal sqs create-queue --queue-name newtonsoft-savvyio-events.fifo --attributes FifoQueue=true,ContentBasedDeduplication=false

# Create SNS topics
awslocal sns create-topic --name member-events-one
awslocal sns create-topic --name member-events-one.fifo --attributes FifoTopic=true,ContentBasedDeduplication=false
awslocal sns create-topic --name member-events-many
awslocal sns create-topic --name member-events-many.fifo --attributes FifoTopic=true,ContentBasedDeduplication=false
awslocal sns create-topic --name newtonsoft-member-events-one
awslocal sns create-topic --name newtonsoft-member-events-one.fifo --attributes FifoTopic=true,ContentBasedDeduplication=false
awslocal sns create-topic --name newtonsoft-member-events-many
awslocal sns create-topic --name newtonsoft-member-events-many.fifo --attributes FifoTopic=true,ContentBasedDeduplication=false

# Subscribe SQS queues to SNS topics
awslocal sns subscribe --topic-arn arn:aws:sns:eu-west-1:000000000000:member-events-one --protocol sqs --notification-endpoint arn:aws:sqs:eu-west-1:000000000000:savvyio-events --attributes RawMessageDelivery=true
awslocal sns subscribe --topic-arn arn:aws:sns:eu-west-1:000000000000:member-events-many --protocol sqs --notification-endpoint arn:aws:sqs:eu-west-1:000000000000:savvyio-events --attributes RawMessageDelivery=true
awslocal sns subscribe --topic-arn arn:aws:sns:eu-west-1:000000000000:member-events-one.fifo --protocol sqs --notification-endpoint arn:aws:sqs:eu-west-1:000000000000:savvyio-events.fifo --attributes RawMessageDelivery=true
awslocal sns subscribe --topic-arn arn:aws:sns:eu-west-1:000000000000:member-events-many.fifo --protocol sqs --notification-endpoint arn:aws:sqs:eu-west-1:000000000000:savvyio-events.fifo --attributes RawMessageDelivery=true
awslocal sns subscribe --topic-arn arn:aws:sns:eu-west-1:000000000000:newtonsoft-member-events-one --protocol sqs --notification-endpoint arn:aws:sqs:eu-west-1:000000000000:newtonsoft-savvyio-events --attributes RawMessageDelivery=true
awslocal sns subscribe --topic-arn arn:aws:sns:eu-west-1:000000000000:newtonsoft-member-events-many --protocol sqs --notification-endpoint arn:aws:sqs:eu-west-1:000000000000:newtonsoft-savvyio-events --attributes RawMessageDelivery=true
awslocal sns subscribe --topic-arn arn:aws:sns:eu-west-1:000000000000:newtonsoft-member-events-one.fifo --protocol sqs --notification-endpoint arn:aws:sqs:eu-west-1:000000000000:newtonsoft-savvyio-events.fifo --attributes RawMessageDelivery=true
awslocal sns subscribe --topic-arn arn:aws:sns:eu-west-1:000000000000:newtonsoft-member-events-many.fifo --protocol sqs --notification-endpoint arn:aws:sqs:eu-west-1:000000000000:newtonsoft-savvyio-events.fifo --attributes RawMessageDelivery=true
