using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _09_Servidor_PPT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string jugador1="";
        string idjug1 = "";
        string jugada1 = "";
        int puntos1 = 0;

        string jugador2="";
        string idjug2 = "";
        string jugada2 = "";
        int puntos2 = 0;

        private void ManejarCliente(TcpClient cli)
        {
            string data;
            NetworkStream ns = cli.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);

            sw.WriteLine("#INCRIBIR#nombre#");
            sw.WriteLine("#JUGADA#{piedra/papel/tijera}#");
            sw.WriteLine("#PUNTUACION#");
            sw.Flush();
            while (true)
            {
                try
                {
                    data = sr.ReadLine();
                    Console.WriteLine(data); //para depuración es server
                    string[] subdatos = data.Split('#');

                    #region INSCRIBIR
                    if (subdatos[1].Equals("INSCRIBIR"))
                    {
                        if (jugador1 == "")
                        {
                            jugador1 = subdatos[2];
                            idjug1 = cli.Client.RemoteEndPoint.ToString();
                            sw.WriteLine("#OK#");
                            sw.Flush();
                        }
                        else if (jugador2 == "")
                        {
                            jugador2 = subdatos[2];
                            idjug2 = cli.Client.RemoteEndPoint.ToString();
                            sw.WriteLine("#OK#");
                            sw.Flush();
                        }
                        else
                        {
                            sw.WriteLine("#NOK#jugador no en partida#");
                            sw.Flush();
                        }
                        #endregion
                    #region JUGADA
                        if (subdatos[1] == "JUGADA")
                        {
                            if ((subdatos[2] != "piedra") && (subdatos[2] != "papel") && (subdatos[2] != "tijera"))
                            {
                                sw.WriteLine("#NOK#valores válidos: piedra/papel/tijera");
                                sw.Flush();
                            }
                           // comprobamos quien hace la jugada
                            if (idjug1 == cli.Client.RemoteEndPoint.ToString() ||
                            idjug2 == cli.Client.RemoteEndPoint.ToString())
                            {
                                if (idjug1 == cli.Client.RemoteEndPoint.ToString())
                                {
                                    jugada1 = subdatos[2];

                                    //pausamos hasta que el jugador 2 no mete una jugada
                                    while (jugada2 == "")
                                    {
                                        Thread.Sleep(100);
                                    }
                                }
                                else if (idjug2 == cli.Client.RemoteEndPoint.ToString())
                                {
                                    jugada2 = subdatos[2];
                                    //pausamos hasta que el jugador1 no mete jugada
                                    while (jugada1 == "")
                                    {
                                        Thread.Sleep(100);
                                    }
                                }
                                //resolvemos la jugada
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
                                sw.WriteLine("#NOK#ya hay dos jugadores#");
                                sw.Flush();
                            }
                        }
                     
                    }
                    #endregion
                    #region PUNTUACION
                    if (subdatos[1] == "PUNTUACION")
                    {
                        sw.WriteLine("#OK#" + jugador1 + ": " + puntos1.ToString() + "#" + jugador2 + ": " + puntos2.ToString() + "#");
                        sw.Flush();
                    }
                    #endregion

                }
                catch (Exception error)
                {
                    Console.WriteLine("Error: {0}", error.ToString());
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
                TcpClient cliente = newsock.AcceptTcpClient(); //linea bloqueante
                Thread t = new Thread(() => this.ManejarCliente(cliente));
                //t.IsBackground = true;
                t.Start();
            }
        }
    }
}
