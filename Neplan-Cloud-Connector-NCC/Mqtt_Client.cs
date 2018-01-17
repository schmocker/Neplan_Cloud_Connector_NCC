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
        private string url;
        private string topic;

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
            client.Subscribe(new string[] { topic + "/toService" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }
        public void setController(Controller controller)
        {
            this.controller = controller;
        }

        public void ReceiveMsg(object sender, MqttMsgPublishEventArgs msg)
        {
            SendReceipt();
            string methodName = null;
            Dictionary<string, object> input = new Dictionary<string, object>();
            try
            {
                string json_string = System.Text.Encoding.UTF8.GetString(msg.Message);

                ConsoleOut.ShowMsgReceived(json_string);
                //pretty: ConsoleOut.ShowMsgReceived(JToken.Parse(json_string).ToString(Newtonsoft.Json.Formatting.Indented));

                JObject json = JsonConvert.DeserializeObject<JObject>(json_string);
                methodName = (string)json["FunctionName"];
                input = json["Input"].ToObject<Dictionary<string, object>>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            controller.TreatCommand(methodName, input);
        }

        public void SendReceipt()
        {
            Dictionary<string, bool> receipt = new Dictionary<string, bool>();
            receipt.Add("Received" , true);
            receipt.Add("Done" , false);
            JToken json = JToken.FromObject(receipt);
            string msg_json = JsonConvert.SerializeObject(json);
            PublishMsg(msg_json);
        }

        public void PublishMsg(string msg_json)
        {
            byte[] msg_bytes = Encoding.UTF8.GetBytes(msg_json);
            client.Publish(topic + "/fromService", msg_bytes, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
    }
}
