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
        VertexBuffer CreateVertexBuffer<T>(int sizeInBytes, T[] vertices) where T : struct;
        VertexDeclaration CreateVertexDeclaration(VertexElement[] vertexElements);
        IndexBuffer CreateIndexBuffer(int sizeInBytes, int[] indexs);
        VertexShader CreateVertexShader(string file, string entryPoint);
        PixelShader CreatePixelShader(string file, string entryPoitn);
        BaseTexture CreateBaseTexture(string file);
        BaseTexture CreateBaseTextureFromCubeTexture(string file);
        void CaptureInput();
        void Dispose();
    }
}
