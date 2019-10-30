using System;
using Emgu.CV;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

namespace Server
{
    class ServerUDP
    {

        double TotalFrame;
        public List<byte[]> Datos;
        double Fps;
        int FrameNo;
        bool IsReadingFrame;
        public bool enviar { get; set; }
        public UdpClient udpClient;
        public IPEndPoint ip;
        public List<IPEndPoint> clientes = new List<IPEndPoint>();
        VideoCapture capture;
        string FileName = "Videos/Video1.mp4";
        public List<byte[]> imagenes = new List<byte[]>();
        public ServerUDP()
        {
            enviar = false;

            Datos = new List<byte[]>();
            //Thread thdUDPServer2 = new Thread(ServerThread2);
            WriteFranes();
            Thread thdUDPServer = new Thread(serverThread);
            thdUDPServer.Start();


        }



        //Se encarga de escuchar al cliente, es decir está al tanto de lo que le envian.
        public void serverThread()
        {
            ip = new IPEndPoint(IPAddress.Any, 11000);
            udpClient = new UdpClient(ip);
            
                Console.WriteLine("ENVIAR DEL WHILE " + enviar);
            for (; true;)
            {
                
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Console.WriteLine("VOY EN LA MITAD DEL FOR" + enviar);

                //if(udpClient.Receive(ref RemoteIpEndPoint) != null) { 
                try { 
                byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);

                string returnData = Encoding.ASCII.GetString(receiveBytes);
                    Console.WriteLine("yo leii esto  " + returnData);
                clientes.Add(RemoteIpEndPoint);
                }
                catch
                {
                    Console.WriteLine("error socket");
                }
                //}
                if (clientes.Count > 0)
                {
                    Thread t = new Thread(ServerThread2);
                    t.Start(clientes.Count - 1);
                }

                //Thread.Sleep(250);
                
                Console.WriteLine(clientes.Count);
                //byte[] senddata = ImageToArray();
                //udpClient.Send(senddata, senddata.Length, RemoteIpEndPoint);
                //Console.WriteLine("Hola, por fin entre y le envie cosas al cliente.");


            }



        }

        //Se encarga de mandarle al cliente.
        public void ServerThread2(object o)
        {
            int ip = Convert.ToInt32(o);
            IPEndPoint IP = (IPEndPoint)clientes[ip];

            int i = 0;

            while (true)
                //{

                try
                {

                    if (i < Datos.Count)
                    {
                        // byte[] senddata = Encoding.ASCII.GetBytes("hello world");
                        byte[] senddata = Datos[i];
                        //udpClient.Send(imagenes[i], imagenes[i].Length, IP);
                        udpClient.Send(senddata, senddata.Length, IP);
                        Console.WriteLine("conta " + i);
                        i++;
                        Thread.Sleep(350);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                }



        }
        public void Boolean(bool send)
        {
            enviar = send;
        }


        public byte[] ImageToArray(Image newImage)
        {

            //Image newImagde = Image.FromFile("Videos/ff.gif");

            MemoryStream ms = new MemoryStream();
            newImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);


            return ms.ToArray();
        }

        public void WriteFranes()
        {

            using (var engine = new Engine())
            {
                var mp4 = new MediaFile { Filename = "Videos/Video1.mp4" };

                engine.GetMetadata(mp4);


                var i = 0;
                var x = 0;
                Console.WriteLine("Entrasndo");
                Console.WriteLine("Mili " + mp4.Metadata.Duration.TotalMilliseconds);
                while (i < mp4.Metadata.Duration.TotalMilliseconds - 400)
                {
                    var options = new ConversionOptions { Seek = TimeSpan.FromMilliseconds(i) };
                    var outputFile = new MediaFile { Filename = string.Format("{0}\\image-{1}.jpeg", "Videos/imagenes", i) };
                    engine.GetThumbnail(mp4, outputFile, options);
                    i += 200;
                    Image actual = Image.FromFile(outputFile.Filename);
                    actual.Tag = "image-" + i + ".jpeg";
                    byte[] send = ImageToArray(actual);
                    Datos.Add(send);
                    //PAQUETE QUE DEBE ENVIAR ,CADA FOTO ES UN PAQUETE
                    x++;
                    Console.WriteLine("Datos " + x);
                    Console.WriteLine("Contador " + i);
                }
                Console.WriteLine("Termine");
            }



        }


        public VideoWriter ShowFrames()
        {
            Size size = new Size(384, 288);


            var VideoWriter = new VideoWriter("Videos/anuncio.mp4", 1446269005, 8, size, true);
            int i = 0;
            Console.WriteLine("Voy a empezar");
            while (i < 5700)
            {
                string path = "Videos/imagenes/image-" + i + ".jpeg";


                Mat image = new Mat(path, Emgu.CV.CvEnum.ImreadModes.Color);
                Console.WriteLine("Haciendo el video fps " + i + " Empty? " + image.IsEmpty);

                VideoWriter.Write(image);
                i += 300;

            }
            Console.WriteLine("ya terminé");

            return VideoWriter;

        }




    }

}



