//
// This file was automatically generated. Please don't edit by hand.
//

#ifndef FABRICEX_CS_HLSL
#define FABRICEX_CS_HLSL
//
// UnityEngine.Rendering.HighDefinition.FabricEx+MaterialFeatureFlags:  static fields
//
#define MATERIALFEATUREFLAGS_FABRIC_EX_COTTON_WOOL (1)
#define MATERIALFEATUREFLAGS_FABRIC_EX_SUBSURFACE_SCATTERING (2)
#define MATERIALFEATUREFLAGS_FABRIC_EX_TRANSMISSION (4)

//
// UnityEngine.Rendering.HighDefinition.FabricEx+SurfaceData:  static fields
//
#define DEBUGVIEW_FABRICEX_SURFACEDATA_MATERIAL_FEATURES (1300)
#define DEBUGVIEW_FABRICEX_SURFACEDATA_BASE_COLOR (1301)
#define DEBUGVIEW_FABRICEX_SURFACEDATA_SPECULAR_OCCLUSION (1302)
#define DEBUGVIEW_FABRICEX_SURFACEDATA_NORMAL (1303)
#define DEBUGVIEW_FABRICEX_SURFACEDATA_NORMAL_VIEW_SPACE (1304)
#define DEBUGVIEW_FABRICEX_SURFACEDATA_GEOMETRIC_NORMAL (1305)
#define DEBUGVIEW_FABRICEX_SURFACEDATA_GEOMETRIC_NORMAL_VIEW_SPACE (1306)
#define DEBUGVIEW_FABRICEX_SURFACEDATA_SMOOTHNESS (1307)
#define DEBUGVIEW_FABRICEX_SURFACEDATA_AMBIENT_OCCLUSION (1308)
#define DEBUGVIEW_FABRICEX_SURFACEDATA_SPECULAR_TINT (1309)
#define DEBUGVIEW_FABRICEX_SURFACEDATA_DIFFUSION_PROFILE_HASH (1310)
#define DEBUGVIEW_FABRICEX_SURFACEDATA_SUBSURFACE_MASK (1311)
#define DEBUGVIEW_FABRICEX_SURFACEDATA_THICKNESS (1312)
#define DEBUGVIEW_FABRICEX_SURFACEDATA_TANGENT (1313)
#define DEBUGVIEW_FABRICEX_SURFACEDATA_ANISOTROPY (1314)
#define DEBUGVIEW_FABRICEX_SURFACEDATA_SHEEN (1315)

//
// UnityEngine.Rendering.HighDefinition.FabricEx+BSDFData:  static fields
//
#define DEBUGVIEW_FABRICEX_BSDFDATA_MATERIAL_FEATURES (1350)
#define DEBUGVIEW_FABRICEX_BSDFDATA_DIFFUSE_COLOR (1351)
#define DEBUGVIEW_FABRICEX_BSDFDATA_FRESNEL0 (1352)
#define DEBUGVIEW_FABRICEX_BSDFDATA_AMBIENT_OCCLUSION (1353)
#define DEBUGVIEW_FABRICEX_BSDFDATA_SPECULAR_OCCLUSION (1354)
#define DEBUGVIEW_FABRICEX_BSDFDATA_NORMAL_WS (1355)
#define DEBUGVIEW_FABRICEX_BSDFDATA_NORMAL_VIEW_SPACE (1356)
#define DEBUGVIEW_FABRICEX_BSDFDATA_GEOMETRIC_NORMAL (1357)
#define DEBUGVIEW_FABRICEX_BSDFDATA_GEOMETRIC_NORMAL_VIEW_SPACE (1358)
#define DEBUGVIEW_FABRICEX_BSDFDATA_PERCEPTUAL_ROUGHNESS (1359)
#define DEBUGVIEW_FABRICEX_BSDFDATA_DIFFUSION_PROFILE_INDEX (1360)
#define DEBUGVIEW_FABRICEX_BSDFDATA_SUBSURFACE_MASK (1361)
#define DEBUGVIEW_FABRICEX_BSDFDATA_THICKNESS (1362)
#define DEBUGVIEW_FABRICEX_BSDFDATA_USE_THICK_OBJECT_MODE (1363)
#define DEBUGVIEW_FABRICEX_BSDFDATA_TRANSMITTANCE (1364)
#define DEBUGVIEW_FABRICEX_BSDFDATA_TANGENT_WS (1365)
#define DEBUGVIEW_FABRICEX_BSDFDATA_BITANGENT_WS (1366)
#define DEBUGVIEW_FABRICEX_BSDFDATA_ROUGHNESS_T (1367)
#define DEBUGVIEW_FABRICEX_BSDFDATA_ROUGHNESS_B (1368)
#define DEBUGVIEW_FABRICEX_BSDFDATA_ANISOTROPY (1369)
#define DEBUGVIEW_FABRICEX_BSDFDATA_SHEEN (1370)

