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
using System.Threading;


namespace _07_MultiSocketServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ManejarCliente(TcpClient cli)
        {
            string data;
            NetworkStream ns = cli.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);

            sw.WriteLine("Bienvenido, intenta adivinar mi numero");
            sw.Flush();
            while (true)
            {
                try
                {
                    data = sr.ReadLine();
                    Console.WriteLine(data); //para depuración es server
                    if (data == "55")
                    {
                        sw.WriteLine("Adivinado");
                        sw.Flush();
                        break;
                    }
                    else if (data == "exit")
                    {
                        break;
                    }
                    else
                    {
                        sw.WriteLine("Sigue intentandolo");
                        sw.Flush();
                    }

                }
                catch (Exception error)
                {
                    Console.WriteLine("Error: {0}", error.ToString());
                }
            }
            ns.Close();
            cli.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            TcpListener newsock = new TcpListener(IPAddress.Any, 2000);
            newsock.Start();

            Console.WriteLine("Esperando por cliente");

            while (true)
            {
                TcpClient cliente = newsock.AcceptTcpClient(); //linea bloqueante
                Thread t = new Thread(()=> this.ManejarCliente(cliente));
                //t.IsBackground = true;
                t.Start();
            }
            //newsock.Stop();
        }
    }
}
