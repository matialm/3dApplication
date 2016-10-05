struct Vertex
{
	float4 Position : POSITION;
	float3 TexCoords : TEXCOORD0;
};

float4x4 View;
float4x4 Projection;
float4x4 World;

Vertex TextureAndTransform(Vertex input)
{
	Vertex output = (Vertex)0;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);

	output.Position = mul(viewPosition, Projection);
	output.TexCoords = worldPosition;

	return output;
}