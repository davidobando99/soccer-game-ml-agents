using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class eventosBotones : MonoBehaviour
{
  
    public void botonJugarOnline()
    {
        SceneManager.LoadScene("ClientLogin");

    }

    public void botonSalir()
    {
        Application.Quit();

    }


}
