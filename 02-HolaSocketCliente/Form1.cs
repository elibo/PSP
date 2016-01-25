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

namespace _02_HolaSocketCliente
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Socket miSocketCliente = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint miServidor = new IPEndPoint(IPAddress.Parse(this.textBox1.Text), 2000);
            try
            {
                miSocketCliente.Connect(miServidor);
                Console.WriteLine("Conectado con éxito");
                miSocketCliente.Close();
              
            }
            catch (Exception error)
            {
                Console.WriteLine("Error : {0}", error.ToString());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
