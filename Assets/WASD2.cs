using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASD2 : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    
    private Rigidbody rigid;
    
    void Start()
    {
        rigid = GetComponent<Rigidbody> ();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3 (Horizontal, 0.0f, Vertical);

        rigid.AddForce (move * speed);
    }
}
