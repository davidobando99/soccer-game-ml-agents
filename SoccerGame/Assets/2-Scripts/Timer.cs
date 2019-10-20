using Assets.Controlador;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public Text tiempoText;
    public bool halfTime = true;
    public int contadorTime;
    public float tiempo = 15f;

    //public void Start()
    //{

    //    tiempoText.text = "00:" + tiempo.ToString("f0");

    //}


    public void Update()
    {
        if (halfTime)
        {
            if (tiempo > 0)
            {

                tiempo -= Time.deltaTime;

                tiempoText.text = "00:" + tiempo.ToString("f0");
                if (tiempo < 9)
                {
                    //tiempoText.text = "hola";
                    tiempoText.text = "00:" + "0" + tiempo.ToString("f0") + "";


                    // 
                }
                //else if (tiempo == 1)
                //{
                //    //tiempoText.text = "00:00";
                //    SceneManager.LoadScene("halfTime");

                //}




                //ControladorCliente.Cliente.Send("Tiempo|" + ControladorCliente.Cliente.clientName+"|"+tiempoText.text);
            }
            else
            {
                if (contadorTime == 0)
                {
                    tiempo = 5f;

                    contadorTime++;
                    SceneManager.LoadScene("halfTime");
                }
                else
                {
                    halfTime = false;
                    tiempo = 15f;
                }





            }
        }
        else
        {
            SceneManager.LoadScene("SampleScene");
            if (tiempo > 0)
            {

                tiempo -= Time.deltaTime;

                tiempoText.text = "00:" + tiempo.ToString("f0");
                if (tiempo < 9)
                {
                    //tiempoText.text = "hola";
                    tiempoText.text = "00:" + "0" + tiempo.ToString("f0") + "";


                    // 
                }
            }
        }

        }


    }

