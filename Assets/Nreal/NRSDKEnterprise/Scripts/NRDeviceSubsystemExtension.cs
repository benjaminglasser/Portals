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
    using AOT;
    using System;
    using System.Runtime.InteropServices;


    #region brightness KeyEvent on NrealLight.
    /// <summary> Values that represent nr brightness key events. </summary>
    public enum NRBrightnessKEYEvent
    {
        /// <summary> An enum constant representing the nr brightness key down option. </summary>
        NR_BRIGHTNESS_KEY_DOWN = 0,
        /// <summary> An enum constant representing the nr brightness key up option. </summary>
        NR_BRIGHTNESS_KEY_UP = 1,
    }

    /// <summary> Brightness key event. </summary>
    /// <param name="key"> The key.</param>
    public delegate void BrightnessKeyEvent(NRBrightnessKEYEvent key);
    /// <summary> Brightness value changed event. </summary>
    /// <param name="value"> The value.</param>
    public delegate void BrightnessValueChangedEvent(int value);

    /// <summary> Callback, called when the nr glasses control brightness key. </summary>
    /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
    /// <param name="key_event">              The key event.</param>
    /// <param name="user_data">              Information describing the user.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void NRGlassesControlBrightnessKeyCallback(UInt64 glasses_control_handle, int key_event, UInt64 user_data);

    /// <summary> Callback, called when the nr glasses control brightness value. </summary>
    /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
    /// <param name="value">                  The value.</param>
    /// <param name="user_data">              Information describing the user.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void NRGlassesControlBrightnessValueCallback(UInt64 glasses_control_handle, int value, UInt64 user_data);
    #endregion

    /// <summary> Callback, called when the nr glasses control temperature. </summary>
    /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
    /// <param name="temperature">            The temperature.</param>
    /// <param name="user_data">              Information describing the user.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void NRGlassesControlTemperatureCallback(UInt64 glasses_control_handle, int temperature, UInt64 user_data);

    /// <summary>
    /// The callback method type which will be called when an light intensity data is ready.
    /// </summary>
    /// <param name="glasses_control_handle"></param>
    /// <param name="value"></param>
    /// <param name="user_data"></param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void NRGlassesControlLightIntensityCallback(UInt64 glasses_control_handle, int value, IntPtr user_data);

    public delegate void LightIntensityChangedEvent(int value);

    public static class NRDeviceSubsystemExtension
    {
        /// <summary> Glass controll key event delegate for native. </summary>
        private delegate void NRGlassesControlKeyEventCallback(UInt64 glasses_control_handle, UInt64 key_event_handle, UInt64 user_data);

        #region Light intensity
        /// <summary>
        /// Event queue for all listeners interested in OnBrightnessKeyCallback events. </summary>
        private static event LightIntensityChangedEvent OnLightIntensityChanged;

        /// <summary>
        /// A NRDevice extension method that regis glasses controller extra callbacks. </summary>
        /// <param name="device"> The device to act on.</param>
        public static void RegisGlassesIntensityCallback(this NRDeviceSubsystem subsystem)
        {
            if (!subsystem.IsAvailable)
            {
                NRDebugger.Warning("[NRDevice] Can not regist event when glasses disconnect...");
                return;
            }
#if !UNITY_EDITOR
            NativeApi.NRGlassesControlSetLightIntensityCallback(subsystem.NativeGlassesHandler, LightIntensityChangedCallbackInternal, IntPtr.Zero);
#endif
        }

        [MonoPInvokeCallback(typeof(NRGlassesControlLightIntensityCallback))]
        private static void LightIntensityChangedCallbackInternal(UInt64 glasses_control_handle, int value, IntPtr user_data)
        {
            OnLightIntensityChanged?.Invoke(value);
        }

        /// <summary>
        /// Sets light intensity state.
        /// </summary>
        /// <param name="state">1:open, 0:close</param>
        /// <returns>The result</returns>
        public static bool SetLightIntensityState(this NRDeviceSubsystem subsystem, int state)
        {
            if (!subsystem.IsAvailable)
            {
                return false;
            }
#if !UNITY_EDITOR
            var result = NativeApi.NRGlassesControlSetLightIntensityState(subsystem.NativeGlassesHandler, state);
            return result == NativeResult.Success;
#else
            return true;
#endif
        }

        /// <summary>
        /// Gets light intensity state.
        /// </summary>
        /// <returns>1:open, 0:close</returns>
        public static int GetLightIntensityState(this NRDeviceSubsystem subsystem)
        {
            if (!subsystem.IsAvailable)
            {
                return 0;
            }
#if !UNITY_EDITOR
            int state = 0;
            NativeApi.NRGlassesControlGetLightIntensityState(subsystem.NativeGlassesHandler, ref state);
            return state;
#else
            return 0;
#endif
        }

        /// <summary>
        /// A NRDevice extension method that adds an event listener to 'callback'. </summary>
        /// <param name="callback"> The LightIntensityChangedEvent callback.</param>
        public static void AddLightIntensityEventListener(this NRDeviceSubsystem subsystem, LightIntensityChangedEvent callback)
        {
            OnLightIntensityChanged += callback;
        }

        /// <summary> A NRDevice extension method that removes the event listener. </summary>
        /// <param name="callback"> The LightIntensityChangedEvent callback.</param>
        public static void RemoveLightIntensityEventListener(this NRDeviceSubsystem subsystem, LightIntensityChangedEvent callback)
        {
            OnLightIntensityChanged -= callback;
        }
        #endregion

        #region Glasses Info
        /// <summary> Event queue for all listeners interested in OnBrightnessKeyCallback events. </summary>
        private static event BrightnessKeyEvent OnBrightnessKeyCallback;
        /// <summary> Event queue for all listeners interested in OnBrightnessValueCallback events. </summary>
        private static event BrightnessValueChangedEvent OnBrightnessValueCallback;
        /// <summary> The brightness minimum. </summary>
        public const int BRIGHTNESS_MIN = 0;
        /// <summary> The brightness maximum. </summary>
        public const int BRIGHTNESS_MAX = 7;

        /// <summary> A NRDevice extension method that adds an event listener to 'callback'. </summary>
        /// <param name="device">   The device to act on.</param>
        /// <param name="callback"> The callback.</param>
        public static void AddEventListener(this NRDeviceSubsystem device, BrightnessKeyEvent callback)
        {
            OnBrightnessKeyCallback += callback;
        }

        /// <summary>
        /// A NRDevice extension method that adds an event listener to 'callback'. </summary>
        /// <param name="device">   The device to act on.</param>
        /// <param name="callback"> The callback.</param>
        public static void AddEventListener(this NRDeviceSubsystem device, BrightnessValueChangedEvent callback)
        {
            OnBrightnessValueCallback += callback;
        }

        /// <summary> A NRDevice extension method that removes the event listener. </summary>
        /// <param name="device">   The device to act on.</param>
        /// <param name="callback"> The callback.</param>
        public static void RemoveEventListener(this NRDeviceSubsystem device, BrightnessKeyEvent callback)
        {
            OnBrightnessKeyCallback -= callback;
        }

        /// <summary> A NRDevice extension method that removes the event listener. </summary>
        /// <param name="device">   The device to act on.</param>
        /// <param name="callback"> The callback.</param>
        public static void RemoveEventListener(this NRDeviceSubsystem device, BrightnessValueChangedEvent callback)
        {
            OnBrightnessValueCallback -= callback;
        }

        private const int MaxMessageSize = 1024;
        /// <summary> The points. </summary>
        private static byte[] m_GlassesStatusInfo;
        private static GCHandle m_TmpHandle;

        /// <summary> A NativeGlassesController extension method that gets a duty. </summary>
        /// <param name="glassescontroller"> The glassescontroller to act on.</param>
        /// <returns> The duty. </returns>
        public static int GetDuty(this NRDeviceSubsystem device)
        {
            if (!device.IsAvailable)
            {
                return -1;
            }

#if !UNITY_EDITOR
            int duty = -1;
            var result = NativeApi.NRGlassesControlGetDuty(device.NativeGlassesHandler, ref duty);
            return result == NativeResult.Success ? duty : -1;
#else
            return 0;
#endif
        }

        /// <summary> A NativeGlassesController extension method that sets a duty. </summary>
        /// <param name="glassescontroller"> The glassescontroller to act on.</param>
        /// <param name="duty">              The duty.</param>
        public static void SetDuty(this NRDeviceSubsystem device, int duty)
        {
            if (!device.IsAvailable)
            {
                return;
            }
#if !UNITY_EDITOR
            NativeApi.NRGlassesControlSetDuty(device.NativeGlassesHandler, duty);
#endif
        }

        /// <summary> A NativeGlassesController extension method that gets the brightness. </summary>
        /// <param name="glassescontroller"> The glassescontroller to act on.</param>
        /// <returns> The brightness. </returns>
        public static int GetBrightness(this NRDeviceSubsystem device)
        {
            if (!device.IsAvailable)
            {
                return -1;
            }

#if !UNITY_EDITOR
            int brightness = -1;
            var result = NativeApi.NRGlassesControlGetBrightness(device.NativeGlassesHandler, ref brightness);
            return result == NativeResult.Success ? brightness : -1;
#else
            return 0;
#endif
        }

        /// <summary> A NativeGlassesController extension method that sets the brightness. </summary>
        /// <param name="glassescontroller"> The glassescontroller to act on.</param>
        /// <param name="brightness">        The brightness.</param>
        public static void SetBrightness(this NRDeviceSubsystem device, int brightness)
        {
            if (!device.IsAvailable)
            {
                return;
            }

            AsyncTaskExecuter.Instance.RunAction(() =>
            {
#if !UNITY_EDITOR
                NativeApi.NRGlassesControlSetBrightness(device.NativeGlassesHandler, brightness);
#endif
            });
        }

        /// <summary> A NativeGlassesController extension method that gets a temprature. </summary>
        /// <param name="glassescontroller"> The glassescontroller to act on.</param>
        /// <param name="temperatureType">   Type of the temperature.</param>
        /// <returns> The temprature. </returns>
        public static int GetTemprature(this NRDeviceSubsystem device, NativeGlassesTemperaturePosition temperatureType)
        {
            if (!device.IsAvailable)
            {
                return 0;
            }

#if !UNITY_EDITOR
            int temp = -1;
            var result = NativeApi.NRGlassesControlGetTemperatureData(device.NativeGlassesHandler, temperatureType, ref temp);
            return result == NativeResult.Success ? temp : -1;
#else
            return 0;
#endif
        }

        /// <summary> A NativeGlassesController extension method that gets a version. </summary>
        /// <param name="glassescontroller"> The glassescontroller to act on.</param>
        /// <returns> The version. </returns>
        public static string GetVersion(this NRDeviceSubsystem device)
        {
            if (!device.IsAvailable)
            {
                return "";
            }

#if !UNITY_EDITOR
            byte[] bytes = new byte[128];
            var result = NativeApi.NRGlassesControlGetVersion(device.NativeGlassesHandler, bytes, bytes.Length);
            return System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);
#else
            return "";
#endif
        }

        /// <summary>
        /// A NativeGlassesController extension method that gets glasses identifier. </summary>
        /// <param name="glassescontroller"> The glassescontroller to act on.</param>
        /// <returns> The glasses identifier. </returns>
        public static string GetGlassesID(this NRDeviceSubsystem device)
        {
            if (!device.IsAvailable)
            {
                return "";
            }

#if !UNITY_EDITOR
            byte[] bytes = new byte[64];
            var result = NativeApi.NRGlassesControlGetGlassesID(device.NativeGlassesHandler, bytes, bytes.Length);
            return System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);
