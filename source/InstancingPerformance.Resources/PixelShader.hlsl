cbuffer cbLight : register(b0) {
	float4 AmbientColor;
	float AmbientIntensity;

	float4 LightColor;
	float3 LightDirection;
};

float4 PS(float4 position : SV_POSITION, float3 normal : NORMAL, float4 color : COLOR) : SV_TARGET
{
	float4 r = saturate(AmbientColor * AmbientIntensity + LightColor * saturate(dot(normal, -LightDirection))) * color;
	r.a = 1;
	return r;
}
