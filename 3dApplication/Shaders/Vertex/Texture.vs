struct VertexToPixel
{
	float4 Position : POSITION;
	float2 TexCoords : TEXCOORD0;
};

float4x4 View : register(c0);
float4x4 Projection : register(c4);
float4x4 World : register(c8);

VertexToPixel TextureAndTransform(float4 inPos : POSITION, float2 inTexCoords : TEXCOORD0)
{
	VertexToPixel Output = (VertexToPixel)0;

	float4 worldPosition = mul(inPos, World);
	float4 worldView = mul(worldPosition, View);
	float4 worldViewProjection = mul(worldView, Projection);

	Output.Position = worldViewProjection;
	Output.TexCoords = inTexCoords;

	return Output;
}