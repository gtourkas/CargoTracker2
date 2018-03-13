using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using MQTTnet;
using MQTTnet.Server;

namespace App.Vessel.Monitor.Host
{
    public class MQTTnetListener : IMqttListener
    {
        private IMqttServer _mqttServer;

        public MQTTnetListener()
        {
            _mqttServer = new MqttFactory().CreateMqttServer();
        }

        public async Task Start()
        {
            _mqttServer.ApplicationMessageReceived += _mqttServer_ApplicationMessageReceived;

            await _mqttServer.StartAsync(new MqttServerOptions());
        }

        private void _mqttServer_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
        }

        public async Task Stop()
        {
            await _mqttServer.StopAsync();
        } 
    }
}
