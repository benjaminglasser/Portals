using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
	public Camera cameraA;
	public Camera cameraB;
	public Camera cameraC;
	public Camera cameraD;

	public Material cameraMatA;
	public Material cameraMatB;
	public Material cameraMatC;
	public Material cameraMatD;

	private int widthAR = 1920;
	private int heightAR = 1080;

	private int widthScreen = Screen.width;
	private int heightScreen = Screen.height;

	void Start () {

		

		if (cameraA.targetTexture != null)
		{
			cameraA.targetTexture.Release();
		}
		cameraA.targetTexture = new RenderTexture(widthAR, heightAR, 24);
		cameraMatA.mainTexture = cameraA.targetTexture;


		if (cameraB.targetTexture != null)
		{
			cameraB.targetTexture.Release();
		}
		cameraB.targetTexture = new RenderTexture(widthAR, heightAR, 24);
		cameraMatB.mainTexture = cameraB.targetTexture;

		if (cameraC.targetTexture != null)
		{
			cameraC.targetTexture.Release();
		}
		cameraC.targetTexture = new RenderTexture(widthAR, heightAR, 24);
		cameraMatC.mainTexture = cameraC.targetTexture;
		
		if (cameraD.targetTexture != null)
		{
			cameraD.targetTexture.Release();
		}
		cameraD.targetTexture = new RenderTexture(widthAR, heightAR, 24);
		cameraMatD.mainTexture = cameraD.targetTexture;
	}
	
}