// Generated from UnityEngine.Rendering.HighDefinition.FabricEx+SurfaceData
// PackingRules = Exact
struct SurfaceData
{
    uint materialFeatures;
    float3 baseColor;
    float specularOcclusion;
    float3 normalWS;
    float3 geomNormalWS;
    float perceptualSmoothness;
    float ambientOcclusion;
    float3 specularColor;
    uint diffusionProfileHash;
    float subsurfaceMask;
    float thickness;
    float3 tangentWS;
    float anisotropy;
    float sheen;
};

// Generated from UnityEngine.Rendering.HighDefinition.FabricEx+BSDFData
// PackingRules = Exact
struct BSDFData
{
    uint materialFeatures;
    float3 diffuseColor;
    float3 fresnel0;
    float ambientOcclusion;
    float specularOcclusion;
    float3 normalWS;
    float3 geomNormalWS;
    float perceptualRoughness;
    uint diffusionProfileIndex;
    float subsurfaceMask;
    float thickness;
    bool useThickObjectMode;
    float3 transmittance;
    float3 tangentWS;
    float3 bitangentWS;
    float roughnessT;
    float roughnessB;
    float anisotropy;
    float sheen;
};

//
// Debug functions
//
void GetGeneratedSurfaceDataDebug(uint paramId, SurfaceData surfacedata, inout float3 result, inout bool needLinearToSRGB)
{
    switch (paramId)
    {
        case DEBUGVIEW_FABRICEX_SURFACEDATA_MATERIAL_FEATURES:
            result = GetIndexColor(surfacedata.materialFeatures);
            break;
        case DEBUGVIEW_FABRICEX_SURFACEDATA_BASE_COLOR:
            result = surfacedata.baseColor;
            needLinearToSRGB = true;
            break;
        case DEBUGVIEW_FABRICEX_SURFACEDATA_SPECULAR_OCCLUSION:
            result = surfacedata.specularOcclusion.xxx;
            break;
        case DEBUGVIEW_FABRICEX_SURFACEDATA_NORMAL:
            result = surfacedata.normalWS * 0.5 + 0.5;
            break;
        case DEBUGVIEW_FABRICEX_SURFACEDATA_NORMAL_VIEW_SPACE:
            result = surfacedata.normalWS * 0.5 + 0.5;
            break;
        case DEBUGVIEW_FABRICEX_SURFACEDATA_GEOMETRIC_NORMAL:
            result = surfacedata.geomNormalWS * 0.5 + 0.5;
            break;
        case DEBUGVIEW_FABRICEX_SURFACEDATA_GEOMETRIC_NORMAL_VIEW_SPACE:
            result = surfacedata.geomNormalWS * 0.5 + 0.5;
            break;
        case DEBUGVIEW_FABRICEX_SURFACEDATA_SMOOTHNESS:
            result = surfacedata.perceptualSmoothness.xxx;
            break;
        case DEBUGVIEW_FABRICEX_SURFACEDATA_AMBIENT_OCCLUSION:
            result = surfacedata.ambientOcclusion.xxx;
            break;
        case DEBUGVIEW_FABRICEX_SURFACEDATA_SPECULAR_TINT:
            result = surfacedata.specularColor;
            needLinearToSRGB = true;
            break;
        case DEBUGVIEW_FABRICEX_SURFACEDATA_DIFFUSION_PROFILE_HASH:
            result = GetIndexColor(surfacedata.diffusionProfileHash);
            break;
        case DEBUGVIEW_FABRICEX_SURFACEDATA_SUBSURFACE_MASK:
            result = surfacedata.subsurfaceMask.xxx;
            break;
        case DEBUGVIEW_FABRICEX_SURFACEDATA_THICKNESS:
            result = surfacedata.thickness.xxx;
            break;
        case DEBUGVIEW_FABRICEX_SURFACEDATA_TANGENT:
            result = surfacedata.tangentWS * 0.5 + 0.5;
            break;
        case DEBUGVIEW_FABRICEX_SURFACEDATA_ANISOTROPY:
            result = surfacedata.anisotropy.xxx;
            break;
        case DEBUGVIEW_FABRICEX_SURFACEDATA_SHEEN:
            result = surfacedata.sheen.xxx;
            break;
    }
}

