//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace Server
//{
//    static class ServerUDP
//    {
//        public static ServerActionUDP server;
        
        
//        /// <summary>
//        /// The main entry point for the application.
//        /// </summary>
//        [STAThread]
//        static void Main()
//        {


//            server = new ServerActionUDP();

            
          

//        }

//    }

//    public class ServerActionUDP
//    {
//        private int port = 11000;

//        private List<ServerClientUDP> clients = new List<ServerClientUDP>();
//        private List<ServerClientUDP> disconnectList = new List<ServerClientUDP>();
//        private IPEndPoint IP;
//        private UdpClient server;
//        private bool serverStarted;
       
//        private bool ResyncNeeded = false;
        
//        public string time;

//        //the constructor, adds the listener
//        public ServerActionUDP()
//        {
//            IP = new IPEndPoint(IPAddress.Any, port);
//            try
//            {
//                server = new UdpClient(IP);
               
//                StartListening();
//                serverStarted = true;
//            }
//            catch (Exception e)
//            {
//                //Program.form.DebugTextBox.Text += "\r\n" + e.Message;
//            }
//        }


//        //Hilo del servidor 
//        //Run del huilo para saber si hay informacion llegando del (los) cliente(s)
//        public void Update()
//        {
//            if (!serverStarted)
//                return;

//            foreach (ServerClientUDP c in clients)
//            {

//                if (!IsConnected(c.udp))
//                {
//                    c.udp.Close();
//                    disconnectList.Add(c);
//                    continue;
//                }
//                else
//                {
//                    byte[] data = server.Receive(ref c.IP);
//                    list.Add(sender);
//                    Console.WriteLine("Client received");
//                    NetworkStream s = c.udp.Receive(c.);
//                    if (s.DataAvailable)
//                    {
//                        StreamReader reader = new StreamReader(s, true);
//                        string data = reader.ReadLine();

//                        if (data != null)
//                            OnIncomingData(c, data);
//                    }
//                }
//            }

//            //jugadores desconectados
           
           
//        }
//        //Empezar a escuchar al cliente despues de aceptarlo
//        private void StartListening()
//        {
//            server.b
//            server.BeginAcceptTcpClient(AcceptTcpClient, server);
//        }
//        //Aceptar un cliente y agregarlo a la lista de clientes conectados
//        private void AcceptUcpClient(IAsyncResult ar)
//        {
            
//            ServerClientUDP sc = new ServerClientUDP();
//            clients.Add(sc);

//            StartListening();
//            //request authentication from client
//            Broadcast("QuienEres|", clients[clients.Count - 1]);
//        }

//        private bool IsConnected(TcpClient c)
//        {
//            try
//            {
//                if (c != null && c.Client != null && c.Client.Connected)
//                {
//                    if (c.Client.Poll(0, SelectMode.SelectRead))
//                        return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

//                    return true;
//                }
//                else
//                    return false;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        private bool IsConnected(UdpClient c)
//        {
//            try
//            {
//                if (c != null && c.Client != null && c.Client.Connected)
//                {
//                    if (c.Client.Poll(0, SelectMode.SelectRead))
//                        return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

//                    return true;
//                }
//                else
//                    return false;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        //Enviar mensaje a los clientes conectados 
//        private void Broadcast(string data, List<ServerClient> cl)
//        {
//            foreach (ServerClient sc in cl)
//            {
//                try
//                {
//                    StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
//                    writer.WriteLine(data);
//                    writer.Flush();
//                }
//                catch (Exception e)
//                {
//                    Program.form.DebugTextBox.Text += "\r\n" + e.Message;
//                }
//            }
//        }

//        private void Broadcast(string data, ServerClient c)
//        {
//            List<ServerClient> sc = new List<ServerClient> { c };
//            Broadcast(data, sc);
//        }

//        // Leer informacion que llega al servidor
//        private void OnIncomingData(ServerClient c, string data)
//        {
//            string[] aData = data.Split('|');

//            //login
//            if (c.clientName != null)
//            {
//                //Program.form.DebugTextBox.Text += "\r\nClient '" + c.clientName + "' sent command: " + data;
//            }
//            else
//            {
//                Program.form.DebugTextBox.Text += "\r\nNuevo cliente tratando de unirse al servidor. Solicitud autenticacion.";
//                if (aData[0] == "YoSoy")
//                {

//                    bool authenticated = Database.AuthenticateUser(aData[1], aData[2]);
//                    if (!authenticated)
//                    {
//                        if (!Database.existUser(aData[1]))
//                        {
//                            Database.addUser(aData[1], aData[2]);
//                            c.clientName = aData[1];
//                            c.id = Database.getId(aData[1]);
//                            Program.form.DebugTextBox.Text += "\r\nUsuario Autenticado";
//                            Broadcast("Autenticado|", c);
//                        }

//                    }
//                    else if (authenticated)
//                    {
//                        foreach (ServerClient client in clients)
//                        {
//                            if (aData[1] == client.clientName)
//                            {
//                                Program.form.DebugTextBox.Text += "\r\nEste usuario ya esta conectado";
//                                c.tcp.Close();
//                                disconnectList.Add(c);
//                                return;
//                            }
//                        }
//                        c.clientName = aData[1];
//                        c.id = Database.getId(aData[1]);
//                        Program.form.DebugTextBox.Text += "\r\nUsuario Autenticado";
//                        Broadcast("Autenticado|", c);
//                    }
//                    else
//                    {
//                        Program.form.DebugTextBox.Text += "\r\nAuntenticacion de usuario fallida. ";
//                        c.tcp.Close();
//                        disconnectList.Add(c);
//                    }
//                    return;
//                }
//            }


//            //gameplay commands
//        }

      
     
//    }


//    public class ServerClientUDP
//    {
//        public string clientName;
//        public int id;
//        public IPEndPoint IP = new IPEndPoint(IPAddress.Any, 0);
//        public UdpClient udp;
      
//        public ServerClientUDP(UdpClient udp)
//        {
//            this.udp = udp;
//        }
        
//    }

   

   

//    //NOTE: never store sensitive user data like passwords like this in an actual product. use something like hashing... 
    
//}
