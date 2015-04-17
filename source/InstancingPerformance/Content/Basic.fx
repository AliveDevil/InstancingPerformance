float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;

float4 LightColor = float4(1, 1, 1, 1);
float3 LightDirection = float3(0, -0.66, 0.33);

float4x4 World;
float4x4 View;
float4x4 Projection;

struct VS_OUT
{
	float4 position : SV_POSITION;
	float3 normal : NORMAL;
	float4 color : COLOR;
};

VS_OUT vs(float4 position : POSITION, float3 normal : NORMAL, float4 color : COLOR)
{
	VS_OUT output;
	output.position = mul(position, World);
	output.position = mul(output.position, View);
	output.position = mul(output.position, Projection);
	output.color = color;
	output.normal = normalize(normal);
	return output;
}

float4 ps(float4 position : SV_POSITION, float3 normal : NORMAL, float4 color : COLOR) : SV_TARGET
{
	float4 r = saturate(AmbientColor * AmbientIntensity + LightColor * saturate(dot(normal, -LightDirection))) * color;
	r.a = 1;
	return r;
}

technique11
{
	pass
	{
		SetVertexShader(CompileShader(vs_5_0, vs()));
		SetGeometryShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, ps()));
	}
};
