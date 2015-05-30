#include "Constants.hlsl"

cbuffer VSB : register(b0)
{
	float4x4 World;
	float4x4 View;
	float4x4 Projection;
};

GSIn VSGeometry(VSGeometryIn input)
{
	GSIn output = (GSIn)0;
	output.Position = input.Position;
	output.Case = input.Case;
	output.Color = input.Color;
	return output;

}
