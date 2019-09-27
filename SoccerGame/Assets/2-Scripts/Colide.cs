using Assets.Controlador;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Colide : MonoBehaviour
{
    private bool Kick = false;
    public string name = "balon";
    public Text Texto;
    public Text Texto2;
    private int num = 0;
    private ControladorCliente cc;
    // Start is called before the first frame update
    void OnCollisionEnter2D(Collision2D collision)
    {
        print("OnCollisionEnter");
        
    }
    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Mouse0) && Kick)
        {
            print("WTF");
            Vector2 kick = new Vector2(moveX * 3f, moveY * 3f);
            transform.Translate(kick);
            float NewmoveX = Mathf.Clamp(transform.position.x, 1, 16);
            float NewmoveY = Mathf.Clamp(transform.position.y, 1, 9);
            transform.position = new Vector3(NewmoveX, NewmoveY);
            ControladorCliente.Cliente.Send("Moving|" + "balon" + "|" + transform.position.x + "|" + transform.position.y);
            Kick = false;
        }

        VerifiedGoal();

    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        print("OnTriggerEnter2");

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 moveVector = new Vector2(moveX*0.8f , moveY*0.8f);
        transform.Translate(moveVector);

        Kick = true;

        float  NewmoveX= Mathf.Clamp(transform.position.x,1,16);
        float NewmoveY = Mathf.Clamp(transform.position.y, 1, 9);
        transform.position = new Vector3(NewmoveX, NewmoveY);
        ControladorCliente.Cliente.Send("Moving|" + "balon" + "|" + transform.position.x + "|" + transform.position.y);
        

    }
    public void VerifiedGoal()
    {

        if (transform.position.x >= 16 && transform.position.y >= 3 && transform.position.y <= 7)
        {
            print("Verificar el marcador");
            num++;
            Texto.text = num + "";
            Vector2 centerField = new Vector2(9, 5);
            Quaternion centerField2 = new Quaternion();
            transform.SetPositionAndRotation(centerField, centerField2);
            cc.GetGoals(true);



        }

    }

}
