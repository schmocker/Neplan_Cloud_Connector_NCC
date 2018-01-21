using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Neplan_Cloud_Connector_NCC
{
    class Mqtt_Client
    {
        private string url, topic, ToClient = "NCC2Client", FromClient = "Client2NCC";

        private Controller controller;
        public MqttClient client;

        public Mqtt_Client(string url, string topic)
        {
            this.url = url;
            this.topic = topic;
            client = new MqttClient(url);
            client.MqttMsgPublishReceived += ReceiveMsg;
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }
        public void setController(Controller controller)
        {
            this.controller = controller;
        }

        public void ReceiveMsg(object sender, MqttMsgPublishEventArgs msg)
        {
            Dictionary<string, object> input = new Dictionary<string, object>();
            try
            {
                

                string json_string = System.Text.Encoding.UTF8.GetString(msg.Message);
                
                JObject json = JsonConvert.DeserializeObject<JObject>(json_string);
                
                if ((string)json["Direction"] == FromClient)
                {
                    ConsoleOut.ShowMsgReceived();
                    string methodName = (string)json["FunctionName"];
                    string id = (string)json["ID"];
                    input = json["Input"].ToObject<Dictionary<string, object>>();
                    controller.TreatCommand(id, methodName, input);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("--> Message received but following error accured:");
                Console.WriteLine(e);
            }
            
        }

        public void PublishMsg(Command cmd)
        {
            cmd.Direction = ToClient;
            string msg_json = JsonConvert.SerializeObject(cmd);
            byte[] msg_bytes = Encoding.UTF8.GetBytes(msg_json);
            client.Publish(topic , msg_bytes, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void PublishMsg(string key, object value)
        {
            Dictionary<string, object> msg = new Dictionary<string, object>();
            msg.Add(key, value);
            msg.Add("Direction", ToClient);
            string msg_json = JsonConvert.SerializeObject(msg);
            byte[] msg_bytes = Encoding.UTF8.GetBytes(msg_json);
            client.Publish(topic, msg_bytes, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
    }
}
