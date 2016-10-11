using SharpDX;
using SharpDX.Direct3D9;
using System.Collections.Generic;

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
            string file = $@"Textures\{fileName}";
            var baseTexture = _device.CreateBaseTexture(file);

            _baseTextures.Add(baseTexture);
        }
        protected void LoadCubeTexture(string fileName)
        {
            string file = $@"Textures\{fileName}";
            var baseTexture = _device.CreateBaseTextureFromCubeTexture(file);

            _baseTextures.Add(baseTexture);
        }
        protected void LoadShaders(string vertexShaderFile, string pixelShaderFile, string vertexShaderFunction, string pixelShaderFunction)
        {
            string vertexshaderFile = $@"Shaders\Vertex\{vertexShaderFile}";
            string pixelshaderFile = $@"Shaders\Pixel\{pixelShaderFile}";

            PixelShader = _device.CreatePixelShader(pixelshaderFile, pixelShaderFunction);
            VertexShader = _device.CreateVertexShader(vertexshaderFile, vertexShaderFunction);
        }
        protected void LoadShadersValues()
        {
            var camera = Camera.Instance;
            VertexShaderValues.Clear();

            VertexShaderValues.Add(new ShaderConstant { StartRegister = 0, Values = camera.View.ToArray() });
            VertexShaderValues.Add(new ShaderConstant { StartRegister = 4, Values = camera.Projection.ToArray() });
            VertexShaderValues.Add(new ShaderConstant { StartRegister = 8, Values = _world.ToArray() });
        }
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
        public List<ShaderConstant> VertexShaderValues { get; set; }
        public List<ShaderConstant> PixelShaderValues { get; set; }
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

            VertexShaderValues = new List<ShaderConstant>();
            PixelShaderValues = new List<ShaderConstant>();
        }

        public virtual void Transform()
        {

        }
        #endregion

        #endregion
    }
}
