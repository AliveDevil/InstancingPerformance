cbuffer Constants
{
	static float3 corner[8] =
	{
		{ -0.5, -0.5, -0.5 },
		{ -0.5,  0.5, -0.5 },
		{  0.5,  0.5, -0.5 },
		{  0.5, -0.5, -0.5 },
		{  0.5, -0.5,  0.5 },
		{  0.5,  0.5,  0.5 },
		{ -0.5,  0.5,  0.5 },
		{ -0.5, -0.5,  0.5 },
	};

	static int faces[6][4] =
	{
		{1,6,5,2},
		{7,0,3,4},
		{4,5,6,7},
		{0,1,2,3},
		{3,2,5,4},
		{7,6,1,0}
	};

	static float3 normals[6] =
	{
		{  0,  1,  0 },
		{  0, -1,  0 },
		{  0,  0,  1 },
		{  0,  0, -1 },
		{  1,  0,  0 },
		{ -1, 0,  0 }
	};
};

struct VSBasicIn
{
	float4 Position : POSITION;
	float3 Normal : NORMAL;
	float4 Color : COLOR;
};

struct VSInstanceIn
{
	float4 Position : POSITION;
	float3 Normal : NORMAL;
	float4x4 World : WORLD;
	float4 Color : COLOR;
};

struct VSGeometryIn
{
	float4 Position : POSITION;
	uint Case : CASE;
	float4 Color : COLOR;
};

struct GSIn
{
	float4 Position : POSITION;
	uint Case : CASE;
	float4 Color : COLOR;
};

struct PSIn
{
	float4 Position : SV_Position;
	float3 Normal : NORMAL;
	float4 Color : COLOR;
};
