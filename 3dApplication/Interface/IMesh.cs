using SharpDX.Direct3D9;
using System.Collections.Generic;

namespace _3dApplication
{
    public interface IMesh
    {
        int Stride { get; set; }
        int PrimitiveCount { get; set; }
        int StartIndex { get; set; }
        int MinVertexIndex { get; set; }
        int BaseVertexIndex { get; set; }
        int VertexCount { get; set; }
        BaseTexture BaseTexture { get; }
        VertexDeclaration VertexDeclaration { get; set; }
        VertexBuffer VertexBuffer { get; set; }
        IndexBuffer IndexBuffer { get; set; }
        PixelShader PixelShader { get; set; }
        VertexShader VertexShader { get; set; }
        PrimitiveType PrimitiveType { get; set; }
        List<ShaderConstant> VertexShaderValues { get; set; }
        List<ShaderConstant> PixelShaderValues { get; set; }

        void Transform();
    }
}
