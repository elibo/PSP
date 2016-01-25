using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultisocketClient
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
        string dato;

        private void button2_Click(object sender, EventArgs e)
        {//intento
            sw.WriteLine(this.textBox2.Text);
            sw.Flush();
            //Console.WriteLine(sr.ReadLine());
            dato = sr.ReadLine();
            DelegadoRespuesta dr = new DelegadoRespuesta(Escribirenlabel);
            Invoke(dr);
        }




        private void Escribirenlabel() {
            label1.Text = dato;
        }

        delegate void DelegadoRespuesta();
        
        

        private void button1_Click(object sender, EventArgs e)
        {//conectar
            try
            {
                client = new TcpClient(this.textBox1.Text, 2000);
                ns = client.GetStream();
                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);
                //Console.WriteLine(sr.ReadLine());
                dato = sr.ReadLine();
                DelegadoRespuesta dr = new DelegadoRespuesta(Escribirenlabel);
                Invoke(dr);
            }
            catch (Exception excep) {
                Console.WriteLine("Error: "+ excep.ToString());
            }
          
        }
    }
}

