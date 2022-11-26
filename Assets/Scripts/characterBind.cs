using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterBind : MonoBehaviour
{

    public GameObject character;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = character.transform.position -new Vector3(0,0.5f,0);
        gameObject.transform.rotation = character.transform.rotation;
    }
}
