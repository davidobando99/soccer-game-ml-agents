using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System.Globalization;

namespace Assets.Modelo
{
    class Cliente : MonoBehaviour
    {
        public string clientName;
        private int portToConnect = 6321;
        private string password;
        private bool socketReady;
        private TcpClient socket;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;
        public InputField clientNameInputField;
        public InputField serverAddressInputField;
        public InputField passwordInputField;
        private List<Unit> unitsOnMap = new List<Unit>();
        private Colide balon;
        private Timer timer;
        private int Goals;
       



        private void Start()
        {
            DontDestroyOnLoad(gameObject);
          
        }
        public int ShowGoals()
        {
            return Goals;
        }
        public void AddGoal(int sum)
        {
            Goals += sum;
            Send("Goal |" + this.Goals + "|");
        }
        //Conectar cliente al servidor
        public bool ConnectToServer(string host, int port)
        {
            if (socketReady)
                return false;

            try
            {
                socket = new TcpClient(host, port);
                stream = socket.GetStream();
                writer = new StreamWriter(stream);
                reader = new StreamReader(stream);


                socketReady = true;
            }
            catch (Exception e)
            {
                Debug.Log("Socket error " + e.Message);
            }

            return socketReady;
        }

        //Hilo cliente 
        private void Update()
        {


            if (socketReady)
            {

                if (stream.DataAvailable)
                {
                    string data = reader.ReadLine();
                    if (data != null)
                        OnIncomingData(data);
                }
            }
        }

        //Enviar mensaje al servidor
        public void Send(string data)
        {
            if (!socketReady)
                return;

            writer.WriteLine(data);
            writer.Flush();
        }

        // Leer mensajes del servidor
        private void OnIncomingData(string data)
        {
            string[] aData = data.Split('|');
            Debug.Log("Recibido desde el servidor: " + data);

            switch (aData[0])
            {
                case "QuienEres":
                    Send("YoSoy|" + clientName + "|" + password);
                    break;
                case "Autenticado":
                    
                    SceneManager.LoadScene("SampleScene");
                    break;
                case "UnidadAgregada":
                  
                    
                 

                    GameObject prefab = Resources.Load("prefabs/Player1") as GameObject;
                    GameObject go = Instantiate(prefab);

                    float parsedX = float.Parse(aData[3]);
                    float parsedY = float.Parse(aData[4]);


                    go.transform.position = new Vector2(parsedX, parsedY);
                    
                        
                    
                   
                    Unit un = go.AddComponent<Unit>();

                    unitsOnMap.Add(un);
                    int parsed;
                    Int32.TryParse(aData[2], out parsed);
                    un.unitID = parsed;
                    if (unitsOnMap.Count == 2)
                    {
                        if (balon == null)
                        {
                            balon = new Colide();
                            GameObject ball = Resources.Load("prefabs/Ball_opt") as GameObject;
                            GameObject ins = Instantiate(ball);

                            balon = ins.AddComponent<Colide>();
                            // balon.transform.position = new Vector2(10, 5);
                        }
                    }
                    if (unitsOnMap.Count == 2)
                    {
                        if (timer == null)
                        {
                            timer = new Timer();
                            GameObject time = GameObject.Find("Tiempo");
                            timer = time.AddComponent<Timer>();
                            timer.tiempoText = time.GetComponent<Text>();

                            //timer.tiempoText.text = "00:" + timer.tiempo.ToString("f0");



                        }
                    }
                    if (aData[1].Equals(clientName))
                    {
                        un.clientName = clientName;
                    }


                    






                    break;
                case "UnidadMovida":
                    if (aData[1] == clientName)
                    {
                        return;
                    }


                    else
                    {
                        Int32.TryParse(aData[2], out parsed);
                        foreach (Unit unit in unitsOnMap)
                        {
                            if (unit.unitID == parsed)
                            {
                                parsedX = float.Parse(aData[3]);
                                parsedY = float.Parse(aData[4]);
                                unit.transform.position = new Vector2(parsedX, parsedY);
                               
                                
                            }
                        }
                    }
                    break;
                case "Sincronizando":

                   
                        int numberOfUnitsOnServersMap;
                        Int32.TryParse(aData[1], out numberOfUnitsOnServersMap);
                        int serverUnitID;
                        int[] serverUnitIDs = new int[numberOfUnitsOnServersMap];

                        for (int i = 0; i < numberOfUnitsOnServersMap; i++)
                        {

                            Int32.TryParse(aData[2 + i * 3], out serverUnitID);
                            serverUnitIDs[i] = serverUnitID;
                            bool didFind = false;
                            print("cantidad units cliente  " + unitsOnMap.Count);
                            foreach (Unit unit in unitsOnMap) //synchronize existing units
                            {
                                if (unit.unitID == serverUnitID)
                                {
                                    parsedX = float.Parse(aData[3 + i * 3]);
                                    parsedY = float.Parse(aData[4 + i * 3]);

                                    unit.MoveTo(parsedX, parsedY);
                                    didFind = true;
                                }
                            }
                            if (!didFind) //add non-existing (at client) units
                            {
                                prefab = Resources.Load("prefabs/Player1") as GameObject;
                                go = Instantiate(prefab);
                                un = go.AddComponent<Unit>();
                                unitsOnMap.Add(un);
                                un.unitID = serverUnitID;
                                parsedX = float.Parse(aData[3 + i * 3]);
                                parsedY = float.Parse(aData[4 + i * 3]);
                                go.transform.position = new Vector2(parsedX, parsedY);



                            }
                        }


                        //remove units which are not on server's list (like disconnected ones)
                        foreach (Unit unit in unitsOnMap)
                        {
                            bool exists = false;
                            for (int i = 0; i < serverUnitIDs.Length; i++)
                            {
                                if (unit.unitID == serverUnitIDs[i])
                                {
                                    exists = true;
                                }
                            }
                            if (!exists)
                            {
                                Destroy(unit.gameObject);
                                unitsOnMap.Remove(unit);
                            }
                        }
                  
                    break;
                case "ballMoved":

                    if (!aData[1].Equals(clientName))
                    {
                        parsedX = float.Parse(aData[3]);
                        parsedY = float.Parse(aData[4]);
                        balon.transform.position = new Vector2(parsedX, parsedY);
                    }
                    
                    
                    break;
               
                
                //case "SincronizarTiempo":
                //    timer.tiempoText.text = aData[1];
                //    break;

                default:
                    Debug.Log("Comando recibido erroneo");
                    break;
            }
        }



        private void OnApplicationQuit()
        {
            CloseSocket();
        }
        private void OnDisable()
        {
            CloseSocket();
        }
        private void CloseSocket()
        {
            if (!socketReady)
                return;

            writer.Close();
            reader.Close();
            socket.Close();
            socketReady = false;
        }


        public void ConnectToServerButton()
        {

            password = passwordInputField.text;
            clientName = clientNameInputField.text;

            CloseSocket();
            try
            {
                ConnectToServer(serverAddressInputField.text, portToConnect);

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}
