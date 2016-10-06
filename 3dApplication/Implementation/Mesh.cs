using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3dApplication
{
    public class Mesh : IMesh
    {
        #region Private

        #region Attributes
        protected IDevice _device;
        protected Matrix _world;
        protected int _textureIndex;
        private IList<BaseTexture> _baseTextures;
        #endregion

        #region Methods
        protected void LoadTexture(string fileName)
        {
            var data = File.ReadAllBytes(Application.StartupPath + $@"\Textures\{fileName}");
            var baseTexture = _device.CreateBaseTexture(data);
            _baseTextures.Add(baseTexture);
        }
        protected void LoadCubeTexture(string fileName)
        {
            var data = File.ReadAllBytes(Application.StartupPath + $@"\Textures\{fileName}");
            var baseTexture = _device.CreateBaseTextureFromCubeTexture(data);
            _baseTextures.Add(baseTexture);
        }
        protected void LoadShaders(string vertexShaderFile, string pixelShaderFile, string vertexShaderFunction, string pixelShaderFunction)
        {
            var dataVS = File.ReadAllBytes(Application.StartupPath + $@"\Shaders\Vertex\{vertexShaderFile}");
            var dataPS = File.ReadAllBytes(Application.StartupPath + $@"\Shaders\Pixel\{pixelShaderFile}");

            PixelShader = _device.CreatePixelShader(dataPS, pixelShaderFunction);
            VertexShader = _device.CreateVertexShader(dataVS, vertexShaderFunction);
        }
        protected void LoadShadersValues()
        {
            var camera = Camera.Instance;
            VertexShaderValues.Clear();
            VertexShaderValues.Add("View", camera.View);
            VertexShaderValues.Add("Projection", camera.Projection);
            VertexShaderValues.Add("World", _world);
        }

        //protected void LoadShadersValues()
        //{
        //    var camera = Camera.Instance;
        //    VertexShader.Function.ConstantTable.SetValue(VertexShader.Device, "View", camera.View);
        //    VertexShader.Function.ConstantTable.SetValue(VertexShader.Device, "Projection", camera.Projection);
        //    VertexShader.Function.ConstantTable.SetValue(VertexShader.Device, "World", _world);
        //}
        #endregion

        #endregion


        #region Public

        #region Attributes
        public int Stride { get; set; }
        public int PrimitiveCount { get; set; }
        public int StartIndex { get; set; }
        public int MinVertexIndex { get; set; }
        public int BaseVertexIndex { get; set; }
        public int VertexCount { get; set; }
        public BaseTexture BaseTexture
        {
            get
            {
                return _baseTextures[_textureIndex];
            }
        }
        public VertexDeclaration VertexDeclaration { get; set; }
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public PixelShader PixelShader { get; set; }
        public VertexShader VertexShader { get; set; }
        public PrimitiveType PrimitiveType { get; set; }
        public Dictionary<string, Matrix> VertexShaderValues { get; set; }
        public Dictionary<string, Matrix> PixelShaderValues { get; set; }
        #endregion

        #region Methods
        public Mesh()
        {
            _device = DXDevice.Instance;
            _world = Matrix.Identity;
            _textureIndex = 0;
            _baseTextures = new List<BaseTexture>();

            PrimitiveType = PrimitiveType.TriangleList;
            BaseVertexIndex = 0;
            MinVertexIndex = 0;
            StartIndex = 0;

            VertexShaderValues = new Dictionary<string, Matrix>();
            PixelShaderValues = new Dictionary<string, Matrix>();
        }

        public virtual void Transform()
        {

        }
        #endregion

        #endregion

    }
}
