using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace PR_LAB3
{
    class UDPSharedWB
    {
        IPAddress broadcast;
        int port;
        Socket sock;
        IPEndPoint destinationEp, senderEp;
        EndPoint remote;

        IPEndPoint remoteAddr;
        byte[] recData;
        string recString;
        int recv;

        bool isRunning = true;
        UDPListenCallback _callBackMethod;

        public UDPSharedWB(int port, string IP, UDPListenCallback callBackMethod)
        {
            this._callBackMethod = callBackMethod;
            this.port = port;
            this.sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); // socket init
            this.sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.broadcast = IPAddress.Parse(IP);
            this.destinationEp = new IPEndPoint(this.broadcast, this.port); // destination
            this.senderEp = new IPEndPoint(IPAddress.Any, this.port); // sender IPeP

            this.remoteAddr = new IPEndPoint(IPAddress.Any, 0);
            this.remote = (EndPoint)remoteAddr; // remote EP
            this.recData = new byte[1024];
        }

        public void sendMouseCoordinates(string coords)
        {
            byte[] sendBuf = Encoding.ASCII.GetBytes(coords);
            this.sock.SendTo(sendBuf, sendBuf.Length, SocketFlags.None, this.destinationEp);
        }

        public void bindUDP()
        {
            this.sock.EnableBroadcast = true;
            this.sock.ExclusiveAddressUse = false;

            this.sock.Bind(this.senderEp);
        }

        public void receiveData()
        {
            
            
            
            
            // waiting for clients
 
              //  while(true)
           // {
                //if (!this.sock.Connected)
                //{
                //    break;
                //}
            while(isRunning)
            {
                this.sock.SendTimeout = 10;
                this.recv = this.sock.ReceiveFrom(this.recData, ref this.remote);
                
                recString = Encoding.ASCII.GetString(this.recData, 0, this.recv);
                if (_callBackMethod != null)
                {
                    _callBackMethod(this.recString);
                }
            }
                    
                
           // }
             
        }
    }
}
