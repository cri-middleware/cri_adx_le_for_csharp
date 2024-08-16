/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Threading;
using CriWare.InteropHelpers;

namespace CriWare
{
	public partial class CriAtom
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtom_SetUserAllocator_(IntPtr pMallocFunc, IntPtr pFreeFunc, IntPtr pObj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtom_SetDefaultConfig_(CriAtom.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeString criAtom_GetVersionString();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtom_SetUserMallocFunction(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtom_SetUserFreeFunction(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtom_IsInitialized();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtom_IsAudioOutputActive();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtom_ExecuteMain();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtom_ExecuteAudioProcess();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtom_SetAudioFrameStartCallback(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtom_SetAudioFrameEndCallback(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtom_SetDeviceUpdateCallback(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtom_Lock();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtom_Unlock();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtom_AttachPerformanceMonitor();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtom_ResetPerformanceMonitor();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtom_GetPerformanceInfo(CriAtom.PerformanceInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtom_DetachPerformanceMonitor();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtom_CalculateAdxBitrate(Int32 numChannels, Int32 samplingRate);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtom_CalculateHcaBitrate(Int32 numChannels, Int32 samplingRate, CriAtom.EncodeQuality quality);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtom_CalculateHcaMxBitrate(Int32 numChannels, Int32 samplingRate, CriAtom.EncodeQuality quality);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtom_GetStreamingInfo(CriAtom.StreamingInfo* streamingInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtom_SetFreeTimeBufferingFlagForDefaultDevice(NativeBool flag);
#else
			internal static void criAtom_SetUserAllocator_(IntPtr pMallocFunc, IntPtr pFreeFunc, IntPtr pObj){}
		internal static void criAtom_SetDefaultConfig_(CriAtom.Config* pConfig){}
		internal static NativeString criAtom_GetVersionString(){return default(NativeString);}
		internal static void criAtom_SetUserMallocFunction(IntPtr func, IntPtr obj){}
		internal static void criAtom_SetUserFreeFunction(IntPtr func, IntPtr obj){}
		internal static NativeBool criAtom_IsInitialized(){return default(NativeBool);}
		internal static NativeBool criAtom_IsAudioOutputActive(){return default(NativeBool);}
		internal static void criAtom_ExecuteMain(){}
		internal static void criAtom_ExecuteAudioProcess(){}
		internal static void criAtom_SetAudioFrameStartCallback(IntPtr func, IntPtr obj){}
		internal static void criAtom_SetAudioFrameEndCallback(IntPtr func, IntPtr obj){}
		internal static void criAtom_SetDeviceUpdateCallback(IntPtr func, IntPtr obj){}
		internal static void criAtom_Lock(){}
		internal static void criAtom_Unlock(){}
		internal static void criAtom_AttachPerformanceMonitor(){}
		internal static void criAtom_ResetPerformanceMonitor(){}
		internal static void criAtom_GetPerformanceInfo(CriAtom.PerformanceInfo* info){}
		internal static void criAtom_DetachPerformanceMonitor(){}
		internal static Int32 criAtom_CalculateAdxBitrate(Int32 numChannels, Int32 samplingRate){return default(Int32);}
		internal static Int32 criAtom_CalculateHcaBitrate(Int32 numChannels, Int32 samplingRate, CriAtom.EncodeQuality quality){return default(Int32);}
		internal static Int32 criAtom_CalculateHcaMxBitrate(Int32 numChannels, Int32 samplingRate, CriAtom.EncodeQuality quality){return default(Int32);}
		internal static NativeBool criAtom_GetStreamingInfo(CriAtom.StreamingInfo* streamingInfo){return default(NativeBool);}
		internal static NativeBool criAtom_SetFreeTimeBufferingFlagForDefaultDevice(NativeBool flag){return default(NativeBool);}
#endif
		}
	}
	public partial class CriAtomEx
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetDefaultConfigForUserPcmOutput_(CriAtomEx.ConfigForUserPcmOutput* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_AttachPerformanceMonitor_();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_DetachPerformanceMonitor_();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_ResetPerformanceMonitor_();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_GetPerformanceInfo_(CriAtom.PerformanceInfo* pInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetChannelMapping_(Int32 nch, UInt32 type);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomEx_CalculateAdxBitrate_(Int32 numChannels, Int32 samplingRate);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomEx_CalculateHcaBitrate_(Int32 numChannels, Int32 samplingRate, CriAtom.EncodeQuality quality);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomEx_CalculateHcaMxBitrate_(Int32 numChannels, Int32 samplingRate, CriAtom.EncodeQuality quality);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomEx_GetStreamingInfo_(CriAtom.StreamingInfo* streamingInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomEx_SetFreeTimeBufferingFlagForDefaultDevice_(NativeBool flag);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetUserAllocator_(IntPtr pMallocFunc, IntPtr pFreeFunc, IntPtr pObj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetDefaultConfig_(CriAtomEx.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomEx_CalculateWorkSize(CriAtomEx.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomEx_Initialize(CriAtomEx.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_Finalize();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomEx_IsInitialized();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_ExecuteMain();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_ExecuteAudioProcess();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_Lock();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_Unlock();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern UInt64 criAtomEx_GetTimeMicro();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_ResetTimer();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_PauseTimer(NativeBool sw);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetConfigForWorkSizeCalculation(CriAtomEx.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomEx_CalculateWorkSizeForRegisterAcfData(IntPtr acfData, Int32 acfDataSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomEx_RegisterAcfData(IntPtr acfData, Int32 acfDataSize, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomEx_CalculateWorkSizeForRegisterAcfFile(IntPtr binder, IntPtr path);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomEx_CalculateWorkSizeForRegisterAcfFileById(IntPtr binder, UInt16 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomEx_RegisterAcfFile(IntPtr binder, IntPtr path, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomEx_RegisterAcfFileById(IntPtr binder, UInt16 id, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_UnregisterAcf();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern UInt32 criAtomEx_GetAcfVersion(IntPtr acfData, Int32 acfDataSize, NativeBool* flag);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern UInt32 criAtomEx_GetAcfVersionFromFile(IntPtr binder, IntPtr path, IntPtr work, Int32 workSize, NativeBool* flag);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern UInt32 criAtomEx_GetAcfVersionFromFileById(IntPtr binder, UInt16 id, IntPtr work, Int32 workSize, NativeBool* flag);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_GetSupportedAcfVersion(UInt32* versionLow, UInt32* versionHigh);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomEx_AnalyzeAudioHeader(IntPtr buffer, Int32 bufferSize, CriAtomEx.FormatInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetRandomSeed(UInt32 seed);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomEx_IsDataPlaying(IntPtr buffer, Int32 size);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomEx_CalculateWorkSizeForDspBusSetting(IntPtr setting);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomEx_CalculateWorkSizeForDspBusSettingFromAcfData(IntPtr acfData, Int32 acfBufferSize, IntPtr settingName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_AttachDspBusSetting(IntPtr setting, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_DetachDspBusSetting();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_ApplyDspBusSnapshot(IntPtr snapshotName, Int32 timeMs);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeString criAtomEx_GetAppliedDspBusSnapshotName();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetCueLinkCallback(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetSpeakerAngles(Single angleL, Single angleR, Single angleSl, Single angleSr);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetSpeakerAngleArray(UInt32 speakerSystem, Single* angleArray);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetVirtualSpeakerAngleArray(UInt32 speakerSystem, Single* angleArray);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_ControlVirtualSpeakerSetting(NativeBool sw);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomEx_GetNumGameVariables();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomEx_GetGameVariableInfo(UInt16 index, CriAtomEx.GameVariableInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomEx_GetGameVariableById(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomEx_GetGameVariableByName(IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetGameVariableById(UInt32 id, Single value);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetGameVariableByName(IntPtr name, Single value);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetPlaybackCancelCallback(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_ControlAcfConsistencyCheck(NativeBool sw);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetAcfConsistencyCheckErrorLevel(CriErr.Level level);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetTrackTransitionBySelectorCallback(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_EnableCalculationAisacControlFrom3dPosition(NativeBool flag);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomEx_IsEnableCalculationAisacControlFrom3dPosition();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetVoiceEventCallback(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_EnumerateVoiceInfos(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetMonitoringVoiceStopCallback(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx_SetMonitoringVoiceStopPlaybackId(UInt32 playbackId);
#else
			internal static void criAtomEx_SetDefaultConfigForUserPcmOutput_(CriAtomEx.ConfigForUserPcmOutput* pConfig){}
		internal static void criAtomEx_AttachPerformanceMonitor_(){}
		internal static void criAtomEx_DetachPerformanceMonitor_(){}
		internal static void criAtomEx_ResetPerformanceMonitor_(){}
		internal static void criAtomEx_GetPerformanceInfo_(CriAtom.PerformanceInfo* pInfo){}
		internal static void criAtomEx_SetChannelMapping_(Int32 nch, UInt32 type){}
		internal static Int32 criAtomEx_CalculateAdxBitrate_(Int32 numChannels, Int32 samplingRate){return default(Int32);}
		internal static Int32 criAtomEx_CalculateHcaBitrate_(Int32 numChannels, Int32 samplingRate, CriAtom.EncodeQuality quality){return default(Int32);}
		internal static Int32 criAtomEx_CalculateHcaMxBitrate_(Int32 numChannels, Int32 samplingRate, CriAtom.EncodeQuality quality){return default(Int32);}
		internal static NativeBool criAtomEx_GetStreamingInfo_(CriAtom.StreamingInfo* streamingInfo){return default(NativeBool);}
		internal static NativeBool criAtomEx_SetFreeTimeBufferingFlagForDefaultDevice_(NativeBool flag){return default(NativeBool);}
		internal static void criAtomEx_SetUserAllocator_(IntPtr pMallocFunc, IntPtr pFreeFunc, IntPtr pObj){}
		internal static void criAtomEx_SetDefaultConfig_(CriAtomEx.Config* pConfig){}
		internal static Int32 criAtomEx_CalculateWorkSize(CriAtomEx.Config* config){return default(Int32);}
		internal static NativeBool criAtomEx_Initialize(CriAtomEx.Config* config, IntPtr work, Int32 workSize){return default(NativeBool);}
		internal static void criAtomEx_Finalize(){}
		internal static NativeBool criAtomEx_IsInitialized(){return default(NativeBool);}
		internal static void criAtomEx_ExecuteMain(){}
		internal static void criAtomEx_ExecuteAudioProcess(){}
		internal static void criAtomEx_Lock(){}
		internal static void criAtomEx_Unlock(){}
		internal static UInt64 criAtomEx_GetTimeMicro(){return default(UInt64);}
		internal static void criAtomEx_ResetTimer(){}
		internal static void criAtomEx_PauseTimer(NativeBool sw){}
		internal static void criAtomEx_SetConfigForWorkSizeCalculation(CriAtomEx.Config* config){}
		internal static Int32 criAtomEx_CalculateWorkSizeForRegisterAcfData(IntPtr acfData, Int32 acfDataSize){return default(Int32);}
		internal static NativeBool criAtomEx_RegisterAcfData(IntPtr acfData, Int32 acfDataSize, IntPtr work, Int32 workSize){return default(NativeBool);}
		internal static Int32 criAtomEx_CalculateWorkSizeForRegisterAcfFile(IntPtr binder, IntPtr path){return default(Int32);}
		internal static Int32 criAtomEx_CalculateWorkSizeForRegisterAcfFileById(IntPtr binder, UInt16 id){return default(Int32);}
		internal static NativeBool criAtomEx_RegisterAcfFile(IntPtr binder, IntPtr path, IntPtr work, Int32 workSize){return default(NativeBool);}
		internal static NativeBool criAtomEx_RegisterAcfFileById(IntPtr binder, UInt16 id, IntPtr work, Int32 workSize){return default(NativeBool);}
		internal static void criAtomEx_UnregisterAcf(){}
		internal static UInt32 criAtomEx_GetAcfVersion(IntPtr acfData, Int32 acfDataSize, NativeBool* flag){return default(UInt32);}
		internal static UInt32 criAtomEx_GetAcfVersionFromFile(IntPtr binder, IntPtr path, IntPtr work, Int32 workSize, NativeBool* flag){return default(UInt32);}
		internal static UInt32 criAtomEx_GetAcfVersionFromFileById(IntPtr binder, UInt16 id, IntPtr work, Int32 workSize, NativeBool* flag){return default(UInt32);}
		internal static void criAtomEx_GetSupportedAcfVersion(UInt32* versionLow, UInt32* versionHigh){}
		internal static NativeBool criAtomEx_AnalyzeAudioHeader(IntPtr buffer, Int32 bufferSize, CriAtomEx.FormatInfo* info){return default(NativeBool);}
		internal static void criAtomEx_SetRandomSeed(UInt32 seed){}
		internal static NativeBool criAtomEx_IsDataPlaying(IntPtr buffer, Int32 size){return default(NativeBool);}
		internal static Int32 criAtomEx_CalculateWorkSizeForDspBusSetting(IntPtr setting){return default(Int32);}
		internal static Int32 criAtomEx_CalculateWorkSizeForDspBusSettingFromAcfData(IntPtr acfData, Int32 acfBufferSize, IntPtr settingName){return default(Int32);}
		internal static void criAtomEx_AttachDspBusSetting(IntPtr setting, IntPtr work, Int32 workSize){}
		internal static void criAtomEx_DetachDspBusSetting(){}
		internal static void criAtomEx_ApplyDspBusSnapshot(IntPtr snapshotName, Int32 timeMs){}
		internal static NativeString criAtomEx_GetAppliedDspBusSnapshotName(){return default(NativeString);}
		internal static void criAtomEx_SetCueLinkCallback(IntPtr func, IntPtr obj){}
		internal static void criAtomEx_SetSpeakerAngles(Single angleL, Single angleR, Single angleSl, Single angleSr){}
		internal static void criAtomEx_SetSpeakerAngleArray(UInt32 speakerSystem, Single* angleArray){}
		internal static void criAtomEx_SetVirtualSpeakerAngleArray(UInt32 speakerSystem, Single* angleArray){}
		internal static void criAtomEx_ControlVirtualSpeakerSetting(NativeBool sw){}
		internal static Int32 criAtomEx_GetNumGameVariables(){return default(Int32);}
		internal static NativeBool criAtomEx_GetGameVariableInfo(UInt16 index, CriAtomEx.GameVariableInfo* info){return default(NativeBool);}
		internal static Single criAtomEx_GetGameVariableById(UInt32 id){return default(Single);}
		internal static Single criAtomEx_GetGameVariableByName(IntPtr name){return default(Single);}
		internal static void criAtomEx_SetGameVariableById(UInt32 id, Single value){}
		internal static void criAtomEx_SetGameVariableByName(IntPtr name, Single value){}
		internal static void criAtomEx_SetPlaybackCancelCallback(IntPtr func, IntPtr obj){}
		internal static void criAtomEx_ControlAcfConsistencyCheck(NativeBool sw){}
		internal static void criAtomEx_SetAcfConsistencyCheckErrorLevel(CriErr.Level level){}
		internal static void criAtomEx_SetTrackTransitionBySelectorCallback(IntPtr func, IntPtr obj){}
		internal static void criAtomEx_EnableCalculationAisacControlFrom3dPosition(NativeBool flag){}
		internal static NativeBool criAtomEx_IsEnableCalculationAisacControlFrom3dPosition(){return default(NativeBool);}
		internal static void criAtomEx_SetVoiceEventCallback(IntPtr func, IntPtr obj){}
		internal static void criAtomEx_EnumerateVoiceInfos(IntPtr func, IntPtr obj){}
		internal static void criAtomEx_SetMonitoringVoiceStopCallback(IntPtr func, IntPtr obj){}
		internal static void criAtomEx_SetMonitoringVoiceStopPlaybackId(UInt32 playbackId){}
#endif
		}
	}
	public partial class CriAtomAwb
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomAwb_CalculateWorkSizeForLoadToc(Int32 num);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomAwb_LoadToc(IntPtr binder, IntPtr path, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomAwb_LoadTocById(IntPtr binder, UInt16 id, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomAwb_LoadTocAsync(IntPtr binder, IntPtr path, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomAwb_LoadTocAsyncById(IntPtr binder, UInt16 id, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomAwb_LoadFromMemory(IntPtr awbMem, Int32 awbMemSize, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern CriAtomAwb.Type criAtomAwb_GetType(IntPtr awb);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomAwb_GetWaveFileInfo(IntPtr awb, Int32 id, Int64* offset, UInt32* size);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomAwb_GetWaveDataInfo(IntPtr awb, Int32 id, IntPtr* waveDataStart, UInt32* size);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern UInt16 criAtomAwb_GetNumContents(IntPtr awb);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomAwb_Release(IntPtr awb);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomAwb_IsReadyToRelease(IntPtr awb);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern CriAtomAwb.Status criAtomAwb_GetStatus(IntPtr awb);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomAwb_GetIdByIndex(IntPtr awb, UInt16 index);
#else
			internal static Int32 criAtomAwb_CalculateWorkSizeForLoadToc(Int32 num){return default(Int32);}
		internal static IntPtr criAtomAwb_LoadToc(IntPtr binder, IntPtr path, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static IntPtr criAtomAwb_LoadTocById(IntPtr binder, UInt16 id, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static IntPtr criAtomAwb_LoadTocAsync(IntPtr binder, IntPtr path, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static IntPtr criAtomAwb_LoadTocAsyncById(IntPtr binder, UInt16 id, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static IntPtr criAtomAwb_LoadFromMemory(IntPtr awbMem, Int32 awbMemSize, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static CriAtomAwb.Type criAtomAwb_GetType(IntPtr awb){return default(CriAtomAwb.Type);}
		internal static NativeBool criAtomAwb_GetWaveFileInfo(IntPtr awb, Int32 id, Int64* offset, UInt32* size){return default(NativeBool);}
		internal static void criAtomAwb_GetWaveDataInfo(IntPtr awb, Int32 id, IntPtr* waveDataStart, UInt32* size){}
		internal static UInt16 criAtomAwb_GetNumContents(IntPtr awb){return default(UInt16);}
		internal static void criAtomAwb_Release(IntPtr awb){}
		internal static NativeBool criAtomAwb_IsReadyToRelease(IntPtr awb){return default(NativeBool);}
		internal static CriAtomAwb.Status criAtomAwb_GetStatus(IntPtr awb){return default(CriAtomAwb.Status);}
		internal static Int32 criAtomAwb_GetIdByIndex(IntPtr awb, UInt16 index){return default(Int32);}
#endif
		}
	}
	public partial class CriAtomDsp
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomDsp_ConvertParameterFromCent(Single cent);
#else
			internal static Single criAtomDsp_ConvertParameterFromCent(Single cent){return default(Single);}
#endif
		}
	}
	public partial class CriAtomAsr
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomAsr_SetDefaultConfig_(CriAtomAsr.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomAsr_CalculateWorkSize(CriAtomAsr.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomAsr_Initialize(CriAtomAsr.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomAsr_Finalize();
#else
			internal static void criAtomAsr_SetDefaultConfig_(CriAtomAsr.Config* pConfig){}
		internal static Int32 criAtomAsr_CalculateWorkSize(CriAtomAsr.Config* config){return default(Int32);}
		internal static void criAtomAsr_Initialize(CriAtomAsr.Config* config, IntPtr work, Int32 workSize){}
		internal static void criAtomAsr_Finalize(){}
#endif
		}
	}
	public partial class CriAtomDbas
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomDbas_GetStreamingPlayerHandles(Int32 dbasId, IntPtr* players, Int32 length);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomDbas_SetDefaultConfig_(CriAtomDbas.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomDbas_CalculateWorkSize(CriAtomDbas.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomDbas_Create(CriAtomDbas.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomDbas_Destroy(Int32 atomDbasId);
#else
			internal static Int32 criAtomDbas_GetStreamingPlayerHandles(Int32 dbasId, IntPtr* players, Int32 length){return default(Int32);}
		internal static void criAtomDbas_SetDefaultConfig_(CriAtomDbas.Config* pConfig){}
		internal static Int32 criAtomDbas_CalculateWorkSize(CriAtomDbas.Config* config){return default(Int32);}
		internal static Int32 criAtomDbas_Create(CriAtomDbas.Config* config, IntPtr work, Int32 workSize){return default(Int32);}
		internal static void criAtomDbas_Destroy(Int32 atomDbasId){}
#endif
		}
	}
	public partial class CriAtomHcaMx
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomHcaMx_SetDefaultConfig_(CriAtomHcaMx.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomHcaMx_CalculateWorkSize(CriAtomHcaMx.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomHcaMx_SetConfigForWorkSizeCalculation(CriAtomHcaMx.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomHcaMx_Initialize(CriAtomHcaMx.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomHcaMx_Finalize();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomHcaMx_SetBusSendLevelByName(Int32 mixerId, IntPtr busName, Single level);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomHcaMx_SetFrequencyRatio(Int32 mixerId, Single ratio);
#else
			internal static void criAtomHcaMx_SetDefaultConfig_(CriAtomHcaMx.Config* pConfig){}
		internal static Int32 criAtomHcaMx_CalculateWorkSize(CriAtomHcaMx.Config* config){return default(Int32);}
		internal static void criAtomHcaMx_SetConfigForWorkSizeCalculation(CriAtomHcaMx.Config* config){}
		internal static void criAtomHcaMx_Initialize(CriAtomHcaMx.Config* config, IntPtr work, Int32 workSize){}
		internal static void criAtomHcaMx_Finalize(){}
		internal static void criAtomHcaMx_SetBusSendLevelByName(Int32 mixerId, IntPtr busName, Single level){}
		internal static void criAtomHcaMx_SetFrequencyRatio(Int32 mixerId, Single ratio){}
#endif
		}
	}
	public partial class CriAtomMeter
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomMeter_SetDefaultConfigForLevelMeter_(CriAtom.LevelMeterConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomMeter_SetDefaultConfigForLoudnessMeter_(CriAtom.LoudnessMeterConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomMeter_SetDefaultConfigForTruePeakMeter_(CriAtom.TruePeakMeterConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomMeter_CalculateWorkSizeForLevelMeter(CriAtom.LevelMeterConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomMeter_AttachLevelMeter(CriAtom.LevelMeterConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomMeter_DetachLevelMeter();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomMeter_GetLevelInfo(CriAtom.LevelInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomMeter_CalculateWorkSizeForLoudnessMeter(CriAtom.LoudnessMeterConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomMeter_AttachLoudnessMeter(CriAtom.LoudnessMeterConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomMeter_DetachLoudnessMeter();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomMeter_GetLoudnessInfo(CriAtom.LoudnessInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomMeter_ResetLoudnessMeter();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomMeter_CalculateWorkSizeForTruePeakMeter(CriAtom.TruePeakMeterConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomMeter_AttachTruePeakMeter(CriAtom.TruePeakMeterConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomMeter_DetachTruePeakMeter();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomMeter_GetTruePeakInfo(CriAtom.TruePeakInfo* info);
#else
			internal static void criAtomMeter_SetDefaultConfigForLevelMeter_(CriAtom.LevelMeterConfig* pConfig){}
		internal static void criAtomMeter_SetDefaultConfigForLoudnessMeter_(CriAtom.LoudnessMeterConfig* pConfig){}
		internal static void criAtomMeter_SetDefaultConfigForTruePeakMeter_(CriAtom.TruePeakMeterConfig* pConfig){}
		internal static Int32 criAtomMeter_CalculateWorkSizeForLevelMeter(CriAtom.LevelMeterConfig* config){return default(Int32);}
		internal static void criAtomMeter_AttachLevelMeter(CriAtom.LevelMeterConfig* config, IntPtr work, Int32 workSize){}
		internal static void criAtomMeter_DetachLevelMeter(){}
		internal static void criAtomMeter_GetLevelInfo(CriAtom.LevelInfo* info){}
		internal static Int32 criAtomMeter_CalculateWorkSizeForLoudnessMeter(CriAtom.LoudnessMeterConfig* config){return default(Int32);}
		internal static void criAtomMeter_AttachLoudnessMeter(CriAtom.LoudnessMeterConfig* config, IntPtr work, Int32 workSize){}
		internal static void criAtomMeter_DetachLoudnessMeter(){}
		internal static void criAtomMeter_GetLoudnessInfo(CriAtom.LoudnessInfo* info){}
		internal static void criAtomMeter_ResetLoudnessMeter(){}
		internal static Int32 criAtomMeter_CalculateWorkSizeForTruePeakMeter(CriAtom.TruePeakMeterConfig* config){return default(Int32);}
		internal static void criAtomMeter_AttachTruePeakMeter(CriAtom.TruePeakMeterConfig* config, IntPtr work, Int32 workSize){}
		internal static void criAtomMeter_DetachTruePeakMeter(){}
		internal static void criAtomMeter_GetTruePeakInfo(CriAtom.TruePeakInfo* info){}
#endif
		}
	}
	public partial class CriAtomExAsr
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_SetDefaultConfigForBusAnalyzer_(CriAtomExAsr.BusAnalyzerConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_SetBusFilterCallbackByName(IntPtr busName, IntPtr preFunc, IntPtr postFunc, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAsr_RegisterSoundxRInterface(IntPtr soundxrInterface);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_SetDefaultConfig_(CriAtomExAsr.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAsr_CalculateWorkSize(CriAtomExAsr.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_SetConfigForWorkSizeCalculation(CriAtomExAsr.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_Initialize(CriAtomExAsr.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_Finalize();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_SetBusVolumeByName(IntPtr busName, Single volume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_GetBusVolumeByName(IntPtr busName, Single* volume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_SetBusPanInfoByName(IntPtr busName, CriAtomExAsr.BusPanInfo* panInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_GetBusPanInfoByName(IntPtr busName, CriAtomExAsr.BusPanInfo* panInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_SetBusMatrixByName(IntPtr busName, Int32 inputChannels, Int32 outputChannels, Single* matrix);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_SetBusSendLevelByName(IntPtr busName, IntPtr sendtoBusName, Single level);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_SetEffectParameter(IntPtr busName, IntPtr effectName, UInt32 parameterIndex, Single parameterValue);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_UpdateEffectParameters(IntPtr busName, IntPtr effectName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomExAsr_GetEffectParameter(IntPtr busName, IntPtr effectName, UInt32 parameterIndex);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_SetEffectBypass(IntPtr busName, IntPtr effectName, NativeBool bypass);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_AttachBusAnalyzerByName(IntPtr busName, CriAtomExAsr.BusAnalyzerConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_DetachBusAnalyzerByName(IntPtr busName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_GetBusAnalyzerInfoByName(IntPtr busName, CriAtomExAsr.BusAnalyzerInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAsr_GetNumBuses();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAsr_RegisterEffectInterface(IntPtr afxInterface);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_UnregisterEffectInterface(IntPtr afxInterface);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_ResetIrReverbPerformanceInfo();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_GetIrReverbPerformanceInfo(CriAtomExAsr.IrReverbPerformanceInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAsr_GetPcmBufferSize();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsr_EnableBinauralizer(NativeBool enabled);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAsr_IsEnabledBinauralizer();
#else
			internal static void criAtomExAsr_SetDefaultConfigForBusAnalyzer_(CriAtomExAsr.BusAnalyzerConfig* pConfig){}
		internal static void criAtomExAsr_SetBusFilterCallbackByName(IntPtr busName, IntPtr preFunc, IntPtr postFunc, IntPtr obj){}
		internal static NativeBool criAtomExAsr_RegisterSoundxRInterface(IntPtr soundxrInterface){return default(NativeBool);}
		internal static void criAtomExAsr_SetDefaultConfig_(CriAtomExAsr.Config* pConfig){}
		internal static Int32 criAtomExAsr_CalculateWorkSize(CriAtomExAsr.Config* config){return default(Int32);}
		internal static void criAtomExAsr_SetConfigForWorkSizeCalculation(CriAtomExAsr.Config* config){}
		internal static void criAtomExAsr_Initialize(CriAtomExAsr.Config* config, IntPtr work, Int32 workSize){}
		internal static void criAtomExAsr_Finalize(){}
		internal static void criAtomExAsr_SetBusVolumeByName(IntPtr busName, Single volume){}
		internal static void criAtomExAsr_GetBusVolumeByName(IntPtr busName, Single* volume){}
		internal static void criAtomExAsr_SetBusPanInfoByName(IntPtr busName, CriAtomExAsr.BusPanInfo* panInfo){}
		internal static void criAtomExAsr_GetBusPanInfoByName(IntPtr busName, CriAtomExAsr.BusPanInfo* panInfo){}
		internal static void criAtomExAsr_SetBusMatrixByName(IntPtr busName, Int32 inputChannels, Int32 outputChannels, Single* matrix){}
		internal static void criAtomExAsr_SetBusSendLevelByName(IntPtr busName, IntPtr sendtoBusName, Single level){}
		internal static void criAtomExAsr_SetEffectParameter(IntPtr busName, IntPtr effectName, UInt32 parameterIndex, Single parameterValue){}
		internal static void criAtomExAsr_UpdateEffectParameters(IntPtr busName, IntPtr effectName){}
		internal static Single criAtomExAsr_GetEffectParameter(IntPtr busName, IntPtr effectName, UInt32 parameterIndex){return default(Single);}
		internal static void criAtomExAsr_SetEffectBypass(IntPtr busName, IntPtr effectName, NativeBool bypass){}
		internal static void criAtomExAsr_AttachBusAnalyzerByName(IntPtr busName, CriAtomExAsr.BusAnalyzerConfig* config){}
		internal static void criAtomExAsr_DetachBusAnalyzerByName(IntPtr busName){}
		internal static void criAtomExAsr_GetBusAnalyzerInfoByName(IntPtr busName, CriAtomExAsr.BusAnalyzerInfo* info){}
		internal static Int32 criAtomExAsr_GetNumBuses(){return default(Int32);}
		internal static NativeBool criAtomExAsr_RegisterEffectInterface(IntPtr afxInterface){return default(NativeBool);}
		internal static void criAtomExAsr_UnregisterEffectInterface(IntPtr afxInterface){}
		internal static void criAtomExAsr_ResetIrReverbPerformanceInfo(){}
		internal static void criAtomExAsr_GetIrReverbPerformanceInfo(CriAtomExAsr.IrReverbPerformanceInfo* info){}
		internal static Int32 criAtomExAsr_GetPcmBufferSize(){return default(Int32);}
		internal static void criAtomExAsr_EnableBinauralizer(NativeBool enabled){}
		internal static NativeBool criAtomExAsr_IsEnabledBinauralizer(){return default(NativeBool);}
#endif
		}
	}
	public partial class CriAtomExAcf
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExAcf_GetOutputPortHnByName(IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcf_GetNumAisacControls();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetAisacControlInfo(UInt16 index, CriAtomEx.AisacControlInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern UInt32 criAtomExAcf_GetAisacControlIdByName(IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeString criAtomExAcf_GetAisacControlNameById(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcf_GetNumDspSettings();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcf_GetNumDspSettingsFromAcfData(IntPtr acfData, Int32 acfDataSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeString criAtomExAcf_GetDspSettingNameByIndex(UInt16 index);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeString criAtomExAcf_GetDspSettingNameByIndexFromAcfData(IntPtr acfData, Int32 acfDataSize, UInt16 index);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetDspSettingInformation(IntPtr name, CriAtomExAcf.DspSettingInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetDspSettingSnapshotInformation(UInt16 index, CriAtomExAcf.DspSettingSnapshotInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetDspBusInformation(UInt16 index, CriAtomExAcf.DspBusInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeString criAtomExAcf_GetDspFxName(UInt16 index);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetDspFxParameters(UInt16 index, IntPtr parameters, Int32 size);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetDspBusLinkInformation(UInt16 index, CriAtomExAcf.DspBusLinkInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcf_GetNumCategoriesFromAcfData(IntPtr acfData, Int32 acfDataSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcf_GetNumCategories();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcf_GetNumCategoriesPerPlaybackFromAcfData(IntPtr acfData, Int32 acfDataSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcf_GetNumCategoriesPerPlayback();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetCategoryInfo(UInt16 index, CriAtomExCategory.Info* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetCategoryInfoByName(IntPtr name, CriAtomExCategory.Info* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetCategoryInfoById(UInt32 id, CriAtomExCategory.Info* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcf_GetNumGlobalAisacs();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetGlobalAisacInfo(UInt16 index, CriAtomEx.GlobalAisacInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetGlobalAisacInfoByName(IntPtr name, CriAtomEx.GlobalAisacInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetGlobalAisacGraphInfo(CriAtomEx.GlobalAisacInfo* aisacInfo, UInt16 graphIndex, CriAtomEx.AisacGraphInfo* graphInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetGlobalAisacValue(CriAtomEx.GlobalAisacInfo* aisacInfo, Single control, CriAtomEx.AisacGraphType type, Single* value);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetAcfInfo(CriAtomExAcf.Info* acfInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetAcfInfoFromAcfData(IntPtr acfData, Int32 acfDataSize, CriAtomExAcf.Info* acfInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcf_GetNumSelectors();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetSelectorInfoByIndex(UInt16 index, CriAtomEx.SelectorInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetSelectorInfoByName(IntPtr name, CriAtomEx.SelectorInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcf_GetSelectorLabelInfo(CriAtomEx.SelectorInfo* selectorInfo, UInt16 labelIndex, CriAtomEx.SelectorLabelInfo* labelInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAcf_SetGlobalLabelToSelectorByName(IntPtr selsectorName, IntPtr labelName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAcf_SetGlobalLabelToSelectorByIndex(UInt16 selsectorIndex, UInt16 labelIndex);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcf_GetNumBusesFromAcfData(IntPtr acfData, Int32 acfDataSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcf_GetNumBuses();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcf_GetMaxBusesOfDspBusSettingsFromAcfData(IntPtr acfData, Int32 acfDataSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcf_GetMaxBusesOfDspBusSettings();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeString criAtomExAcf_FindBusName(IntPtr busName);
#else
			internal static IntPtr criAtomExAcf_GetOutputPortHnByName(IntPtr name){return default(IntPtr);}
		internal static Int32 criAtomExAcf_GetNumAisacControls(){return default(Int32);}
		internal static NativeBool criAtomExAcf_GetAisacControlInfo(UInt16 index, CriAtomEx.AisacControlInfo* info){return default(NativeBool);}
		internal static UInt32 criAtomExAcf_GetAisacControlIdByName(IntPtr name){return default(UInt32);}
		internal static NativeString criAtomExAcf_GetAisacControlNameById(UInt32 id){return default(NativeString);}
		internal static Int32 criAtomExAcf_GetNumDspSettings(){return default(Int32);}
		internal static Int32 criAtomExAcf_GetNumDspSettingsFromAcfData(IntPtr acfData, Int32 acfDataSize){return default(Int32);}
		internal static NativeString criAtomExAcf_GetDspSettingNameByIndex(UInt16 index){return default(NativeString);}
		internal static NativeString criAtomExAcf_GetDspSettingNameByIndexFromAcfData(IntPtr acfData, Int32 acfDataSize, UInt16 index){return default(NativeString);}
		internal static NativeBool criAtomExAcf_GetDspSettingInformation(IntPtr name, CriAtomExAcf.DspSettingInfo* info){return default(NativeBool);}
		internal static NativeBool criAtomExAcf_GetDspSettingSnapshotInformation(UInt16 index, CriAtomExAcf.DspSettingSnapshotInfo* info){return default(NativeBool);}
		internal static NativeBool criAtomExAcf_GetDspBusInformation(UInt16 index, CriAtomExAcf.DspBusInfo* info){return default(NativeBool);}
		internal static NativeString criAtomExAcf_GetDspFxName(UInt16 index){return default(NativeString);}
		internal static NativeBool criAtomExAcf_GetDspFxParameters(UInt16 index, IntPtr parameters, Int32 size){return default(NativeBool);}
		internal static NativeBool criAtomExAcf_GetDspBusLinkInformation(UInt16 index, CriAtomExAcf.DspBusLinkInfo* info){return default(NativeBool);}
		internal static Int32 criAtomExAcf_GetNumCategoriesFromAcfData(IntPtr acfData, Int32 acfDataSize){return default(Int32);}
		internal static Int32 criAtomExAcf_GetNumCategories(){return default(Int32);}
		internal static Int32 criAtomExAcf_GetNumCategoriesPerPlaybackFromAcfData(IntPtr acfData, Int32 acfDataSize){return default(Int32);}
		internal static Int32 criAtomExAcf_GetNumCategoriesPerPlayback(){return default(Int32);}
		internal static NativeBool criAtomExAcf_GetCategoryInfo(UInt16 index, CriAtomExCategory.Info* info){return default(NativeBool);}
		internal static NativeBool criAtomExAcf_GetCategoryInfoByName(IntPtr name, CriAtomExCategory.Info* info){return default(NativeBool);}
		internal static NativeBool criAtomExAcf_GetCategoryInfoById(UInt32 id, CriAtomExCategory.Info* info){return default(NativeBool);}
		internal static Int32 criAtomExAcf_GetNumGlobalAisacs(){return default(Int32);}
		internal static NativeBool criAtomExAcf_GetGlobalAisacInfo(UInt16 index, CriAtomEx.GlobalAisacInfo* info){return default(NativeBool);}
		internal static NativeBool criAtomExAcf_GetGlobalAisacInfoByName(IntPtr name, CriAtomEx.GlobalAisacInfo* info){return default(NativeBool);}
		internal static NativeBool criAtomExAcf_GetGlobalAisacGraphInfo(CriAtomEx.GlobalAisacInfo* aisacInfo, UInt16 graphIndex, CriAtomEx.AisacGraphInfo* graphInfo){return default(NativeBool);}
		internal static NativeBool criAtomExAcf_GetGlobalAisacValue(CriAtomEx.GlobalAisacInfo* aisacInfo, Single control, CriAtomEx.AisacGraphType type, Single* value){return default(NativeBool);}
		internal static NativeBool criAtomExAcf_GetAcfInfo(CriAtomExAcf.Info* acfInfo){return default(NativeBool);}
		internal static NativeBool criAtomExAcf_GetAcfInfoFromAcfData(IntPtr acfData, Int32 acfDataSize, CriAtomExAcf.Info* acfInfo){return default(NativeBool);}
		internal static Int32 criAtomExAcf_GetNumSelectors(){return default(Int32);}
		internal static NativeBool criAtomExAcf_GetSelectorInfoByIndex(UInt16 index, CriAtomEx.SelectorInfo* info){return default(NativeBool);}
		internal static NativeBool criAtomExAcf_GetSelectorInfoByName(IntPtr name, CriAtomEx.SelectorInfo* info){return default(NativeBool);}
		internal static NativeBool criAtomExAcf_GetSelectorLabelInfo(CriAtomEx.SelectorInfo* selectorInfo, UInt16 labelIndex, CriAtomEx.SelectorLabelInfo* labelInfo){return default(NativeBool);}
		internal static void criAtomExAcf_SetGlobalLabelToSelectorByName(IntPtr selsectorName, IntPtr labelName){}
		internal static void criAtomExAcf_SetGlobalLabelToSelectorByIndex(UInt16 selsectorIndex, UInt16 labelIndex){}
		internal static Int32 criAtomExAcf_GetNumBusesFromAcfData(IntPtr acfData, Int32 acfDataSize){return default(Int32);}
		internal static Int32 criAtomExAcf_GetNumBuses(){return default(Int32);}
		internal static Int32 criAtomExAcf_GetMaxBusesOfDspBusSettingsFromAcfData(IntPtr acfData, Int32 acfDataSize){return default(Int32);}
		internal static Int32 criAtomExAcf_GetMaxBusesOfDspBusSettings(){return default(Int32);}
		internal static NativeString criAtomExAcf_FindBusName(IntPtr busName){return default(NativeString);}
#endif
		}
	}
	public partial class CriAtomExAcb
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_CalculateWorkSizeForLoadAcbData(IntPtr acbData, Int32 acbDataSize, IntPtr awbBinder, IntPtr awbPath);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_CalculateWorkSizeForLoadAcbDataById(IntPtr acbData, Int32 acbDataSize, IntPtr awbBinder, UInt16 awbId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExAcb_LoadAcbData(IntPtr acbData, Int32 acbDataSize, IntPtr awbBinder, IntPtr awbPath, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExAcb_LoadAcbDataById(IntPtr acbData, Int32 acbDataSize, IntPtr awbBinder, UInt16 awbId, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_CalculateWorkSizeForLoadAcbFile(IntPtr acbBinder, IntPtr acbPath, IntPtr awbBinder, IntPtr awbPath);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_CalculateWorkSizeForLoadAcbFileById(IntPtr acbBinder, UInt16 acbId, IntPtr awbBinder, UInt16 awbId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExAcb_LoadAcbFile(IntPtr acbBinder, IntPtr acbPath, IntPtr awbBinder, IntPtr awbPath, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExAcb_LoadAcbFileById(IntPtr acbBinder, UInt16 acbId, IntPtr awbBinder, UInt16 awbId, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAcb_Release(IntPtr acbHn);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcb_IsReadyToRelease(IntPtr acbHn);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAcb_ReleaseAll();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_EnumerateHandles(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern UInt32 criAtomExAcb_GetVersion(IntPtr acbData, Int32 acbDataSize, IntPtr flag);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern UInt32 criAtomExAcb_GetVersionFromFile(IntPtr acbBinder, IntPtr acbPath, IntPtr work, Int32 workSize, NativeBool* flag);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAcb_GetSupportedVersion(UInt32* versionLow, UInt32* versionHigh);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetNumCues(IntPtr acbHn);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcb_ExistsId(IntPtr acbHn, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcb_ExistsName(IntPtr acbHn, IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcb_ExistsIndex(IntPtr acbHn, Int32 index);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetCueIdByIndex(IntPtr acbHn, Int32 index);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetCueIdByName(IntPtr acbHn, IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeString criAtomExAcb_GetCueNameByIndex(IntPtr acbHn, Int32 index);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeString criAtomExAcb_GetCueNameById(IntPtr acbHn, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetCueIndexById(IntPtr acbHn, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetCueIndexByName(IntPtr acbHn, IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeString criAtomExAcb_GetUserDataById(IntPtr acbHn, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeString criAtomExAcb_GetUserDataByName(IntPtr acbHn, IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int64 criAtomExAcb_GetLengthById(IntPtr acbHn, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int64 criAtomExAcb_GetLengthByName(IntPtr acbHn, IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetNumUsableAisacControlsById(IntPtr acbHn, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetNumUsableAisacControlsByName(IntPtr acbHn, IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcb_GetUsableAisacControlById(IntPtr acbHn, Int32 id, UInt16 index, CriAtomEx.AisacControlInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcb_GetUsableAisacControlByName(IntPtr acbHn, IntPtr name, UInt16 index, CriAtomEx.AisacControlInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcb_IsUsingAisacControlById(IntPtr acbHn, Int32 id, UInt32 aisacControlId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcb_IsUsingAisacControlByName(IntPtr acbHn, IntPtr name, IntPtr aisacControlName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetCuePriorityById(IntPtr acbHn, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetCuePriorityByName(IntPtr acbHn, IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcb_GetWaveformInfoById(IntPtr acbHn, Int32 id, CriAtomEx.WaveformInfo* waveformInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcb_GetWaveformInfoByName(IntPtr acbHn, IntPtr name, CriAtomEx.WaveformInfo* waveformInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExAcb_GetOnMemoryAwbHandle(IntPtr acbHn);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExAcb_GetStreamingAwbHandle(IntPtr acbHn);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExAcb_GetStreamingAwbHandleBySlotName(IntPtr acbHn, IntPtr awbSlotName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExAcb_GetStreamingAwbHandleBySlotIndex(IntPtr acbHn, UInt16 awbSlotIndex);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcb_GetCueInfoByName(IntPtr acbHn, IntPtr name, CriAtomEx.CueInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcb_GetCueInfoById(IntPtr acbHn, Int32 id, CriAtomEx.CueInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcb_GetCueInfoByIndex(IntPtr acbHn, Int32 index, CriAtomEx.CueInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetNumCuePlayingCountByName(IntPtr acbHn, IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetNumCuePlayingCountById(IntPtr acbHn, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetNumCuePlayingCountByIndex(IntPtr acbHn, Int32 index);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetBlockIndexByIndex(IntPtr acbHn, Int32 index, IntPtr blockName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetBlockIndexById(IntPtr acbHn, Int32 id, IntPtr blockName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetBlockIndexByName(IntPtr acbHn, IntPtr name, IntPtr blockName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAcb_SetDetectionInGamePreviewDataCallback(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcb_GetAcbInfo(IntPtr acbHn, CriAtomExAcb.Info* acbInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAcb_ResetCueTypeStateByName(IntPtr acbHn, IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAcb_ResetCueTypeStateById(IntPtr acbHn, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAcb_ResetCueTypeStateByIndex(IntPtr acbHn, Int32 index);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAcb_AttachAwbFile(IntPtr acbHn, IntPtr awbBinder, IntPtr awbPath, IntPtr awbName, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAcb_DetachAwbFile(IntPtr acbHn, IntPtr awbName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_CalculateWorkSizeForAttachAwbFile(IntPtr awbBinder, IntPtr awbPath);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAcb_GetNumAwbFileSlots(IntPtr acbHn);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeString criAtomExAcb_GetAwbFileSlotName(IntPtr acbHn, UInt16 index);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAcb_IsAttachedAwbFile(IntPtr acbHn, IntPtr awbName);
#else
			internal static Int32 criAtomExAcb_CalculateWorkSizeForLoadAcbData(IntPtr acbData, Int32 acbDataSize, IntPtr awbBinder, IntPtr awbPath){return default(Int32);}
		internal static Int32 criAtomExAcb_CalculateWorkSizeForLoadAcbDataById(IntPtr acbData, Int32 acbDataSize, IntPtr awbBinder, UInt16 awbId){return default(Int32);}
		internal static IntPtr criAtomExAcb_LoadAcbData(IntPtr acbData, Int32 acbDataSize, IntPtr awbBinder, IntPtr awbPath, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static IntPtr criAtomExAcb_LoadAcbDataById(IntPtr acbData, Int32 acbDataSize, IntPtr awbBinder, UInt16 awbId, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static Int32 criAtomExAcb_CalculateWorkSizeForLoadAcbFile(IntPtr acbBinder, IntPtr acbPath, IntPtr awbBinder, IntPtr awbPath){return default(Int32);}
		internal static Int32 criAtomExAcb_CalculateWorkSizeForLoadAcbFileById(IntPtr acbBinder, UInt16 acbId, IntPtr awbBinder, UInt16 awbId){return default(Int32);}
		internal static IntPtr criAtomExAcb_LoadAcbFile(IntPtr acbBinder, IntPtr acbPath, IntPtr awbBinder, IntPtr awbPath, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static IntPtr criAtomExAcb_LoadAcbFileById(IntPtr acbBinder, UInt16 acbId, IntPtr awbBinder, UInt16 awbId, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static void criAtomExAcb_Release(IntPtr acbHn){}
		internal static NativeBool criAtomExAcb_IsReadyToRelease(IntPtr acbHn){return default(NativeBool);}
		internal static void criAtomExAcb_ReleaseAll(){}
		internal static Int32 criAtomExAcb_EnumerateHandles(IntPtr func, IntPtr obj){return default(Int32);}
		internal static UInt32 criAtomExAcb_GetVersion(IntPtr acbData, Int32 acbDataSize, IntPtr flag){return default(UInt32);}
		internal static UInt32 criAtomExAcb_GetVersionFromFile(IntPtr acbBinder, IntPtr acbPath, IntPtr work, Int32 workSize, NativeBool* flag){return default(UInt32);}
		internal static void criAtomExAcb_GetSupportedVersion(UInt32* versionLow, UInt32* versionHigh){}
		internal static Int32 criAtomExAcb_GetNumCues(IntPtr acbHn){return default(Int32);}
		internal static NativeBool criAtomExAcb_ExistsId(IntPtr acbHn, Int32 id){return default(NativeBool);}
		internal static NativeBool criAtomExAcb_ExistsName(IntPtr acbHn, IntPtr name){return default(NativeBool);}
		internal static NativeBool criAtomExAcb_ExistsIndex(IntPtr acbHn, Int32 index){return default(NativeBool);}
		internal static Int32 criAtomExAcb_GetCueIdByIndex(IntPtr acbHn, Int32 index){return default(Int32);}
		internal static Int32 criAtomExAcb_GetCueIdByName(IntPtr acbHn, IntPtr name){return default(Int32);}
		internal static NativeString criAtomExAcb_GetCueNameByIndex(IntPtr acbHn, Int32 index){return default(NativeString);}
		internal static NativeString criAtomExAcb_GetCueNameById(IntPtr acbHn, Int32 id){return default(NativeString);}
		internal static Int32 criAtomExAcb_GetCueIndexById(IntPtr acbHn, Int32 id){return default(Int32);}
		internal static Int32 criAtomExAcb_GetCueIndexByName(IntPtr acbHn, IntPtr name){return default(Int32);}
		internal static NativeString criAtomExAcb_GetUserDataById(IntPtr acbHn, Int32 id){return default(NativeString);}
		internal static NativeString criAtomExAcb_GetUserDataByName(IntPtr acbHn, IntPtr name){return default(NativeString);}
		internal static Int64 criAtomExAcb_GetLengthById(IntPtr acbHn, Int32 id){return default(Int64);}
		internal static Int64 criAtomExAcb_GetLengthByName(IntPtr acbHn, IntPtr name){return default(Int64);}
		internal static Int32 criAtomExAcb_GetNumUsableAisacControlsById(IntPtr acbHn, Int32 id){return default(Int32);}
		internal static Int32 criAtomExAcb_GetNumUsableAisacControlsByName(IntPtr acbHn, IntPtr name){return default(Int32);}
		internal static NativeBool criAtomExAcb_GetUsableAisacControlById(IntPtr acbHn, Int32 id, UInt16 index, CriAtomEx.AisacControlInfo* info){return default(NativeBool);}
		internal static NativeBool criAtomExAcb_GetUsableAisacControlByName(IntPtr acbHn, IntPtr name, UInt16 index, CriAtomEx.AisacControlInfo* info){return default(NativeBool);}
		internal static NativeBool criAtomExAcb_IsUsingAisacControlById(IntPtr acbHn, Int32 id, UInt32 aisacControlId){return default(NativeBool);}
		internal static NativeBool criAtomExAcb_IsUsingAisacControlByName(IntPtr acbHn, IntPtr name, IntPtr aisacControlName){return default(NativeBool);}
		internal static Int32 criAtomExAcb_GetCuePriorityById(IntPtr acbHn, Int32 id){return default(Int32);}
		internal static Int32 criAtomExAcb_GetCuePriorityByName(IntPtr acbHn, IntPtr name){return default(Int32);}
		internal static NativeBool criAtomExAcb_GetWaveformInfoById(IntPtr acbHn, Int32 id, CriAtomEx.WaveformInfo* waveformInfo){return default(NativeBool);}
		internal static NativeBool criAtomExAcb_GetWaveformInfoByName(IntPtr acbHn, IntPtr name, CriAtomEx.WaveformInfo* waveformInfo){return default(NativeBool);}
		internal static IntPtr criAtomExAcb_GetOnMemoryAwbHandle(IntPtr acbHn){return default(IntPtr);}
		internal static IntPtr criAtomExAcb_GetStreamingAwbHandle(IntPtr acbHn){return default(IntPtr);}
		internal static IntPtr criAtomExAcb_GetStreamingAwbHandleBySlotName(IntPtr acbHn, IntPtr awbSlotName){return default(IntPtr);}
		internal static IntPtr criAtomExAcb_GetStreamingAwbHandleBySlotIndex(IntPtr acbHn, UInt16 awbSlotIndex){return default(IntPtr);}
		internal static NativeBool criAtomExAcb_GetCueInfoByName(IntPtr acbHn, IntPtr name, CriAtomEx.CueInfo* info){return default(NativeBool);}
		internal static NativeBool criAtomExAcb_GetCueInfoById(IntPtr acbHn, Int32 id, CriAtomEx.CueInfo* info){return default(NativeBool);}
		internal static NativeBool criAtomExAcb_GetCueInfoByIndex(IntPtr acbHn, Int32 index, CriAtomEx.CueInfo* info){return default(NativeBool);}
		internal static Int32 criAtomExAcb_GetNumCuePlayingCountByName(IntPtr acbHn, IntPtr name){return default(Int32);}
		internal static Int32 criAtomExAcb_GetNumCuePlayingCountById(IntPtr acbHn, Int32 id){return default(Int32);}
		internal static Int32 criAtomExAcb_GetNumCuePlayingCountByIndex(IntPtr acbHn, Int32 index){return default(Int32);}
		internal static Int32 criAtomExAcb_GetBlockIndexByIndex(IntPtr acbHn, Int32 index, IntPtr blockName){return default(Int32);}
		internal static Int32 criAtomExAcb_GetBlockIndexById(IntPtr acbHn, Int32 id, IntPtr blockName){return default(Int32);}
		internal static Int32 criAtomExAcb_GetBlockIndexByName(IntPtr acbHn, IntPtr name, IntPtr blockName){return default(Int32);}
		internal static void criAtomExAcb_SetDetectionInGamePreviewDataCallback(IntPtr func, IntPtr obj){}
		internal static NativeBool criAtomExAcb_GetAcbInfo(IntPtr acbHn, CriAtomExAcb.Info* acbInfo){return default(NativeBool);}
		internal static void criAtomExAcb_ResetCueTypeStateByName(IntPtr acbHn, IntPtr name){}
		internal static void criAtomExAcb_ResetCueTypeStateById(IntPtr acbHn, Int32 id){}
		internal static void criAtomExAcb_ResetCueTypeStateByIndex(IntPtr acbHn, Int32 index){}
		internal static void criAtomExAcb_AttachAwbFile(IntPtr acbHn, IntPtr awbBinder, IntPtr awbPath, IntPtr awbName, IntPtr work, Int32 workSize){}
		internal static void criAtomExAcb_DetachAwbFile(IntPtr acbHn, IntPtr awbName){}
		internal static Int32 criAtomExAcb_CalculateWorkSizeForAttachAwbFile(IntPtr awbBinder, IntPtr awbPath){return default(Int32);}
		internal static Int32 criAtomExAcb_GetNumAwbFileSlots(IntPtr acbHn){return default(Int32);}
		internal static NativeString criAtomExAcb_GetAwbFileSlotName(IntPtr acbHn, UInt16 index){return default(NativeString);}
		internal static NativeBool criAtomExAcb_IsAttachedAwbFile(IntPtr acbHn, IntPtr awbName){return default(NativeBool);}
#endif
		}
	}
	public partial class CriAtomPlayer
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetDefaultConfigForInstrumentPlayer_(CriAtomInstrument.PlayerConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetDefaultConfig_ASR_(CriAtomPlayer.ConfigASR* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetFilterCallback(IntPtr player, IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetDefaultConfigForAdxPlayer_(CriAtom.AdxPlayerConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetDefaultConfigForAiffPlayer_(CriAtom.AiffPlayerConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetDefaultConfigForHcaMxPlayer_(CriAtomHcaMx.PlayerConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetDefaultConfigForHcaPlayer_(CriAtom.HcaPlayerConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetDefaultConfigForRawPcmPlayer_(CriAtom.RawPcmPlayerConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetDefaultConfigForStandardPlayer_(CriAtom.StandardPlayerConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetDefaultConfigForWavePlayer_(CriAtom.WavePlayerConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomPlayer_CalculateWorkSizeForStandardPlayer(CriAtom.StandardPlayerConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomPlayer_CreateStandardPlayer(CriAtom.StandardPlayerConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_Destroy(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomPlayer_CalculateWorkSizeForAdxPlayer(CriAtom.AdxPlayerConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomPlayer_CreateAdxPlayer(CriAtom.AdxPlayerConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomPlayer_CalculateWorkSizeForHcaPlayer(CriAtom.HcaPlayerConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomPlayer_CreateHcaPlayer(CriAtom.HcaPlayerConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomPlayer_CalculateWorkSizeForHcaMxPlayer(CriAtomHcaMx.PlayerConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomPlayer_CreateHcaMxPlayer(CriAtomHcaMx.PlayerConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomPlayer_CalculateWorkSizeForWavePlayer(CriAtom.WavePlayerConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomPlayer_CreateWavePlayer(CriAtom.WavePlayerConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomPlayer_CalculateWorkSizeForAiffPlayer(CriAtom.AiffPlayerConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomPlayer_CreateAiffPlayer(CriAtom.AiffPlayerConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomPlayer_CalculateWorkSizeForRawPcmPlayer(CriAtom.RawPcmPlayerConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomPlayer_CreateRawPcmPlayer(CriAtom.RawPcmPlayerConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetData(IntPtr player, IntPtr buffer, Int32 bufferSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_Start(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetDataRequestCallback(IntPtr player, IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetFile(IntPtr player, IntPtr binder, IntPtr path);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetContentId(IntPtr player, IntPtr binder, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetWaveId(IntPtr player, IntPtr awb, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetPreviousDataAgain(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_DeferCallback(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern CriAtomPlayer.Status criAtomPlayer_GetStatus(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_Pause(IntPtr player, NativeBool flag);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_Stop(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomPlayer_IsPaused(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomPlayer_GetNumChannels(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomPlayer_GetNumPlayedSamples(IntPtr player, Int64* numPlayed, Int32* samplingRate);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomPlayer_GetNumRenderedSamples(IntPtr player, Int64* numRendered, Int32* samplingRate);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int64 criAtomPlayer_GetDecodedDataSize(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int64 criAtomPlayer_GetNumDecodedSamples(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int64 criAtomPlayer_GetTime(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomPlayer_GetFormatInfo(IntPtr player, CriAtom.FormatInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomPlayer_GetInputBufferRemainSize(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomPlayer_GetOutputBufferRemainSamples(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetStartTime(IntPtr player, Int64 startTimeMs);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetVolume(IntPtr player, Single vol);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomPlayer_GetVolume(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetChannelVolume(IntPtr player, Int32 ch, Single vol);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetSendLevel(IntPtr player, Int32 ch, CriAtom.SpeakerId spk, Single level);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_ResetSendLevel(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetPanAdx1Compatible(IntPtr player, Int32 ch, Single pan);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_ResetPan(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetFrequencyRatio(IntPtr player, Single ratio);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetMaxFrequencyRatio(IntPtr player, Single ratio);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_LimitLoopCount(IntPtr player, Int32 count);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetHcaMxMixerId(IntPtr player, Int32 mixerId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetAsrRackId(IntPtr player, Int32 rackId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetRawPcmFormat(IntPtr player, CriAtom.PcmFormat pcmFormat, Int32 numChannels, Int32 samplingRate);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetStatusChangeCallback(IntPtr player, IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetParameterChangeCallback(IntPtr player, IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomPlayer_SetLoadRequestCallback(IntPtr player, IntPtr func, IntPtr obj);
#else
			internal static void criAtomPlayer_SetDefaultConfigForInstrumentPlayer_(CriAtomInstrument.PlayerConfig* pConfig){}
		internal static void criAtomPlayer_SetDefaultConfig_ASR_(CriAtomPlayer.ConfigASR* pConfig){}
		internal static void criAtomPlayer_SetFilterCallback(IntPtr player, IntPtr func, IntPtr obj){}
		internal static void criAtomPlayer_SetDefaultConfigForAdxPlayer_(CriAtom.AdxPlayerConfig* pConfig){}
		internal static void criAtomPlayer_SetDefaultConfigForAiffPlayer_(CriAtom.AiffPlayerConfig* pConfig){}
		internal static void criAtomPlayer_SetDefaultConfigForHcaMxPlayer_(CriAtomHcaMx.PlayerConfig* pConfig){}
		internal static void criAtomPlayer_SetDefaultConfigForHcaPlayer_(CriAtom.HcaPlayerConfig* pConfig){}
		internal static void criAtomPlayer_SetDefaultConfigForRawPcmPlayer_(CriAtom.RawPcmPlayerConfig* pConfig){}
		internal static void criAtomPlayer_SetDefaultConfigForStandardPlayer_(CriAtom.StandardPlayerConfig* pConfig){}
		internal static void criAtomPlayer_SetDefaultConfigForWavePlayer_(CriAtom.WavePlayerConfig* pConfig){}
		internal static Int32 criAtomPlayer_CalculateWorkSizeForStandardPlayer(CriAtom.StandardPlayerConfig* config){return default(Int32);}
		internal static IntPtr criAtomPlayer_CreateStandardPlayer(CriAtom.StandardPlayerConfig* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static void criAtomPlayer_Destroy(IntPtr player){}
		internal static Int32 criAtomPlayer_CalculateWorkSizeForAdxPlayer(CriAtom.AdxPlayerConfig* config){return default(Int32);}
		internal static IntPtr criAtomPlayer_CreateAdxPlayer(CriAtom.AdxPlayerConfig* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static Int32 criAtomPlayer_CalculateWorkSizeForHcaPlayer(CriAtom.HcaPlayerConfig* config){return default(Int32);}
		internal static IntPtr criAtomPlayer_CreateHcaPlayer(CriAtom.HcaPlayerConfig* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static Int32 criAtomPlayer_CalculateWorkSizeForHcaMxPlayer(CriAtomHcaMx.PlayerConfig* config){return default(Int32);}
		internal static IntPtr criAtomPlayer_CreateHcaMxPlayer(CriAtomHcaMx.PlayerConfig* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static Int32 criAtomPlayer_CalculateWorkSizeForWavePlayer(CriAtom.WavePlayerConfig* config){return default(Int32);}
		internal static IntPtr criAtomPlayer_CreateWavePlayer(CriAtom.WavePlayerConfig* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static Int32 criAtomPlayer_CalculateWorkSizeForAiffPlayer(CriAtom.AiffPlayerConfig* config){return default(Int32);}
		internal static IntPtr criAtomPlayer_CreateAiffPlayer(CriAtom.AiffPlayerConfig* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static Int32 criAtomPlayer_CalculateWorkSizeForRawPcmPlayer(CriAtom.RawPcmPlayerConfig* config){return default(Int32);}
		internal static IntPtr criAtomPlayer_CreateRawPcmPlayer(CriAtom.RawPcmPlayerConfig* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static void criAtomPlayer_SetData(IntPtr player, IntPtr buffer, Int32 bufferSize){}
		internal static void criAtomPlayer_Start(IntPtr player){}
		internal static void criAtomPlayer_SetDataRequestCallback(IntPtr player, IntPtr func, IntPtr obj){}
		internal static void criAtomPlayer_SetFile(IntPtr player, IntPtr binder, IntPtr path){}
		internal static void criAtomPlayer_SetContentId(IntPtr player, IntPtr binder, Int32 id){}
		internal static void criAtomPlayer_SetWaveId(IntPtr player, IntPtr awb, Int32 id){}
		internal static void criAtomPlayer_SetPreviousDataAgain(IntPtr player){}
		internal static void criAtomPlayer_DeferCallback(IntPtr player){}
		internal static CriAtomPlayer.Status criAtomPlayer_GetStatus(IntPtr player){return default(CriAtomPlayer.Status);}
		internal static void criAtomPlayer_Pause(IntPtr player, NativeBool flag){}
		internal static void criAtomPlayer_Stop(IntPtr player){}
		internal static NativeBool criAtomPlayer_IsPaused(IntPtr player){return default(NativeBool);}
		internal static Int32 criAtomPlayer_GetNumChannels(IntPtr player){return default(Int32);}
		internal static NativeBool criAtomPlayer_GetNumPlayedSamples(IntPtr player, Int64* numPlayed, Int32* samplingRate){return default(NativeBool);}
		internal static NativeBool criAtomPlayer_GetNumRenderedSamples(IntPtr player, Int64* numRendered, Int32* samplingRate){return default(NativeBool);}
		internal static Int64 criAtomPlayer_GetDecodedDataSize(IntPtr player){return default(Int64);}
		internal static Int64 criAtomPlayer_GetNumDecodedSamples(IntPtr player){return default(Int64);}
		internal static Int64 criAtomPlayer_GetTime(IntPtr player){return default(Int64);}
		internal static NativeBool criAtomPlayer_GetFormatInfo(IntPtr player, CriAtom.FormatInfo* info){return default(NativeBool);}
		internal static Int32 criAtomPlayer_GetInputBufferRemainSize(IntPtr player){return default(Int32);}
		internal static Int32 criAtomPlayer_GetOutputBufferRemainSamples(IntPtr player){return default(Int32);}
		internal static void criAtomPlayer_SetStartTime(IntPtr player, Int64 startTimeMs){}
		internal static void criAtomPlayer_SetVolume(IntPtr player, Single vol){}
		internal static Single criAtomPlayer_GetVolume(IntPtr player){return default(Single);}
		internal static void criAtomPlayer_SetChannelVolume(IntPtr player, Int32 ch, Single vol){}
		internal static void criAtomPlayer_SetSendLevel(IntPtr player, Int32 ch, CriAtom.SpeakerId spk, Single level){}
		internal static void criAtomPlayer_ResetSendLevel(IntPtr player){}
		internal static void criAtomPlayer_SetPanAdx1Compatible(IntPtr player, Int32 ch, Single pan){}
		internal static void criAtomPlayer_ResetPan(IntPtr player){}
		internal static void criAtomPlayer_SetFrequencyRatio(IntPtr player, Single ratio){}
		internal static void criAtomPlayer_SetMaxFrequencyRatio(IntPtr player, Single ratio){}
		internal static void criAtomPlayer_LimitLoopCount(IntPtr player, Int32 count){}
		internal static void criAtomPlayer_SetHcaMxMixerId(IntPtr player, Int32 mixerId){}
		internal static void criAtomPlayer_SetAsrRackId(IntPtr player, Int32 rackId){}
		internal static void criAtomPlayer_SetRawPcmFormat(IntPtr player, CriAtom.PcmFormat pcmFormat, Int32 numChannels, Int32 samplingRate){}
		internal static void criAtomPlayer_SetStatusChangeCallback(IntPtr player, IntPtr func, IntPtr obj){}
		internal static void criAtomPlayer_SetParameterChangeCallback(IntPtr player, IntPtr func, IntPtr obj){}
		internal static void criAtomPlayer_SetLoadRequestCallback(IntPtr player, IntPtr func, IntPtr obj){}
#endif
		}
	}
	public partial struct CriAtomExDbas
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExDbas_SetDefaultConfig_(CriAtomExDbas.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExDbas_CalculateWorkSize_(CriAtomExDbas.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExDbas_Create_(CriAtomExDbas.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExDbas_Destroy_(Int32 atomDbasId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExDbas_GetStreamingPlayerHandles_(Int32 dbasId, IntPtr* players, Int32 length);
#else
			internal static void criAtomExDbas_SetDefaultConfig_(CriAtomExDbas.Config* pConfig){}
		internal static Int32 criAtomExDbas_CalculateWorkSize_(CriAtomExDbas.Config* config){return default(Int32);}
		internal static Int32 criAtomExDbas_Create_(CriAtomExDbas.Config* config, IntPtr work, Int32 workSize){return default(Int32);}
		internal static void criAtomExDbas_Destroy_(Int32 atomDbasId){}
		internal static Int32 criAtomExDbas_GetStreamingPlayerHandles_(Int32 dbasId, IntPtr* players, Int32 length){return default(Int32);}
#endif
		}
	}
	public partial class CriAtomExDebug
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExDebug_GetResourcesInfo(CriAtomExDebug.ResourcesInfo* resourcesInfo);
#else
			internal static void criAtomExDebug_GetResourcesInfo(CriAtomExDebug.ResourcesInfo* resourcesInfo){}
#endif
		}
	}
	public partial class CriAtomExHcaMx
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExHcaMx_SetDefaultConfig_(CriAtomExHcaMx.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExHcaMx_CalculateWorkSize(CriAtomExHcaMx.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExHcaMx_SetConfigForWorkSizeCalculation(CriAtomExHcaMx.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExHcaMx_Initialize(CriAtomExHcaMx.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExHcaMx_Finalize();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExHcaMx_SetBusSendLevelByName(Int32 mixerId, IntPtr busName, Single level);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExHcaMx_SetFrequencyRatio(Int32 mixerId, Single ratio);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExHcaMx_SetAsrRackId(Int32 mixerId, Int32 rackId);
#else
			internal static void criAtomExHcaMx_SetDefaultConfig_(CriAtomExHcaMx.Config* pConfig){}
		internal static Int32 criAtomExHcaMx_CalculateWorkSize(CriAtomExHcaMx.Config* config){return default(Int32);}
		internal static void criAtomExHcaMx_SetConfigForWorkSizeCalculation(CriAtomExHcaMx.Config* config){}
		internal static void criAtomExHcaMx_Initialize(CriAtomExHcaMx.Config* config, IntPtr work, Int32 workSize){}
		internal static void criAtomExHcaMx_Finalize(){}
		internal static void criAtomExHcaMx_SetBusSendLevelByName(Int32 mixerId, IntPtr busName, Single level){}
		internal static void criAtomExHcaMx_SetFrequencyRatio(Int32 mixerId, Single ratio){}
		internal static void criAtomExHcaMx_SetAsrRackId(Int32 mixerId, Int32 rackId){}
#endif
		}
	}
	public partial class CriAtomExTween
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExTween_SetDefaultConfig_(CriAtomExTween.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExTween_CalculateWorkSize(CriAtomExTween.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExTween_Create(CriAtomExTween.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExTween_Destroy(IntPtr tween);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomExTween_GetValue(IntPtr tween);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExTween_MoveTo(IntPtr tween, UInt16 timeMs, Single value);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExTween_MoveFrom(IntPtr tween, UInt16 timeMs, Single value);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExTween_Stop(IntPtr tween);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExTween_Reset(IntPtr tween);
#else
			internal static void criAtomExTween_SetDefaultConfig_(CriAtomExTween.Config* pConfig){}
		internal static Int32 criAtomExTween_CalculateWorkSize(CriAtomExTween.Config* config){return default(Int32);}
		internal static IntPtr criAtomExTween_Create(CriAtomExTween.Config* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static void criAtomExTween_Destroy(IntPtr tween){}
		internal static Single criAtomExTween_GetValue(IntPtr tween){return default(Single);}
		internal static void criAtomExTween_MoveTo(IntPtr tween, UInt16 timeMs, Single value){}
		internal static void criAtomExTween_MoveFrom(IntPtr tween, UInt16 timeMs, Single value){}
		internal static void criAtomExTween_Stop(IntPtr tween){}
		internal static void criAtomExTween_Reset(IntPtr tween){}
#endif
		}
	}
	public partial class CriAtomExFader
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExFader_SetDefaultConfig_(CriAtomExFader.Config* pConfig);
#else
			internal static void criAtomExFader_SetDefaultConfig_(CriAtomExFader.Config* pConfig){}
#endif
		}
	}
	public partial class CriAtomExPlayer
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_OverrideDefaultPanMethod(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetFilterCallback(IntPtr player, IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetWideness(IntPtr player, Single wideness);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExPlayer_GetSoundObject(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetDefaultConfig_(CriAtomExPlayer.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_AddOutputPort(IntPtr player, IntPtr outputPort);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_RemoveOutputPort(IntPtr player, IntPtr outputPort);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_ClearOutputPorts(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_AddPreferredOutputPort(IntPtr player, IntPtr outputPort);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_RemovePreferredOutputPort(IntPtr player, IntPtr outputPort);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_RemovePreferredOutputPortByName(IntPtr player, IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_ClearPreferredOutputPorts(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExPlayer_CalculateWorkSize(CriAtomExPlayer.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExPlayer_Create(CriAtomExPlayer.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_Destroy(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetCueId(IntPtr player, IntPtr acbHn, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern UInt32 criAtomExPlayer_Start(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetCueName(IntPtr player, IntPtr acbHn, IntPtr cueName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetCueIndex(IntPtr player, IntPtr acbHn, Int32 index);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetData(IntPtr player, IntPtr buffer, Int32 size);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetFormat(IntPtr player, UInt32 format);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetNumChannels(IntPtr player, Int32 numChannels);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetSamplingRate(IntPtr player, Int32 samplingRate);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetFile(IntPtr player, IntPtr binder, IntPtr path);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetContentId(IntPtr player, IntPtr binder, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetWaveId(IntPtr player, IntPtr awb, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern CriAtomExPlayer.Status criAtomExPlayer_GetStatus(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_Pause(IntPtr player, NativeBool sw);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern UInt32 criAtomExPlayer_Prepare(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_Stop(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_StopWithoutReleaseTime(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_StopAllPlayers();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_StopAllPlayersWithoutReleaseTime();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_EnumeratePlayers(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_Resume(IntPtr player, CriAtomEx.ResumeMode mode);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayer_IsPaused(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_EnumeratePlaybacks(IntPtr player, IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExPlayer_GetNumPlaybacks(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern UInt32 criAtomExPlayer_GetLastPlaybackId(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int64 criAtomExPlayer_GetTime(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetSoundRendererType(IntPtr player, CriAtom.SoundRendererType type);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetGroupNumber(IntPtr player, Int32 groupNo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetVoiceControlMethod(IntPtr player, CriAtomEx.VoiceControlMethod method);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetVoicePoolIdentifier(IntPtr player, UInt32 identifier);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetHcaMxMixerId(IntPtr player, Int32 mixerId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetAsrRackId(IntPtr player, Int32 rackId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetAsrRackIdArray(IntPtr player, Int32* rackIdArray, Int32 numRacks);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetStartTime(IntPtr player, Int64 startTimeMs);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetSyncPlaybackId(IntPtr player, UInt32 playbackId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetPlaybackRatio(IntPtr player, Single playbackRatio);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_LimitLoopCount(IntPtr player, Int32 count);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetVolume(IntPtr player, Single volume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_UpdateAll(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_Update(IntPtr player, UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_ResetParameters(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomExPlayer_GetParameterFloat32(IntPtr player, CriAtomEx.ParameterId id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern UInt32 criAtomExPlayer_GetParameterUint32(IntPtr player, CriAtomEx.ParameterId id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExPlayer_GetParameterSint32(IntPtr player, CriAtomEx.ParameterId id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetPitch(IntPtr player, Single pitch);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetMaxPitch(IntPtr player, Single pitch);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetPan3dAngle(IntPtr player, Single pan3dAngle);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetPan3dInteriorDistance(IntPtr player, Single pan3dInteriorDistance);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetPan3dVolume(IntPtr player, Single pan3dVolume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetPanType(IntPtr player, CriAtomEx.PanType panType);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern CriAtomEx.PanType criAtomExPlayer_GetPanTypeOnPlayback(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetPanSpeakerType(IntPtr player, CriAtomEx.PanSpeakerType panSpeakerType);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_AddMixDownCenterVolumeOffset(IntPtr player, Single mixdownCenterVolumeOffset);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_AddMixDownLfeVolumeOffset(IntPtr player, Single mixdownLfeVolumeOffset);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_ChangeDefaultPanSpeakerType(CriAtomEx.PanSpeakerType panSpeakerType);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetPanAngleType(IntPtr player, CriAtomEx.PanAngleType panAngleType);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetSendLevel(IntPtr player, Int32 ch, CriAtomEx.SpeakerId spk, Single level);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetBusSendLevelByName(IntPtr player, IntPtr busName, Single level);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_ResetBusSends(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayer_GetBusSendLevelByName(IntPtr player, IntPtr busName, Single* level);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetBusSendLevelOffsetByName(IntPtr player, IntPtr busName, Single levelOffset);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayer_GetBusSendLevelOffsetByName(IntPtr player, IntPtr busName, Single* levelOffset);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetPanAdx1Compatible(IntPtr player, Int32 ch, Single pan);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetBandpassFilterParameters(IntPtr player, Single cofLow, Single cofHigh);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetBiquadFilterParameters(IntPtr player, CriAtomEx.BiquadFilterType type, Single frequency, Single gain, Single qValue);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetVoicePriority(IntPtr player, Int32 priority);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetAisacControlById(IntPtr player, UInt32 controlId, Single controlValue);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetAisacControlByName(IntPtr player, IntPtr controlName, Single controlValue);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_ClearAisacControls(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_Set3dListenerHn(IntPtr player, IntPtr listener);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_Set3dSourceHn(IntPtr player, IntPtr source);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_Set3dSourceListHn(IntPtr player, IntPtr sourceList);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomExPlayer_GetAisacControlById(IntPtr player, UInt32 controlId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomExPlayer_GetAisacControlByName(IntPtr player, IntPtr controlName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetCategoryById(IntPtr player, UInt32 categoryId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetCategoryByName(IntPtr player, IntPtr categoryName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_UnsetCategory(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExPlayer_GetNumCategories(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayer_GetCategoryInfo(IntPtr player, UInt16 index, CriAtomExCategory.Info* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetTrackInfo(IntPtr player, Int32 numTracks, Int32* channelsPerTrack);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetTrackVolume(IntPtr player, Int32 trackNo, Single volume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetSilentMode(IntPtr player, CriAtomEx.SilentMode silentMode);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetCuePriority(IntPtr player, Int32 cuePriority);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetPreDelayTime(IntPtr player, Single predelayTimeMs);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetEnvelopeAttackTime(IntPtr player, Single attackTimeMs);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetEnvelopeAttackCurve(IntPtr player, CriAtomEx.CurveType curveType, Single strength);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetEnvelopeHoldTime(IntPtr player, Single holdTimeMs);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetEnvelopeDecayTime(IntPtr player, Single decayTimeMs);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetEnvelopeDecayCurve(IntPtr player, CriAtomEx.CurveType curveType, Single strength);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetEnvelopeReleaseTime(IntPtr player, Single releaseTimeMs);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetEnvelopeReleaseCurve(IntPtr player, CriAtomEx.CurveType curveType, Single strength);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetEnvelopeSustainLevel(IntPtr player, Single susutainLevel);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetDataRequestCallback(IntPtr player, IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetRandomSeed(IntPtr player, UInt32 seed);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetDspParameter(IntPtr player, Int32 paramId, Single paramVal);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetDspBypass(IntPtr player, NativeBool isBypassed);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_AttachAisac(IntPtr player, IntPtr globalAisacName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_DetachAisac(IntPtr player, IntPtr globalAisacName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_DetachAisacAll(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExPlayer_GetNumAttachedAisacs(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayer_GetAttachedAisacInfo(IntPtr player, Int32 aisacAttachedIndex, CriAtomEx.AisacInfo* aisacInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetStreamingCacheId(IntPtr player, IntPtr cacheId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_AttachTween(IntPtr player, IntPtr tween);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_DetachTween(IntPtr player, IntPtr tween);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_DetachTweenAll(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetFirstBlockIndex(IntPtr player, Int32 index);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetBlockTransitionCallback(IntPtr player, IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetDrySendLevel(IntPtr player, CriAtomEx.SpeakerId spk, Single offset, Single gain);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetSelectorLabel(IntPtr player, IntPtr selector, IntPtr label);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_UnsetSelectorLabel(IntPtr player, IntPtr selector);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_ClearSelectorLabels(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetPlaybackTrackInfoNotificationCallback(IntPtr player, IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetPlaybackEventCallback(IntPtr player, IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetChannelConfig(IntPtr player, Int32 numChannels, CriAtom.ChannelConfig channelConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExPlayer_CalculateWorkSizeForFader(CriAtomExFader.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_AttachFader(IntPtr player, CriAtomExFader.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetFadeInTime(IntPtr player, Int32 ms);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetFadeOutTime(IntPtr player, Int32 ms);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_DetachFader(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExPlayer_GetFadeOutTime(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExPlayer_GetFadeInTime(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetFadeInStartOffset(IntPtr player, Int32 ms);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExPlayer_GetFadeInStartOffset(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_SetFadeOutEndDelay(IntPtr player, Int32 ms);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExPlayer_GetFadeOutEndDelay(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayer_IsFading(IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayer_ResetFaderParameters(IntPtr player);
#else
			internal static void criAtomExPlayer_OverrideDefaultPanMethod(IntPtr func, IntPtr obj){}
		internal static void criAtomExPlayer_SetFilterCallback(IntPtr player, IntPtr func, IntPtr obj){}
		internal static void criAtomExPlayer_SetWideness(IntPtr player, Single wideness){}
		internal static IntPtr criAtomExPlayer_GetSoundObject(IntPtr player){return default(IntPtr);}
		internal static void criAtomExPlayer_SetDefaultConfig_(CriAtomExPlayer.Config* pConfig){}
		internal static void criAtomExPlayer_AddOutputPort(IntPtr player, IntPtr outputPort){}
		internal static void criAtomExPlayer_RemoveOutputPort(IntPtr player, IntPtr outputPort){}
		internal static void criAtomExPlayer_ClearOutputPorts(IntPtr player){}
		internal static void criAtomExPlayer_AddPreferredOutputPort(IntPtr player, IntPtr outputPort){}
		internal static void criAtomExPlayer_RemovePreferredOutputPort(IntPtr player, IntPtr outputPort){}
		internal static void criAtomExPlayer_RemovePreferredOutputPortByName(IntPtr player, IntPtr name){}
		internal static void criAtomExPlayer_ClearPreferredOutputPorts(IntPtr player){}
		internal static Int32 criAtomExPlayer_CalculateWorkSize(CriAtomExPlayer.Config* config){return default(Int32);}
		internal static IntPtr criAtomExPlayer_Create(CriAtomExPlayer.Config* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static void criAtomExPlayer_Destroy(IntPtr player){}
		internal static void criAtomExPlayer_SetCueId(IntPtr player, IntPtr acbHn, Int32 id){}
		internal static UInt32 criAtomExPlayer_Start(IntPtr player){return default(UInt32);}
		internal static void criAtomExPlayer_SetCueName(IntPtr player, IntPtr acbHn, IntPtr cueName){}
		internal static void criAtomExPlayer_SetCueIndex(IntPtr player, IntPtr acbHn, Int32 index){}
		internal static void criAtomExPlayer_SetData(IntPtr player, IntPtr buffer, Int32 size){}
		internal static void criAtomExPlayer_SetFormat(IntPtr player, UInt32 format){}
		internal static void criAtomExPlayer_SetNumChannels(IntPtr player, Int32 numChannels){}
		internal static void criAtomExPlayer_SetSamplingRate(IntPtr player, Int32 samplingRate){}
		internal static void criAtomExPlayer_SetFile(IntPtr player, IntPtr binder, IntPtr path){}
		internal static void criAtomExPlayer_SetContentId(IntPtr player, IntPtr binder, Int32 id){}
		internal static void criAtomExPlayer_SetWaveId(IntPtr player, IntPtr awb, Int32 id){}
		internal static CriAtomExPlayer.Status criAtomExPlayer_GetStatus(IntPtr player){return default(CriAtomExPlayer.Status);}
		internal static void criAtomExPlayer_Pause(IntPtr player, NativeBool sw){}
		internal static UInt32 criAtomExPlayer_Prepare(IntPtr player){return default(UInt32);}
		internal static void criAtomExPlayer_Stop(IntPtr player){}
		internal static void criAtomExPlayer_StopWithoutReleaseTime(IntPtr player){}
		internal static void criAtomExPlayer_StopAllPlayers(){}
		internal static void criAtomExPlayer_StopAllPlayersWithoutReleaseTime(){}
		internal static void criAtomExPlayer_EnumeratePlayers(IntPtr func, IntPtr obj){}
		internal static void criAtomExPlayer_Resume(IntPtr player, CriAtomEx.ResumeMode mode){}
		internal static NativeBool criAtomExPlayer_IsPaused(IntPtr player){return default(NativeBool);}
		internal static void criAtomExPlayer_EnumeratePlaybacks(IntPtr player, IntPtr func, IntPtr obj){}
		internal static Int32 criAtomExPlayer_GetNumPlaybacks(IntPtr player){return default(Int32);}
		internal static UInt32 criAtomExPlayer_GetLastPlaybackId(IntPtr player){return default(UInt32);}
		internal static Int64 criAtomExPlayer_GetTime(IntPtr player){return default(Int64);}
		internal static void criAtomExPlayer_SetSoundRendererType(IntPtr player, CriAtom.SoundRendererType type){}
		internal static void criAtomExPlayer_SetGroupNumber(IntPtr player, Int32 groupNo){}
		internal static void criAtomExPlayer_SetVoiceControlMethod(IntPtr player, CriAtomEx.VoiceControlMethod method){}
		internal static void criAtomExPlayer_SetVoicePoolIdentifier(IntPtr player, UInt32 identifier){}
		internal static void criAtomExPlayer_SetHcaMxMixerId(IntPtr player, Int32 mixerId){}
		internal static void criAtomExPlayer_SetAsrRackId(IntPtr player, Int32 rackId){}
		internal static void criAtomExPlayer_SetAsrRackIdArray(IntPtr player, Int32* rackIdArray, Int32 numRacks){}
		internal static void criAtomExPlayer_SetStartTime(IntPtr player, Int64 startTimeMs){}
		internal static void criAtomExPlayer_SetSyncPlaybackId(IntPtr player, UInt32 playbackId){}
		internal static void criAtomExPlayer_SetPlaybackRatio(IntPtr player, Single playbackRatio){}
		internal static void criAtomExPlayer_LimitLoopCount(IntPtr player, Int32 count){}
		internal static void criAtomExPlayer_SetVolume(IntPtr player, Single volume){}
		internal static void criAtomExPlayer_UpdateAll(IntPtr player){}
		internal static void criAtomExPlayer_Update(IntPtr player, UInt32 id){}
		internal static void criAtomExPlayer_ResetParameters(IntPtr player){}
		internal static Single criAtomExPlayer_GetParameterFloat32(IntPtr player, CriAtomEx.ParameterId id){return default(Single);}
		internal static UInt32 criAtomExPlayer_GetParameterUint32(IntPtr player, CriAtomEx.ParameterId id){return default(UInt32);}
		internal static Int32 criAtomExPlayer_GetParameterSint32(IntPtr player, CriAtomEx.ParameterId id){return default(Int32);}
		internal static void criAtomExPlayer_SetPitch(IntPtr player, Single pitch){}
		internal static void criAtomExPlayer_SetMaxPitch(IntPtr player, Single pitch){}
		internal static void criAtomExPlayer_SetPan3dAngle(IntPtr player, Single pan3dAngle){}
		internal static void criAtomExPlayer_SetPan3dInteriorDistance(IntPtr player, Single pan3dInteriorDistance){}
		internal static void criAtomExPlayer_SetPan3dVolume(IntPtr player, Single pan3dVolume){}
		internal static void criAtomExPlayer_SetPanType(IntPtr player, CriAtomEx.PanType panType){}
		internal static CriAtomEx.PanType criAtomExPlayer_GetPanTypeOnPlayback(IntPtr player){return default(CriAtomEx.PanType);}
		internal static void criAtomExPlayer_SetPanSpeakerType(IntPtr player, CriAtomEx.PanSpeakerType panSpeakerType){}
		internal static void criAtomExPlayer_AddMixDownCenterVolumeOffset(IntPtr player, Single mixdownCenterVolumeOffset){}
		internal static void criAtomExPlayer_AddMixDownLfeVolumeOffset(IntPtr player, Single mixdownLfeVolumeOffset){}
		internal static void criAtomExPlayer_ChangeDefaultPanSpeakerType(CriAtomEx.PanSpeakerType panSpeakerType){}
		internal static void criAtomExPlayer_SetPanAngleType(IntPtr player, CriAtomEx.PanAngleType panAngleType){}
		internal static void criAtomExPlayer_SetSendLevel(IntPtr player, Int32 ch, CriAtomEx.SpeakerId spk, Single level){}
		internal static void criAtomExPlayer_SetBusSendLevelByName(IntPtr player, IntPtr busName, Single level){}
		internal static void criAtomExPlayer_ResetBusSends(IntPtr player){}
		internal static NativeBool criAtomExPlayer_GetBusSendLevelByName(IntPtr player, IntPtr busName, Single* level){return default(NativeBool);}
		internal static void criAtomExPlayer_SetBusSendLevelOffsetByName(IntPtr player, IntPtr busName, Single levelOffset){}
		internal static NativeBool criAtomExPlayer_GetBusSendLevelOffsetByName(IntPtr player, IntPtr busName, Single* levelOffset){return default(NativeBool);}
		internal static void criAtomExPlayer_SetPanAdx1Compatible(IntPtr player, Int32 ch, Single pan){}
		internal static void criAtomExPlayer_SetBandpassFilterParameters(IntPtr player, Single cofLow, Single cofHigh){}
		internal static void criAtomExPlayer_SetBiquadFilterParameters(IntPtr player, CriAtomEx.BiquadFilterType type, Single frequency, Single gain, Single qValue){}
		internal static void criAtomExPlayer_SetVoicePriority(IntPtr player, Int32 priority){}
		internal static void criAtomExPlayer_SetAisacControlById(IntPtr player, UInt32 controlId, Single controlValue){}
		internal static void criAtomExPlayer_SetAisacControlByName(IntPtr player, IntPtr controlName, Single controlValue){}
		internal static void criAtomExPlayer_ClearAisacControls(IntPtr player){}
		internal static void criAtomExPlayer_Set3dListenerHn(IntPtr player, IntPtr listener){}
		internal static void criAtomExPlayer_Set3dSourceHn(IntPtr player, IntPtr source){}
		internal static void criAtomExPlayer_Set3dSourceListHn(IntPtr player, IntPtr sourceList){}
		internal static Single criAtomExPlayer_GetAisacControlById(IntPtr player, UInt32 controlId){return default(Single);}
		internal static Single criAtomExPlayer_GetAisacControlByName(IntPtr player, IntPtr controlName){return default(Single);}
		internal static void criAtomExPlayer_SetCategoryById(IntPtr player, UInt32 categoryId){}
		internal static void criAtomExPlayer_SetCategoryByName(IntPtr player, IntPtr categoryName){}
		internal static void criAtomExPlayer_UnsetCategory(IntPtr player){}
		internal static Int32 criAtomExPlayer_GetNumCategories(IntPtr player){return default(Int32);}
		internal static NativeBool criAtomExPlayer_GetCategoryInfo(IntPtr player, UInt16 index, CriAtomExCategory.Info* info){return default(NativeBool);}
		internal static void criAtomExPlayer_SetTrackInfo(IntPtr player, Int32 numTracks, Int32* channelsPerTrack){}
		internal static void criAtomExPlayer_SetTrackVolume(IntPtr player, Int32 trackNo, Single volume){}
		internal static void criAtomExPlayer_SetSilentMode(IntPtr player, CriAtomEx.SilentMode silentMode){}
		internal static void criAtomExPlayer_SetCuePriority(IntPtr player, Int32 cuePriority){}
		internal static void criAtomExPlayer_SetPreDelayTime(IntPtr player, Single predelayTimeMs){}
		internal static void criAtomExPlayer_SetEnvelopeAttackTime(IntPtr player, Single attackTimeMs){}
		internal static void criAtomExPlayer_SetEnvelopeAttackCurve(IntPtr player, CriAtomEx.CurveType curveType, Single strength){}
		internal static void criAtomExPlayer_SetEnvelopeHoldTime(IntPtr player, Single holdTimeMs){}
		internal static void criAtomExPlayer_SetEnvelopeDecayTime(IntPtr player, Single decayTimeMs){}
		internal static void criAtomExPlayer_SetEnvelopeDecayCurve(IntPtr player, CriAtomEx.CurveType curveType, Single strength){}
		internal static void criAtomExPlayer_SetEnvelopeReleaseTime(IntPtr player, Single releaseTimeMs){}
		internal static void criAtomExPlayer_SetEnvelopeReleaseCurve(IntPtr player, CriAtomEx.CurveType curveType, Single strength){}
		internal static void criAtomExPlayer_SetEnvelopeSustainLevel(IntPtr player, Single susutainLevel){}
		internal static void criAtomExPlayer_SetDataRequestCallback(IntPtr player, IntPtr func, IntPtr obj){}
		internal static void criAtomExPlayer_SetRandomSeed(IntPtr player, UInt32 seed){}
		internal static void criAtomExPlayer_SetDspParameter(IntPtr player, Int32 paramId, Single paramVal){}
		internal static void criAtomExPlayer_SetDspBypass(IntPtr player, NativeBool isBypassed){}
		internal static void criAtomExPlayer_AttachAisac(IntPtr player, IntPtr globalAisacName){}
		internal static void criAtomExPlayer_DetachAisac(IntPtr player, IntPtr globalAisacName){}
		internal static void criAtomExPlayer_DetachAisacAll(IntPtr player){}
		internal static Int32 criAtomExPlayer_GetNumAttachedAisacs(IntPtr player){return default(Int32);}
		internal static NativeBool criAtomExPlayer_GetAttachedAisacInfo(IntPtr player, Int32 aisacAttachedIndex, CriAtomEx.AisacInfo* aisacInfo){return default(NativeBool);}
		internal static void criAtomExPlayer_SetStreamingCacheId(IntPtr player, IntPtr cacheId){}
		internal static void criAtomExPlayer_AttachTween(IntPtr player, IntPtr tween){}
		internal static void criAtomExPlayer_DetachTween(IntPtr player, IntPtr tween){}
		internal static void criAtomExPlayer_DetachTweenAll(IntPtr player){}
		internal static void criAtomExPlayer_SetFirstBlockIndex(IntPtr player, Int32 index){}
		internal static void criAtomExPlayer_SetBlockTransitionCallback(IntPtr player, IntPtr func, IntPtr obj){}
		internal static void criAtomExPlayer_SetDrySendLevel(IntPtr player, CriAtomEx.SpeakerId spk, Single offset, Single gain){}
		internal static void criAtomExPlayer_SetSelectorLabel(IntPtr player, IntPtr selector, IntPtr label){}
		internal static void criAtomExPlayer_UnsetSelectorLabel(IntPtr player, IntPtr selector){}
		internal static void criAtomExPlayer_ClearSelectorLabels(IntPtr player){}
		internal static void criAtomExPlayer_SetPlaybackTrackInfoNotificationCallback(IntPtr player, IntPtr func, IntPtr obj){}
		internal static void criAtomExPlayer_SetPlaybackEventCallback(IntPtr player, IntPtr func, IntPtr obj){}
		internal static void criAtomExPlayer_SetChannelConfig(IntPtr player, Int32 numChannels, CriAtom.ChannelConfig channelConfig){}
		internal static Int32 criAtomExPlayer_CalculateWorkSizeForFader(CriAtomExFader.Config* config){return default(Int32);}
		internal static void criAtomExPlayer_AttachFader(IntPtr player, CriAtomExFader.Config* config, IntPtr work, Int32 workSize){}
		internal static void criAtomExPlayer_SetFadeInTime(IntPtr player, Int32 ms){}
		internal static void criAtomExPlayer_SetFadeOutTime(IntPtr player, Int32 ms){}
		internal static void criAtomExPlayer_DetachFader(IntPtr player){}
		internal static Int32 criAtomExPlayer_GetFadeOutTime(IntPtr player){return default(Int32);}
		internal static Int32 criAtomExPlayer_GetFadeInTime(IntPtr player){return default(Int32);}
		internal static void criAtomExPlayer_SetFadeInStartOffset(IntPtr player, Int32 ms){}
		internal static Int32 criAtomExPlayer_GetFadeInStartOffset(IntPtr player){return default(Int32);}
		internal static void criAtomExPlayer_SetFadeOutEndDelay(IntPtr player, Int32 ms){}
		internal static Int32 criAtomExPlayer_GetFadeOutEndDelay(IntPtr player){return default(Int32);}
		internal static NativeBool criAtomExPlayer_IsFading(IntPtr player){return default(NativeBool);}
		internal static void criAtomExPlayer_ResetFaderParameters(IntPtr player){}
#endif
		}
	}
	public partial class CriAtomExAsrRack
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_SetBusFilterCallbackByName(Int32 rackId, IntPtr busName, IntPtr preFunc, IntPtr postFunc, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAsrRack_GetChannelBasedAudioRackId();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAsrRack_GetObjectBasedAudioRackId();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_SetDefaultConfig_(CriAtomExAsrRack.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAsrRack_CalculateWorkSize(CriAtomExAsrRack.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAsrRack_CalculateWorkSizeForDspBusSettingFromConfig(CriAtomExAsrRack.Config* config, IntPtr setting);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAsrRack_CalculateWorkSizeForDspBusSetting(Int32 rackId, IntPtr setting);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAsrRack_CalculateWorkSizeForDspBusSettingFromAcfDataAndConfig(IntPtr acfData, Int32 acfDataSize, CriAtomExAsrRack.Config* rackConfig, IntPtr setting);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAsrRack_Create(CriAtomExAsrRack.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_Destroy(Int32 rackId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_GetNumRenderedSamples(Int32 rackId, Int64* numSamples, Int32* samplingRate);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_ResetPerformanceMonitor(Int32 rackId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_GetPerformanceInfo(Int32 rackId, CriAtomExAsrRack.PerformanceInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_AttachDspBusSetting(Int32 rackId, IntPtr setting, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_DetachDspBusSetting(Int32 rackId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_ApplyDspBusSnapshot(Int32 rackId, IntPtr snapshotName, Int32 timeMs);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeString criAtomExAsrRack_GetAppliedDspBusSnapshotName(Int32 rackId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_SetBusVolumeByName(Int32 rackId, IntPtr busName, Single volume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_GetBusVolumeByName(Int32 rackId, IntPtr busName, Single* volume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_SetBusPanInfoByName(Int32 rackId, IntPtr busName, CriAtomExAsr.BusPanInfo* panInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_GetBusPanInfoByName(Int32 rackId, IntPtr busName, CriAtomExAsr.BusPanInfo* panInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_SetBusMatrixByName(Int32 rackId, IntPtr busName, Int32 inputChannels, Int32 outputChannels, Single* matrix);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_SetBusSendLevelByName(Int32 rackId, IntPtr busName, IntPtr sendtoBusName, Single level);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_SetEffectParameter(Int32 rackId, IntPtr busName, IntPtr effectName, UInt32 parameterIndex, Single parameterValue);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_UpdateEffectParameters(Int32 rackId, IntPtr busName, IntPtr effectName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomExAsrRack_GetEffectParameter(Int32 rackId, IntPtr busName, IntPtr effectName, UInt32 parameterIndex);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_SetEffectBypass(Int32 rackId, IntPtr busName, IntPtr effectName, NativeBool bypass);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAsrRack_GetEffectBypass(Int32 rackId, IntPtr busName, IntPtr effectName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_AttachBusAnalyzerByName(Int32 rackId, IntPtr busName, CriAtomExAsr.BusAnalyzerConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_DetachBusAnalyzerByName(Int32 rackId, IntPtr busName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_GetBusAnalyzerInfoByName(Int32 rackId, IntPtr busName, CriAtomExAsr.BusAnalyzerInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_SetAlternateRackId(Int32 rackId, Int32 altRackId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAsrRack_GetNumBuses(Int32 rackId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAsrRack_GetAmplitudeAnalyzerRms(Int32 rackId, Int32 busNo, Single* rms, UInt32 numChannels);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAsrRack_GetAmplitudeAnalyzerRmsByName(Int32 rackId, IntPtr busName, Single* rms, UInt32 numChannels);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAsrRack_GetCompressorGain(Int32 rackId, Int32 busNo, Single* gain, UInt32 numChannels);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAsrRack_GetCompressorGainByName(Int32 rackId, IntPtr busName, Single* gain, UInt32 numChannels);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAsrRack_GetCompressorRms(Int32 rackId, Int32 busNo, Single* rms, UInt32 numChannels);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAsrRack_GetCompressorRmsByName(Int32 rackId, IntPtr busName, Single* rms, UInt32 numChannels);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAsrRack_SetAisacControlById(Int32 rackId, UInt32 controlId, Single controlValue);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAsrRack_SetAisacControlByName(Int32 rackId, IntPtr controlName, Single controlValue);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAsrRack_GetAisacControlById(Int32 rackId, UInt32 controlId, Single* controlValue);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExAsrRack_GetAisacControlByName(Int32 rackId, IntPtr controlName, Single* controlValue);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern CriAtom.DeviceType criAtomExAsrRack_GetDeviceType(Int32 rackId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAsrRack_GetAmbisonicRackId();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAsrRack_CalculateWorkSizeForLevelMeter(Int32 rackId, CriAtom.LevelMeterConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_AttachLevelMeter(Int32 rackId, CriAtom.LevelMeterConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_DetachLevelMeter(Int32 rackId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_GetLevelInfo(Int32 rackId, CriAtom.LevelInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAsrRack_CalculateWorkSizeForLoudnessMeter(Int32 rackId, CriAtom.LoudnessMeterConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_AttachLoudnessMeter(Int32 rackId, CriAtom.LoudnessMeterConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_DetachLoudnessMeter(Int32 rackId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_GetLoudnessInfo(Int32 rackId, CriAtom.LoudnessInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_ResetLoudnessMeter(Int32 rackId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExAsrRack_CalculateWorkSizeForTruePeakMeter(Int32 rackId, CriAtom.TruePeakMeterConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_AttachTruePeakMeter(Int32 rackId, CriAtom.TruePeakMeterConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_DetachTruePeakMeter(Int32 rackId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExAsrRack_GetTruePeakInfo(Int32 rackId, CriAtom.TruePeakInfo* info);
#else
			internal static void criAtomExAsrRack_SetBusFilterCallbackByName(Int32 rackId, IntPtr busName, IntPtr preFunc, IntPtr postFunc, IntPtr obj){}
		internal static Int32 criAtomExAsrRack_GetChannelBasedAudioRackId(){return default(Int32);}
		internal static Int32 criAtomExAsrRack_GetObjectBasedAudioRackId(){return default(Int32);}
		internal static void criAtomExAsrRack_SetDefaultConfig_(CriAtomExAsrRack.Config* pConfig){}
		internal static Int32 criAtomExAsrRack_CalculateWorkSize(CriAtomExAsrRack.Config* config){return default(Int32);}
		internal static Int32 criAtomExAsrRack_CalculateWorkSizeForDspBusSettingFromConfig(CriAtomExAsrRack.Config* config, IntPtr setting){return default(Int32);}
		internal static Int32 criAtomExAsrRack_CalculateWorkSizeForDspBusSetting(Int32 rackId, IntPtr setting){return default(Int32);}
		internal static Int32 criAtomExAsrRack_CalculateWorkSizeForDspBusSettingFromAcfDataAndConfig(IntPtr acfData, Int32 acfDataSize, CriAtomExAsrRack.Config* rackConfig, IntPtr setting){return default(Int32);}
		internal static Int32 criAtomExAsrRack_Create(CriAtomExAsrRack.Config* config, IntPtr work, Int32 workSize){return default(Int32);}
		internal static void criAtomExAsrRack_Destroy(Int32 rackId){}
		internal static void criAtomExAsrRack_GetNumRenderedSamples(Int32 rackId, Int64* numSamples, Int32* samplingRate){}
		internal static void criAtomExAsrRack_ResetPerformanceMonitor(Int32 rackId){}
		internal static void criAtomExAsrRack_GetPerformanceInfo(Int32 rackId, CriAtomExAsrRack.PerformanceInfo* info){}
		internal static void criAtomExAsrRack_AttachDspBusSetting(Int32 rackId, IntPtr setting, IntPtr work, Int32 workSize){}
		internal static void criAtomExAsrRack_DetachDspBusSetting(Int32 rackId){}
		internal static void criAtomExAsrRack_ApplyDspBusSnapshot(Int32 rackId, IntPtr snapshotName, Int32 timeMs){}
		internal static NativeString criAtomExAsrRack_GetAppliedDspBusSnapshotName(Int32 rackId){return default(NativeString);}
		internal static void criAtomExAsrRack_SetBusVolumeByName(Int32 rackId, IntPtr busName, Single volume){}
		internal static void criAtomExAsrRack_GetBusVolumeByName(Int32 rackId, IntPtr busName, Single* volume){}
		internal static void criAtomExAsrRack_SetBusPanInfoByName(Int32 rackId, IntPtr busName, CriAtomExAsr.BusPanInfo* panInfo){}
		internal static void criAtomExAsrRack_GetBusPanInfoByName(Int32 rackId, IntPtr busName, CriAtomExAsr.BusPanInfo* panInfo){}
		internal static void criAtomExAsrRack_SetBusMatrixByName(Int32 rackId, IntPtr busName, Int32 inputChannels, Int32 outputChannels, Single* matrix){}
		internal static void criAtomExAsrRack_SetBusSendLevelByName(Int32 rackId, IntPtr busName, IntPtr sendtoBusName, Single level){}
		internal static void criAtomExAsrRack_SetEffectParameter(Int32 rackId, IntPtr busName, IntPtr effectName, UInt32 parameterIndex, Single parameterValue){}
		internal static void criAtomExAsrRack_UpdateEffectParameters(Int32 rackId, IntPtr busName, IntPtr effectName){}
		internal static Single criAtomExAsrRack_GetEffectParameter(Int32 rackId, IntPtr busName, IntPtr effectName, UInt32 parameterIndex){return default(Single);}
		internal static void criAtomExAsrRack_SetEffectBypass(Int32 rackId, IntPtr busName, IntPtr effectName, NativeBool bypass){}
		internal static NativeBool criAtomExAsrRack_GetEffectBypass(Int32 rackId, IntPtr busName, IntPtr effectName){return default(NativeBool);}
		internal static void criAtomExAsrRack_AttachBusAnalyzerByName(Int32 rackId, IntPtr busName, CriAtomExAsr.BusAnalyzerConfig* config){}
		internal static void criAtomExAsrRack_DetachBusAnalyzerByName(Int32 rackId, IntPtr busName){}
		internal static void criAtomExAsrRack_GetBusAnalyzerInfoByName(Int32 rackId, IntPtr busName, CriAtomExAsr.BusAnalyzerInfo* info){}
		internal static void criAtomExAsrRack_SetAlternateRackId(Int32 rackId, Int32 altRackId){}
		internal static Int32 criAtomExAsrRack_GetNumBuses(Int32 rackId){return default(Int32);}
		internal static NativeBool criAtomExAsrRack_GetAmplitudeAnalyzerRms(Int32 rackId, Int32 busNo, Single* rms, UInt32 numChannels){return default(NativeBool);}
		internal static NativeBool criAtomExAsrRack_GetAmplitudeAnalyzerRmsByName(Int32 rackId, IntPtr busName, Single* rms, UInt32 numChannels){return default(NativeBool);}
		internal static NativeBool criAtomExAsrRack_GetCompressorGain(Int32 rackId, Int32 busNo, Single* gain, UInt32 numChannels){return default(NativeBool);}
		internal static NativeBool criAtomExAsrRack_GetCompressorGainByName(Int32 rackId, IntPtr busName, Single* gain, UInt32 numChannels){return default(NativeBool);}
		internal static NativeBool criAtomExAsrRack_GetCompressorRms(Int32 rackId, Int32 busNo, Single* rms, UInt32 numChannels){return default(NativeBool);}
		internal static NativeBool criAtomExAsrRack_GetCompressorRmsByName(Int32 rackId, IntPtr busName, Single* rms, UInt32 numChannels){return default(NativeBool);}
		internal static NativeBool criAtomExAsrRack_SetAisacControlById(Int32 rackId, UInt32 controlId, Single controlValue){return default(NativeBool);}
		internal static NativeBool criAtomExAsrRack_SetAisacControlByName(Int32 rackId, IntPtr controlName, Single controlValue){return default(NativeBool);}
		internal static NativeBool criAtomExAsrRack_GetAisacControlById(Int32 rackId, UInt32 controlId, Single* controlValue){return default(NativeBool);}
		internal static NativeBool criAtomExAsrRack_GetAisacControlByName(Int32 rackId, IntPtr controlName, Single* controlValue){return default(NativeBool);}
		internal static CriAtom.DeviceType criAtomExAsrRack_GetDeviceType(Int32 rackId){return default(CriAtom.DeviceType);}
		internal static Int32 criAtomExAsrRack_GetAmbisonicRackId(){return default(Int32);}
		internal static Int32 criAtomExAsrRack_CalculateWorkSizeForLevelMeter(Int32 rackId, CriAtom.LevelMeterConfig* config){return default(Int32);}
		internal static void criAtomExAsrRack_AttachLevelMeter(Int32 rackId, CriAtom.LevelMeterConfig* config, IntPtr work, Int32 workSize){}
		internal static void criAtomExAsrRack_DetachLevelMeter(Int32 rackId){}
		internal static void criAtomExAsrRack_GetLevelInfo(Int32 rackId, CriAtom.LevelInfo* info){}
		internal static Int32 criAtomExAsrRack_CalculateWorkSizeForLoudnessMeter(Int32 rackId, CriAtom.LoudnessMeterConfig* config){return default(Int32);}
		internal static void criAtomExAsrRack_AttachLoudnessMeter(Int32 rackId, CriAtom.LoudnessMeterConfig* config, IntPtr work, Int32 workSize){}
		internal static void criAtomExAsrRack_DetachLoudnessMeter(Int32 rackId){}
		internal static void criAtomExAsrRack_GetLoudnessInfo(Int32 rackId, CriAtom.LoudnessInfo* info){}
		internal static void criAtomExAsrRack_ResetLoudnessMeter(Int32 rackId){}
		internal static Int32 criAtomExAsrRack_CalculateWorkSizeForTruePeakMeter(Int32 rackId, CriAtom.TruePeakMeterConfig* config){return default(Int32);}
		internal static void criAtomExAsrRack_AttachTruePeakMeter(Int32 rackId, CriAtom.TruePeakMeterConfig* config, IntPtr work, Int32 workSize){}
		internal static void criAtomExAsrRack_DetachTruePeakMeter(Int32 rackId){}
		internal static void criAtomExAsrRack_GetTruePeakInfo(Int32 rackId, CriAtom.TruePeakInfo* info){}
#endif
		}
	}
	public partial class CriAtomInstrument
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomInstrument_RegisterInstrumentInterface(IntPtr ainstInterface);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomInstrument_UnregisterInstrumentInterface(IntPtr ainstInterface);
#else
			internal static NativeBool criAtomInstrument_RegisterInstrumentInterface(IntPtr ainstInterface){return default(NativeBool);}
		internal static void criAtomInstrument_UnregisterInstrumentInterface(IntPtr ainstInterface){}
#endif
		}
	}
	public partial struct CriAtomExCategory
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_SetVolumeById(UInt32 id, Single volume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomExCategory_GetVolumeById(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomExCategory_GetTotalVolumeById(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_SetVolumeByName(IntPtr name, Single volume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomExCategory_GetVolumeByName(IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomExCategory_GetTotalVolumeByName(IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_MuteById(UInt32 id, NativeBool mute);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExCategory_IsMutedById(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_MuteByName(IntPtr name, NativeBool mute);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExCategory_IsMutedByName(IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_SoloById(UInt32 id, NativeBool solo, Single muteVolume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExCategory_IsSoloedById(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_SoloByName(IntPtr name, NativeBool solo, Single muteVolume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExCategory_IsSoloedByName(IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_PauseById(UInt32 id, NativeBool sw);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExCategory_IsPausedById(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_PauseByName(IntPtr name, NativeBool sw);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExCategory_IsPausedByName(IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_SetFadeInTimeById(UInt32 id, UInt16 ms);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_SetFadeInTimeByName(IntPtr name, UInt16 ms);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_SetFadeOutTimeById(UInt32 id, UInt16 ms);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_SetFadeOutTimeByName(IntPtr name, UInt16 ms);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_SetAisacControlById(UInt32 id, UInt32 controlId, Single controlValue);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_SetAisacControlByName(IntPtr name, IntPtr controlName, Single controlValue);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExCategory_ResetAllAisacControlById(UInt32 categoryId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExCategory_ResetAllAisacControlByName(IntPtr categoryName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_AttachAisacById(UInt32 id, IntPtr globalAisacName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_AttachAisacByName(IntPtr name, IntPtr globalAisacName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_DetachAisacById(UInt32 id, IntPtr globalAisacName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_DetachAisacByName(IntPtr name, IntPtr globalAisacName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_DetachAisacAllById(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_DetachAisacAllByName(IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExCategory_GetNumAttachedAisacsById(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExCategory_GetNumAttachedAisacsByName(IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExCategory_GetAttachedAisacInfoById(UInt32 id, Int32 aisacAttachedIndex, CriAtomEx.AisacInfo* aisacInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExCategory_GetAttachedAisacInfoByName(IntPtr name, Int32 aisacAttachedIndex, CriAtomEx.AisacInfo* aisacInfo);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExCategory_GetCurrentAisacControlValueById(UInt32 categoryId, UInt32 aisacControlId, Single* controlValue);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExCategory_GetCurrentAisacControlValueByName(IntPtr categoryName, IntPtr aisacControlName, Single* controlValue);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_SetReactParameter(IntPtr reactName, CriAtomEx.ReactParameter* reactParameter);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExCategory_GetReactParameter(IntPtr reactName, CriAtomEx.ReactParameter* reactParameter);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExCategory_GetNumCuePlayingCountById(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExCategory_GetNumCuePlayingCountByName(IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_StopById(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_StopByName(IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_StopWithoutReleaseTimeById(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExCategory_StopWithoutReleaseTimeByName(IntPtr name);
#else
			internal static void criAtomExCategory_SetVolumeById(UInt32 id, Single volume){}
		internal static Single criAtomExCategory_GetVolumeById(UInt32 id){return default(Single);}
		internal static Single criAtomExCategory_GetTotalVolumeById(UInt32 id){return default(Single);}
		internal static void criAtomExCategory_SetVolumeByName(IntPtr name, Single volume){}
		internal static Single criAtomExCategory_GetVolumeByName(IntPtr name){return default(Single);}
		internal static Single criAtomExCategory_GetTotalVolumeByName(IntPtr name){return default(Single);}
		internal static void criAtomExCategory_MuteById(UInt32 id, NativeBool mute){}
		internal static NativeBool criAtomExCategory_IsMutedById(UInt32 id){return default(NativeBool);}
		internal static void criAtomExCategory_MuteByName(IntPtr name, NativeBool mute){}
		internal static NativeBool criAtomExCategory_IsMutedByName(IntPtr name){return default(NativeBool);}
		internal static void criAtomExCategory_SoloById(UInt32 id, NativeBool solo, Single muteVolume){}
		internal static NativeBool criAtomExCategory_IsSoloedById(UInt32 id){return default(NativeBool);}
		internal static void criAtomExCategory_SoloByName(IntPtr name, NativeBool solo, Single muteVolume){}
		internal static NativeBool criAtomExCategory_IsSoloedByName(IntPtr name){return default(NativeBool);}
		internal static void criAtomExCategory_PauseById(UInt32 id, NativeBool sw){}
		internal static NativeBool criAtomExCategory_IsPausedById(UInt32 id){return default(NativeBool);}
		internal static void criAtomExCategory_PauseByName(IntPtr name, NativeBool sw){}
		internal static NativeBool criAtomExCategory_IsPausedByName(IntPtr name){return default(NativeBool);}
		internal static void criAtomExCategory_SetFadeInTimeById(UInt32 id, UInt16 ms){}
		internal static void criAtomExCategory_SetFadeInTimeByName(IntPtr name, UInt16 ms){}
		internal static void criAtomExCategory_SetFadeOutTimeById(UInt32 id, UInt16 ms){}
		internal static void criAtomExCategory_SetFadeOutTimeByName(IntPtr name, UInt16 ms){}
		internal static void criAtomExCategory_SetAisacControlById(UInt32 id, UInt32 controlId, Single controlValue){}
		internal static void criAtomExCategory_SetAisacControlByName(IntPtr name, IntPtr controlName, Single controlValue){}
		internal static NativeBool criAtomExCategory_ResetAllAisacControlById(UInt32 categoryId){return default(NativeBool);}
		internal static NativeBool criAtomExCategory_ResetAllAisacControlByName(IntPtr categoryName){return default(NativeBool);}
		internal static void criAtomExCategory_AttachAisacById(UInt32 id, IntPtr globalAisacName){}
		internal static void criAtomExCategory_AttachAisacByName(IntPtr name, IntPtr globalAisacName){}
		internal static void criAtomExCategory_DetachAisacById(UInt32 id, IntPtr globalAisacName){}
		internal static void criAtomExCategory_DetachAisacByName(IntPtr name, IntPtr globalAisacName){}
		internal static void criAtomExCategory_DetachAisacAllById(UInt32 id){}
		internal static void criAtomExCategory_DetachAisacAllByName(IntPtr name){}
		internal static Int32 criAtomExCategory_GetNumAttachedAisacsById(UInt32 id){return default(Int32);}
		internal static Int32 criAtomExCategory_GetNumAttachedAisacsByName(IntPtr name){return default(Int32);}
		internal static NativeBool criAtomExCategory_GetAttachedAisacInfoById(UInt32 id, Int32 aisacAttachedIndex, CriAtomEx.AisacInfo* aisacInfo){return default(NativeBool);}
		internal static NativeBool criAtomExCategory_GetAttachedAisacInfoByName(IntPtr name, Int32 aisacAttachedIndex, CriAtomEx.AisacInfo* aisacInfo){return default(NativeBool);}
		internal static NativeBool criAtomExCategory_GetCurrentAisacControlValueById(UInt32 categoryId, UInt32 aisacControlId, Single* controlValue){return default(NativeBool);}
		internal static NativeBool criAtomExCategory_GetCurrentAisacControlValueByName(IntPtr categoryName, IntPtr aisacControlName, Single* controlValue){return default(NativeBool);}
		internal static void criAtomExCategory_SetReactParameter(IntPtr reactName, CriAtomEx.ReactParameter* reactParameter){}
		internal static NativeBool criAtomExCategory_GetReactParameter(IntPtr reactName, CriAtomEx.ReactParameter* reactParameter){return default(NativeBool);}
		internal static Int32 criAtomExCategory_GetNumCuePlayingCountById(UInt32 id){return default(Int32);}
		internal static Int32 criAtomExCategory_GetNumCuePlayingCountByName(IntPtr name){return default(Int32);}
		internal static void criAtomExCategory_StopById(UInt32 id){}
		internal static void criAtomExCategory_StopByName(IntPtr name){}
		internal static void criAtomExCategory_StopWithoutReleaseTimeById(UInt32 id){}
		internal static void criAtomExCategory_StopWithoutReleaseTimeByName(IntPtr name){}
#endif
		}
	}
	public partial struct CriAtomExPlayback
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayback_Stop(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayback_StopWithoutReleaseTime(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayback_Pause(UInt32 id, NativeBool sw);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayback_Resume(UInt32 id, CriAtomEx.ResumeMode mode);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayback_IsPaused(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern CriAtomExPlayback.Status criAtomExPlayback_GetStatus(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayback_GetFormatInfo(UInt32 id, CriAtomEx.FormatInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayback_GetSource(UInt32 id, CriAtomEx.SourceInfo* source);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExPlayback_GetAtomPlayer(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int64 criAtomExPlayback_GetTime(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int64 criAtomExPlayback_GetTimeSyncedWithAudio(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int64 criAtomExPlayback_GetTimeSyncedWithAudioMicro(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int64 criAtomExPlayback_GetSequencePosition(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayback_GetNumPlayedSamples(UInt32 id, Int64* numSamples, Int32* samplingRate);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayback_GetNumRenderedSamples(UInt32 id, Int64* numSamples, Int32* samplingRate);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayback_GetParameterFloat32(UInt32 playbackId, CriAtomEx.ParameterId parameterId, Single* valueFloat32);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayback_GetParameterUint32(UInt32 playbackId, CriAtomEx.ParameterId parameterId, UInt32* valueUint32);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayback_GetParameterSint32(UInt32 playbackId, CriAtomEx.ParameterId parameterId, Int32* valueSint32);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayback_GetAisacControlById(UInt32 playbackId, UInt32 controlId, Single* controlValue);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayback_GetAisacControlByName(UInt32 playbackId, IntPtr controlName, Single* controlValue);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExPlayback_SetNextBlockIndex(UInt32 id, Int32 index);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExPlayback_GetCurrentBlockIndex(UInt32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayback_GetPlaybackTrackInfo(UInt32 id, CriAtomExPlayback.TrackInfo* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayback_GetBeatSyncInfo(UInt32 id, CriAtomExBeatSync.Info* info);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExPlayback_SetBeatSyncOffset(UInt32 id, Int16 timeMs);
#else
			internal static void criAtomExPlayback_Stop(UInt32 id){}
		internal static void criAtomExPlayback_StopWithoutReleaseTime(UInt32 id){}
		internal static void criAtomExPlayback_Pause(UInt32 id, NativeBool sw){}
		internal static void criAtomExPlayback_Resume(UInt32 id, CriAtomEx.ResumeMode mode){}
		internal static NativeBool criAtomExPlayback_IsPaused(UInt32 id){return default(NativeBool);}
		internal static CriAtomExPlayback.Status criAtomExPlayback_GetStatus(UInt32 id){return default(CriAtomExPlayback.Status);}
		internal static NativeBool criAtomExPlayback_GetFormatInfo(UInt32 id, CriAtomEx.FormatInfo* info){return default(NativeBool);}
		internal static NativeBool criAtomExPlayback_GetSource(UInt32 id, CriAtomEx.SourceInfo* source){return default(NativeBool);}
		internal static IntPtr criAtomExPlayback_GetAtomPlayer(UInt32 id){return default(IntPtr);}
		internal static Int64 criAtomExPlayback_GetTime(UInt32 id){return default(Int64);}
		internal static Int64 criAtomExPlayback_GetTimeSyncedWithAudio(UInt32 id){return default(Int64);}
		internal static Int64 criAtomExPlayback_GetTimeSyncedWithAudioMicro(UInt32 id){return default(Int64);}
		internal static Int64 criAtomExPlayback_GetSequencePosition(UInt32 id){return default(Int64);}
		internal static NativeBool criAtomExPlayback_GetNumPlayedSamples(UInt32 id, Int64* numSamples, Int32* samplingRate){return default(NativeBool);}
		internal static NativeBool criAtomExPlayback_GetNumRenderedSamples(UInt32 id, Int64* numSamples, Int32* samplingRate){return default(NativeBool);}
		internal static NativeBool criAtomExPlayback_GetParameterFloat32(UInt32 playbackId, CriAtomEx.ParameterId parameterId, Single* valueFloat32){return default(NativeBool);}
		internal static NativeBool criAtomExPlayback_GetParameterUint32(UInt32 playbackId, CriAtomEx.ParameterId parameterId, UInt32* valueUint32){return default(NativeBool);}
		internal static NativeBool criAtomExPlayback_GetParameterSint32(UInt32 playbackId, CriAtomEx.ParameterId parameterId, Int32* valueSint32){return default(NativeBool);}
		internal static NativeBool criAtomExPlayback_GetAisacControlById(UInt32 playbackId, UInt32 controlId, Single* controlValue){return default(NativeBool);}
		internal static NativeBool criAtomExPlayback_GetAisacControlByName(UInt32 playbackId, IntPtr controlName, Single* controlValue){return default(NativeBool);}
		internal static void criAtomExPlayback_SetNextBlockIndex(UInt32 id, Int32 index){}
		internal static Int32 criAtomExPlayback_GetCurrentBlockIndex(UInt32 id){return default(Int32);}
		internal static NativeBool criAtomExPlayback_GetPlaybackTrackInfo(UInt32 id, CriAtomExPlayback.TrackInfo* info){return default(NativeBool);}
		internal static NativeBool criAtomExPlayback_GetBeatSyncInfo(UInt32 id, CriAtomExBeatSync.Info* info){return default(NativeBool);}
		internal static NativeBool criAtomExPlayback_SetBeatSyncOffset(UInt32 id, Int16 timeMs){return default(NativeBool);}
#endif
		}
	}
	public partial class CriAtomExBeatSync
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExBeatSync_SetCallback(IntPtr func, IntPtr obj);
#else
			internal static void criAtomExBeatSync_SetCallback(IntPtr func, IntPtr obj){}
#endif
		}
	}
	public partial class CriAtomEx3dSource
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetDefaultConfigForRandomPosition_(CriAtomEx3dSource.RandomPositionConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetMinMaxDistance_(IntPtr ex3dSource, Single minAttenuationDistance, Single maxAttenuationDistance);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_ChangeDefaultMinMaxDistance_(Single minAttenuationDistance, Single maxAttenuationDistance);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetDefaultConfig_(CriAtomEx3dSource.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomEx3dSource_CalculateWorkSize(CriAtomEx3dSource.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomEx3dSource_Create(CriAtomEx3dSource.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_Destroy(IntPtr ex3dSource);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetPosition(IntPtr ex3dSource, CriAtomEx.Vector* position);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetVelocity(IntPtr ex3dSource, CriAtomEx.Vector* velocity);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_Update(IntPtr ex3dSource);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_ResetParameters(IntPtr ex3dSource);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern CriAtomEx.Vector criAtomEx3dSource_GetPosition(IntPtr ex3dSource);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetOrientation(IntPtr ex3dSource, CriAtomEx.Vector* front, CriAtomEx.Vector* top);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetConeParameter(IntPtr ex3dSource, Single insideAngle, Single outsideAngle, Single outsideVolume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_ChangeDefaultConeParameter(Single insideAngle, Single outsideAngle, Single outsideVolume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetMinMaxAttenuationDistance(IntPtr ex3dSource, Single minAttenuationDistance, Single maxAttenuationDistance);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_ChangeDefaultMinMaxAttenuationDistance(Single minAttenuationDistance, Single maxAttenuationDistance);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetInteriorPanField(IntPtr ex3dSource, Single sourceRadius, Single interiorDistance);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_ChangeDefaultInteriorPanField(Single sourceRadius, Single interiorDistance);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetDopplerFactor(IntPtr ex3dSource, Single dopplerFactor);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_ChangeDefaultDopplerFactor(Single dopplerFactor);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetVolume(IntPtr ex3dSource, Single volume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_ChangeDefaultVolume(Single volume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetMaxAngleAisacDelta(IntPtr ex3dSource, Single maxDelta);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetDistanceAisacControlId(IntPtr ex3dSource, UInt32 aisacControlId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetListenerBasedAzimuthAngleAisacControlId(IntPtr ex3dSource, UInt32 aisacControlId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetListenerBasedElevationAngleAisacControlId(IntPtr ex3dSource, UInt32 aisacControlId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetSourceBasedAzimuthAngleAisacControlId(IntPtr ex3dSource, UInt32 aisacControlId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetSourceBasedElevationAngleAisacControlId(IntPtr ex3dSource, UInt32 aisacControlId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_Set3dRegionHn(IntPtr ex3dSource, IntPtr ex3dRegion);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetRandomPositionConfig(IntPtr ex3dSource, CriAtomEx3dSource.RandomPositionConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetRandomPositionCalculationCallback(IntPtr ex3dSource, IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetRandomPositionList(IntPtr ex3dSource, CriAtomEx.Vector* positionList, UInt32 length);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSource_SetRandomPositionResultCallback(IntPtr ex3dSource, IntPtr func, IntPtr obj);
#else
			internal static void criAtomEx3dSource_SetDefaultConfigForRandomPosition_(CriAtomEx3dSource.RandomPositionConfig* pConfig){}
		internal static void criAtomEx3dSource_SetMinMaxDistance_(IntPtr ex3dSource, Single minAttenuationDistance, Single maxAttenuationDistance){}
		internal static void criAtomEx3dSource_ChangeDefaultMinMaxDistance_(Single minAttenuationDistance, Single maxAttenuationDistance){}
		internal static void criAtomEx3dSource_SetDefaultConfig_(CriAtomEx3dSource.Config* pConfig){}
		internal static Int32 criAtomEx3dSource_CalculateWorkSize(CriAtomEx3dSource.Config* config){return default(Int32);}
		internal static IntPtr criAtomEx3dSource_Create(CriAtomEx3dSource.Config* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static void criAtomEx3dSource_Destroy(IntPtr ex3dSource){}
		internal static void criAtomEx3dSource_SetPosition(IntPtr ex3dSource, CriAtomEx.Vector* position){}
		internal static void criAtomEx3dSource_SetVelocity(IntPtr ex3dSource, CriAtomEx.Vector* velocity){}
		internal static void criAtomEx3dSource_Update(IntPtr ex3dSource){}
		internal static void criAtomEx3dSource_ResetParameters(IntPtr ex3dSource){}
		internal static CriAtomEx.Vector criAtomEx3dSource_GetPosition(IntPtr ex3dSource){return default(CriAtomEx.Vector);}
		internal static void criAtomEx3dSource_SetOrientation(IntPtr ex3dSource, CriAtomEx.Vector* front, CriAtomEx.Vector* top){}
		internal static void criAtomEx3dSource_SetConeParameter(IntPtr ex3dSource, Single insideAngle, Single outsideAngle, Single outsideVolume){}
		internal static void criAtomEx3dSource_ChangeDefaultConeParameter(Single insideAngle, Single outsideAngle, Single outsideVolume){}
		internal static void criAtomEx3dSource_SetMinMaxAttenuationDistance(IntPtr ex3dSource, Single minAttenuationDistance, Single maxAttenuationDistance){}
		internal static void criAtomEx3dSource_ChangeDefaultMinMaxAttenuationDistance(Single minAttenuationDistance, Single maxAttenuationDistance){}
		internal static void criAtomEx3dSource_SetInteriorPanField(IntPtr ex3dSource, Single sourceRadius, Single interiorDistance){}
		internal static void criAtomEx3dSource_ChangeDefaultInteriorPanField(Single sourceRadius, Single interiorDistance){}
		internal static void criAtomEx3dSource_SetDopplerFactor(IntPtr ex3dSource, Single dopplerFactor){}
		internal static void criAtomEx3dSource_ChangeDefaultDopplerFactor(Single dopplerFactor){}
		internal static void criAtomEx3dSource_SetVolume(IntPtr ex3dSource, Single volume){}
		internal static void criAtomEx3dSource_ChangeDefaultVolume(Single volume){}
		internal static void criAtomEx3dSource_SetMaxAngleAisacDelta(IntPtr ex3dSource, Single maxDelta){}
		internal static void criAtomEx3dSource_SetDistanceAisacControlId(IntPtr ex3dSource, UInt32 aisacControlId){}
		internal static void criAtomEx3dSource_SetListenerBasedAzimuthAngleAisacControlId(IntPtr ex3dSource, UInt32 aisacControlId){}
		internal static void criAtomEx3dSource_SetListenerBasedElevationAngleAisacControlId(IntPtr ex3dSource, UInt32 aisacControlId){}
		internal static void criAtomEx3dSource_SetSourceBasedAzimuthAngleAisacControlId(IntPtr ex3dSource, UInt32 aisacControlId){}
		internal static void criAtomEx3dSource_SetSourceBasedElevationAngleAisacControlId(IntPtr ex3dSource, UInt32 aisacControlId){}
		internal static void criAtomEx3dSource_Set3dRegionHn(IntPtr ex3dSource, IntPtr ex3dRegion){}
		internal static void criAtomEx3dSource_SetRandomPositionConfig(IntPtr ex3dSource, CriAtomEx3dSource.RandomPositionConfig* config){}
		internal static void criAtomEx3dSource_SetRandomPositionCalculationCallback(IntPtr ex3dSource, IntPtr func, IntPtr obj){}
		internal static void criAtomEx3dSource_SetRandomPositionList(IntPtr ex3dSource, CriAtomEx.Vector* positionList, UInt32 length){}
		internal static void criAtomEx3dSource_SetRandomPositionResultCallback(IntPtr ex3dSource, IntPtr func, IntPtr obj){}
#endif
		}
	}
	public partial class CriAtomEx3dRegion
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dRegion_SetDefaultConfig_(CriAtomEx3dRegion.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomEx3dRegion_CalculateWorkSize(CriAtomEx3dRegion.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomEx3dRegion_Create(CriAtomEx3dRegion.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dRegion_Destroy(IntPtr ex3dRegion);
#else
			internal static void criAtomEx3dRegion_SetDefaultConfig_(CriAtomEx3dRegion.Config* pConfig){}
		internal static Int32 criAtomEx3dRegion_CalculateWorkSize(CriAtomEx3dRegion.Config* config){return default(Int32);}
		internal static IntPtr criAtomEx3dRegion_Create(CriAtomEx3dRegion.Config* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static void criAtomEx3dRegion_Destroy(IntPtr ex3dRegion){}
#endif
		}
	}
	public partial class CriAtomExVoicePool
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_SetDefaultConfigForAdxVoicePool_(CriAtomEx.AdxVoicePoolConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_SetDefaultConfigForAiffVoicePool_(CriAtomEx.AiffVoicePoolConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_SetDefaultConfigForDspPitchShifter_(CriAtomEx.DspPitchShifterConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_SetDefaultConfigForDspTimeStretch_(CriAtomEx.DspTimeStretchConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_SetDefaultConfigForHcaMxVoicePool_(CriAtomExHcaMx.VoicePoolConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_SetDefaultConfigForHcaVoicePool_(CriAtomEx.HcaVoicePoolConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_SetDefaultConfigForInstrumentVoicePool_(CriAtomEx.InstrumentVoicePoolConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_SetDefaultConfigForRawPcmVoicePool_(CriAtomEx.RawPcmVoicePoolConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_SetDefaultConfigForWaveVoicePool_(CriAtomEx.WaveVoicePoolConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_SetDefaultConfigForStandardVoicePool_(CriAtomEx.StandardVoicePoolConfig* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_EnumerateVoicePools(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExVoicePool_CalculateWorkSizeForStandardVoicePool(CriAtomEx.StandardVoicePoolConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExVoicePool_AllocateStandardVoicePool(CriAtomEx.StandardVoicePoolConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExVoicePool_CalculateWorkSizeForAdxVoicePool(CriAtomEx.AdxVoicePoolConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExVoicePool_AllocateAdxVoicePool(CriAtomEx.AdxVoicePoolConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExVoicePool_CalculateWorkSizeForHcaVoicePool(CriAtomEx.HcaVoicePoolConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExVoicePool_AllocateHcaVoicePool(CriAtomEx.HcaVoicePoolConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExVoicePool_CalculateWorkSizeForHcaMxVoicePool(CriAtomExHcaMx.VoicePoolConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExVoicePool_AllocateHcaMxVoicePool(CriAtomExHcaMx.VoicePoolConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExVoicePool_CalculateWorkSizeForWaveVoicePool(CriAtomEx.WaveVoicePoolConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExVoicePool_AllocateWaveVoicePool(CriAtomEx.WaveVoicePoolConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExVoicePool_CalculateWorkSizeForAiffVoicePool(CriAtomEx.AiffVoicePoolConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExVoicePool_AllocateAiffVoicePool(CriAtomEx.AiffVoicePoolConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExVoicePool_CalculateWorkSizeForRawPcmVoicePool(CriAtomEx.RawPcmVoicePoolConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExVoicePool_AllocateRawPcmVoicePool(CriAtomEx.RawPcmVoicePoolConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExVoicePool_AllocateInstrumentVoicePool(CriAtomEx.InstrumentVoicePoolConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_Free(IntPtr pool);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_FreeAll();
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_GetNumUsedVoices(IntPtr pool, Int32* curNum, Int32* limit);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExVoicePool_GetPlayerHandle(IntPtr pool, Int32 index);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExVoicePool_CalculateWorkSizeForInstrumentVoicePool(CriAtomEx.InstrumentVoicePoolConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_DetachDsp(IntPtr pool);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExVoicePool_CalculateWorkSizeForDspPitchShifter(CriAtomEx.DspPitchShifterConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_AttachDspPitchShifter(IntPtr pool, CriAtomEx.DspPitchShifterConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExVoicePool_CalculateWorkSizeForDspTimeStretch(CriAtomEx.DspTimeStretchConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_AttachDspTimeStretch(IntPtr pool, CriAtomEx.DspTimeStretchConfig* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExVoicePool_CalculateWorkSizeForDspAfx(CriAtomEx.DspAfxConfig* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExVoicePool_AttachDspAfx(IntPtr pool, CriAtomEx.DspAfxConfig* config, IntPtr work, Int32 workSize);
#else
			internal static void criAtomExVoicePool_SetDefaultConfigForAdxVoicePool_(CriAtomEx.AdxVoicePoolConfig* pConfig){}
		internal static void criAtomExVoicePool_SetDefaultConfigForAiffVoicePool_(CriAtomEx.AiffVoicePoolConfig* pConfig){}
		internal static void criAtomExVoicePool_SetDefaultConfigForDspPitchShifter_(CriAtomEx.DspPitchShifterConfig* pConfig){}
		internal static void criAtomExVoicePool_SetDefaultConfigForDspTimeStretch_(CriAtomEx.DspTimeStretchConfig* pConfig){}
		internal static void criAtomExVoicePool_SetDefaultConfigForHcaMxVoicePool_(CriAtomExHcaMx.VoicePoolConfig* pConfig){}
		internal static void criAtomExVoicePool_SetDefaultConfigForHcaVoicePool_(CriAtomEx.HcaVoicePoolConfig* pConfig){}
		internal static void criAtomExVoicePool_SetDefaultConfigForInstrumentVoicePool_(CriAtomEx.InstrumentVoicePoolConfig* pConfig){}
		internal static void criAtomExVoicePool_SetDefaultConfigForRawPcmVoicePool_(CriAtomEx.RawPcmVoicePoolConfig* pConfig){}
		internal static void criAtomExVoicePool_SetDefaultConfigForWaveVoicePool_(CriAtomEx.WaveVoicePoolConfig* pConfig){}
		internal static void criAtomExVoicePool_SetDefaultConfigForStandardVoicePool_(CriAtomEx.StandardVoicePoolConfig* pConfig){}
		internal static void criAtomExVoicePool_EnumerateVoicePools(IntPtr func, IntPtr obj){}
		internal static Int32 criAtomExVoicePool_CalculateWorkSizeForStandardVoicePool(CriAtomEx.StandardVoicePoolConfig* config){return default(Int32);}
		internal static IntPtr criAtomExVoicePool_AllocateStandardVoicePool(CriAtomEx.StandardVoicePoolConfig* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static Int32 criAtomExVoicePool_CalculateWorkSizeForAdxVoicePool(CriAtomEx.AdxVoicePoolConfig* config){return default(Int32);}
		internal static IntPtr criAtomExVoicePool_AllocateAdxVoicePool(CriAtomEx.AdxVoicePoolConfig* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static Int32 criAtomExVoicePool_CalculateWorkSizeForHcaVoicePool(CriAtomEx.HcaVoicePoolConfig* config){return default(Int32);}
		internal static IntPtr criAtomExVoicePool_AllocateHcaVoicePool(CriAtomEx.HcaVoicePoolConfig* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static Int32 criAtomExVoicePool_CalculateWorkSizeForHcaMxVoicePool(CriAtomExHcaMx.VoicePoolConfig* config){return default(Int32);}
		internal static IntPtr criAtomExVoicePool_AllocateHcaMxVoicePool(CriAtomExHcaMx.VoicePoolConfig* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static Int32 criAtomExVoicePool_CalculateWorkSizeForWaveVoicePool(CriAtomEx.WaveVoicePoolConfig* config){return default(Int32);}
		internal static IntPtr criAtomExVoicePool_AllocateWaveVoicePool(CriAtomEx.WaveVoicePoolConfig* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static Int32 criAtomExVoicePool_CalculateWorkSizeForAiffVoicePool(CriAtomEx.AiffVoicePoolConfig* config){return default(Int32);}
		internal static IntPtr criAtomExVoicePool_AllocateAiffVoicePool(CriAtomEx.AiffVoicePoolConfig* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static Int32 criAtomExVoicePool_CalculateWorkSizeForRawPcmVoicePool(CriAtomEx.RawPcmVoicePoolConfig* config){return default(Int32);}
		internal static IntPtr criAtomExVoicePool_AllocateRawPcmVoicePool(CriAtomEx.RawPcmVoicePoolConfig* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static IntPtr criAtomExVoicePool_AllocateInstrumentVoicePool(CriAtomEx.InstrumentVoicePoolConfig* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static void criAtomExVoicePool_Free(IntPtr pool){}
		internal static void criAtomExVoicePool_FreeAll(){}
		internal static void criAtomExVoicePool_GetNumUsedVoices(IntPtr pool, Int32* curNum, Int32* limit){}
		internal static IntPtr criAtomExVoicePool_GetPlayerHandle(IntPtr pool, Int32 index){return default(IntPtr);}
		internal static Int32 criAtomExVoicePool_CalculateWorkSizeForInstrumentVoicePool(CriAtomEx.InstrumentVoicePoolConfig* config){return default(Int32);}
		internal static void criAtomExVoicePool_DetachDsp(IntPtr pool){}
		internal static Int32 criAtomExVoicePool_CalculateWorkSizeForDspPitchShifter(CriAtomEx.DspPitchShifterConfig* config){return default(Int32);}
		internal static void criAtomExVoicePool_AttachDspPitchShifter(IntPtr pool, CriAtomEx.DspPitchShifterConfig* config, IntPtr work, Int32 workSize){}
		internal static Int32 criAtomExVoicePool_CalculateWorkSizeForDspTimeStretch(CriAtomEx.DspTimeStretchConfig* config){return default(Int32);}
		internal static void criAtomExVoicePool_AttachDspTimeStretch(IntPtr pool, CriAtomEx.DspTimeStretchConfig* config, IntPtr work, Int32 workSize){}
		internal static Int32 criAtomExVoicePool_CalculateWorkSizeForDspAfx(CriAtomEx.DspAfxConfig* config){return default(Int32);}
		internal static void criAtomExVoicePool_AttachDspAfx(IntPtr pool, CriAtomEx.DspAfxConfig* config, IntPtr work, Int32 workSize){}
#endif
		}
	}
	public partial class CriAtomExSequencer
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExSequencer_SetEventCallback(IntPtr func, IntPtr obj);
#else
			internal static void criAtomExSequencer_SetEventCallback(IntPtr func, IntPtr obj){}
#endif
		}
	}
	public partial class CriAtomExOutputPort
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExOutputPort_IsDestroyable(IntPtr outputPort);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExOutputPort_SetDefaultConfig_(CriAtomExOutputPort.Config* pConfig, IntPtr outputportName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExOutputPort_CalculateWorkSize(CriAtomExOutputPort.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExOutputPort_Create(CriAtomExOutputPort.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExOutputPort_Destroy(IntPtr outputPort);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExOutputPort_SetAsrRackId(IntPtr outputPort, Int32 rackId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExOutputPort_SetVibrationChannelLevel(IntPtr outputPort, Int32 channel, Single level);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExOutputPort_SetMonauralMix(IntPtr outputPort, NativeBool monauralMix);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExOutputPort_IgnoreCategoryParametersById(IntPtr outputPortHn, UInt32 categoryId, NativeBool ignoreParameters);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExOutputPort_ResetIgnoreCategory(IntPtr outputPortHn);
#else
			internal static NativeBool criAtomExOutputPort_IsDestroyable(IntPtr outputPort){return default(NativeBool);}
		internal static void criAtomExOutputPort_SetDefaultConfig_(CriAtomExOutputPort.Config* pConfig, IntPtr outputportName){}
		internal static Int32 criAtomExOutputPort_CalculateWorkSize(CriAtomExOutputPort.Config* config){return default(Int32);}
		internal static IntPtr criAtomExOutputPort_Create(CriAtomExOutputPort.Config* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static void criAtomExOutputPort_Destroy(IntPtr outputPort){}
		internal static void criAtomExOutputPort_SetAsrRackId(IntPtr outputPort, Int32 rackId){}
		internal static void criAtomExOutputPort_SetVibrationChannelLevel(IntPtr outputPort, Int32 channel, Single level){}
		internal static void criAtomExOutputPort_SetMonauralMix(IntPtr outputPort, NativeBool monauralMix){}
		internal static void criAtomExOutputPort_IgnoreCategoryParametersById(IntPtr outputPortHn, UInt32 categoryId, NativeBool ignoreParameters){}
		internal static void criAtomExOutputPort_ResetIgnoreCategory(IntPtr outputPortHn){}
#endif
		}
	}
	public partial class CriAtomEx3dListener
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dListener_SetDefaultConfig_(CriAtomEx3dListener.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomEx3dListener_CalculateWorkSize(CriAtomEx3dListener.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomEx3dListener_Create(CriAtomEx3dListener.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dListener_Destroy(IntPtr ex3dListener);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dListener_SetPosition(IntPtr ex3dListener, CriAtomEx.Vector* position);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dListener_SetVelocity(IntPtr ex3dListener, CriAtomEx.Vector* velocity);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dListener_Update(IntPtr ex3dListener);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dListener_ResetParameters(IntPtr ex3dListener);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern CriAtomEx.Vector criAtomEx3dListener_GetPosition(IntPtr ex3dListener);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dListener_SetOrientation(IntPtr ex3dListener, CriAtomEx.Vector* front, CriAtomEx.Vector* top);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dListener_SetDopplerMultiplier(IntPtr ex3dListener, Single dopplerMultiplier);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dListener_SetFocusPoint(IntPtr ex3dListener, CriAtomEx.Vector* focusPoint);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dListener_SetDistanceFocusLevel(IntPtr ex3dListener, Single distanceFocusLevel);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dListener_SetDirectionFocusLevel(IntPtr ex3dListener, Single directionFocusLevel);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dListener_GetFocusPoint(IntPtr ex3dListener, CriAtomEx.Vector* focusPoint);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomEx3dListener_GetDistanceFocusLevel(IntPtr ex3dListener);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Single criAtomEx3dListener_GetDirectionFocusLevel(IntPtr ex3dListener);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dListener_Set3dRegionHn(IntPtr ex3dListener, IntPtr ex3dRegion);
#else
			internal static void criAtomEx3dListener_SetDefaultConfig_(CriAtomEx3dListener.Config* pConfig){}
		internal static Int32 criAtomEx3dListener_CalculateWorkSize(CriAtomEx3dListener.Config* config){return default(Int32);}
		internal static IntPtr criAtomEx3dListener_Create(CriAtomEx3dListener.Config* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static void criAtomEx3dListener_Destroy(IntPtr ex3dListener){}
		internal static void criAtomEx3dListener_SetPosition(IntPtr ex3dListener, CriAtomEx.Vector* position){}
		internal static void criAtomEx3dListener_SetVelocity(IntPtr ex3dListener, CriAtomEx.Vector* velocity){}
		internal static void criAtomEx3dListener_Update(IntPtr ex3dListener){}
		internal static void criAtomEx3dListener_ResetParameters(IntPtr ex3dListener){}
		internal static CriAtomEx.Vector criAtomEx3dListener_GetPosition(IntPtr ex3dListener){return default(CriAtomEx.Vector);}
		internal static void criAtomEx3dListener_SetOrientation(IntPtr ex3dListener, CriAtomEx.Vector* front, CriAtomEx.Vector* top){}
		internal static void criAtomEx3dListener_SetDopplerMultiplier(IntPtr ex3dListener, Single dopplerMultiplier){}
		internal static void criAtomEx3dListener_SetFocusPoint(IntPtr ex3dListener, CriAtomEx.Vector* focusPoint){}
		internal static void criAtomEx3dListener_SetDistanceFocusLevel(IntPtr ex3dListener, Single distanceFocusLevel){}
		internal static void criAtomEx3dListener_SetDirectionFocusLevel(IntPtr ex3dListener, Single directionFocusLevel){}
		internal static void criAtomEx3dListener_GetFocusPoint(IntPtr ex3dListener, CriAtomEx.Vector* focusPoint){}
		internal static Single criAtomEx3dListener_GetDistanceFocusLevel(IntPtr ex3dListener){return default(Single);}
		internal static Single criAtomEx3dListener_GetDirectionFocusLevel(IntPtr ex3dListener){return default(Single);}
		internal static void criAtomEx3dListener_Set3dRegionHn(IntPtr ex3dListener, IntPtr ex3dRegion){}
#endif
		}
	}
	public partial class CriAtomExSoundObject
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExSoundObject_SetDefaultConfig_(CriAtomExSoundObject.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExSoundObject_CalculateWorkSize(CriAtomExSoundObject.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExSoundObject_Create(CriAtomExSoundObject.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExSoundObject_Destroy(IntPtr soundObject);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExSoundObject_AddPlayer(IntPtr soundObject, IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExSoundObject_DeletePlayer(IntPtr soundObject, IntPtr player);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExSoundObject_DeleteAllPlayers(IntPtr soundObject);
#else
			internal static void criAtomExSoundObject_SetDefaultConfig_(CriAtomExSoundObject.Config* pConfig){}
		internal static Int32 criAtomExSoundObject_CalculateWorkSize(CriAtomExSoundObject.Config* config){return default(Int32);}
		internal static IntPtr criAtomExSoundObject_Create(CriAtomExSoundObject.Config* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static void criAtomExSoundObject_Destroy(IntPtr soundObject){}
		internal static void criAtomExSoundObject_AddPlayer(IntPtr soundObject, IntPtr player){}
		internal static void criAtomExSoundObject_DeletePlayer(IntPtr soundObject, IntPtr player){}
		internal static void criAtomExSoundObject_DeleteAllPlayers(IntPtr soundObject){}
#endif
		}
	}
	public partial class CriAtomStreamingCache
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomStreamingCache_SetDefaultConfig_(CriAtomStreamingCache.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomStreamingCache_CalculateWorkSize(CriAtomStreamingCache.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomStreamingCache_Create(CriAtomStreamingCache.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomStreamingCache_Destroy(IntPtr stmCacheId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomStreamingCache_Clear(IntPtr cacheId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomStreamingCache_IsCachedWaveId(IntPtr stmCacheId, IntPtr awb, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomStreamingCache_IsCachedFile(IntPtr stmCacheId, IntPtr srcBinder, IntPtr path);
#else
			internal static void criAtomStreamingCache_SetDefaultConfig_(CriAtomStreamingCache.Config* pConfig){}
		internal static Int32 criAtomStreamingCache_CalculateWorkSize(CriAtomStreamingCache.Config* config){return default(Int32);}
		internal static IntPtr criAtomStreamingCache_Create(CriAtomStreamingCache.Config* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static void criAtomStreamingCache_Destroy(IntPtr stmCacheId){}
		internal static void criAtomStreamingCache_Clear(IntPtr cacheId){}
		internal static NativeBool criAtomStreamingCache_IsCachedWaveId(IntPtr stmCacheId, IntPtr awb, Int32 id){return default(NativeBool);}
		internal static NativeBool criAtomStreamingCache_IsCachedFile(IntPtr stmCacheId, IntPtr srcBinder, IntPtr path){return default(NativeBool);}
#endif
		}
	}
	public partial class CriAtomEx3dSourceList
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSourceList_SetDefaultConfig_(CriAtomEx3dSourceList.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomEx3dSourceList_CalculateWorkSize(CriAtomEx3dSourceList.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomEx3dSourceList_Create(CriAtomEx3dSourceList.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSourceList_Destroy(IntPtr ex3dSourceList);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSourceList_Add(IntPtr ex3dSourceList, IntPtr ex3dSource);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSourceList_Remove(IntPtr ex3dSourceList, IntPtr ex3dSource);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dSourceList_RemoveAll(IntPtr ex3dSourceList);
#else
			internal static void criAtomEx3dSourceList_SetDefaultConfig_(CriAtomEx3dSourceList.Config* pConfig){}
		internal static Int32 criAtomEx3dSourceList_CalculateWorkSize(CriAtomEx3dSourceList.Config* config){return default(Int32);}
		internal static IntPtr criAtomEx3dSourceList_Create(CriAtomEx3dSourceList.Config* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static void criAtomEx3dSourceList_Destroy(IntPtr ex3dSourceList){}
		internal static void criAtomEx3dSourceList_Add(IntPtr ex3dSourceList, IntPtr ex3dSource){}
		internal static void criAtomEx3dSourceList_Remove(IntPtr ex3dSourceList, IntPtr ex3dSource){}
		internal static void criAtomEx3dSourceList_RemoveAll(IntPtr ex3dSourceList){}
#endif
		}
	}
	public partial class CriAtomEx3dTransceiver
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetDefaultConfig_(CriAtomEx3dTransceiver.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomEx3dTransceiver_CalculateWorkSize(CriAtomEx3dTransceiver.Config* config);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomEx3dTransceiver_Create(CriAtomEx3dTransceiver.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_Destroy(IntPtr ex3dTransceiver);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetInputPosition(IntPtr ex3dTransceiver, CriAtomEx.Vector* position);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetOutputPosition(IntPtr ex3dTransceiver, CriAtomEx.Vector* position);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_Update(IntPtr ex3dTransceiver);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetInputOrientation(IntPtr ex3dTransceiver, CriAtomEx.Vector* front, CriAtomEx.Vector* top);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetOutputOrientation(IntPtr ex3dTransceiver, CriAtomEx.Vector* front, CriAtomEx.Vector* top);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetOutputConeParameter(IntPtr ex3dTransceiver, Single insideAngle, Single outsideAngle, Single outsideVolume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetOutputMinMaxAttenuationDistance(IntPtr ex3dTransceiver, Single minAttenuationDistance, Single maxAttenuationDistance);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetOutputInteriorPanField(IntPtr ex3dTransceiver, Single transceiverRadius, Single interiorDistance);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetInputCrossFadeField(IntPtr ex3dTransceiver, Single directAudioRadius, Single crossfadeDistance);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetOutputVolume(IntPtr ex3dTransceiver, Single volume);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_AttachAisac(IntPtr ex3dTransceiver, IntPtr globalAisacName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_DetachAisac(IntPtr ex3dTransceiver, IntPtr globalAisacName);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetMaxAngleAisacDelta(IntPtr ex3dTransceiver, Single maxDelta);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetDistanceAisacControlId(IntPtr ex3dTransceiver, UInt32 aisacControlId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetListenerBasedAzimuthAngleAisacControlId(IntPtr ex3dTransceiver, UInt32 aisacControlId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetListenerBasedElevationAngleAisacControlId(IntPtr ex3dTransceiver, UInt32 aisacControlId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetTransceiverOutputBasedAzimuthAngleAisacControlId(IntPtr ex3dTransceiver, UInt32 aisacControlId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_SetTransceiverOutputBasedElevationAngleAisacControlId(IntPtr ex3dTransceiver, UInt32 aisacControlId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomEx3dTransceiver_Set3dRegionHn(IntPtr ex3dTransceiver, IntPtr ex3dRegion);
#else
			internal static void criAtomEx3dTransceiver_SetDefaultConfig_(CriAtomEx3dTransceiver.Config* pConfig){}
		internal static Int32 criAtomEx3dTransceiver_CalculateWorkSize(CriAtomEx3dTransceiver.Config* config){return default(Int32);}
		internal static IntPtr criAtomEx3dTransceiver_Create(CriAtomEx3dTransceiver.Config* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static void criAtomEx3dTransceiver_Destroy(IntPtr ex3dTransceiver){}
		internal static void criAtomEx3dTransceiver_SetInputPosition(IntPtr ex3dTransceiver, CriAtomEx.Vector* position){}
		internal static void criAtomEx3dTransceiver_SetOutputPosition(IntPtr ex3dTransceiver, CriAtomEx.Vector* position){}
		internal static void criAtomEx3dTransceiver_Update(IntPtr ex3dTransceiver){}
		internal static void criAtomEx3dTransceiver_SetInputOrientation(IntPtr ex3dTransceiver, CriAtomEx.Vector* front, CriAtomEx.Vector* top){}
		internal static void criAtomEx3dTransceiver_SetOutputOrientation(IntPtr ex3dTransceiver, CriAtomEx.Vector* front, CriAtomEx.Vector* top){}
		internal static void criAtomEx3dTransceiver_SetOutputConeParameter(IntPtr ex3dTransceiver, Single insideAngle, Single outsideAngle, Single outsideVolume){}
		internal static void criAtomEx3dTransceiver_SetOutputMinMaxAttenuationDistance(IntPtr ex3dTransceiver, Single minAttenuationDistance, Single maxAttenuationDistance){}
		internal static void criAtomEx3dTransceiver_SetOutputInteriorPanField(IntPtr ex3dTransceiver, Single transceiverRadius, Single interiorDistance){}
		internal static void criAtomEx3dTransceiver_SetInputCrossFadeField(IntPtr ex3dTransceiver, Single directAudioRadius, Single crossfadeDistance){}
		internal static void criAtomEx3dTransceiver_SetOutputVolume(IntPtr ex3dTransceiver, Single volume){}
		internal static void criAtomEx3dTransceiver_AttachAisac(IntPtr ex3dTransceiver, IntPtr globalAisacName){}
		internal static void criAtomEx3dTransceiver_DetachAisac(IntPtr ex3dTransceiver, IntPtr globalAisacName){}
		internal static void criAtomEx3dTransceiver_SetMaxAngleAisacDelta(IntPtr ex3dTransceiver, Single maxDelta){}
		internal static void criAtomEx3dTransceiver_SetDistanceAisacControlId(IntPtr ex3dTransceiver, UInt32 aisacControlId){}
		internal static void criAtomEx3dTransceiver_SetListenerBasedAzimuthAngleAisacControlId(IntPtr ex3dTransceiver, UInt32 aisacControlId){}
		internal static void criAtomEx3dTransceiver_SetListenerBasedElevationAngleAisacControlId(IntPtr ex3dTransceiver, UInt32 aisacControlId){}
		internal static void criAtomEx3dTransceiver_SetTransceiverOutputBasedAzimuthAngleAisacControlId(IntPtr ex3dTransceiver, UInt32 aisacControlId){}
		internal static void criAtomEx3dTransceiver_SetTransceiverOutputBasedElevationAngleAisacControlId(IntPtr ex3dTransceiver, UInt32 aisacControlId){}
		internal static void criAtomEx3dTransceiver_Set3dRegionHn(IntPtr ex3dTransceiver, IntPtr ex3dRegion){}
#endif
		}
	}
	public partial struct CriAtomExStreamingCache
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExStreamingCache_IsCachedWaveformById(IntPtr stmCacheId, IntPtr acbHn, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExStreamingCache_IsCachedWaveformByName(IntPtr stmCacheId, IntPtr acbHn, IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExStreamingCache_LoadWaveformById(IntPtr stmCacheId, IntPtr acbHn, Int32 cueId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExStreamingCache_LoadWaveformByName(IntPtr stmCacheId, IntPtr acbHn, IntPtr name);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExStreamingCache_SetDefaultConfig_(CriAtomExStreamingCache.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criAtomExStreamingCache_CalculateWorkSize_(CriAtomExStreamingCache.Config* pConfig);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criAtomExStreamingCache_Create_(CriAtomExStreamingCache.Config* config, IntPtr work, Int32 workSize);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExStreamingCache_Destroy_(IntPtr stmCacheId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criAtomExStreamingCache_Clear_(IntPtr cacheId);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExStreamingCache_IsCachedWaveId_(IntPtr stmCacheId, IntPtr awb, Int32 id);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern NativeBool criAtomExStreamingCache_IsCachedFile_(IntPtr stmCacheId, IntPtr srcBinder, IntPtr path);
#else
			internal static NativeBool criAtomExStreamingCache_IsCachedWaveformById(IntPtr stmCacheId, IntPtr acbHn, Int32 id){return default(NativeBool);}
		internal static NativeBool criAtomExStreamingCache_IsCachedWaveformByName(IntPtr stmCacheId, IntPtr acbHn, IntPtr name){return default(NativeBool);}
		internal static NativeBool criAtomExStreamingCache_LoadWaveformById(IntPtr stmCacheId, IntPtr acbHn, Int32 cueId){return default(NativeBool);}
		internal static NativeBool criAtomExStreamingCache_LoadWaveformByName(IntPtr stmCacheId, IntPtr acbHn, IntPtr name){return default(NativeBool);}
		internal static void criAtomExStreamingCache_SetDefaultConfig_(CriAtomExStreamingCache.Config* pConfig){}
		internal static Int32 criAtomExStreamingCache_CalculateWorkSize_(CriAtomExStreamingCache.Config* pConfig){return default(Int32);}
		internal static IntPtr criAtomExStreamingCache_Create_(CriAtomExStreamingCache.Config* config, IntPtr work, Int32 workSize){return default(IntPtr);}
		internal static void criAtomExStreamingCache_Destroy_(IntPtr stmCacheId){}
		internal static void criAtomExStreamingCache_Clear_(IntPtr cacheId){}
		internal static NativeBool criAtomExStreamingCache_IsCachedWaveId_(IntPtr stmCacheId, IntPtr awb, Int32 id){return default(NativeBool);}
		internal static NativeBool criAtomExStreamingCache_IsCachedFile_(IntPtr stmCacheId, IntPtr srcBinder, IntPtr path){return default(NativeBool);}
#endif
		}
	}
}