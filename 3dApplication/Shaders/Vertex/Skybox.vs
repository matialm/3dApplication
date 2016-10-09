struct Vertex
{
	float4 Position : POSITION;
	float3 TexCoords : TEXCOORD0;
};

float4x4 View : register(c0);
float4x4 Projection : register(c4);
float4x4 World : register(c8);
float3 CameraPosition : register(c12);

Vertex TextureAndTransform(Vertex input)
{
	Vertex output = (Vertex)0;

	float4 worldPosition = mul(input.Position, World);
	float4 worldView = mul(worldPosition, View);
	float4 worldViewProjection = mul(worldView, Projection);

	output.Position = worldViewProjection;
	output.TexCoords = worldPosition + CameraPosition;

	return output;
}