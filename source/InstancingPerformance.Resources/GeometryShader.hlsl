#include "Constants.hlsl"

cbuffer GSB : register(b0)
{
	float4x4 World;
	float4x4 View;
	float4x4 Projection;
};

[instance(24)]
[maxvertexcount(36)]
void GS(point GSIn instances[1], inout TriangleStream<PSIn> output)
{
	float4x4 mvp = mul(mul(World, View), Projection);

	GSIn instance = instances[0];
	PSIn ps;
	ps.Color = instance.Color;
	for (int i = 0; i < 6; i++)
	{
		if ((instance.Case & (1 << i)) != 0)
		{
			ps.Normal = normals[i];
			int face[4] = faces[i];
			ps.Position = mul(float4(corner[face[0]], 0) + instance.Position, mvp);
			output.Append(ps);
			ps.Position = mul(float4(corner[face[1]], 0) + instance.Position, mvp);
			output.Append(ps);
			ps.Position = mul(float4(corner[face[2]], 0) + instance.Position, mvp);
			output.Append(ps);
			output.RestartStrip();
			ps.Position = mul(float4(corner[face[0]], 0) + instance.Position, mvp);
			output.Append(ps);
			ps.Position = mul(float4(corner[face[2]], 0) + instance.Position, mvp);
			output.Append(ps);
			ps.Position = mul(float4(corner[face[3]], 0) + instance.Position, mvp);
			output.Append(ps);
			output.RestartStrip();
		}
	}
}
