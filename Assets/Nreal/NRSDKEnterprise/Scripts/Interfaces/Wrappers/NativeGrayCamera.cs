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
    using System.Runtime.InteropServices;

    /// <summary> Session Native API. </summary>
    public class NativeGrayCamera : ICameraDataProvider
    {
        /// <summary> Handle of the native camera. </summary>
        private UInt64 m_NativeCameraHandle;

        /// <summary> Creates a new bool. </summary>
        /// <returns> True if it succeeds, false if it fails. </returns>
        public bool Create()
        {
            var result = NativeApi.NRGrayscaleCameraCreate(ref m_NativeCameraHandle);
            NativeErrorListener.Check(result, this, "Create");
            return result == NativeResult.Success;
        }

        /// <summary> Gets raw data. </summary>
        /// <param name="imageHandle"> Handle of the image.</param>
        /// <param name="eye">         The eye.</param>
        /// <param name="ptr">         [in,out] The pointer.</param>
        /// <param name="size">        [in,out] The size.</param>
        /// <returns> True if it succeeds, false if it fails. </returns>
        public bool GetRawData(UInt64 imageHandle, int eye, ref IntPtr ptr, ref int size)
        {
            uint data_width = 0;
            uint data_height = 0;
            var result = NativeApi.NRGrayscaleCameraImageGetData(m_NativeCameraHandle, imageHandle, eye, ref ptr, ref data_width, ref data_height);
            size = (int)(data_width * data_height);
            return result == NativeResult.Success;
        }

        /// <summary> Gets a resolution. </summary>
        /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
        ///                                                the required range.</exception>
        /// <param name="imageHandle"> Handle of the image.</param>
        /// <param name="eye">         The eye.</param>
        /// <returns> The resolution. </returns>
        public NativeResolution GetResolution(UInt64 imageHandle, int eye)
        {
            if (!Enum.IsDefined(typeof(NativeGrayEye), eye))
            {
                throw new ArgumentOutOfRangeException("GetResolution of eye:" + eye + ", which should be type of NativeGrayEye");
            }
            return NRFrameExtension.GetGrayCameraResolution((NativeGrayEye)eye);
        }

        /// <summary> Gets hmd time nanos. </summary>
        /// <param name="imageHandle"> Handle of the image.</param>
        /// <param name="eye">         The eye.</param>
        /// <returns> The hmd time nanos. </returns>
        public UInt64 GetHMDTimeNanos(UInt64 imageHandle, int eye)
        {
            UInt64 time = 0;
            NativeApi.NRGrayscaleCameraImageGetTime(m_NativeCameraHandle, imageHandle, eye, ref time);
            return time;
        }

        /// <summary> Callback, called when the set capture. </summary>
        /// <param name="callback"> The callback.</param>
        /// <param name="userdata"> (Optional) The userdata.</param>
        /// <returns> True if it succeeds, false if it fails. </returns>
        public bool SetCaptureCallback(CameraImageCallback callback, UInt64 userdata = 0)
        {
            var result = NativeApi.NRGrayscaleCameraSetCaptureCallback(m_NativeCameraHandle, callback, userdata);
            NativeErrorListener.Check(result, this, "SetCaptureCallback");
            return result == NativeResult.Success;
        }

        /// <summary> Sets image format. </summary>
        /// <param name="format"> Describes the format to use.</param>
        /// <returns> True if it succeeds, false if it fails. </returns>
        public bool SetImageFormat(CameraImageFormat format)
        {
            return true;
        }

        /// <summary> Starts a capture. </summary>
        /// <returns> True if it succeeds, false if it fails. </returns>
        public bool StartCapture()
        {
            var result = NativeApi.NRGrayscaleCameraStartCapture(m_NativeCameraHandle);
            NativeErrorListener.Check(result, this, "StartCapture");
            return result == NativeResult.Success;
        }

        /// <summary> Stops a capture. </summary>
        /// <returns> True if it succeeds, false if it fails. </returns>
        public bool StopCapture()
        {
            var result = NativeApi.NRGrayscaleCameraStopCapture(m_NativeCameraHandle);
            NativeErrorListener.Check(result, this, "StopCapture");
            return result == NativeResult.Success;
        }

        /// <summary> Destroys the image described by imageHandle. </summary>
        /// <param name="imageHandle"> Handle of the image.</param>
        /// <returns> True if it succeeds, false if it fails. </returns>
        public bool DestroyImage(UInt64 imageHandle)
        {
            var result = NativeApi.NRGrayscaleCameraImageDestroy(m_NativeCameraHandle, imageHandle);
            NativeErrorListener.Check(result, this, "DestroyImage");
            return result == NativeResult.Success;
        }

        /// <summary> Releases this object. </summary>
        /// <returns> True if it succeeds, false if it fails. </returns>
        public bool Release()
        {
            var result = NativeApi.NRGrayscaleCameraDestroy(m_NativeCameraHandle);
            NativeErrorListener.Check(result, this, "Release");
            return result == NativeResult.Success;
        }

        /// <summary> A native api. </summary>
        private struct NativeApi
        {
            /// <summary> Nr grayscale camera image get data. </summary>
            /// <param name="grayscale_camera_handle">       Handle of the grayscale camera.</param>
            /// <param name="grayscale_camera_image_handle"> Handle of the grayscale camera image.</param>
            /// <param name="eye">                           The eye.</param>
            /// <param name="out_grey_image">                [in,out] The out grey image.</param>
            /// <param name="out_width">                     [in,out] Width of the out.</param>
            /// <param name="out_height">                    [in,out] Height of the out.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGrayscaleCameraImageGetData(UInt64 grayscale_camera_handle,
                UInt64 grayscale_camera_image_handle, int eye, ref IntPtr out_grey_image,
                ref UInt32 out_width, ref UInt32 out_height);

            /// <summary> Nr grayscale camera image get time. </summary>
            /// <param name="grayscale_camera_handle">       Handle of the grayscale camera.</param>
            /// <param name="grayscale_camera_image_handle"> Handle of the grayscale camera image.</param>
            /// <param name="eye">                           The eye.</param>
            /// <param name="out_nano_time">                 [in,out] The out nano time.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGrayscaleCameraImageGetTime(UInt64 grayscale_camera_handle,
                UInt64 grayscale_camera_image_handle, int eye, ref UInt64 out_nano_time);

            /// <summary> Nr grayscale camera create. </summary>
            /// <param name="out_grayscale_camera_handle"> [in,out] Handle of the out grayscale camera.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGrayscaleCameraCreate(ref UInt64 out_grayscale_camera_handle);

            /// <summary> Nr grayscale camera destroy. </summary>
            /// <param name="grayscale_camera_handle"> Handle of the grayscale camera.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGrayscaleCameraDestroy(UInt64 grayscale_camera_handle);

            /// <summary> Callback, called when the nr grayscale camera set capture. </summary>
            /// <param name="grayscale_camera_handle"> Handle of the grayscale camera.</param>
            /// <param name="image_callback">          The image callback.</param>
            /// <param name="userdata">                The userdata.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeResult NRGrayscaleCameraSetCaptureCallback(
                UInt64 grayscale_camera_handle, CameraImageCallback image_callback, UInt64 userdata);

            /// <summary> Nr grayscale camera start capture. </summary>
            /// <param name="grayscale_camera_handle"> Handle of the grayscale camera.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGrayscaleCameraStartCapture(UInt64 grayscale_camera_handle);

            /// <summary> Nr grayscale camera stop capture. </summary>
            /// <param name="grayscale_camera_handle"> Handle of the grayscale camera.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGrayscaleCameraStopCapture(UInt64 grayscale_camera_handle);

            /// <summary> Nr grayscale camera image destroy. </summary>
            /// <param name="grayscale_camera_handle">       Handle of the grayscale camera.</param>
            /// <param name="grayscale_camera_image_handle"> Handle of the grayscale camera image.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGrayscaleCameraImageDestroy(UInt64 grayscale_camera_handle,
                UInt64 grayscale_camera_image_handle);
        };
    }
}
