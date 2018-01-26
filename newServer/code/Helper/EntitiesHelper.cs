using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System.Threading;
using newServer;
namespace newserver
{

    class EntitiesHelper
    {
        private Entities10 hospitalEntity;
        public EntitiesHelper()
        {
            hospitalEntity = new Entities10();
        }
        public Entities10 getHospitalEntity()
        {
            return hospitalEntity;
        }
    }
}
