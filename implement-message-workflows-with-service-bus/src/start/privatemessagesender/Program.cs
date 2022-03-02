using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace privatemessagesender
{
    class Program
    {

        const string ServiceBusConnectionString = "Endpoint=sb://salesteamappfar2022.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=kYS+c5YkJaM2RbsoJ0+N4SLsUcT/Ig0T9tXYsPvEIVw=";
        const string QueueName = "salesperformancemessages";

        static void Main(string[] args)
        {
            Console.WriteLine("Sending a message to the Sales Messages queue...");

            SendSalesMessageAsync().GetAwaiter().GetResult();

            Console.WriteLine("Message was sent successfully.");
        }

        static async Task SendSalesMessageAsync()
        {
            // Create a Service Bus client here

            await using var client = new ServiceBusClient(ServiceBusConnectionString);

            // Create a sender here

            await using var sender = client.CreateSender(QueueName);

            try
            {
                // Create and send a message here

                var messageBody = $"$10,000 order for bicycle parts from retailer Adventure Works.";
                var message = new ServiceBusMessage(messageBody);

                Console.WriteLine($"Sending message: {messageBody}");

                await sender.SendMessageAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }

            // Close the connection to the sender here

            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
