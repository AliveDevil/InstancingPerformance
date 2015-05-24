cbuffer VS : register(b0)
{
	float4x4 World;
	float4x4 View;
	float4x4 Projection;
};

struct VS_OUT
{
	float4 position : SV_POSITION;
	float3 normal : NORMAL;
	float4 color : COLOR;
};

VS_OUT VSInstance(float4 position : POSITION, float3 normal : NORMAL, float4x4 world : WORLD, float4 color : COLOR)
{
	VS_OUT output;
	output.position = mul(position, world);
	output.position = mul(output.position, World);
	output.position = mul(output.position, View);
	output.position = mul(output.position, Projection);
	output.color = color;
	output.normal = normalize(mul(normal, (float3x3)world));
	return output;
}
