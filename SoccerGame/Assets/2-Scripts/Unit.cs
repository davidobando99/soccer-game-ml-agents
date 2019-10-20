using Assets.Controlador;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private float Speed;

    public int unitID;
    public bool isPlayersUnit;
    public string clientName;
    public int clientId;

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        //Obtengo posiciones




        //realizo el avance


        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (clientName.Equals("admin"))
            {
                print(clientId);
                float moveX = Input.GetAxis("Horizontal");
                float moveY = Input.GetAxis("Vertical");
                Vector2 moveVector = new Vector2(moveX * 5f * Time.deltaTime, moveY * 5f * Time.deltaTime);
                transform.Translate(moveVector);
                ControladorCliente.Cliente.Send("Moving|" + 0 + "|" + transform.position.x + "|" + transform.position.y);
                // ControladorCliente.Cliente.Send("SynchronizeRequest|"+"admin2");
            }
            else
            {
                float moveX = Input.GetAxis("Horizontal");
                float moveY = Input.GetAxis("Vertical");
                Vector2 moveVector = new Vector2(moveX * 5f * Time.deltaTime, moveY * 5f * Time.deltaTime);
                transform.Translate(moveVector);
                ControladorCliente.Cliente.Send("Moving|" + 1 + "|" + transform.position.x + "|" + transform.position.y);
                //ControladorCliente.Cliente.Send("SynchronizeRequest|" + "admin");
            }





        }

        //Fijo limites
        //float newmovex = Mathf.Clamp(transform.position.x, 1, 17);
        //float newmovey = Mathf.Clamp(transform.position.y, 1, 9);
        //transform.position = new Vector3(newmovex, newmovey);

        // SendCordenadas();



    }
    public void MoveTo(float moveX, float moveY)
    {

        Vector2 moveVector = new Vector2(moveX, moveY);
        transform.Translate(moveVector);

   
    }




}
