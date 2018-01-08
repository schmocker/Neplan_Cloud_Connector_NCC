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
        public static MqttClient client;

        static void Main(string[] args)
        {
            String MQTT_BROKER_ADDRESS = "192.168.1.119";




            client = new MqttClient(IPAddress.Parse(MQTT_BROKER_ADDRESS));

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
