using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
	public Camera cameraRomeToTheatre;
	public Camera cameraTheatreToRome;
	public Camera cameraTheatreToBeach;
	public Camera cameraBeachToTheatre;

	public Material cameraMatRomeToTheatre;
	public Material cameraMatTheatreToRome;
	public Material cameraMatTheatreToBeach;
	public Material cameraMatBeachToTheatre;

	void Start () {


		if (cameraRomeToTheatre.targetTexture != null)
		{
			cameraRomeToTheatre.targetTexture.Release();
		}
		cameraRomeToTheatre.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		cameraMatRomeToTheatre.mainTexture = cameraRomeToTheatre.targetTexture;


		if (cameraTheatreToRome.targetTexture != null)
		{
			cameraTheatreToRome.targetTexture.Release();
		}
		cameraTheatreToRome.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		cameraMatTheatreToRome.mainTexture = cameraTheatreToRome.targetTexture;

		if (cameraTheatreToBeach.targetTexture != null)
		{
			cameraTheatreToBeach.targetTexture.Release();
		}
		cameraTheatreToBeach.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		cameraMatTheatreToBeach.mainTexture = cameraTheatreToBeach.targetTexture;


		if (cameraBeachToTheatre.targetTexture != null)
		{
			cameraBeachToTheatre.targetTexture.Release();
		}
		cameraBeachToTheatre.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		cameraMatBeachToTheatre.mainTexture = cameraBeachToTheatre.targetTexture;
	}
	
}
