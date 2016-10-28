using SharpDX.Direct3D9;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        private static DXDevice _instance = null;
        private Device _device;
        private Camera _camera;
        private bool _wireframe = false;
        private string _root;
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
        public static DXDevice Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DXDevice();
                }

                return _instance;
            }
        }
        #endregion

        #region Methods
        private DXDevice()
        {
            _root = Application.StartupPath;
            _camera = Camera.Instance;
            IsAlive = true;
            Size = new Size(800, 600);
            Text = "DirectX";
            Resize += DXDevice_Resize;
            FormClosed += DXDevice_FormClosed;
            //FormBorderStyle = FormBorderStyle.None;

            var parameters = new PresentParameters(ClientSize.Width, ClientSize.Height);
            parameters.SwapEffect = SwapEffect.Discard;
            parameters.Windowed = true;
            parameters.PresentationInterval = PresentInterval.Immediate;
            parameters.BackBufferCount = 1;

            _device = new Device(new Direct3D(), 0, DeviceType.Hardware, Handle, CreateFlags.HardwareVertexProcessing, parameters);
            _camera.SetSize(Size.Width, Size.Height);
        }
        public void Render(IEnumerable<IMesh> meshes)
        {
            _device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Blue, 1.0f, 0);

            _device.SetRenderState(RenderState.FillMode, _wireframe ? FillMode.Wireframe : FillMode.Solid);
            _device.SetRenderState(RenderState.CullMode, _wireframe ? Cull.None : Cull.Counterclockwise);
            _device.SetRenderState(RenderState.Lighting, false);

            _device.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Anisotropic);
            _device.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Anisotropic);

            foreach (IMesh mesh in meshes)
            {
                _device.BeginScene();

                _device.SetTexture(0, mesh.BaseTexture);
                _device.SetStreamSource(0, mesh.VertexBuffer, 0, mesh.Stride);
                _device.VertexDeclaration = mesh.VertexDeclaration;
                _device.Indices = mesh.IndexBuffer;

                _device.PixelShader = mesh.PixelShader;
                _device.VertexShader = mesh.VertexShader;
 
                foreach (ShaderConstant constant in mesh.VertexShaderValues)
                    _device.SetVertexShaderConstant(constant.StartRegister, constant.Values);

                foreach (ShaderConstant constant in mesh.PixelShaderValues)
                    _device.SetPixelShaderConstant(constant.StartRegister, constant.Values);

                _device.DrawIndexedPrimitive(mesh.PrimitiveType, mesh.BaseVertexIndex, mesh.MinVertexIndex, mesh.VertexCount, mesh.StartIndex, mesh.PrimitiveCount);
                _device.EndScene();
            }

            _device.Present();
        }
        public VertexBuffer CreateVertexBuffer<T>(int sizeInBytes, T[] vertices) where T : struct
        {
            var buffer = new VertexBuffer(_device, sizeInBytes, Usage.WriteOnly, VertexFormat.None, Pool.Default);
            buffer.Lock(0, 0, LockFlags.None).WriteRange(vertices);
            buffer.Unlock();

            return buffer;
        }
        public VertexDeclaration CreateVertexDeclaration(VertexElement[] vertexElements)
        {
            var VertexDeclaration = new VertexDeclaration(_device, vertexElements);
            return VertexDeclaration;
        }
        public IndexBuffer CreateIndexBuffer(int sizeInBytes, int[] indexs)
        {
            var buffer = new IndexBuffer(_device, sizeInBytes, Usage.None, Pool.Managed, false);
            buffer.Lock(0, 0, LockFlags.None).WriteRange(indexs);
            buffer.Unlock();

            return buffer;
        }
        public BaseTexture CreateBaseTexture(string file)
        {
            var data = File.ReadAllBytes($@"{_root}\{file}");
            var texture = Texture.FromMemory(_device, data);
            var baseTexture = new BaseTexture(texture.NativePointer);

            return baseTexture;
        }
        public BaseTexture CreateBaseTextureFromCubeTexture(string file)
        {
            var data = File.ReadAllBytes($@"{_root}\{file}");
            var texture = CubeTexture.FromMemory(_device, data);
            var baseTexture = new CubeTexture(texture.NativePointer);

            return baseTexture;
        }
        public PixelShader CreatePixelShader(string file, string entryPoint)
        {
            var data = File.ReadAllBytes($@"{_root}\{file}");
            var result = ShaderBytecode.Compile(data, entryPoint, "ps_2_0", ShaderFlags.PackMatrixRowMajor);
            var pixelShader = new PixelShader(_device, result.Bytecode);

            return pixelShader;
        }
        public VertexShader CreateVertexShader(string file, string entryPoint)
        {
            var data = File.ReadAllBytes($@"{_root}\{file}");
            var result = ShaderBytecode.Compile(data, entryPoint, "vs_2_0", ShaderFlags.PackMatrixRowMajor);
            var vertexShader = new VertexShader(_device, result.Bytecode);

            return vertexShader;
        }
        public void CaptureInput()
        {
            var input = Input.Instance;

            if (input.KeyPress(Key.Escape))
            {
                IsAlive = false;
            }

            if(input.KeyPress(Key.F1))
            {
                _wireframe = !_wireframe;
            }
        }
        #endregion

        #endregion
    }
}
