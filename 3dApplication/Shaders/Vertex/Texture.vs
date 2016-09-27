struct VertexToPixel
{
	float4 Position     : POSITION;
	float2 TexCoords    : TEXCOORD0;
};

float4x4 viewProjection;
float4x4 transformation;

VertexToPixel TextureAndTransform(float4 inPos : POSITION, float2 inTexCoords : TEXCOORD0)
{
	VertexToPixel Output = (VertexToPixel)0;

	Output.Position = mul(mul(inPos,transformation), viewProjection);

	Output.TexCoords = inTexCoords;

	return Output;
}