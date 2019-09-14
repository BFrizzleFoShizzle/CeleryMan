// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Hidden/DefferedCelShading" {
Properties {
    _LightTexture0 ("", any) = "" {}
    _LightTextureB0 ("", 2D) = "" {}
    _ShadowMapTexture ("", any) = "" {}
    _SrcBlend ("", Float) = 1
    _DstBlend ("", Float) = 1
		/*
	_Cuts("Cuts", Int) = 2
	_InterpolationRange("Interpolation range (range interpolated at light cut)", Float) = 0.01
	_MinimumLight("Minimum light (lowest cut value)", Float) = 0.01
	*/
}

SubShader {

// Pass 1: Lighting pass
//  LDR case - Lighting encoded into a subtractive ARGB8 buffer
//  HDR case - Lighting additively blended into floating point buffer
Pass {
    ZWrite Off
    Blend [_SrcBlend] [_DstBlend]

CGPROGRAM
#pragma target 3.0
#pragma vertex vert_deferred
#pragma fragment frag
#pragma multi_compile_lightpass
#pragma multi_compile ___ UNITY_HDR_ON

#pragma exclude_renderers nomrt

#include "UnityCG.cginc"
#include "UnityDeferredLibrary.cginc"
#include "UnityPBSLighting.cginc"
#include "UnityStandardUtils.cginc"
#include "UnityGBuffer.cginc"
#include "UnityStandardBRDF.cginc"

sampler2D _CameraGBufferTexture0;
sampler2D _CameraGBufferTexture1;
sampler2D _CameraGBufferTexture2;

half4 CellShade(half4 greyLit, half4 materialLit, half4 baseColour)
{
	const int cuts = 2;
	const float falloff = 1.0f;
	const float minimumLight = 0.05f;
	const float interpolationRange = 0.01f;

	// vec from base colour to light
	half3 greyDelta = greyLit - half4(0.5, 0.5, 0.5, 0);
	half3 absGreyDelta = abs(greyDelta);
	half3 greyDeltaMax = max(max(absGreyDelta.r, absGreyDelta.g), absGreyDelta.b);
	half3 greyDeltaNorm = absGreyDelta / greyDeltaMax;

	half3 colourDelta = materialLit - baseColour;
	half3 absColourDelta = abs(colourDelta);
	half3 colourDeltaMax = max(max(absColourDelta.r, absColourDelta.g), absColourDelta.b);
	half3 colourDeltaNorm = absColourDelta / colourDeltaMax;

	// max. colour is cell "intensity"
	// should be "distance from fully-lit colour"
	float lightFactor = max(max(greyLit.r, greyLit.g), greyLit.b);

	if (lightFactor < 0.001f)
		return half4(0,0,0,1);

	float lightContribution = lightFactor;

	lightContribution = pow(lightContribution, falloff);

	float cutLightContribution = lightContribution;

	cutLightContribution = round(cutLightContribution * cuts) / cuts;
	
	float interpolationVal = lightContribution - (cutLightContribution - (0.5f / cuts));
	if (interpolationVal > 0.0f && interpolationVal < interpolationRange)
		cutLightContribution -= (1.0 - (interpolationVal / interpolationRange)) / cuts;
		
	// TODO should this shift the whole range of cuts, or just set the lowest cut?
	// value must be BELOW 1.0/cuts to look right
	cutLightContribution = max(cutLightContribution, minimumLight);

	float bidirectionalLightContribution = cutLightContribution - 0.5f;

	half3 colourDiff = colourDelta / colourDeltaMax;

	//baseColour = baseColour / lightFactor;
	//return half4(colourDiff, 1.0f);
	return half4(baseColour + colourDeltaNorm * half3(bidirectionalLightContribution, bidirectionalLightContribution, bidirectionalLightContribution), 1.0f);

	//return half4(half3(0.5, 0.5, 0.5) + greyDeltaNorm * half3(bidirectionalLightContribution, bidirectionalLightContribution, bidirectionalLightContribution), 1.0f);
	//return half4(half3(0.5, 0.5, 0.5) + greyDeltaNorm * half3(bidirectionalLightContribution, bidirectionalLightContribution, bidirectionalLightContribution), 1.0f);
	//return half4(half3(0.5, 0.5, 0.5)  * half3(cutLightContribution, cutLightContribution, cutLightContribution), 1.0f);

}

half4 CalculateLight (unity_v2f_deferred i)
{

    float3 wpos;
    float2 uv;
    float atten, fadeDist;
    UnityLight light;
    UNITY_INITIALIZE_OUTPUT(UnityLight, light);
    UnityDeferredCalculateLightParams (i, wpos, uv, light.dir, atten, fadeDist);

    light.color = _LightColor.rgb * atten;

    // unpack Gbuffer
    half4 gbuffer0 = tex2D (_CameraGBufferTexture0, uv);
    half4 gbuffer1 = tex2D (_CameraGBufferTexture1, uv);
    half4 gbuffer2 = tex2D (_CameraGBufferTexture2, uv);
    UnityStandardData data = UnityStandardDataFromGbuffer(gbuffer0, gbuffer1, gbuffer2);

    float3 eyeVec = normalize(wpos-_WorldSpaceCameraPos);
    half oneMinusReflectivity = 1 - SpecularStrength(data.specularColor.rgb);

    UnityIndirect ind;
    UNITY_INITIALIZE_OUTPUT(UnityIndirect, ind);
    ind.diffuse = 0;
    ind.specular = 0;

    half4 standardRender = UNITY_BRDF_PBS (data.diffuseColor, data.specularColor, oneMinusReflectivity, data.smoothness, data.normalWorld, -eyeVec, light, ind);

	half4 whiteRender = UNITY_BRDF_PBS(half4(0.5,0.5,0.5,1), half4(0.5, 0.5, 0.5, 1), oneMinusReflectivity, data.smoothness, data.normalWorld, -eyeVec, light, ind);


	// CEL START
	//return gbuffer0;
	return CellShade(whiteRender, standardRender, gbuffer0);
	//return whiteRender;
	//return res;

	// CEL END
}

#ifdef UNITY_HDR_ON
half4
#else
fixed4
#endif
frag(unity_v2f_deferred i) : SV_Target
{
    half4 c = CalculateLight(i);

    #ifdef UNITY_HDR_ON
    return c;
    #else
    return exp2(-c);
    #endif
}

ENDCG
}


// Pass 2: Final decode pass.
// Used only with HDR off, to decode the logarithmic buffer into the main RT
Pass {
    ZTest Always Cull Off ZWrite Off
    Stencil {
        ref [_StencilNonBackground]
        readmask [_StencilNonBackground]
        // Normally just comp would be sufficient, but there's a bug and only front face stencil state is set (case 583207)
        compback equal
        compfront equal
    }

CGPROGRAM
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#pragma exclude_renderers nomrt

#include "UnityCG.cginc"

sampler2D _LightBuffer;
struct v2f {
    float4 vertex : SV_POSITION;
    float2 texcoord : TEXCOORD0;
};

v2f vert (float4 vertex : POSITION, float2 texcoord : TEXCOORD0)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(vertex);
    o.texcoord = texcoord.xy;
#ifdef UNITY_SINGLE_PASS_STEREO
    o.texcoord = TransformStereoScreenSpaceTex(o.texcoord, 1.0f);
#endif
    return o;
}

fixed4 frag (v2f i) : SV_Target
{
    return -log2(tex2D(_LightBuffer, i.texcoord));
}
ENDCG
}

}
Fallback Off
}
