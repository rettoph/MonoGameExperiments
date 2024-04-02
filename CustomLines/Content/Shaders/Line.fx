#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0

matrix WorldViewProjection;


struct VertexShaderInput
{
    float3 Position : POSITION0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

    output.Position = float4(input.Position, 1);
    output.Position = mul(output.Position, WorldViewProjection);
	
    output.Color = input.Color;

	return output;
}

float4 MainPS(VertexShaderOutput input) : SV_TARGET
{
    return input.Color;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};