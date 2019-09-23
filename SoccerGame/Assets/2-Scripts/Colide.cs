using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colide : MonoBehaviour
{
    // Start is called before the first frame update
     void OnCollisionEnter2D(Collision2D collision)
    {
        print("yaper");
        
    }
    private void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 moveVector = new Vector2(moveX*2f , moveY* 2f);
        transform.Translate(moveVector);





        float  NewmoveX= Mathf.Clamp(transform.position.x,1,16);
        float NewmoveY = Mathf.Clamp(transform.position.y, 1, 9);
        transform.position = new Vector3(NewmoveX, NewmoveY);

        if (transform.position.x >= 16 && transform.position.y >=3 && transform.position.y<=7)
        {
            Vector3 centerField = new Vector2(9, 5);
            Quaternion centerField2 = new Quaternion();
            transform.SetPositionAndRotation(centerField,centerField2);
              
            
        }

    }
}
