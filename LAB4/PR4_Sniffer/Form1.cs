using System;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PR4_Sniffer
{
    public partial class Form1 : Form
    {
        Socket sock;

        byte[] byTrue;
        byte[] byOut;
        byte[] data;

        int nReceived;
        bool bContinueCapturing;

        IPHeader ipHeader;

        delegate void AddTreeNode(TreeNode node);

        public Form1()
        {
            InitializeComponent();

            this.data = new byte[4096];   
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (this.comboBox1.Text == "")
            {
                MessageBox.Show("Please Select an interface to capture packets");
                return;
            }

            try
            {
                if(!bContinueCapturing)
                {
                    buttonStart.Text = "Stop";
                    bContinueCapturing = true;

                    this.sock = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);

                    sock.Bind(new IPEndPoint(IPAddress.Parse(comboBox1.Text), 0));

                    this.sock.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

                    this.byTrue = new byte[4] { 1, 0, 0, 0 };
                    this.byOut = new byte[4];

                    sock.IOControl(IOControlCode.ReceiveAll, byTrue, byOut); // receive all packets that pass through a network interface

                    sock.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(onReceive), null);
                }
                else
                {
                    buttonStart.Text = "Start";
                    bContinueCapturing = false;
                    sock.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }

        private void onReceive(IAsyncResult ar)
        {
            try
            {
                nReceived = sock.EndReceive(ar);
                parseData(data, nReceived);

                if (this.bContinueCapturing)
                {
                    data = new byte[4096];
                    sock.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(onReceive), null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sniffer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void parseData(byte[] data, int nReceived)
        {
            TreeNode rootNode = new TreeNode();

            this.ipHeader = new IPHeader(data, nReceived);

            TreeNode ipNode = MakeIPTreeNode(ipHeader);

            rootNode.Nodes.Add(ipNode);

            AddTreeNode addTreeNode = new AddTreeNode(onAddTreeNode);

            rootNode.Text = ipHeader.SourceAddress.ToString() + "-" + ipHeader.DestinationAddress.ToString();

            treeView1.Invoke(addTreeNode, new object[] { rootNode });
        }  
        
        private void onAddTreeNode(TreeNode node)
        {
            treeView1.Nodes.Add(node);
        }

        private TreeNode MakeIPTreeNode(IPHeader ipHeader)
        {
            TreeNode ipNode = new TreeNode();

            ipNode.Text = "IP";
            ipNode.Nodes.Add("Ver: " + ipHeader.Version);
            ipNode.Nodes.Add("Header Length: " + ipHeader.HeaderLength);
            ipNode.Nodes.Add("Differntiated Services: " + ipHeader.DifferentiatedServices);
            ipNode.Nodes.Add("Total Length: " + ipHeader.TotalLength);
            ipNode.Nodes.Add("Identification: " + ipHeader.Identification);
            ipNode.Nodes.Add("Flags: " + ipHeader.Flags);
            ipNode.Nodes.Add("Fragmentation Offset: " + ipHeader.FragmentationOffset);
            ipNode.Nodes.Add("Time to live: " + ipHeader.TTL);
            ipNode.Nodes.Add("Protocol: " + ipHeader.ProtocolType);
            ipNode.Nodes.Add("Checksum: " + ipHeader.Checksum);
            ipNode.Nodes.Add("Source: " + ipHeader.SourceAddress.ToString());
            ipNode.Nodes.Add("Destination: " + ipHeader.DestinationAddress.ToString());

            return ipNode;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string strIP = null;

            IPHostEntry hEntry = Dns.GetHostEntry((Dns.GetHostName()));
            if(hEntry.AddressList.Length > 0)
            {
                foreach(IPAddress ip in hEntry.AddressList)
                {
                    strIP = ip.ToString();
                    comboBox1.Items.Add(strIP);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(bContinueCapturing)
            {
                sock.Close();
            }
        }
    }
}