//
// Debug functions
//
void GetGeneratedBSDFDataDebug(uint paramId, BSDFData bsdfdata, inout float3 result, inout bool needLinearToSRGB)
{
    switch (paramId)
    {
        case DEBUGVIEW_FABRICEX_BSDFDATA_MATERIAL_FEATURES:
            result = GetIndexColor(bsdfdata.materialFeatures);
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_DIFFUSE_COLOR:
            result = bsdfdata.diffuseColor;
            needLinearToSRGB = true;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_FRESNEL0:
            result = bsdfdata.fresnel0;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_AMBIENT_OCCLUSION:
            result = bsdfdata.ambientOcclusion.xxx;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_SPECULAR_OCCLUSION:
            result = bsdfdata.specularOcclusion.xxx;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_NORMAL_WS:
            result = bsdfdata.normalWS * 0.5 + 0.5;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_NORMAL_VIEW_SPACE:
            result = bsdfdata.normalWS * 0.5 + 0.5;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_GEOMETRIC_NORMAL:
            result = bsdfdata.geomNormalWS * 0.5 + 0.5;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_GEOMETRIC_NORMAL_VIEW_SPACE:
            result = bsdfdata.geomNormalWS * 0.5 + 0.5;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_PERCEPTUAL_ROUGHNESS:
            result = bsdfdata.perceptualRoughness.xxx;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_DIFFUSION_PROFILE_INDEX:
            result = GetIndexColor(bsdfdata.diffusionProfileIndex);
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_SUBSURFACE_MASK:
            result = bsdfdata.subsurfaceMask.xxx;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_THICKNESS:
            result = bsdfdata.thickness.xxx;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_USE_THICK_OBJECT_MODE:
            result = (bsdfdata.useThickObjectMode) ? float3(1.0, 1.0, 1.0) : float3(0.0, 0.0, 0.0);
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_TRANSMITTANCE:
            result = bsdfdata.transmittance;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_TANGENT_WS:
            result = bsdfdata.tangentWS * 0.5 + 0.5;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_BITANGENT_WS:
            result = bsdfdata.bitangentWS * 0.5 + 0.5;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_ROUGHNESS_T:
            result = bsdfdata.roughnessT.xxx;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_ROUGHNESS_B:
            result = bsdfdata.roughnessB.xxx;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_ANISOTROPY:
            result = bsdfdata.anisotropy.xxx;
            break;
        case DEBUGVIEW_FABRICEX_BSDFDATA_SHEEN:
            result = bsdfdata.sheen.xxx;
            break;
    }
}


#endif
