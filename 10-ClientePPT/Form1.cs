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

namespace _10_ClientePPT
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
        {//conectar con el servidor
            try
            {
                client = new TcpClient(textBox1.Text, 2000);
                ns = client.GetStream();
                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);
                //Console.WriteLine(sr.ReadLine());
                //Console.WriteLine(sr.ReadLine());
                //Console.WriteLine(sr.ReadLine());
                dato = sr.ReadLine() + System.Environment.NewLine+
                      sr.ReadLine() + System.Environment.NewLine+
                      sr.ReadLine();
                DelegadoRespuesta dr = new DelegadoRespuesta(EscribirFormulario);
                Invoke(dr); 


            }
            catch (Exception error)
            {
                Console.WriteLine("Error: " + error.ToString());
            }
        }

        String dato;
        delegate void DelegadoRespuesta();
        private void EscribirFormulario()
        {
            label1.Text = dato;
        }

        private void button2_Click(object sender, EventArgs e)
        {//inscribir jugador
            sw.WriteLine("#INSCRIBIR#" + textBox2.Text +"#");
            sw.Flush();
            dato = sr.ReadLine();

            DelegadoRespuesta dr = new DelegadoRespuesta(EscribirFormulario);
            Invoke(dr);         
        }

        private void button3_Click(object sender, EventArgs e)
        {//jugada
            sw.WriteLine("#JUGADA#" + comboBox1.Text+ "#");
            sw.Flush();
            dato = sr.ReadLine();

            DelegadoRespuesta dr = new DelegadoRespuesta(EscribirFormulario);
            Invoke(dr);              
        }

        private void button4_Click(object sender, EventArgs e)
        {//obtener puntuacion
            sw.WriteLine("#PUNTUACION#");
            sw.Flush();
            dato = sr.ReadLine();

            DelegadoRespuesta dr = new DelegadoRespuesta(EscribirFormulario);
            Invoke(dr); 
        }

    }
}
