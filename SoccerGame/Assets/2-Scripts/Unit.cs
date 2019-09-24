using Assets.Controlador;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private float Speed;
    private ControladorCliente cc;
    public int unitID;
    public bool isPlayersUnit;

    // Start is called before the first frame update
    void Start()
    {
        Speed = 5f;
        
        cc = new ControladorCliente();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Obtengo posiciones
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        

        //realizo el avance
        Vector2 moveVector = new Vector2(moveX * Speed * Time.deltaTime, moveY * Speed * Time.deltaTime);
        transform.Translate(moveVector);
        if (isPlayersUnit && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||Input.GetKey(KeyCode.D))
        {
            ControladorCliente.Cliente.Send("Moving|" + unitID + "|" + transform.position.x + "|" + transform.position.y);
            //if (unitID >= 2)
            //{
            //    ControladorCliente.Cliente.Send("SynchronizeRequest|");
            //}
           
        }
            
        //Fijo limites
        float newmovex = Mathf.Clamp(transform.position.x, 1, 17);
        float newmovey = Mathf.Clamp(transform.position.y, 1, 9);
        transform.position = new Vector3(newmovex, newmovey);
        
        // SendCordenadas();



    }
    public void MoveTo(Vector2 vector)
    {
        transform.Translate(vector);
       
        float NewmoveX = Mathf.Clamp(transform.position.x, 1, 17);
        float NewmoveY = Mathf.Clamp(transform.position.y, 1, 9);
        transform.position = new Vector3(NewmoveX, NewmoveY);
    }

 

}
