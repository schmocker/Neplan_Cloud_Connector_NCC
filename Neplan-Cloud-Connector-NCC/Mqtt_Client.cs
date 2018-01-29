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
        // fields for url, topic and directions
        private string url, topic, 
            ToClient = "NCC2Client", FromClient = "Client2NCC";

        // field for the controller handle
        private Controller controller;

        // field for the acctual MQTT-Client using 'uPLibrary.Networking.M2Mqtt';
        public MqttClient client;

        // field for the MQTT quality of service
        byte qos = MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE;

        // constructo class
        public Mqtt_Client(string url, string topic)
        {
            // set the url and topic, then connect to MQTT
            this.url = url;
            this.topic = topic;
            client = new MqttClient(url);
            client.MqttMsgPublishReceived += ReceiveMsg;
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
            client.Subscribe(new string[] { topic }, new byte[] { qos});
        }

        // the controller will be set after construction
        public void setController(Controller controller)
        {
            this.controller = controller;
        }

        // this method willl be called, whenever a message has received
        public void ReceiveMsg(object sender, MqttMsgPublishEventArgs msg)
        {
            // create empty directory
            Dictionary<string, object> input = new Dictionary<string, object>();
            try
            {
                // econvert the received bytes from the incomming message
                // to a string
                string json_string = Encoding.UTF8.GetString(msg.Message);
                
                // convert the string to a JObject.
                JObject json = JsonConvert.DeserializeObject<JObject>(json_string);
                
                // if the message is from a client...
                if ((string)json["Direction"] == FromClient)
                {
                    // ... show a confirmation in the console ...
                    ConsoleOut.ShowMsgReceived();
                    // ... set the method name, the id and convert the input to
                    // a dictionary
                    string methodName = (string)json["FunctionName"];
                    string id = (string)json["ID"];
                    input = json["Input"].ToObject<Dictionary<string, object>>();

                    // let the controller treat the command
                    controller.TreatCommand(id, methodName, input);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("--> Message received but following error accured:");
                Console.WriteLine(e);
            }
            
        }

        // method to publish a full command
        public void PublishMsg(Command cmd)
        {
            // set direction to NCC client
            cmd.Direction = ToClient;
            // convert the command to json
            string msg_json = JsonConvert.SerializeObject(cmd);
            // convert the json to bytes
            byte[] msg_bytes = Encoding.UTF8.GetBytes(msg_json);
            // send the bytes
            client.Publish(topic , msg_bytes, qos, false);
        }

        // method to publish a anything
        public void PublishMsg(string key, object value)
        {
            Dictionary<string, object> msg = new Dictionary<string, object>();
            // make a directory
            msg.Add(key, value);
            // set direction to NCC client
            msg.Add("Direction", ToClient);
            // convert the dictionary to json
            string msg_json = JsonConvert.SerializeObject(msg);
            // convert the json to bytes
            byte[] msg_bytes = Encoding.UTF8.GetBytes(msg_json);
            // send the bytes
            client.Publish(topic, msg_bytes, qos, false);
        }
    }
}