#else
            return "";
#endif
        }

        /// <summary>
        /// A NativeGlassesController extension method that gets glasses serial number. </summary>
        /// <param name="glassescontroller"> The glassescontroller to act on.</param>
        /// <returns> The glasses serial number. </returns>
        public static string GetGlassesSN(this NRDeviceSubsystem device)
        {
            if (!device.IsAvailable)
            {
                return "";
            }

#if !UNITY_EDITOR
            byte[] bytes = new byte[64];
            var result = NativeApi.NRGlassesControlGetGlassesSN(device.NativeGlassesHandler, bytes, bytes.Length);
            return System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);
#else
            return "";
#endif
        }

        /// <summary> A NativeGlassesController extension method that gets a mode. </summary>
        /// <param name="glassescontroller"> The glassescontroller to act on.</param>
        /// <returns> The mode. </returns>
        public static NativeGlassesMode GetMode(this NRDeviceSubsystem device)
        {
            if (!device.IsAvailable)
            {
                return NativeGlassesMode.ThreeD_1080;
            }

            NativeGlassesMode mode = NativeGlassesMode.TwoD_1080;
#if !UNITY_EDITOR
            var result = NativeApi.NRGlassesControlGet2D3DMode(device.NativeGlassesHandler, ref mode);
#endif
            return mode;
        }

        /// <summary> A NativeGlassesController extension method that sets a mode. </summary>
        /// <param name="glassescontroller"> The glassescontroller to act on.</param>
        /// <param name="mode">              The mode.</param>
        public static void SetMode(this NRDeviceSubsystem device, NativeGlassesMode mode)
        {
            if (!device.IsAvailable)
            {
                return;
            }

#if !UNITY_EDITOR
            NativeApi.NRGlassesControlSet2D3DMode(device.NativeGlassesHandler, mode);
#endif
        }

        public static string GetGlassesStatus(this NRDeviceSubsystem device)
        {
            if (!device.IsAvailable)
            {
                return "";
            }

#if !UNITY_EDITOR
            if (m_GlassesStatusInfo == null)
            {
                m_GlassesStatusInfo = new byte[MaxMessageSize];
                m_TmpHandle = GCHandle.Alloc(m_GlassesStatusInfo, GCHandleType.Pinned);
            }
            var result = NativeApi.NRGlassesControlGet7211ICStatus(device.NativeGlassesHandler, 1, (m_TmpHandle.AddrOfPinnedObject()), MaxMessageSize);
            return System.Text.UTF8Encoding.UTF8.GetString(m_GlassesStatusInfo);
#else
            return "";
#endif
        }

        public static void SetLoggerTrigger(this NRDeviceSubsystem device)
        {
            if (!device.IsAvailable)
            {
                return;
            }

#if !UNITY_EDITOR
            NativeApi.NRGlassesControlSetLogTrigger(device.NativeGlassesHandler, 1);
#endif
        }

        /// <summary> Executes the 'brightness key callback internal' action. </summary>
        /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
        /// <param name="key_event">              The key event.</param>
        /// <param name="user_data">              Information describing the user.</param>
        [MonoPInvokeCallback(typeof(NRGlassesControlBrightnessKeyCallback))]
        private static void OnBrightnessKeyCallbackInternal(UInt64 glasses_control_handle, int key_event, UInt64 user_data)
        {
            OnBrightnessKeyCallback?.Invoke((NRBrightnessKEYEvent)key_event);
        }

        /// <summary> Executes the 'brightness value callback internal' action. </summary>
        /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
        /// <param name="brightness">             The brightness.</param>
        /// <param name="user_data">              Information describing the user.</param>
        [MonoPInvokeCallback(typeof(NRGlassesControlBrightnessValueCallback))]
        private static void OnBrightnessValueCallbackInternal(UInt64 glasses_control_handle, int brightness, UInt64 user_data)
        {
            OnBrightnessValueCallback?.Invoke(brightness);
        }

        /// <summary> Event queue for all listeners interested in KeyEvent events. </summary>
        private static event NRGlassControlKeyEvent OnKeyEventCallback;

        /// <summary>
        /// A NRDevice extension method that adds an key event listener. </summary>
        /// <param name="callback"> The callback.</param>
        public static void AddKeyEventListener(this NRDeviceSubsystem device, NRGlassControlKeyEvent callback)
        {
            OnKeyEventCallback += callback;
        }

        /// <summary> A NRDevice extension method that removes the key event listener. </summary>
        /// <param name="callback"> The callback.</param>
        public static void RemoveKeyEventListener(this NRDeviceSubsystem device, NRGlassControlKeyEvent callback)
        {
            OnKeyEventCallback -= callback;
        }

        /// <summary> Executes the 'key event callback internal' action. </summary>
        /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
        /// <param name="key_event_handle">       Handle of the key event.</param>
        /// <param name="user_data">              Information describing the user.</param>
        [MonoPInvokeCallback(typeof(NRGlassesControlKeyEventCallback))]
        private static void OnKeyEventCallbackInternal(UInt64 glasses_control_handle, UInt64 key_event_handle, UInt64 user_data)
        {
            int keyType = 0;
            int keyFunc = 0;
            int keyParam = 0;

#if !UNITY_EDITOR
            NativeResult result = NativeApi.NRGlassesControlKeyEventGetType(glasses_control_handle, key_event_handle, ref keyType);
            if (result == NativeResult.Success)
                result = NativeApi.NRGlassesControlKeyEventGetFunction(glasses_control_handle, key_event_handle, ref keyFunc);
            if (result == NativeResult.Success)
                result = NativeApi.NRGlassesControlKeyEventGetParam(glasses_control_handle, key_event_handle, ref keyParam);
#endif
            NRKeyEventInfo keyEvtInfo = new NRKeyEventInfo();
            keyEvtInfo.keyType = (NRKeyType)keyType;
            keyEvtInfo.keyFunc = (NRKeyFunction)keyFunc;
            keyEvtInfo.keyParam = keyParam;

            OnKeyEventCallback?.Invoke(keyEvtInfo);
        }

        /// <summary>
        /// A NRDevice extension method that regis glasses controller extra callbacks. </summary>
        /// <param name="device"> The device to act on.</param>
        public static void RegisGlassesControllerExtraCallbacks(this NRDeviceSubsystem device)
        {
            if (!device.IsAvailable)
            {
                NRDebugger.Warning("[NRDevice] Can not regist event when glasses disconnect...");
                return;
            }

#if !UNITY_EDITOR
            NativeApi.NRGlassesControlSetBrightnessKeyCallback(device.NativeGlassesHandler, OnBrightnessKeyCallbackInternal, 0);
            NativeApi.NRGlassesControlSetBrightnessValueCallback(device.NativeGlassesHandler, OnBrightnessValueCallbackInternal, 0);
            NativeApi.NRGlassesControlSetKeyEventCallback(device.NativeGlassesHandler, OnKeyEventCallbackInternal, 0);
#endif
        }
        #endregion

        public static bool IsRGBCameraEnable(this NRDeviceSubsystem device)
        {
#if !UNITY_EDITOR
            return device.NativeHMD.NRHMDIsSensorEnabled(NRSensorType.NR_SENSOR_TYPE_RGB_CAMERA);
#else
            return false;
#endif
        }

        public static bool SetEyeIPD(this NRDeviceSubsystem device, int ipd)
        {
#if !UNITY_EDITOR
            return device.NativeHMD.NRHMDSetEyeIPD(ipd);
#else
            return false;
#endif
        }

        private struct NativeApi
        {
            /// <summary>
            /// Sets the callback method .
            /// </summary>
            /// <param name="glasses_control_handle">glasses_control_handle The handle of light intensity object.</param>
            /// <param name="data_callback">data_callback The callback function.</param>
            /// <param name="user_data">user_data The data which will be returned when callback is triggered.</param>
            /// <returns>The result of operation.</returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlSetLightIntensityCallback(UInt64 glasses_control_handle,
                NRGlassesControlLightIntensityCallback data_callback, IntPtr user_data);

            /// <summary>
            /// Enable/Disable light intensity notify.
            /// </summary>
            /// <param name="glasses_control_handle">glasses_control_handle The handle of light intensity object.</param>
            /// <param name="state">state  0 for disable, 1 for enable.</param>
            /// <returns>The result of operation.</returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlSetLightIntensityState(UInt64 glasses_control_handle, int state);

            /// <summary>
            /// Stops light intensity notify.
            /// </summary>
            /// <param name="glasses_control_handle">glasses_control_handle The handle of light intensity object.</param>
            /// <param name="out_state">out_state The state of light intensity function.</param>
            /// <returns>The result of operation.</returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlGetLightIntensityState(UInt64 glasses_control_handle, ref int out_state);

            /// <summary> Nr glasses control get duty. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="out_dute">               [in,out] The out dute.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlGetDuty(UInt64 glasses_control_handle, ref int out_dute);

            /// <summary> Nr glasses control set duty. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="dute">                   The dute.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlSetDuty(UInt64 glasses_control_handle, int dute);

            /// <summary> Nr glasses control get brightness. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="out_brightness">         [in,out] The out brightness.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlGetBrightness(UInt64 glasses_control_handle, ref int out_brightness);

            /// <summary> Nr glasses control set brightness. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="brightness">             The brightness.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlSetBrightness(UInt64 glasses_control_handle, int brightness);

            /// <summary> Nr glasses control get temperature data. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="position">               The position.</param>
            /// <param name="out_temperature">        [in,out] The out temperature.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlGetTemperatureData(UInt64 glasses_control_handle, NativeGlassesTemperaturePosition position, ref int out_temperature);

            /// <summary> Nr glasses control get version. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="out_version">            The out version.</param>
            /// <param name="len">                    The length.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlGetVersion(UInt64 glasses_control_handle, byte[] out_version, int len);

            /// <summary> Nr glasses control get glasses identifier. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="out_glasses_id">         Identifier for the out glasses.</param>
            /// <param name="len">                    The length.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlGetGlassesID(UInt64 glasses_control_handle,
               byte[] out_glasses_id, int len);

            /// <summary> Nr glasses control get glasses serial number. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="out_glasses_sn">         The out glasses serial number.</param>
            /// <param name="len">                    The length.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlGetGlassesSN(UInt64 glasses_control_handle,
                byte[] out_glasses_sn, int len);

            /// <summary> Nr glasses control get 3D mode. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="out_mode">               [in,out] The out mode.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlGet2D3DMode(UInt64 glasses_control_handle, ref NativeGlassesMode out_mode);

            /// <summary> Nr glasses control set 3D mode. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="mode">                   The mode.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRGlassesControlSet2D3DMode(UInt64 glasses_control_handle, NativeGlassesMode mode);

            /// <summary> Callback, called when the nr glasses control set brightness key. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="callback">               The callback.</param>
            /// <param name="user_data">              Information describing the user.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeResult NRGlassesControlSetBrightnessKeyCallback(UInt64 glasses_control_handle, NRGlassesControlBrightnessKeyCallback callback, UInt64 user_data);

            /// <summary> Callback, called when the nr glasses control set brightness value. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="callback">               The callback.</param>
            /// <param name="user_data">              Information describing the user.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeResult NRGlassesControlSetBrightnessValueCallback(UInt64 glasses_control_handle, NRGlassesControlBrightnessValueCallback callback, UInt64 user_data);

            /// <summary> Registe the callback when the nr glasses control issue key event. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="callback">               The called.</param>
            /// <param name="user_data">              Information describing the user.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeResult NRGlassesControlSetKeyEventCallback(UInt64 glasses_control_handle, NRGlassesControlKeyEventCallback callback, UInt64 user_data);

            /// <summary> Get key type of key event. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="key_event_handle">       Handle of key event.</param>
            /// <param name="out_key_event_type">     Key type retrieved.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeResult NRGlassesControlKeyEventGetType(UInt64 glasses_control_handle, UInt64 key_event_handle, ref int out_key_event_type);

            /// <summary> Get key function of key event. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="key_event_handle">       Handle of key event.</param>
            /// <param name="out_key_event_type">     Key funtion retrieved.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeResult NRGlassesControlKeyEventGetFunction(UInt64 glasses_control_handle, UInt64 key_event_handle, ref int out_key_event_function);

            /// <summary> Get key parameter of key event. </summary>
            /// <param name="glasses_control_handle"> Handle of the glasses control.</param>
            /// <param name="key_event_handle">       Handle of key event.</param>
            /// <param name="out_key_event_type">     Key parameter retrieved.</param>
            /// <returns> A NativeResult. </returns>
            [DllImport(NativeConstants.NRNativeLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeResult NRGlassesControlKeyEventGetParam(UInt64 glasses_control_handle, UInt64 key_event_handle, ref int out_key_event_param);

            [DllImport(NativeConstants.NRNativeLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeResult NRGlassesControlGet7211ICStatus(UInt64 glasses_control_handle, int flag, IntPtr out_status, int len);

            [DllImport(NativeConstants.NRNativeLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeResult NRGlassesControlSetLogTrigger(UInt64 glasses_control_handle, int state);
        }
    }
}