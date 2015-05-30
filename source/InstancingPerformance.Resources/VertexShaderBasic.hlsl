#include "Constants.hlsl"

cbuffer VS : register(b0)
{
	float4x4 World;
	float4x4 View;
	float4x4 Projection;
};

PSIn VSBasic(VSBasicIn input)
{
	PSIn output;
	output.Position = mul(mul(mul(input.Position, World), View), Projection);
	output.Color = input.Color;
	output.Normal = normalize(input.Normal);
	return output;
}
