struct VertexToPixel
{
	float4 Position : POSITION;
	float2 TexCoords : TEXCOORD0;
};

float4x4 View;
float4x4 Projection;
float4x4 World;

VertexToPixel TextureAndTransform(float4 inPos : POSITION, float2 inTexCoords : TEXCOORD0)
{
	VertexToPixel Output = (VertexToPixel)0;

	float4 worldPosition = mul(inPos, World);
	float4 worldView = mul(worldPosition, View);

	Output.Position = mul(worldView, Projection);
	Output.TexCoords = inTexCoords;

	return Output;
}