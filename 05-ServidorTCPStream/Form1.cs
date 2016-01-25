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
using System.IO;

namespace _05_ServidorTCPStream
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string data;
            TcpListener newsock = new TcpListener(IPAddress.Any, 2000);
            newsock.Start();

            Console.WriteLine("Esperando por cliente");
            TcpClient cliente = newsock.AcceptTcpClient(); //linea bloqueante

            NetworkStream ns = cliente.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);

            sw.WriteLine("Bienvenido");
            sw.Flush();
            this.label1.Text = "";

            while (true)
            {
                try
                {
                    data = sr.ReadLine();
                    //this.label1.Text += data + System.Environment.NewLine;
                    //this.label1.Refresh();
                    Console.WriteLine(data);
                    sw.WriteLine("#"+data+"#");
                    sw.Flush();
                    if (data == "exit")
                        break;
                }
                catch (Exception error)
                {
                    Console.WriteLine("Error: {0}", error.ToString());
                }
            }

            ns.Close();
            cliente.Close();
            newsock.Stop();

        }
    }
}
