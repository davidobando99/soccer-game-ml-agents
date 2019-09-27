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
        public string time;

        private void Awake()
        {


            GameObject go = GameObject.Find("Cliente");
            if (go == null)
            {
                Debug.Log("GameObject cliente no encontrado");
                SceneManager.LoadScene("ClientLogin");
                return;
            }
            Cliente = go.GetComponent<Cliente>();
            if (Cliente == null)
            {
                Debug.Log("No se pudo encontrar el script");
                return;
            }

            print("awakeee");
            Cliente.Send("SolicitudSincronizacion|");
        }

        public void Jugar()
        {
           // print("EEEEE");
            Cliente.Send("Jugar|");
            jugarButton.interactable = false;

        }
        public void SetGoals(bool gol)
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
