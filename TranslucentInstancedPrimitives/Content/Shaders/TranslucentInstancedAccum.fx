#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0

matrix WorldViewProjection;


struct VertexShaderStaticInput
{
	float2 Position : POSITION0;
};

struct VertexShaderInstanceInput
{
    float3 Position : BLENDWEIGHT0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
};

VertexShaderOutput MainVS(in VertexShaderStaticInput input_static, in VertexShaderInstanceInput input_instance)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

    output.Position = float4(input_static.Position, 0, 0);
    output.Position += float4(input_instance.Position, 1);
	
    output.Position = mul(output.Position, WorldViewProjection);

	return output;
}

float4 MainPS(VertexShaderOutput input) : SV_TARGET
{
    return float4(1, 1, 1, 1);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};