using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Valve.RemoteConsole
{
    public class RconPacket
    {

    }

    public class RCON : IDisposable
    {
        public enum PacketType
        {
            SERVERDATA_RESPONSE_VALUE = 0,
            SERVERDATA_AUTH_RESPONSE = 2,
            SERVERDATA_EXECCOMMAND = 2,
            SERVERDATA_AUTH = 3
        }

        public Socket socket = null;
        public bool IsAuthorized { get; private set; }

        public RCON()
        {
            
        }

        public bool Connect(string password, IPAddress ip, int port)
        {
            if(socket == null)
            {
                socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ip, port);
                
            }
            else if(socket.Connected)
            {
                Disconnect();
            }
            return true;
        }

        public bool Disconnect()
        {
            return true;
        }

        public void Dispose()
        {
            if(socket != null)
            {
                socket.Dispose();
                socket = null;
            }
        }
    }
}
