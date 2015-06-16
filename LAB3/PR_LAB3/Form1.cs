using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace PR_LAB3
{
    public delegate void UDPListenCallback(string strPoints);

    public partial class Form1 : Form
    {
        Point startingPoint, endingPoint;
        Pen penPanel;
        Color colorPanel;
        Graphics graphicsPanel;
        bool mouseFlag, drawingFlag;

        string mouseCoords;
        Point recPointStart, recPointEnd;
        UDPListenCallback listenCallback;
        Thread T1;
        UDPSharedWB udp;
        bool bCallback;

        public Form1()
        {
            InitializeComponent();

            this.startingPoint = new Point();
            this.endingPoint = new Point();
            this.recPointStart = new Point();
            this.recPointEnd = new Point();
         
            this.penPanel = new Pen(Color.White);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.graphicsPanel = panel1.CreateGraphics();

            this.listenCallback = new UDPListenCallback(receiveString);
            this.udp = new UDPSharedWB(8233, "127.0.0.1", listenCallback);
            this.bCallback = false;
            this.udp.bindUDP();
            this.T1 = new Thread(this.udp.receiveData);
            T1.Start();
        }

        private void receiveString(string strPoints)
        {
            //M{X=391,Y=90}{X=391,Y=90} - the message
            if(strPoints.StartsWith("M"))
            {
                string[] sPoints = strPoints.Substring(1, strPoints.Length - 1).Split('Q');
                var g = Regex.Replace(sPoints[0], @"[\{\}a-zA-Z=]", "").Split(',');
                this.recPointStart = new Point(int.Parse(g[0]), int.Parse(g[1]));
                g = Regex.Replace(sPoints[1], @"[\{\}a-zA-Z=]", "").Split(',');
                this.recPointEnd = new Point(int.Parse(g[0]), int.Parse(g[1]));
                this.bCallback = true;

                this.graphicsPanel.DrawLine(penPanel, recPointStart, recPointEnd);
            }
           
        }

        private void drawElements()
        {
          this.graphicsPanel.DrawLine(penPanel, startingPoint, endingPoint);
          this.drawingFlag = false;

          this.mouseCoords = "M" + this.startingPoint.ToString() + "Q" + this.endingPoint.ToString();
          this.udp.sendMouseCoordinates(this.mouseCoords);
        }

        private void selectColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cDialog = new ColorDialog();
            cDialog.AllowFullOpen = false;
            if(cDialog.ShowDialog() == DialogResult.OK)
            {
                this.colorPanel = cDialog.Color;
            }
            cDialog.Dispose();
            this.penPanel.Color = colorPanel;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void eraseEverythingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.panel1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.udp.receiveData();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(mouseFlag)
            {
                if(drawingFlag == false)
                {
                    startingPoint = endingPoint;
                }
                endingPoint = e.Location;
                this.drawElements();  
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startingPoint = e.Location;
                drawingFlag = true;
                mouseFlag = true;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseFlag = false;
        }
    }
}
