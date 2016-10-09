using SharpDX;

namespace _3dApplication
{
    public struct Vertex
    {
        public Vector3 Position { get; set; }
    }

    public struct VertexTexture
    {
        public Vector3 Position { get; set; }
        public Vector2 UV { get; set; }
    }

    public struct VertexColored
    {
        public Vector3 Position { get; set; }
        public Color Color { get; set; }
    }

    public struct ShaderConstant
    {
        public int StartRegister { get; set; }
        public float[] Values { get; set; }
    }
}
