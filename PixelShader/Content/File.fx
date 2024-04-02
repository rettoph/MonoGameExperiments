	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1

Texture2D<float4> Texture : register(t0);
sampler TextureSampler : register(s0);

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : SV_TARGET
{
    return Texture.Sample(TextureSampler, input.TextureCoordinates);
}

technique SpriteDrawing
{
	pass P0
	{
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};