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
    public class HeightMap : Mesh
    {
        #region Private

        #region Attributes
        private int _height;
        private int _width;
        private float _heightFactor = 20.0f;
        private float _min = float.MaxValue;
        private bool _diffuseMap;
        private Vector3 _center;
        private Input _input;
        #endregion

        #region Methods
        private void LoadProperties()
        {
            _input = Input.Instance;
            Stride = Marshal.SizeOf<VertexTexture>();
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

            PrimitiveCount = indexBufferLength / 3;
            VertexCount = vertices.Count();

            VertexBuffer = _device.CreateVertexBuffer(Stride * VertexCount, vertices);
            IndexBuffer = _device.CreateIndexBuffer(sizeof(int) * indexs.Count(), indexs.ToArray());
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

            VertexCount = vertices.Count();
            VertexBuffer = _device.CreateVertexBuffer(Stride * VertexCount, vertices);
        }
        private void LoadIndexs()
        {
            int indexLength = (_width - 1) * (_height - 1) * 6;
            PrimitiveCount = indexLength / 3;
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

            IndexBuffer = _device.CreateIndexBuffer(sizeof(int) * indexs.Count(), indexs.ToArray());
        }
        private void LoadVertexDeclaration()
        {
            var vertexElements = new List<VertexElement>();
            vertexElements.Add(new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0));
            vertexElements.Add(new VertexElement(0, (short)Vector3.SizeInBytes, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0));
            vertexElements.Add(VertexElement.VertexDeclarationEnd);

            VertexDeclaration = _device.CreateVertexDeclaration(vertexElements.ToArray());
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

            LoadShaders("Texture.vs", "Texture.ps", "TextureAndTransform", "TexturePixel");
            CalculateCenter();
        }
        public override void Transform()
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

            LoadShadersValues();
        }
        public int GetWidth()
        {
            return _width;
        }
        #endregion

        #endregion
    }
}
