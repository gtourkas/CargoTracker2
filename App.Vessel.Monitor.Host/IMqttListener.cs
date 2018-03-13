using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Vessel.Monitor.Host
{
    public interface IMqttListener
    {
        Task Start();

        Task Stop();
    }
}
