using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System.Threading;
using System.Security.Cryptography;
using System.Data;
using System.Collections;
using newServer;
namespace newserver
{
    class MainClass
    {
        private static Socket socket;
        private static Controller controller;
        private static ResourceController Rescontroller;
        private static DoctorController Doccontroller;
        static void Main(string[] args)
        {

            socket = SocketIO.getSocket();
            controller = new Controller(socket);
            Rescontroller = new ResourceController(socket);
            Doccontroller = new DoctorController(socket);

            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Console.WriteLine("Connected to the server.\n");
                SocketIO.setStatus(true);

            });
            socket.Emit("java_dataBase_reg", new Object[] { "a" });
            socket.On(Socket.EVENT_CONNECT_TIMEOUT, () =>
            {
                Console.WriteLine("Connection Timeout. \n");
            });
            controller.startListening();
            Rescontroller.startListening();
            Doccontroller.startListening(); 
            while (true)
            {
                Thread.Sleep(1000);
            }
            
        }
    }
}
