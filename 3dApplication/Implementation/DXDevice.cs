using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Color = SharpDX.Color;

namespace _3dApplication
{
    public class DXDevice : Form, IDevice
    {
        #region Private

        #region Attributes

        private float _aspectRatio;
        private Device _device { get; set; }
        #endregion

        #region Methods
        private void DXDevice_Resize(object sender, EventArgs e)
        {
            var size = (sender as Form).ClientSize;
            _aspectRatio = (float)size.Width / (float)size.Height;
        }
        private void DXDevice_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsAlive = false;
        }
        #endregion

        #endregion

        #region Public

        #region Attributes
        public bool IsAlive { get; set; }
        public bool Accesible
        {
            get
            {
                return _device.TestCooperativeLevel().Success;
            }
        }
        #endregion

        #region Methods
        public DXDevice()
        {
            IsAlive = true;
            Size = new Size(800, 600);
            Text = "DirectX";
            Resize += DXDevice_Resize;
            FormClosed += DXDevice_FormClosed;
            //FormBorderStyle = FormBorderStyle.None;

            var parameters = new PresentParameters(this.ClientSize.Width, this.ClientSize.Height);
            parameters.SwapEffect = SwapEffect.Discard;
            parameters.Windowed = true;
            parameters.PresentationInterval = PresentInterval.Immediate;
            parameters.BackBufferCount = 1;

            _aspectRatio = (float)this.ClientSize.Width / (float)this.ClientSize.Height;
            _device = new Device(new Direct3D(), 0, DeviceType.Hardware, this.Handle, CreateFlags.HardwareVertexProcessing, parameters);
        }
        public void Render(IEnumerable<IMesh> meshes)
        {
            _device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            _device.BeginScene();

            _device.SetRenderState(RenderState.Lighting, false);
            _device.SetTransform(TransformState.Projection, Matrix.PerspectiveFovRH((float)Math.PI / 4, _aspectRatio, 8f, 0f));
            _device.SetTransform(TransformState.View, Matrix.LookAtRH(new Vector3(0, 0, 15), new Vector3(0, 0, 0), new Vector3(0, 1, 0)));

            foreach (IMesh mesh in meshes)
            {
                _device.SetTexture(0, mesh.BaseTexture);
                _device.SetStreamSource(0, mesh.VertexBuffer, 0, mesh.Stride);
                _device.VertexDeclaration = mesh.VertexDeclaration;
                _device.Indices = mesh.IndexBuffer;

                _device.SetTransform(TransformState.World, mesh.Transformation);

                _device.DrawIndexedPrimitive(mesh.PrimitiveType, mesh.BaseVertexIndex, mesh.MinVertexIndex, mesh.NumVertices, mesh.StartIndex, mesh.PrimitiveCount);
            }

            _device.EndScene();
            _device.Present();
        }
        public VertexBuffer CreateVertexBuffer(int sizeInBytes, VertexTexture[] vertices)
        {
            VertexBuffer buffer = new VertexBuffer(_device, sizeInBytes, Usage.WriteOnly, VertexFormat.PositionW, Pool.Default);
            buffer.Lock(0, 0, LockFlags.None).WriteRange(vertices);
            buffer.Unlock();

            return buffer;
        }
        public VertexDeclaration CreateVertexDeclaration(VertexElement[] vertexElements)
        {
            VertexDeclaration VertexDeclaration = new VertexDeclaration(_device, vertexElements);
            return VertexDeclaration;
        }
        public IndexBuffer CreateIndexBuffer(int sizeInBytes, int[] indexs)
        {
            IndexBuffer buffer = new IndexBuffer(_device, sizeInBytes, Usage.None, Pool.Managed, false);
            buffer.Lock(0, 0, LockFlags.None).WriteRange(indexs);
            buffer.Unlock();

            return buffer;
        }
        public BaseTexture CreateBaseTexture(byte[] data)
        {
            Texture texture = Texture.FromMemory(_device, data);
            BaseTexture baseTexture = new BaseTexture(texture.NativePointer);
            return baseTexture;
        }
        #endregion      

        #endregion
    }
}
