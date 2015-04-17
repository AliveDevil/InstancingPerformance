struct PS_IN
{
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD;
};

PS_IN vs(float2 position : POSITION, float2 tex : TEXCOORD)
{
	PS_IN output;

	output.tex = tex;
	output.position = float4(position, 0, 1);

	return output;
}

float4 ps(float4 position : SV_POSITION, float2 tex : TEXCOORD) : SV_TARGET
{
	return float4(tex, 0.5, 1);
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
