float4 vs(float2 position : POSITION, float2 tex : TEXCOORD) : SV_POSITION
{
	return float4(position, 0, 1);
}

float4 ps(float4 position : SV_POSITION) : SV_TARGET
{
	return float4(1, 1, 1, 1);
}

technique11
{
	pass
	{
		SetVertexShader(CompileShader(vs_4_0, vs()));
		SetGeometryShader(NULL);
		SetPixelShader(CompileShader(ps_4_0, ps()));
	}
};
