using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class eventosBotones : MonoBehaviour
{
  
    public void botonJugarOnline()
    {
        SceneManager.LoadScene("ClientLogin");

    }

    public void botonJugarBOTS()
    {
        //ProcessStartInfo psi = new ProcessStartInfo();
        //psi.UseShellExecute = true;



        Process.Start(@"ml-agents\UnitySDK\Unity Environment.exe");



    }

    public void botonSalir()
    {
        Application.Quit();

    }


}
