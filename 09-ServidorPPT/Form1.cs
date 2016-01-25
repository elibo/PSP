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
namespace _09_ServidorPPT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string jugador1 = "";
        string jugador2 = "";
        string idJug1 = "";
        string idJug2 = "";
        string jugada1 = "";
        string jugada2 = "";

        int puntos1 = 0;
        int puntos2 = 0;
        public void ManejarCliente(TcpClient cli)
        {
            NetworkStream ns = cli.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);

            Console.WriteLine(cli.Client.RemoteEndPoint.ToString());

            String dato;
            sw.WriteLine("#INSCRIBIR#nombre#");
            sw.WriteLine("#JUGADA#{piedra/papel/tijera}#");
            sw.WriteLine("#PUNTUACION#");
            sw.Flush();
            //this.label1.Text = "";
            while (true)
            {
                try
                {
                    dato = sr.ReadLine();
                    Console.WriteLine(dato);
                    string[] subdatos = dato.Split('#');
                    
                    
                    if (subdatos[1] == "INSCRIBIR")
                    {
                        if (jugador1 == "")
                        { 
                            jugador1 = subdatos[2];
                            idJug1 = cli.Client.RemoteEndPoint.ToString();    
                            sw.WriteLine("#OK#");
                            sw.Flush();
                        }
                        else if (jugador2 =="")
                        {
                            jugador2 = subdatos[2];
                            idJug2 = cli.Client.RemoteEndPoint.ToString();
                            sw.WriteLine("#OK#");
                            sw.Flush();
                        }
                        else
                        {
                            sw.WriteLine("#NOK#ya hay dos jugadores#");
                            sw.Flush();
                        }
                    }

                    
                    
                    if (subdatos[1] == "JUGADA")
                    {
                        if ((subdatos[2] != "piedra") && (subdatos[2] != "papel") && (subdatos[2] != "tijera"))
                        {
                            sw.WriteLine("#NOK#valores válidos: piedra papel tijera");
                            sw.Flush();
                        }
                        if (idJug1 == cli.Client.RemoteEndPoint.ToString() ||
                            idJug2 == cli.Client.RemoteEndPoint.ToString())
                        {
                            if (idJug1 == cli.Client.RemoteEndPoint.ToString())
                            {
                                jugada1 = subdatos[2];

                                while (jugada2 == "")
                                {
                                    Thread.Sleep(100);
                                }
                            }
                            else if (idJug2 == cli.Client.RemoteEndPoint.ToString())
                            {
                                jugada2 = subdatos[2];

                                while (jugada1 == "")
                                {
                                    Thread.Sleep(100);
                                }
                            }
                            if ((jugada1 == "piedra" && jugada2 == "piedra") ||
                                (jugada1 == "papel" && jugada2 == "papel") ||
                                (jugada1 == "tijera" && jugada2 == "tijera"))
                            {
                                sw.WriteLine("#OK#empate#");
                                sw.Flush();
                            }
                            else if ((jugada1 == "piedra" && jugada2 == "tijera") ||
                                (jugada1 == "tijera" && jugada2 == "papel") ||
                                (jugada1 == "papel" && jugada2 == "piedra"))
                            {
                                sw.WriteLine("#OK#ganador:" + jugador1 + "#");
                                puntos1++;
                                sw.Flush();
                            }
                            else if ((jugada2 == "piedra" && jugada1 == "tijera") ||
                                (jugada2 == "tijera" && jugada1 == "papel") ||
                                (jugada2 == "papel" && jugada1 == "piedra"))
                            {
                                sw.WriteLine("#OK#ganador:" + jugador2 + "#");
                                puntos2++;
                                sw.Flush();
                            }

                            //dormimos el thread 1 segundo para que de tiempo a que el otro jugador analice el resultado
                            //antes de borrar las jugadas
                            Thread.Sleep(1000);
                            jugada1 = "";
                            jugada2 = "";
                        }
                       
                        else
                        {
                            sw.WriteLine("#NOK#jugador no en partida#");
                            sw.Flush();
                        }

                      
                    }

                    if (subdatos[1] == "PUNTUACION")
                    {
                        sw.WriteLine("#OK#" + jugador1 + ": " + puntos1.ToString() + "#" + jugador2 + ": " + puntos2.ToString() + "#");
                        sw.Flush();
                    }
                 
                }
                catch (IOException)
                {
                    break;
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
                TcpClient client = newsock.AcceptTcpClient();
                Thread t = new Thread(() => this.ManejarCliente(client));
                t.IsBackground = true;
                t.Start();

            }
        }
    }
}