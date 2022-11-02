using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamara : MonoBehaviour
{

    public Transform playerCamera;
    // portal in theatre
    public Transform nextWorldPortal;
      // portal in rome
    public Transform currentWorldPortal;
  

    void LateUpdate()
    {
        Vector3 playerOffsetFromPortal = playerCamera.position - currentWorldPortal.position;
        transform.position = nextWorldPortal.position + playerOffsetFromPortal;

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(nextWorldPortal.rotation, currentWorldPortal.rotation);

        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);

    }


}
