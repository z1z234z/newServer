using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System.Threading;

namespace newserver
{
    class SocketIO
    {
        static Socket socket;
        static bool connected = false;

        public SocketIO()
        {

        }
        public static Socket getSocket()
        {
            if (socket == null)
            {
                socket = false ? IO.Socket("http://127.0.0.1:3200") : IO.Socket("http://192.168.1.106:3200/db");
                return socket;
            }
            else
            {
                return socket;
            }
        }
        public static bool getStatus()
        {
            return connected;
        }
        public static void setStatus(bool Status)
        {
            connected = Status;
        }
    }
}
