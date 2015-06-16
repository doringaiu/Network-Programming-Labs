using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace PR_LAB4_Bonus
{
    public partial class Form1 : Form
    {
        string data;
        int listenPort;
        IPAddress ip;

        public Form1()
        {
            InitializeComponent();
            this.listenPort = 8167;
            this.ip = IPAddress.Parse("255.255.255.255");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartListener();
        }

        private void StartListener()
        {
            bool done = false;

            UdpClient listener = new UdpClient(this.listenPort);
            IPEndPoint groupEP = new IPEndPoint(this.ip, this.listenPort);

            try
            {
                while (!done)
                {
                    label1.Text = "Waiting for broadcast";
                    byte[] bytes = listener.Receive(ref groupEP);
                    label2.Text = groupEP.ToString();
                    data = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    label3.Text = data;
                }

            }
            catch (Exception e)
            {
                label3.Text = e.ToString();
            }
            finally
            {
                listener.Close();
            }
        }
    }
}
