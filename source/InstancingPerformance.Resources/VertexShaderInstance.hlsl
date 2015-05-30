#include "Constants.hlsl"

cbuffer VSB : register(b0)
{
	float4x4 World;
	float4x4 View;
	float4x4 Projection;
};

PSIn VSInstance(VSInstanceIn input)
{
	PSIn output;
	output.Position = mul(mul(mul(mul(input.Position, input.World), World), View), Projection);
	output.Color = input.Color;
	output.Normal = normalize(mul(input.Normal, (float3x3)input.World));
	return output;
}
