using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace privatemessagereceiver
{
    class Program
    {

        const string ServiceBusConnectionString = "Endpoint=sb://salesteamappfar2022.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=kYS+c5YkJaM2RbsoJ0+N4SLsUcT/Ig0T9tXYsPvEIVw=";
        const string QueueName = "salesperformancemessages";

        static void Main(string[] args)
        {

            ReceiveSalesMessageAsync().GetAwaiter().GetResult();

        }

        static async Task ReceiveSalesMessageAsync()
        {

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after receiving all the messages.");
            Console.WriteLine("======================================================");


            // Create a Service Bus client that will authenticate using a connection string

            var client = new ServiceBusClient(ServiceBusConnectionString);

            // Create the options to use for configuring the processor

            var processorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false
            };

            // Create a processor that we can use to process the messages

            await using var processor = client.CreateProcessor(QueueName, processorOptions);

            // Configure the message and error handler to use

            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;

            // Start processing

            await processor.StartProcessingAsync();

            Console.Read();

            // Close the processor here

            await processor.CloseAsync();
        }

        private static Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private static async Task MessageHandler(ProcessMessageEventArgs arg)
        {
            var body = arg.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");

            await arg.CompleteMessageAsync(arg.Message);
        }

        //// handle received messages
        //static async Task MessageHandler(ProcessMessageEventArgs args)
        //{
        //    string body = args.Message.Body.ToString();
        //    Console.WriteLine($"Received: {body}");

        //    // complete the message. messages is deleted from the queue. 
        //    await args.CompleteMessageAsync(args.Message);
        //}

        //// handle any errors when receiving messages
        //static Task ErrorHandler(ProcessErrorEventArgs args)
        //{
        //    Console.WriteLine(args.Exception.ToString());
        //    return Task.CompletedTask;
        //}
    }
}
