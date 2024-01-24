#include "pch-c.h"
#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include "codegen/il2cpp-codegen-metadata.h"





// 0x00000001 System.Boolean UnityEngine.XR.XRSettings::get_enabled()
extern void XRSettings_get_enabled_mC22ABF5BF7D835DAB861A1FA384DBB8904D15E70 (void);
// 0x00000002 System.Void UnityEngine.XR.XRDevice::DisableAutoXRCameraTracking(UnityEngine.Camera,System.Boolean)
extern void XRDevice_DisableAutoXRCameraTracking_m1243FCAD2AC9C4D5C2E551255A1B2BA266E12A52 (void);
// 0x00000003 System.Void UnityEngine.XR.XRDevice::InvokeDeviceLoaded(System.String)
extern void XRDevice_InvokeDeviceLoaded_mBE2198DE44A72E2F5059566C46B9907D82782790 (void);
static Il2CppMethodPointer s_methodPointers[3] = 
{
	XRSettings_get_enabled_mC22ABF5BF7D835DAB861A1FA384DBB8904D15E70,
	XRDevice_DisableAutoXRCameraTracking_m1243FCAD2AC9C4D5C2E551255A1B2BA266E12A52,
	XRDevice_InvokeDeviceLoaded_mBE2198DE44A72E2F5059566C46B9907D82782790,
};
static const int32_t s_InvokerIndices[3] = 
{
	12114,
	9949,
	11347,
};
IL2CPP_EXTERN_C const Il2CppCodeGenModule g_UnityEngine_VRModule_CodeGenModule;
const Il2CppCodeGenModule g_UnityEngine_VRModule_CodeGenModule = 
{
	"UnityEngine.VRModule.dll",
	3,
	s_methodPointers,
	0,
	NULL,
	s_InvokerIndices,
	0,
	NULL,
	0,
	NULL,
	0,
	NULL,
	NULL,
	NULL, // module initializer,
	NULL,
	NULL,
	NULL,
};
