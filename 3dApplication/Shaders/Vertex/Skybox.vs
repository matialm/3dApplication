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

	float4x4 worldViewProjection = mul(World, mul(View, Projection));
	float4 worldPosition = mul(input.Position, World);

	output.Position = mul(input.Position + CameraPosition, (worldViewProjection)).xyzz;
	output.TexCoords = worldPosition;

	return output;
}