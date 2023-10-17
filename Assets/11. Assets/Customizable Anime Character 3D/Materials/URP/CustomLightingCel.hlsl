// This is the Lighting.hlsl file for early parts of the tutorial

#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED
#ifndef SHADERGRAPH_PREVIEW
	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
	#if (SHADERPASS != SHADERPASS_FORWARD)
	#undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
#endif
#endif
struct CustomLightingData {
	float3 positionWS;
	float3 normalWS;
	float4 shadowCoord;
	float3 albedo;
	float ambientOcclusion;

	float3 bakedGI;
	float fogfactor;
	
};
#ifndef SHADERGRAPH_PREVIEW

float3 CustomGlobalIllumination(CustomLightingData d) {
	float3 indirectDiffuse =  d.bakedGI * d.ambientOcclusion;

	return indirectDiffuse;
}
float3 CustomLightHandling(CustomLightingData d, Light l) {
	float3 radiance = l.distanceAttenuation * l.shadowAttenuation;
	float diffuse = saturate(dot(d.normalWS, l.direction));
	float3 color = radiance * diffuse;
	return color;
}
#endif
float3 CalculateCustomLighting(CustomLightingData d) {
#ifdef SHADERGRAPH_PREVIEW
	float3 lightDir = float3(0.5, 0.5, 0);
	float intensity = saturate(dot(d.normalWS, lightDir));
	return intensity;
#else
	Light mainlight = GetMainLight(d.shadowCoord, d.positionWS, 1);
	MixRealtimeAndBakedGI(mainlight, d.normalWS, d.bakedGI);
	float3 color = CustomGlobalIllumination(d);

	color += CustomLightHandling(d, mainlight);
#ifdef _ADDITIONAL_LIGHTS

	uint numAdditionalLights = GetAdditionalLightsCount();
	for (uint lightI = 0; lightI < numAdditionalLights; lightI++) {
		Light light = GetAdditionalLight(lightI, d.positionWS, 1);
		color += CustomLightHandling(d, light);
	}
#endif

	//color = MixFog(color, d.fogfactor);
	return color;
#endif
}




#ifndef SHADERGRAPH_PREVIEW
float3 CustomColorHandling(CustomLightingData d, Light l) {
	float3 radiance = l.color*l.distanceAttenuation;
	
	float3 color = d.albedo * radiance;
	return color;
}
#endif
float3 CalculateCustomColor(CustomLightingData d) {
#ifdef SHADERGRAPH_PREVIEW
	
	return (0,0,0);
#else
	float3 color = (0, 0, 0);
	Light mainlight = GetMainLight(d.shadowCoord, d.positionWS, 1);
	color += CustomColorHandling(d, mainlight);
	#ifdef _ADDITIONAL_LIGHTS
	
	uint numAdditionalLights = GetAdditionalLightsCount();
	for (uint lightI = 0; lightI < numAdditionalLights; lightI++) {
		Light light = GetAdditionalLight(lightI, d.positionWS, 1);
		color += CustomColorHandling(d, light);
	}
	#endif

	color = MixFog(color, d.fogfactor);
	return color;
#endif
}



void CalculateCustomLighting_float(float3 Position, float3 Normal, float3 Albedo, float AmbientOcclusion,
	float2 LightmapUV, out float3 Color,out float3 Shadow) {
	CustomLightingData d;
	d.positionWS = Position;
	d.albedo = Albedo;
	d.normalWS = Normal;
	d.ambientOcclusion = AmbientOcclusion;
	#ifdef SHADERGRAPH_PREVIEW
	d.shadowCoord = 0;
	d.bakedGI = 0;
	d.fogfactor = 0;
	#else
		float4 positionCS = TransformWorldToHClip(Position);
		#if SHADOWS_SCREEN
			d.shadowCoord = ComputeScreenPos(positionCS);
		#else
			d.shadowCoord = TransformWorldToShadowCoord(Position);
		#endif
			float2 lightmapUV;
			OUTPUT_LIGHTMAP_UV(LightmapUV, unity_LightmapST, lightmapUV);
			
			float3 vertexSH;
			OUTPUT_SH(Normal, vertexSH);
			
			d.bakedGI = SAMPLE_GI(lightmapUV, vertexSH, Normal);
			
			
			
			d.fogfactor = ComputeFogFactor(positionCS.z);
	#endif
	Color = CalculateCustomColor(d);
	Shadow = CalculateCustomLighting(d);
}

#endif