using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace performancemessagesender
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://salesteamappfar2022.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=kYS+c5YkJaM2RbsoJ0+N4SLsUcT/Ig0T9tXYsPvEIVw=";
        const string TopicName = "salesperformancemessages1";

        static void Main(string[] args)
        {

            Console.WriteLine("Sending a message to the Sales Performance topic...");

            SendPerformanceMessageAsync().GetAwaiter().GetResult();

            Console.WriteLine("Message was sent successfully.");

        }

        static async Task SendPerformanceMessageAsync()
        {
            // Create a Service Bus client here

            await using var client = new ServiceBusClient(ServiceBusConnectionString);

            // Create a sender here

            await using var sender = client.CreateSender(TopicName);

            // Send messages.
            try
            {
                // Create and send a message here

                var messageBody = "Total sales for Argentina in August: $13m.";
                var message = new ServiceBusMessage(messageBody);

                Console.WriteLine($"Sending message: {messageBody}");

                await sender.SendMessageAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
