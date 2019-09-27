using Assets.Controlador;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Colide : MonoBehaviour
{
    private bool Kick;
    public string name = "balon";
    public Text Text;
    public Text Text2;
    private int num1;
    private int num2;
    private ControladorCliente cc;
    // Start is called before the first frame update
    void Start()
    {
        cc = new ControladorCliente();
        num1 = 0;
        num2 = 0;
        Kick = false;
        Text = GameObject.Find("Text1").GetComponent<Text>();
        Text2 = GameObject.Find("Text2").GetComponent<Text>();

    }
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
            //print(" ANTES X " + transform.position.x + " Y " + transform.position.y);
            //print("Verificar el marcador");
            num1++;
            Text2.text = num1 + "";

            Vector2 centerField = new Vector2(9, 5);
            //Quaternion centerField2 = new Quaternion();
            transform.position = centerField;
            print(" DESPUES X " + transform.position.x + " Y " + transform.position.y);

             cc.SetGoals(true);




        }
        else if (transform.position.x <= 2.37 && transform.position.y <= 6.78 && transform.position.y >= 4.14)
        {
            print("Verificar el marcador2");
            num2++;
            Text.text = num2 + "";
            Vector2 centerField = new Vector2(9, 5);
            transform.position = centerField;
            cc.SetGoals(true);
        }


    }

}
