/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

using System;
using System.Collections.Generic;
using NRKernal;
using UnityEngine;
using UnityEngine.VFX;

public class TrackingImageVisualizer : MonoBehaviour
{


    [SerializeField] private GameObject[] m_ParticleEffects;
    private Transform[] Pos3s;
    private List<NRTrackableImage> m_Images
            = new List<NRTrackableImage>();

    private bool isPlaying = false;

    private void Start()
    {
        //Cache all the Pos 3 Empty objects to avoid calling Find in the Update Method
        //Pos3 is the last movement target of the particle effects
        Pos3s = new Transform[m_ParticleEffects.Length];
        for(int i = 0; i<m_ParticleEffects.Length; i++)
        {
            Pos3s[i]=m_ParticleEffects[i].transform.Find("Pos3").transform;
        }
    }

    public void Update()
    {
        foreach (var image in m_Images)
        {

            int v = image.GetDataBaseIndex();
            if (v < m_ParticleEffects.Length && image.GetTrackingState() == TrackingState.Tracking) { 
                
            var center = image.GetCenterPose();
            m_ParticleEffects[v].transform.position = center.position;
            m_ParticleEffects[v].transform.rotation = Quaternion.Euler(90, 180, -90);
            //m_ParticleEffects[v].transform.rotation = center.rotation;

                Debug.Log("Updating image " + v+"  t  "+ center.position);
            if (v != 0)
            {
                    //place the last movement target of the previous particle effect to the start position of the new particle effect
                    Pos3s[v - 1].SetPositionAndRotation(center.position, Quaternion.Euler(90, 180, 0));
                    //Pos3s[v - 1].SetPositionAndRotation(center.position, center.rotation);
                }
        }
        }
        /*if (Image == null || Image.GetTrackingState() != TrackingState.Tracking)
        {
            return;
        }
        if (Image.GetDataBaseIndex() == 0)
        {
            var center = Image.GetCenterPose();
            firstParticleEffect.transform.position = center.position;
            firstParticleEffect.transform.rotation = center.rotation;
            firstParticleEffect.gameObject.SetActive(true);
            Debug.Log("First image");
        }

        if (Image.GetDataBaseIndex() == 1)
        {
            var center = Image.GetCenterPose();
            firstParticleEffect.transform.Find("Pos3").transform.SetPositionAndRotation(center.position, center.rotation);
            secondParticleEffect.transform.position = center.position;
            secondParticleEffect.transform.rotation = center.rotation;
            secondParticleEffect.gameObject.SetActive(true);
            Debug.Log("Second image");
        }

        if (Image.GetDataBaseIndex() == 2)
        {
            var center = Image.GetCenterPose();
            secondParticleEffect.transform.Find("Pos3").transform.SetPositionAndRotation(center.position, center.rotation);
            thirdParticleEffect.transform.position = center.position;
            thirdParticleEffect.transform.rotation = center.rotation;
            thirdParticleEffect.gameObject.SetActive(true);
            Debug.Log("Third image");
        }

        if (Image.GetDataBaseIndex() == 3)
        {
            var center = Image.GetCenterPose();
            thirdParticleEffect.transform.Find("Pos3").transform.SetPositionAndRotation(center.position, center.rotation);
            fourthParticleEffect.transform.position = center.position;
            fourthParticleEffect.transform.rotation = center.rotation;
            fourthParticleEffect.gameObject.SetActive(true);
            Debug.Log("Fourth image");
        }
        */

    }

    
    void playParticleSystem(int v)
    {
        //m_ParticleEffects[v].GetComponentInChildren<VisualEffect>().Play();
        m_ParticleEffects[v].gameObject.SetActive(true);
        isPlaying = true;
    }

    internal void PlayParticles(NRTrackableImage image)
    {
        int v = image.GetDataBaseIndex();
        Debug.Log("Playing Particle System: " + v);

        m_Images.Add(image);

        switch (v)
        {
            default:
                Debug.LogWarning("This Particle System hasn't been implemented"); break;
            case 0:
                playParticleSystem(0);
                break;

            case 1:
                playParticleSystem(1);
                break;

            case 2:
                playParticleSystem(2);
                break;

            case 3:
                playParticleSystem(3);
                break;
        }
    }
}


