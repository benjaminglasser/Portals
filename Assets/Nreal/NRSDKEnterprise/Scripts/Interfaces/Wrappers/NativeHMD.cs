/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

namespace NRKernal.Enterprise
{
    using System;
    using UnityEngine;
    using System.Runtime.InteropServices;

    public enum NRSensorType
    {
        NR_SENSOR_TYPE_RGB_CAMERA = 1,
    }
    /// <summary> HMD Gray Eye offset Native API . </summary>
    public static class NativeHMDExtension
    {

        /// <summary> A NativeHMD extension method that gets eye pose from head. </summary>
        /// <param name="nativehmd"> The nativehmd to act on.</param>
        /// <param name="eye">       The eye.</param>
        /// <returns> The eye pose from head. </returns>
        [Obsolete("Use 'NRFrame.GetDevicePoseFromHead' instead.")]
        public static Pose GetEyePoseFromHead(this NativeHMD nativehmd, NativeGrayEye eye)
        {
            return nativehmd.GetDevicePoseFromHead((int)eye);
        }

        /// <summary> Get if RGBCamera is enable(It's disable while PowerSavingMode is open). </summary>
        /// <param name="nativehmd"> The nativehmd to act on.</param>
        /// <returns> Is RGBCamera enable or not. </returns>
        public static bool NRHMDIsSensorEnabled(this NativeHMD nativehmd, NRSensorType sensorType)
        {
            bool enable = true;
            NativeResult result = NativeApi.NRHMDIsSensorEnabled(nativehmd.HmdHandle, sensorType, ref enable);
            return result == NativeResult.Success ? enable : true;
        }

        /// <summary> Get if RGBCamera is enable(It's disable while PowerSavingMode is open). </summary>
        /// <param name="nativehmd"> The nativehmd to act on.</param>
        /// <param name="ipd"> IPD of the two eyes.</param>
        /// <returns> Is RGBCamera enable or not. </returns>
        public static bool NRHMDSetEyeIPD(this NativeHMD nativehmd, int ipd)
        {
            bool enable = true;
            NativeResult result = NativeApi.NRHMDSetEyeIPD(nativehmd.HmdHandle, ipd);
            return result == NativeResult.Success ? enable : true;
        }

        /// <summary> A native api. </summary>
        private struct NativeApi
        {
            /// <summary> Nrhmd create. </summary>
            /// <param name="out_hmd_handle"> [in,out] Handle of the out hmd.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRHMDIsSensorEnabled(UInt64 hmd_handle, NRSensorType sensorType,  ref bool out_is_enabled);


            /// <summary> Nrhmd set eye's IPD. </summary>
            /// <param name="hmd_handle"> Handle of the hmd.</param>
            /// <param name="eye_ipd"> IPD of two eyes.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRHMDSetEyeIPD(UInt64 hmd_handle, int eye_ipd);
        }
    }
}
