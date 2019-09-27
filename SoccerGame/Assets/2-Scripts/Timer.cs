using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public Text tiempoText;
    public float tiempo = 59f;

    public void Start()
    {

        tiempoText.text = "00:" + tiempo.ToString("f0");

    }


    public void Update()
    {

        if (tiempo >= 0)
        {

            tiempo -= Time.deltaTime;

            tiempoText.text = "00:" + tiempo.ToString("f0");
            if (tiempo < 9)
            {
                tiempoText.text = "00:" + "0" + tiempo.ToString("f0") + "";
            }
            else if (tiempo == 0)
            {
                tiempoText.text = "00:00";

            }

        }

    }


}
