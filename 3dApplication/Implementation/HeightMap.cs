using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace _3dApplication
{
    public class HeightMap : IMesh
    {
        #region Private

        #region Attributes
        private int _height;
        private int _width;
        private float _heightFactor = 20.0f;
        private float _min = float.MaxValue;
        private int _textureIndex;
        private bool _diffuseMap;

        private int _stride;
        private float _scale;
        private int _verticesCount;
        private int _primitiveCount;
        private int _startIndex;
        private int _baseVertexIndex;
        private int _minVertexIndex;

        private Matrix _world;
        private Vector3 _angle;
        private Vector3 _position;
        private Vector3 _center;

        private IDevice _device;
        private Input _input;
        private VertexDeclaration _vertexDeclaration;
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private IList<BaseTexture> _baseTextures;
        private PixelShader _pixelShader;
        private VertexShader _vertexShader;
        private PrimitiveType _primitiveType;
        #endregion

        #region Methods
        private void LoadProperties()
        {
            _device = DXDevice.Instance();
            _input = Input.Instance();
            _primitiveType = PrimitiveType.TriangleList;
            _baseVertexIndex = 0;
            _minVertexIndex = 0;
            _startIndex = 0;
            _stride = Marshal.SizeOf<VertexTexture>();
            _world = Matrix.Identity;
        }
        private void LoadHeightMap()
        {
            var image = Image.FromFile(Application.StartupPath + @"\HeightMaps\Map01.bmp");
            var bitmap = new Bitmap(image);

            _height = image.Height;
            _width = image.Width;

            var heightmap = new float[_height, _width];
            for (int z = 0; z < _height; z++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var color = bitmap.GetPixel(x, z);
                    float height = (float)Math.Sqrt(Math.Pow(color.R, 2) + Math.Pow(color.G, 2) + Math.Pow(color.B, 2) + Math.Pow(color.A, 2));
                    heightmap[z, x] = height / _heightFactor;

                    if (heightmap[z, x] < _min)
                        _min = heightmap[z, x];
                }
            }

            image.Dispose();
            bitmap.Dispose();

            int index = 0;
            int indexBufferLength = (_width - 1) * (_height - 1) * 6;

            var vertices = new VertexTexture[(_height - 1) * (_width - 1) * 4];
            var indexs = new List<int>();

            for (int z = 0; z < (_height - 1); z++)
            {
                for (int x = 0; x < (_width - 1); x++)
                {
                    vertices[index] = new VertexTexture { Position = new Vector3(x, heightmap[z, x], z), UV = new Vector2(0f, 1f) };
                    vertices[index+1] = new VertexTexture { Position = new Vector3(x, heightmap[z+1, x], z+1), UV = new Vector2(0f, 0f) };
                    vertices[index+2] = new VertexTexture { Position = new Vector3(x+1, heightmap[z+1, x+1], z+1), UV = new Vector2(1f, 0f) };
                    vertices[index+3] = new VertexTexture { Position = new Vector3(x+1, heightmap[z, x+1], z), UV = new Vector2(1f, 1f) };

                    indexs.Add(index);
                    indexs.Add(index + 1);
                    indexs.Add(index + 3);

                    indexs.Add(index + 3);
                    indexs.Add(index + 1);
                    indexs.Add(index + 2);

                    index += 4;
                }
            }

            _primitiveCount = indexBufferLength / 3;
            _verticesCount = vertices.Count();

            _vertexBuffer = _device.CreateVertexBuffer(_stride * _verticesCount, vertices);
            _indexBuffer = _device.CreateIndexBuffer(sizeof(int) * indexs.Count(), indexs.ToArray());
        }
        private void LoadVertices()
        {
            var image = Image.FromFile(Application.StartupPath + @"\HeightMaps\Map01.bmp");
            var bitmap = new Bitmap(image);

            _height = image.Height;
            _width = image.Width;


            var heightmap = new float[_height, _width];
            for (int z = 0; z < _height; z++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var color = bitmap.GetPixel(x, z);
                    float height = (float)Math.Sqrt(Math.Pow(color.R, 2) + Math.Pow(color.G, 2) + Math.Pow(color.B, 2) + Math.Pow(color.A, 2));
                    heightmap[z, x] = height / _heightFactor;

                    if (heightmap[z, x] < _min)
                        _min = heightmap[z, x];
                }
            }

            image.Dispose();
            bitmap.Dispose();

            int index = 0;
            var vertices = new VertexTexture[_height * _width];
            for (int z = 0; z < _height; z++)
            {
                for (int x = 0; x < _width; x++)
                {
                    vertices[index] = new VertexTexture { Position = new Vector3(x, heightmap[z, x], z), UV = new Vector2((float)x / _width, (float)z / _height) };
                    index++;
                }
            }

            _verticesCount = vertices.Count();
            _vertexBuffer = _device.CreateVertexBuffer(_stride * _verticesCount, vertices);
        }
        private void LoadIndexs()
        {
            int indexLength = (_width - 1) * (_height - 1) * 6;
            _primitiveCount = indexLength / 3;
            var indexs = new int[indexLength];

            int index = 0;
            for (int z = 0; z < (_height - 1); z++)
            {
                for (int x = 0; x < (_width - 1); x++)
                {
                    indexs[index++] = x + _width * z;
                    indexs[index++] = x + _width * (z + 1);
                    indexs[index++] = (x + 1) + _width * z;

                    indexs[index++] = (x + 1) + _width * z;
                    indexs[index++] = x + _width * (z + 1);
                    indexs[index++] = (x + 1) + _width * (z + 1);
                }
            }

            _indexBuffer = _device.CreateIndexBuffer(sizeof(int) * indexs.Count(), indexs.ToArray());
        }
        private void LoadVertexDeclaration()
        {
            var vertexElements = new List<VertexElement>();
            vertexElements.Add(new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0));
            vertexElements.Add(new VertexElement(0, (short)Vector3.SizeInBytes, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0));
            vertexElements.Add(VertexElement.VertexDeclarationEnd);

            _vertexDeclaration = _device.CreateVertexDeclaration(vertexElements.ToArray());
        }
        private void LoadTexture(string terrain)
        {
            var data = File.ReadAllBytes(Application.StartupPath + @"\Textures\" + terrain);
            var texture = _device.CreateBaseTexture(data);
            _baseTextures.Add(texture);
        }
        private void LoadShaders()
        {
            var dataVS = File.ReadAllBytes(Application.StartupPath + @"\Shaders\Vertex\Texture.vs");
            var dataPS = File.ReadAllBytes(Application.StartupPath + @"\Shaders\Pixel\Texture.ps");

            _pixelShader = _device.CreatePixelShader(dataPS, "TexturePixel");
            _vertexShader = _device.CreateVertexShader(dataVS, "TextureAndTransform");
        }
        private void CalculateCenter()
        {
            _center = new Vector3(_width / 2, _min, _height / 2);
        }
        #endregion

        #endregion

        #region Public

        #region Attributes

        #endregion

        #region Methods
        public HeightMap() : this(false)
        {

        }
        public HeightMap(bool diffuseMap)
        {
            _textureIndex = 0;
            _baseTextures = new List<BaseTexture>();
            _diffuseMap = diffuseMap;

            LoadProperties();

            if(_diffuseMap)
            {
                LoadVertices();
                LoadIndexs();
                LoadTexture("DiffuseMap01.jpg");
            }
            else
            {
                LoadHeightMap();
                LoadTexture("TerrainTest.jpg");
                LoadTexture("Terrain01.jpg");
            }

            LoadVertexDeclaration();

            LoadShaders();
            CalculateCenter();
        }
        public void Transform()
        {
            _world = Matrix.Translation(-1*_center);

            if(!_diffuseMap && _input.KeyDown(Key.D1))
            {
                _textureIndex = 0;
            }

            if (!_diffuseMap && _input.KeyDown(Key.D2))
            {
                _textureIndex = 1;
            }
        }
        public int GetWidth()
        {
            return _width;
        }
        public void Render()
        {
            _device.Render(_stride, _primitiveCount, _startIndex, _minVertexIndex, _baseVertexIndex, _verticesCount, _baseTextures[_textureIndex], _vertexDeclaration, _vertexBuffer, _indexBuffer, _pixelShader, _vertexShader, _world, _primitiveType);
        }
        #endregion

        #endregion
    }
}
