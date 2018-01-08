using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ConsoleApp1
{
    class Program
    {
        public static String url = "tcp://www.tobiasschmocker.ch";
        public static String topic = "Neplan";
        public static IPAddress ip;
        public static MqttClient client;

        static void Main(string[] args)
        {

            // get ip-adress of url
            Uri Uri = new Uri(url);
            ip = Dns.GetHostAddresses(Uri.Host)[0];


            client = new MqttClient(ip);

            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            client.Subscribe(new string[] { "Neplan/toService" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received
            
            // decode input
            String input = System.Text.Encoding.UTF8.GetString(e.Message);
            Console.WriteLine(input);

            // do command

            // encode output
            String strValue = "rwhgpuhGPUHG55";
            byte[] output = Encoding.UTF8.GetBytes(strValue);
            //return values

            client.Publish("Neplan/fromService", output, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
    }
}
