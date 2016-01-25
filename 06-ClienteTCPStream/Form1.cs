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

namespace _06_ClienteTCPStream
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        TcpClient client;
        NetworkStream ns;
        StreamReader sr;
        StreamWriter sw;

        private void button1_Click(object sender, EventArgs e)
        {
            client = new TcpClient(this.textBox1.Text, 2000);
            ns = client.GetStream();
            sr = new StreamReader(ns);
            sw = new StreamWriter(ns);
            Console.WriteLine(sr.ReadLine());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sw.WriteLine(this.textBox2.Text);
            sw.Flush();
            Console.WriteLine(sr.ReadLine());
        }
    }
}
