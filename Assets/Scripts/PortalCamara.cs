using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamara : MonoBehaviour
{

    public Transform playerCamera;
    public Transform portal;
    public Transform otherPortal;

    void Update()
    {
        Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
        transform.position = portal.position + playerOffsetFromPortal;

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);

        Vector3 rot = transform.rotation.eulerAngles;
        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
        Vector3 newRot = Quaternion.LookRotation(newCameraDirection, Vector3.up).eulerAngles;

        rot = new Vector3(newRot.x, newRot.y, newRot.z);
        
        transform.rotation = Quaternion.Euler(newRot.x, newRot.y, 0f);



    }


}
