using Assets.Controlador;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private float Speed;
    private ControladorCliente cc;
    // Start is called before the first frame update
    void Start()
    {
        Speed = 5f;
        cc = new ControladorCliente();
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 moveVector = new Vector2(moveX*Speed*Time.deltaTime, moveY*Speed* Time.deltaTime);
        transform.Translate(moveVector);

        float NewmoveX = Mathf.Clamp(transform.position.x, 1, 17);
        float NewmoveY = Mathf.Clamp(transform.position.y, 1, 9);
        transform.position = new Vector3(NewmoveX, NewmoveY);

        

    }
    public void SendCordenadas()
    {
        cc.CoordenadasGameObject(transform.position.x, transform.position.y);
      
    }

}
