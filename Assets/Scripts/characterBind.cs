using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterBind : MonoBehaviour
{

    public GameObject cam;
    public GameObject parentCam;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = cam.transform.position -new Vector3(0,0.5f,0);

        // Vector3 camRot = parentCam.transform.localEulerAngles; 
        // Vector3 rot = gameObject.transform.localEulerAngles; 

        // Debug.Log(camRot.y);   

        // Quaternion rot = transform.rotation; 
        // rot = new Vector3 (camRot.y, camRot.y, camRot.y);
        // gameObject.transform.rotation.y = character.transform.rotation.y;
    }
}
