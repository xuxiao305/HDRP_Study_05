#ifndef one2 
    #define one2 1.0
    #define one3 1.0
    #define one4 1.0
    #define zero2 0.0
    #define zero3 0.0
    #define zero4 0.0
#endif

float3 CombineDetailNormal(float3 baseNormal, float3 detailNormal)
{
    float3 t     = baseNormal;
           t.z   = t.z + 1.0f;
    float3 u     = detailNormal;
           u.xy *= -1.0f;
    return float3(t * dot(t, u) / t.z - u);
}       


void CHR_CostumeFabric_float(
    float _Thickness,
    float _TranslucencyInt,
    float4 _DetailTex,
    float4 _DetailPatternTex,
    float4 _DetailWeatheringTex,
    float4 _PatternTex,
    float4 _BaseColorTex, 
    float4 _BaseNormalTex, 
    float4 _Mask0Tex,
    float4 _Mask1Tex,
    float4 _Mask2Tex,

    float SmoothnessInt,
    float  _SheenInt,                  
    float3 _FabricColorH,                
    float3 _FabricColorV,               
    float3 _BleachColor,                 
    float3 _PatinaColor, 
    float  _WeatheringInt,             
    float _DetailMetallicH,
    float _DetailMetallicV,    
    float3 _PatternColor1,
    float3 _PatternColor2,
    float3 _PatternColor3,
    float _PatternMetallic1,
    float _PatternMetallic2,
    float _PatternMetallic3,

    float2 _TintPatternMask,

    out float3 _AlbedoOut,             
    out float3 _NormalOut,        
    out float _SmoothnessOut,         
    out float _AlphaOut,            
    out float _TransOut,              
    out float _MicrovisibilityOut,     
    out float _MetallicOut,          
    out float _SheenOut
)
{
    // ====================================================
    // Initialize Output ==================================
    // ====================================================
    _SmoothnessOut = 0.0;
    _SheenOut = 0.0;
    _MetallicOut = 0.0;
    _MicrovisibilityOut = 1.0;
    _NormalOut = float3(0.0, 0.0, 1.0);
    _AlbedoOut = float3(1.0, 1.0, 1.0);
    _AlphaOut = 1.0;
    _TransOut = 0.0;


    // ====================================================
    // Sample Fabric Textures ====================================
    // ====================================================
    _AlbedoOut.rgb = _BaseColorTex.rgb;

    float   baseAlpha = _BaseColorTex.a;
            _MetallicOut = _BaseNormalTex.b;
            _SmoothnessOut = _BaseNormalTex.a;
            _NormalOut.xy = _BaseNormalTex.xy * 2.0 - 1.0;
            
    // Fabric Weathering ==================================
    float   baseAO = _Mask0Tex.r;
    float   bleachMask = lerp(1.0,  _Mask0Tex.g, _WeatheringInt);
    float   patinaMask = lerp(1.0,  _Mask0Tex.b, _WeatheringInt);

    float2  tearNormal = (_Mask1Tex.rg * 2.0 - 1.0) * _WeatheringInt;  
    float   tearMV = _Mask1Tex.b; 
    float   tearGloss = _Mask1Tex.a;  

    float   tearAlpha = lerp( 1.0, _Mask1Tex.b * 10.0,_WeatheringInt); 


    float   tintingMask = step(0.5, _TintPatternMask.r);   float detailMask = step(0.01, _TintPatternMask.r);
    float   patternMask = _TintPatternMask.g;

            _AlphaOut = tearAlpha * baseAlpha;
            _SmoothnessOut *= lerp(1.0, SmoothnessInt, detailMask);

            _SheenOut = _SheenInt * detailMask;

    // Fabric Pattern ====================================================
    float   patternMapAll = max(_PatternTex.r, max(_PatternTex.g, _PatternTex.b)) * patternMask;
    
    // Fabric Details ================================
    float4  detailMap = lerp(_DetailWeatheringTex, _DetailTex, saturate(patinaMask * 2.0));
            patternMask *= tearMV;
            detailMap = lerp(detailMap, _DetailPatternTex, patternMapAll);

    float3  detailNormal = float3(detailMap.rg * 2.0 - 1.0, 1.0);
            detailNormal.xy *= detailMask;
    float   detailGloss = detailMap.a;
    float   detailColorCompositionMask = detailMap.b;

    float   detailAO = lerp(1.0, saturate((detailGloss - 0.1) * 3.0),  detailMask);

    // Sheen ================================
            _SheenOut = lerp(0.5, detailGloss, detailMask) * _SheenInt * 2.0;

    // Base and Pattern Color ================================
    float3  fabricColorH        = lerp(_FabricColorH,    _PatternColor1,     patternMask * _PatternTex.r);
    float3  fabricColorV        = lerp(_FabricColorV,    _PatternColor2,     patternMask * _PatternTex.g);
    float3  fabricMetallicH     = lerp(_DetailMetallicH, _PatternMetallic1,  patternMask * _PatternTex.r);
    float3  fabricMetallicV     = lerp(_DetailMetallicV, _PatternMetallic2,  patternMask * _PatternTex.g);

    float3  fabricColorHV       = lerp(fabricColorH,     fabricColorV,       detailColorCompositionMask);
    float   fabricMetallicHV    = lerp(fabricMetallicH,  fabricMetallicV,    detailColorCompositionMask);

            fabricColorHV       = lerp(fabricColorHV,    _PatternColor3,     patternMask * _PatternTex.b);
            fabricMetallicHV    = lerp(fabricMetallicHV, _PatternMetallic3,  patternMask * _PatternTex.b);
    float   fabricGlossHV       = fabricColorHV.r;

            _AlbedoOut.rgb      *= lerp(one3, fabricColorHV.rgb, tintingMask);
            _MetallicOut        = lerp(_MetallicOut, fabricMetallicHV, tintingMask);
            _MetallicOut        = lerp(_MetallicOut * 0.5, _MetallicOut, patinaMask);
            _SmoothnessOut      = lerp(_SmoothnessOut * 0.5, _SmoothnessOut, patinaMask);

             // Fabric Bleach =================================
            bleachMask = ((0.5 - detailMap.a) * (1 - bleachMask) + bleachMask); 

            _AlbedoOut.rgb = lerp(_AlbedoOut.rgb * _PatinaColor.rgb * 2.0, _AlbedoOut.rgb, patinaMask);
            _AlbedoOut.rgb  = lerp(_AlbedoOut.rgb + _BleachColor.rgb, _AlbedoOut.rgb, bleachMask);

            _SmoothnessOut *= lerp(1.0, detailGloss, detailMask);    
            _SmoothnessOut *= saturate(tearGloss * 2.0);

            _MicrovisibilityOut *= baseAO;
            _MicrovisibilityOut *= tearMV;
            _MicrovisibilityOut *= detailAO;

            // _NormalOut = CombineDetailNormal(_NormalOut.xyz, float3(patternNormal.xy * patternMask, 1.0));
            _NormalOut.xy += tearNormal.xy;
            _NormalOut.xyz = normalize(_NormalOut.xyz); 
            _NormalOut = CombineDetailNormal(_NormalOut.xyz, detailNormal);

}
