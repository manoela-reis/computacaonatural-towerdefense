float4x4 World;
float4x4 View;
float4x4 Projection;

Texture colorTexture;

sampler colorTextureSampler = sampler_state
{
    texture = <colorTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = wrap;
	AddressV = wrap;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 textureCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 textureCoord : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.textureCoord = input.textureCoord;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 output = tex2D(colorTextureSampler, input.textureCoord);
	return output; 
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
