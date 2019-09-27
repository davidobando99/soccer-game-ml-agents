using Assets.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Assets.Controlador
{
    class ControladorCliente : MonoBehaviour

    {

        public static Cliente Cliente;
        public Button jugarButton;

        private void Awake()
        {


            GameObject go = GameObject.Find("Cliente");
            if (go == null)
            {
                Debug.Log("Client object not found");
                SceneManager.LoadScene("ClientLogin");
                return;
            }
            Cliente = go.GetComponent<Cliente>();
            if (Cliente == null)
            {
                Debug.Log("Couldn't find client script");
                return;
            }

            print("awakeee");
            Cliente.Send("SynchronizeRequest|");
        }

        public void Jugar()
        {
           // print("EEEEE");
            Cliente.Send("Jugar|");
            jugarButton.interactable = false;

        }
        public void GetGoals(bool gol)
        {
            Cliente.AddGoal(1);
        }
        //public void Send(float x, float y, int unitID)
        //{
        //    String info = "Moving|" + unitID + "|" + x + "|" + y ;
        //    Cliente.Send(info);
        //}


    }
}
