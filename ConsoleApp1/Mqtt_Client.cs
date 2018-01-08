using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace NeplanMqttService
{
    class Mqtt_Client
    {
        public static string url = "www.tobiasschmocker.ch";
        public static string topic = "Neplan";


        public static MqttClient client;

        public Mqtt_Client()
        {
            // get ip-adress of url

            client = new MqttClient(url);

            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            client.Subscribe(new string[] { "Neplan/toService" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }


        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received
            string message_json = System.Text.Encoding.UTF8.GetString(e.Message);
            Dictionary<string, string> message = JsonConvert.DeserializeObject<Dictionary<string, string>>(message_json);

            string fnc = message["fnc"];
            string id = message["id"];
            Dictionary<string, string> pars = JsonConvert.DeserializeObject<Dictionary<string, string>>(message["input"]);

            Program.HandleCommand(id, fnc, pars);
        }

        public static void Publish(Dictionary<string, string> output)
        {
            string msg_json = JsonConvert.SerializeObject(output, Formatting.Indented);
            byte[] msg_bytes = Encoding.UTF8.GetBytes(msg_json);
            client.Publish("Neplan/fromService", msg_bytes, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }


    }
    
}
