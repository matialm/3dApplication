using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace _3dApplication
{
    public interface IDevice
    {
        bool Focused { get; }
        bool IsAlive { get; set; }
        bool Accesible { get; }
        Size Size { get; set; }
        IntPtr Handle { get; }

        void Show();
        bool Focus();
        void Render(IEnumerable<IMesh> meshes);
        void Render(int stride, int primitiveCount, int startIndex, int minVertexIndex, int baseVertexIndex, int vertexCount, BaseTexture baseTexture, VertexDeclaration vertexDeclaration, VertexBuffer vertexBuffer, IndexBuffer indexBuffer, PixelShader pixelShader, VertexShader vertexShader, Matrix world, PrimitiveType primitiveType);
        VertexBuffer CreateVertexBuffer<T>(int sizeInBytes, T[] vertices) where T : struct;
        VertexDeclaration CreateVertexDeclaration(VertexElement[] vertexElements);
        IndexBuffer CreateIndexBuffer(int sizeInBytes, int[] indexs);
        VertexShader CreateVertexShader(byte[] data, string entryPoint);
        PixelShader CreatePixelShader(byte[] data, string entryPoitn);
        BaseTexture CreateBaseTexture(byte[] data);
        void CaptureInput();
        void Dispose();
    }
}
