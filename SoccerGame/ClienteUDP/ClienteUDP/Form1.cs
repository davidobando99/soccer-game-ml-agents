using Emgu.CV;
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


namespace ClienteUDP
{
    
    public partial class Form1 : Form
    {

        public bool video = false;
        //public UdpClient udpClient;
        //public IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
        public Form1()
        {
            //udpClient = new UdpClient();
            //udpClient.Connect(RemoteIpEndPoint);
            InitializeComponent();
            Thread thdUDPServer = new Thread(Connect);
            thdUDPServer.Start();
            //Thread.Sleep(1000);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //private void Send(string datos)
        //{
           
        //    Byte[] senddata = Encoding.ASCII.GetBytes(datos);
        //    udpClient.Send(senddata, senddata.Length);
        //}
        public void Connect()
        {

            var client = new UdpClient();
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000); 
            client.Connect(RemoteIpEndPoint);

            // send data
            byte[] m = Encoding.ASCII.GetBytes("hola soy el cliente");
            client.Send(m, m.Length);
            Console.WriteLine("ooooooo");
            int i = 0;
            while (true) {
                Console.WriteLine("uuuuuuu");

                var receiveBytes = client.Receive(ref RemoteIpEndPoint);
                
            Image img = ByteArrayToImage(receiveBytes);
                
                img.Save("Video/foto-"+i+".jpeg");
                Console.WriteLine("Foto " + i);
                //pictureBox1.Image = img;
                i++;
            string returnData = Encoding.ASCII.GetString(receiveBytes);
            //txtDatos.Text = returnData;

                Console.WriteLine("DATOS DEL SERVER " + "imagen");
                Thread.Sleep(100);

                if (i == 28)
                {
                    ShowFrames();
                }
               

            }
        }

        public void ShowFrames()
        {
            Size size = new Size(384, 288);


            var VideoWriter = new VideoWriter("Video/videito1.mp4", 1446269005, 10, size, true);
            int i = 0;
            Console.WriteLine("Voy a empezar");
            while (i < 29)
            {
                string path = "Video/foto-" + i + ".jpeg";


                Mat image = new Mat(path, Emgu.CV.CvEnum.ImreadModes.Color);
                Console.WriteLine("Haciendo el video fps " + i + " Empty? " + image.IsEmpty);

                VideoWriter.Write(image);
                i ++;

            }
            VideoWriter.Dispose();
            Console.WriteLine("ya terminé");
            video = true;

           

        }

        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);

            //txtDatos.Text = "Ya pusé la foto";
            

            return returnImage;
        }

        private void butRead_Click(object sender, EventArgs e)
        {

            if (video)
            {
                axWindowsMediaPlayer1.URL = "Video/videito1.mp4";
                axWindowsMediaPlayer1.Ctlcontrols.play();

            }
        
            
            //Connect("Holi soy tu cliente");
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
