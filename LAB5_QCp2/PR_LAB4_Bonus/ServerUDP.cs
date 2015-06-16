using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace PR_LAB4_Bonus
{
    class ServerUDP
    {
        private int recv;
        private byte[] data;
        private IPEndPoint listen;
        private IPEndPoint remote;
        private EndPoint remoteEndPoint;
        private Socket server;

       public ServerUDP(string ip, int port)
        {
            listen = new IPEndPoint(IPAddress.Parse(ip), port);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(listen);

            remote = new IPEndPoint(IPAddress.Any, port);
            remoteEndPoint = (EndPoint)remote;
        }

        public string receiveData()
        {
            data = new byte[1024];
            recv = server.ReceiveFrom(data, ref remoteEndPoint);
            return Encoding.ASCII.GetString(data, 0, recv);
        }

        public void sendData(string sendData)
        {
            if(server.Connected)
            {
                this.data = new byte[1024];
                this.data = Encoding.ASCII.GetBytes(sendData);
                server.Send(this.data);
            }
        }

        public bool connectedState()
        {
            return this.server.Connected;
        }

        public int receivedBytes()
        {
            return this.recv;
        }
    }
}
