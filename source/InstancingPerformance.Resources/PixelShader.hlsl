#include "Constants.hlsl"

cbuffer Light : register(b0) {
	float4 AmbientColor;
	float AmbientIntensity;

	float4 LightColor;
	float3 LightDirection;
};

float4 PS(PSIn input) : SV_TARGET
{
	float4 r = saturate(AmbientColor * AmbientIntensity + LightColor * saturate(dot(input.Normal, -LightDirection))) * input.Color;
	r.a = 1;
	return r;
}
