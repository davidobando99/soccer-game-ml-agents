using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    static class Program
    {
        public static ServerAction server;
        public static ServerUDP serverUdp;
        public static Form1 form;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            server = new ServerAction();
            serverUdp = new ServerUDP();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new Form1();
            Application.Run(form);

        }

    }

    public class ServerAction
    {
        private int port = 6321;

        private List<ServerClient> clients = new List<ServerClient>();
        private List<ServerClient> disconnectList = new List<ServerClient>();
         
        private TcpListener server;
        private bool serverStarted;
        private List<Unit> units = new List<Unit>();
        private bool ResyncNeeded = false;
        private Ball balon = new Ball();
        public string time;

        //the constructor, adds the listener
        public ServerAction()
        {
            try
            {
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                StartListening();
                serverStarted = true;
            }
            catch (Exception e)
            {
                //Program.form.DebugTextBox.Text += "\r\n" + e.Message;
            }
        }


        //Hilo del servidor 
        //Run del huilo para saber si hay informacion llegando del (los) cliente(s)
        public void Update()
        {
            if (!serverStarted)
                return;

            foreach (ServerClient c in clients)
            {
                
                if (!IsConnected(c.tcp))
                {
                    c.tcp.Close();
                    disconnectList.Add(c);
                    continue;
                }
                else
                {
                    NetworkStream s = c.tcp.GetStream();
                    if (s.DataAvailable)
                    {
                        StreamReader reader = new StreamReader(s, true);
                        string data = reader.ReadLine();

                        if (data != null)
                            OnIncomingData(c, data);
                    }
                }
            }

            //jugadores desconectados
            for (int i = 0; i < disconnectList.Count - 1; i++)
            {
                for (int j = 0; j < units.Count; j++)
                {
                    if (units[j].clientName == disconnectList[i].clientName)
                    {
                        units.RemoveAt(j);
                    }
                }
                Program.form.DebugTextBox.Text += "\r\nUsuario desconectado:" + disconnectList[i].clientName;
                clients.Remove(disconnectList[i]);
                disconnectList.RemoveAt(i);
                ResyncNeeded = true;
            }
            //sincronizar jugadores cuando uno esta desconectado
            if (ResyncNeeded)
            {
                SynchronizeUnits();
                ResyncNeeded = false;
            }
        }
        //Empezar a escuchar al cliente despues de aceptarlo
        private void StartListening()
        {
            server.BeginAcceptTcpClient(AcceptTcpClient, server);
        }
        //Aceptar un cliente y agregarlo a la lista de clientes conectados
        private void AcceptTcpClient(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;

            string allUsers = "";
            foreach (ServerClient i in clients)
            {
                allUsers += i.clientName + '|';
            }

            ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));
            clients.Add(sc);

            StartListening();
            //request authentication from client
            Broadcast("QuienEres|", clients[clients.Count - 1]);
        }

        private bool IsConnected(TcpClient c)
        {
            try
            {
                if (c != null && c.Client != null && c.Client.Connected)
                {
                    if (c.Client.Poll(0, SelectMode.SelectRead))
                        return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        private bool IsConnected(UdpClient c)
        {
            try
            {
                if (c != null && c.Client != null && c.Client.Connected)
                {
                    if (c.Client.Poll(0, SelectMode.SelectRead))
                        return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        //Enviar mensaje a los clientes conectados 
        private void Broadcast(string data, List<ServerClient> cl)
        {
            foreach (ServerClient sc in cl)
            {
                try
                {
                    StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
                    writer.WriteLine(data);
                    writer.Flush();
                }
                catch (Exception e)
                {
                    Program.form.DebugTextBox.Text += "\r\n" + e.Message;
                }
            }
        }

        private void Broadcast(string data, ServerClient c)
        {
            List<ServerClient> sc = new List<ServerClient> { c };
            Broadcast(data, sc);
        }

        // Leer informacion que llega al servidor
        private void OnIncomingData(ServerClient c, string data)
        {
            string[] aData = data.Split('|');

            //login
            if (c.clientName != null)
            {
                //Program.form.DebugTextBox.Text += "\r\nClient '" + c.clientName + "' sent command: " + data;
            }
            else
            {
                Program.form.DebugTextBox.Text += "\r\nNuevo cliente tratando de unirse al servidor. Solicitud autenticacion.";
                if (aData[0] == "YoSoy")
                {

                    bool authenticated = Database.AuthenticateUser(aData[1], aData[2]);
                    if (!authenticated)
                    {
                        if (!Database.existUser(aData[1]))
                        {
                            Database.addUser(aData[1], aData[2]);
                            c.clientName = aData[1];
                            c.id = Database.getId(aData[1]);
                            Program.form.DebugTextBox.Text += "\r\nUsuario Autenticado";
                            Broadcast("Autenticado|", c);
                        }
                        
                    }
                    else if (authenticated)
                    {
                        foreach (ServerClient client in clients)
                        {
                            if (aData[1] == client.clientName)
                            {
                                Program.form.DebugTextBox.Text += "\r\nEste usuario ya esta conectado";
                                c.tcp.Close();
                                disconnectList.Add(c);
                                return;
                            }
                        }
                        c.clientName = aData[1];
                        c.id = Database.getId(aData[1]);
                        Program.form.DebugTextBox.Text += "\r\nUsuario Autenticado";
                        Broadcast("Autenticado|", c);
                    }
                    else
                    {
                        Program.form.DebugTextBox.Text += "\r\nAuntenticacion de usuario fallida. ";
                        c.tcp.Close();
                        disconnectList.Add(c);
                    }
                    return;
                }
            }


            //gameplay commands
            switch (aData[0])
            {
                case "SolicitudSincronizacion":
                    SynchronizeUnits(c,aData[1]);
                   // SynchronizeBalon(c);
                    break;
                case "Jugar":
                    Unit unit = new Unit();
                   
                    unit.clientName = c.clientName;
                    unit.clientId = c.id;
                    //give a new ID to the new units
                    int newid = 0;
                    //if (c.id%2!=0) unit.unitID = 0;
                    //else unit.unitID = 1;
                    foreach (Unit u in units)
                    {
                        if (u.unitID >= newid) { newid = u.unitID + 1; }
                        
                    }

                    unit.unitID = newid;






                    units.Add(unit);
                    if (units.Count == 1)
                    {
                        unit.unitPositionX = 3.37f;
                        unit.unitPositionY = 5.5f;
                    }
                    else if (units.Count == 2)
                    {
                        unit.unitPositionX = 14.68f;
                        unit.unitPositionY = 5.5f;
                    }
                        Broadcast("UnidadAgregada|" + c.clientName + "|" + unit.unitID + "|" + unit.unitPositionX + "|" + unit.unitPositionY + "|" + c.id, clients );
                   
                    
                    break;
                case "Moving":
                    
                    if (aData[1].Equals("balon"))
                    {
                        Broadcast("ballMoved|" + c.clientName + "|" + aData[1] + "|" + aData[2] + "|" + aData[3], clients);
                        int ball;
                        Int32.TryParse(aData[1], out ball);
                        float X;
                        float Y;

                        float.TryParse(aData[2], out X);
                        float.TryParse(aData[3], out Y);
                        balon.ballPositionX = X;
                        balon.ballPositionY = Y;
                        balon.clientName = c.clientName;
                    }
                    else
                    {
                        Broadcast("UnidadMovida|" + c.clientName + "|" + aData[1] + "|" + aData[2] + "|" + aData[3], clients);
                        int id;
                        Int32.TryParse(aData[1], out id);
                        float parsedX;
                        float parsedY;

                        float.TryParse(aData[2], out parsedX);
                        float.TryParse(aData[3], out parsedY);

                        foreach (Unit u in units)
                        {
                            if (u.unitID == id)
                            {
                                // Console.WriteLine(u.unitID+"");
                                // Console.WriteLine(parsedX + "");
                                // Console.WriteLine(parsedY + "");
                                u.unitPositionX = parsedX;
                                u.unitPositionY = parsedY;

                            }
                        }

                        //Program.form.DebugTextBox.Text += "\r\n" + parsedX + "  " + parsedY;

                    }

                    break;
                case "Goal":
                    string GoalsAmount = aData[1];
                    Broadcast("ChangeMarker|" + c.clientName + "|" + GoalsAmount, clients);

                    break;


                

                default:
                    //Program.form.DebugTextBox.Text += "\r\nReceived unknown signal => skipping";
                    break;
            }
        }

        //Sincronizar un cliente
        private void SynchronizeUnits(ServerClient c, string nameUnit)
        {
            if (nameUnit.Equals("balon"))
            {
                SynchronizeBalon(c);
            }
            else { 
            string dataToSend = "Sincronizando|" + units.Count;
                foreach (Unit u in units)
                {
                dataToSend += "|" + (u.unitID) + "|" + u.unitPositionX + "|" + u.unitPositionY;
                }
                Broadcast(dataToSend, c);
            }
            
            //Program.form.DebugTextBox.Text += "\r\nSynchronization request sent: " + dataToSend;
        }
        // Sincronizar balon
        private void SynchronizeBalon(ServerClient c)
        {
            string dataToSend = "Sincronizando|"

               + "balon|" +balon.ballPositionX + "|" + balon.ballPositionY;
            
            Broadcast(dataToSend, c);
        }

        //Sincronizar todos los clientes
        private void SynchronizeUnits()
        {
            string dataToSend = "Sincronizando|" + units.Count;
            foreach (Unit u in units)
            {
                dataToSend += "|" + (u.unitID) + "|" + u.unitPositionX + "|" + u.unitPositionY;
            }
            Broadcast(dataToSend, clients);
            // Program.form.DebugTextBox.Text += "\r\nSynchronization request sent: " + dataToSend;
        }
    }


    public class ServerClient
    {
        public string clientName;
        public int id;
        public string typeConection;
        public TcpClient tcp;
       
        public ServerClient(TcpClient tcp)
        {
            this.tcp = tcp;

        }

        
    }

    public class Unit
    {
        public string clientName;
        public int clientId;
        public int unitID;
        public float unitPositionX;
        public float unitPositionY;

    }
    
    public class Ball
    {

        public string clientName;
        public int ballID;
        public float ballPositionX;
        public float ballPositionY;
    }

    //NOTE: never store sensitive user data like passwords like this in an actual product. use something like hashing... 
    public static class Database
    {
        public static bool AuthenticateUser(string username, string password)
        {
            int result = 0;
            result = (int)Program.form.usersTableAdapter.Authenticate(username, password);
            if (result == 1)
            {
                return true;
            }
            else return false;
        }

        public static void addUser(string username, string password)
        {
            Program.form.usersTableAdapter.InsertUser(username, password);
            
        }
        public static void users()
        {
            Program.form.usersTableAdapter.GetData();
        }
        public static int getId(string username)
        {
            
           return Program.form.usersTableAdapter.GetData().First(i => i.Name.Equals(username)).Id;
        }

        public static bool existUser(string username)
        {
            return Program.form.usersTableAdapter.GetData().Where(i => i.Name.Equals(username)).Any();
        }


    }
}
