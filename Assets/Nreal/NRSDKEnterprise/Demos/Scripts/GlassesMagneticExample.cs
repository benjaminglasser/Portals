/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace NRKernal.Enterprise.NRExamples
{
    public class GlassesMagneticExample : MonoBehaviour
    {
        public Text m_Lable;
        private NRGlassesMagneticProvider m_NRGlassesMagneticProvider;

        void Start()
        {
            m_NRGlassesMagneticProvider = new NRGlassesMagneticProvider();
            m_NRGlassesMagneticProvider.Start();
        }

        void Update()
        {
            var frame = m_NRGlassesMagneticProvider.GetCurrentFrame();
            m_Lable.text = string.Format("magnetic:{0} timestamp:{1}", frame.magnetic, frame.timestamp);
        }
    }
}