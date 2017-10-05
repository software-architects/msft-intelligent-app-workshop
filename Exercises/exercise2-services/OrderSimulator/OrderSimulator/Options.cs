using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSimulator
{
    public class Options
    {
        [Option('c', "connectionstring", Required = true, HelpText = "Connection string to an existing Service bus topic")]
        public string ConnectionString { get; set; }

        [Option('t', "topic", Required = true, HelpText = "Name of an existing topic")]
        public string TopicName { get; set; }
        
        public Options()
        {
            
        }
    }

    [Verb("send", HelpText = "Send a new order message to the topic.")]
    public class SendMessageOptions : Options
    {
        [Option('s', "supplierid", Default = 4, HelpText = "Supplier id that the subscription is filtered to")]
        public int SupplierID { get; set; }
    }

    [Verb("create", HelpText = "Create a new filtered subscription")]
    public class CreateSubscriptionOptions : Options
    {
        [Option('s', "supplierid", Default = 4, HelpText = "Supplier id that the subscription is filtered to")]
        public int SupplierID { get; set; }

        [Option('n', "subscription-name", Required = true, HelpText = "Name of the subscription to create")]
        public string SubscriptionName { get; set; }
    }
}
