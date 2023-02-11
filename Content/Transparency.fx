#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
Texture2D FadePalette;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
	Filter = Point;
};

sampler2D FadePaletteSampler = sampler_state
{
	Texture = <FadePalette>;
	Filter = Point;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	clip(color.rgb == float3(1, 0, 1) ? -1 : 1);
	return float4(color.rgb, input.Color.a);
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};