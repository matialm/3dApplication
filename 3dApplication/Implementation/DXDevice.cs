using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Color = SharpDX.Color;
using Device = SharpDX.Direct3D9.Device;
using DeviceType = SharpDX.Direct3D9.DeviceType;

namespace _3dApplication
{
    public class DXDevice : Form, IDevice
    {
        #region Private

        #region Attributes
        private Device _device;
        private Camera _camera;
        private bool _wireframe = false;
        #endregion

        #region Methods
        private void DXDevice_Resize(object sender, EventArgs e)
        {
            var size = (sender as Form).ClientSize;
            _camera.SetSize(size.Width, size.Height);
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

        public Input Input { get; set; }
        #endregion

        #region Methods
        public DXDevice(Camera camera)
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

            _camera = camera;
            _device = new Device(new Direct3D(), 0, DeviceType.Hardware, this.Handle, CreateFlags.HardwareVertexProcessing, parameters);
            _camera.SetSize(Size.Width, Size.Height);
        }
        public void Render(Camera camera, IEnumerable<IMesh> meshes)
        {
            _device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Blue, 1.0f, 0);
            _device.BeginScene();

            _device.SetRenderState(RenderState.FillMode, _wireframe ? FillMode.Wireframe : FillMode.Solid);
            _device.SetRenderState(RenderState.CullMode, _wireframe);
            _device.SetRenderState(RenderState.Lighting, false);
            _device.SetTransform(TransformState.Projection, camera.Projection);
            _device.SetTransform(TransformState.View, camera.View);

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
        public VertexBuffer CreateVertexBuffer<T>(int sizeInBytes, T[] vertices) where T : struct
        {
            VertexBuffer buffer = new VertexBuffer(_device, sizeInBytes, Usage.WriteOnly, VertexFormat.None, Pool.Default);
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

        public void CaptureInput()
        {
            if(Input.KeyPress(Key.Escape))
            {
                IsAlive = false;
            }

            if(Input.KeyPress(Key.F1))
            {
                _wireframe = !_wireframe;
            }
        }
        #endregion

        #endregion
    }
}
