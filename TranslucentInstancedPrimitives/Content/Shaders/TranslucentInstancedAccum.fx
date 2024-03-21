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
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};

VertexShaderOutput MainVS(in VertexShaderStaticInput input_static, in VertexShaderInstanceInput input_instance)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

    output.Position = float4(input_static.Position, 0, 0);
    output.Position += float4(input_instance.Position, 1);
	
    output.Position = mul(output.Position, WorldViewProjection);
	
    output.Color = input_instance.Color;

	return output;
}

float4 MainPS(VertexShaderOutput input) : SV_TARGET
{
    // Alpha channel is:
    // (layers * 1000) + alpha;
    return input.Color + float4(0, 0, 0, 1000);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};