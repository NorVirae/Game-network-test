using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public interface INetworkEventListener
    {
        public void OnConnected();
        public void OnDisconnected();
        public void OnNetworkMessage(short messageId, object message);
    }
}
