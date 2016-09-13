using SharpDX.Direct3D9;
using System.Collections.Generic;

namespace _3dApplication
{
    public interface IDevice
    {
        bool Focused { get; }
        bool IsAlive { get; set; }
        bool Accesible { get; }

        void Show();
        bool Focus();
        void Render(IEnumerable<IMesh> meshes);
        VertexBuffer CreateVertexBuffer(int sizeInBytes, VertexTexture[] vertices);
        VertexDeclaration CreateVertexDeclaration(VertexElement[] vertexElements);
        IndexBuffer CreateIndexBuffer(int sizeInBytes, int[] indexs);
        BaseTexture CreateBaseTexture(byte[] data);
        void Dispose();
    }
}
