using System.Threading.Tasks;
using UnityEngine;
using MQTTnet;
using MQTTnet.Client;
using System.Threading;
using System;

public class StarwatchSDK : MonoBehaviour
{
    void Start()
    {
        Task.Run(Publish_Application_Message);
    }

    public static async Task Publish_Application_Message()
    {
        var mqttFactory = new MqttFactory();
        using (var mqttClient = mqttFactory.CreateMqttClient())
        {
            try
            {
                var mqttClientOptions = new MqttClientOptionsBuilder()
                      .WithClientId("my_user_name")
                      .WithTcpServer("192.168.1.73", 1883)
                      .WithCleanSession()
                      .WithCredentials("simple_user", "simple_password")
                      .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311)
                      .Build();
                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("samples/temperature/living_room")
                    .WithPayload("19.5")
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
                await mqttClient.DisconnectAsync();

                Debug.Log("MQTT application message is published.");
            }
            catch (Exception e)
            {
                Debug.Log("Exception caught");
                Debug.Log(e);
            }
        }
    }
}
