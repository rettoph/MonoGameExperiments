#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0

matrix WorldViewProjection;

Texture2D AccumTexture;
sampler2D AccumTextureSampler = sampler_state
{
    Texture = <AccumTexture>;
};


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
    float2 TextureCoordinates : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderStaticInput input_static, in VertexShaderInstanceInput input_instance)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

    output.Position = float4(input_static.Position, 0, 0);
    output.Position += float4(input_instance.Position, 1);
    output.Position = mul(output.Position, WorldViewProjection);
	
    output.Color = input_instance.Color;
    
    // Convert to normalized device coordinates
    output.TextureCoordinates = output.Position.xy / output.Position.w;
    output.TextureCoordinates = float2(output.TextureCoordinates.x + 1, 1 - output.TextureCoordinates.y);
    output.TextureCoordinates /= 2;
    
	return output;
}

float4 MainPS(VertexShaderOutput input) : SV_TARGET
{
    float4 accum = tex2D(AccumTextureSampler, input.TextureCoordinates);
    
    if (accum.a < 2000)
    { // Indicates theres only 1 layer, as 2 layers would at least be 2000
        return input.Color;
    }

    // Alpha channel is:
    // (layers * 1000) + alpha;
    float layers = round(accum.a / 1000);
    float alpha = accum.a % 1000;
    
    // Divide the accum colors by the total number of layers
    float4 rgba = float4(accum.rgb, alpha) / layers;
    
    // Average the int color + avg.rgb, use the accum alpha avg
    return float4((input.Color.rgb + (rgba.rgb * rgba.a)) / 2, rgba.a);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};