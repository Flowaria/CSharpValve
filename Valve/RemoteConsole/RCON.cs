using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Valve.RemoteConsole
{
    public enum RconPacketType
    {
        SERVERDATA_RESPONSE_VALUE = 0,
        SERVERDATA_AUTH_RESPONSE = 2,
        SERVERDATA_EXECCOMMAND = 2,
        SERVERDATA_AUTH = 3
    }

    public class RconPacket
    {
        public const int PacketMinimumSize = 10;
        public const int PacketMinimumSizeActual = 14;
        public const int PacketMaximumSize = 4096;

        //Size 4
        //Type 4
        //Body 1~
        //EndT 1
        public int Size
        {
            get
            {
                return (Body.Length) + 10;
            }
        }

        public int ID { get; private set; }
        public int Type { get; private set; }
        public string Body { get; private set; }

        private RconPacket()
        {

        }

        public static RconPacket CreatePacket(int id, RconPacketType type, string body)
        {
            var packet = new RconPacket();
            packet.ID = id;
            packet.Type = (int)type;
            packet.Body = body;
            return packet;
        }

        public static RconPacket ParsePacket(byte[] buffer)
        {
            if ((PacketMinimumSize <= buffer.Length) && (buffer.Length <= PacketMaximumSize))
            {
                if (buffer[buffer.Length - 1] == 0x00)
                {
                    using (var ms = new MemoryStream(buffer))
                    {
                        byte[] bf_size = new byte[4];
                        byte[] bf_id = new byte[4];
                        byte[] bf_type = new byte[4];
                        byte[] bf_body = new byte[buffer.Length-13];
                        ms.Read(bf_size, 0, 4);
                        ms.Read(bf_id, 0, 4);
                        ms.Read(bf_type, 0, 4);
                        if(!BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(bf_size);
                            Array.Reverse(bf_id);
                            Array.Reverse(bf_type);
                        }
                        ms.Read(bf_body, 0, buffer.Length - 13);

                        int id = BitConverter.ToInt32(bf_id, 0);
                        int size = BitConverter.ToInt32(bf_size, 0);
                        int type = BitConverter.ToInt32(bf_type, 0);
                        String body = Encoding.ASCII.GetString(bf_body).TrimEnd((char)0);

                        if (size == 10 + body.Length && (type == 0 || type == 2 || type == 5))
                        {
                            var packet = new RconPacket();
                            packet.ID = id;
                            packet.Type = type;
                            return packet;
                        }
                        else
                            throw new ArgumentException("Invalid Buffer");
                    }
                        
                }
                else
                    throw new ArgumentException("Rcon Buffer Must be end with 0x00");
            }
            else
                throw new ArgumentOutOfRangeException("buffer size is not in size range of rcon");
        }

        public byte[] ToArray()
        {
            byte[] buffer = new byte[14 + Body.Length];
            using (var ms = new MemoryStream(buffer))
            {
                byte[] bf_id = BitConverter.GetBytes(ID);
                byte[] bf_size = BitConverter.GetBytes(Size);   
                byte[] bf_type = BitConverter.GetBytes(Type);
                if (!BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bf_id);
                    Array.Reverse(bf_size);
                    Array.Reverse(bf_type);
                }

                byte[] bf_body = Encoding.ASCII.GetBytes(Body +char.MinValue);
                ms.Write(bf_size, 0, 4);
                ms.Write(bf_id, 0, 4);
                ms.Write(bf_type, 0, 4);
                ms.Write(bf_body, 0, Body.Length);
                ms.WriteByte(0x00);
            }
            return buffer;
        }
    }

    public class RCON : IDisposable
    {
        

        public Socket socket = null;
        public bool IsAuthorized { get; private set; }

        public RCON()
        {
            
        }

        public bool Connect(string password, IPEndPoint ip)
        {
            if(socket == null)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ip);
                var respond = SendPacket(RconPacket.CreatePacket(5653, RconPacketType.SERVERDATA_AUTH, "password"));

                IsAuthorized = respond.ID != -1;
            }
            else if(socket.Connected)
            {
                Disconnect();
            }
            return true;
        }

        public RconPacket SendPacket(RconPacket packet)
        {
            socket.Send(packet.ToArray(), packet.Size, SocketFlags.None);

            byte[] buffer = new byte[RconPacket.PacketMaximumSize];

            int recv;
            while ((recv = socket.Receive(buffer)) > 0)
            {
                return RconPacket.ParsePacket(buffer);
            }
            return null;
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
