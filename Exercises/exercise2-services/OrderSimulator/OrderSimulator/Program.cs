using CommandLine;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OrderSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            var result = CommandLine.Parser.Default.ParseArguments(args, typeof(SendMessageOptions), typeof(CreateSubscriptionOptions));

            result.WithParsed(v =>
            {
                if (v is SendMessageOptions)
                {
                    SendOrderMessage((SendMessageOptions)v);
                }
                else if (v is CreateSubscriptionOptions)
                {
                    CreateSubscriptionFilter((CreateSubscriptionOptions)v);
                }
            });

            if (result is NotParsed<object>)
            {
                Environment.Exit(-1);
            }

            Console.ReadLine();
        }

        private static void CreateSubscriptionFilter(CreateSubscriptionOptions options)
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(options.ConnectionString);

            var subscription = new SubscriptionDescription(options.TopicName, options.SubscriptionName)
            {
                DefaultMessageTimeToLive = TimeSpan.FromDays(7),
                LockDuration = TimeSpan.FromMinutes(1),
                MaxDeliveryCount = 10,
                EnableDeadLetteringOnFilterEvaluationExceptions = true
            };

            var filter = new SqlFilter($"user.SupplierID = '{options.SupplierID}'");

            namespaceManager.CreateSubscription(subscription, filter);

            Console.WriteLine($"Created subscription {options.SubscriptionName} on topic {options.TopicName}.");
        }

        private static void SendOrderMessage(SendMessageOptions options)
        {
            var client = TopicClient.CreateFromConnectionString(options.ConnectionString, options.TopicName);

            var order = new
            {
                StockItemId = 81,
                StockItemName = "\"The Gu\" red shirt XML tag t-shirt (White) M",
                SupplierID = options.SupplierID.ToString(),
                SupplierName = "Fabrikam, Inc.",
                Quantity = 100,
                PricePerItem = 18.0
            };

            var json = JsonConvert.SerializeObject(order);
            var message = new BrokeredMessage(new MemoryStream(Encoding.UTF8.GetBytes(json)));

            message.Properties["SupplierID"] = order.SupplierID;

            Console.WriteLine(order);

            client.Send(message);

            Console.WriteLine("Order successfully sent!");
        }
    }
}
