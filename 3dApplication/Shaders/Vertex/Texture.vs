struct VertexToPixel
{
	float4 Position     : POSITION;
	float2 TexCoords    : TEXCOORD0;
};

float4x4 View;
float4x4 Projection;
float4x4 World;

VertexToPixel TextureAndTransform(float4 inPos : POSITION, float2 inTexCoords : TEXCOORD0)
{
	float4x4 viewProjection = mul(View, Projection);

	VertexToPixel Output = (VertexToPixel)0;

	Output.Position = mul(mul(inPos, World), viewProjection);

	Output.TexCoords = inTexCoords;

	return Output;
}