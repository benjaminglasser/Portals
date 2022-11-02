using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamara : MonoBehaviour
{

    public Transform playerCamera;
    // portal in theatre
    public Transform PortalCameraIsIn;
      // portal in rome
    public Transform PortalPlayerIsIn;
  

    void LateUpdate()
    {
        Vector3 playerOffsetFromPortal = playerCamera.position - PortalPlayerIsIn.position;
        transform.position = PortalCameraIsIn.position + playerOffsetFromPortal;

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(PortalCameraIsIn.rotation, PortalPlayerIsIn.rotation);

        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);

    }


}
