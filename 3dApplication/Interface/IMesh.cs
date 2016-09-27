using SharpDX;
using SharpDX.Direct3D9;

namespace _3dApplication
{
    public interface IMesh
    {
        VertexDeclaration VertexDeclaration { get; set; }
        PrimitiveType PrimitiveType { get; set; }
        VertexBuffer VertexBuffer { get; set; }
        VertexShader VertexShader { get; set; }
        PixelShader PixelShader { get; set; }
        IndexBuffer IndexBuffer { get; set; }
        BaseTexture BaseTexture { get; }
        Matrix Transformation { get; set; }
        int BaseVertexIndex { get; set; }
        int MinVertexIndex { get; set; }
        int NumVertices { get; set; }
        int StartIndex { get; set; }
        int PrimitiveCount { get; set; }
        int Stride { get; set; }

        void Transform();
    }
}
