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
        Input Input { get; set; }

        void Show();
        bool Focus();
        void Render(Camera camera, IEnumerable<IMesh> meshes);
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
