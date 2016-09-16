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
        private Vector3 _center;
        private bool _diffuseMap;
        private IList<BaseTexture> _baseTextures;
        #endregion

        #region Methods
        private void LoadProperties()
        {
            PrimitiveType = PrimitiveType.TriangleList;
            BaseVertexIndex = 0;
            MinVertexIndex = 0;
            StartIndex = 0;
            Stride = Marshal.SizeOf<VertexTexture>();
            Transformation = Matrix.Identity;
        }
        private void LoadHeightMap(IDevice device)
        {
            Image image = Image.FromFile(Application.StartupPath + @"\HeightMaps\Map01.bmp");
            Bitmap bitmap = new Bitmap(image);

            _height = image.Height;
            _width = image.Width;

            float[,] heightmap = new float[_height, _width];
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

            VertexTexture[] vertices = new VertexTexture[(_height - 1) * (_width - 1) * 4];
            IList<int> indexs = new List<int>();

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
            NumVertices = vertices.Count();

            VertexBuffer = device.CreateVertexBuffer(Stride * NumVertices, vertices);
            IndexBuffer = device.CreateIndexBuffer(sizeof(int) * indexs.Count(), indexs.ToArray());
        }
        private void LoadVertices(IDevice device)
        {
            Image image = Image.FromFile(Application.StartupPath + @"\HeightMaps\Map01.bmp");
            Bitmap bitmap = new Bitmap(image);

            _height = image.Height;
            _width = image.Width;


            float[,] heightmap = new float[_height, _width];

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
            VertexTexture[] vertices = new VertexTexture[_height * _width];
            for (int z = 0; z < _height; z++)
            {
                for (int x = 0; x < _width; x++)
                {
                    vertices[index] = new VertexTexture { Position = new Vector3(x, heightmap[z, x], z), UV = new Vector2((float)x / _width, (float)z / _height) };
                    index++;
                }
            }

            NumVertices = vertices.Count();
            VertexBuffer = device.CreateVertexBuffer(Stride * NumVertices, vertices);
        }
        private void LoadIndexs(IDevice device)
        {
            int indexLength = (_width - 1) * (_height - 1) * 6;
            PrimitiveCount = indexLength / 3;
            int[] indexs = new int[indexLength];

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

            IndexBuffer = device.CreateIndexBuffer(sizeof(int) * indexs.Count(), indexs.ToArray());
        }
        private void LoadVertexDeclaration(IDevice device)
        {
            IList<VertexElement> vertexElements = new List<VertexElement>();
            vertexElements.Add(new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0));
            vertexElements.Add(new VertexElement(0, (short)Vector3.SizeInBytes, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0));
            vertexElements.Add(VertexElement.VertexDeclarationEnd);

            VertexDeclaration = device.CreateVertexDeclaration(vertexElements.ToArray());
        }
        private void LoadTexture(IDevice device, string terrain)
        {
            byte[] data = File.ReadAllBytes(Application.StartupPath + @"\Textures\" + terrain);
            BaseTexture texture = device.CreateBaseTexture(data);
            _baseTextures.Add(texture);
        }
        private void CalculateCenter()
        {
            _center = new Vector3(_width / 2, _min, _height / 2);
        }
        #endregion

        #endregion

        #region Public

        #region Attributes
        public VertexDeclaration VertexDeclaration { get; set; }
        public PrimitiveType PrimitiveType { get; set; }
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public BaseTexture BaseTexture { get { return _baseTextures[_textureIndex]; } }
        public Matrix Transformation { get; set; }
        public int BaseVertexIndex { get; set; }
        public int MinVertexIndex { get; set; }
        public int NumVertices { get; set; }
        public int StartIndex { get; set; }
        public int PrimitiveCount { get; set; }
        public int Stride { get; set; }
        public Input Input { get; set; }
        #endregion

        #region Methods
        public HeightMap(IDevice device) : this(device, false)
        {

        }
        public HeightMap(IDevice device, bool diffuseMap)
        {
            _textureIndex = 0;
            _baseTextures = new List<BaseTexture>();
            _diffuseMap = diffuseMap;

            LoadProperties();

            if(_diffuseMap)
            {
                LoadVertices(device);
                LoadIndexs(device);
            }
            else
                LoadHeightMap(device);

            LoadVertexDeclaration(device);

            if (!_diffuseMap)
            {
                LoadTexture(device, "TerrainTest.jpg");
                LoadTexture(device, "Terrain01.jpg");
            }
            else
                LoadTexture(device, "DiffuseMap01.jpg");

            CalculateCenter();
        }
        public void Transform()
        {
            Transformation = Matrix.Translation(-1*_center);

            if(Input.KeyDown(Key.D1))
            {
                _textureIndex = 0;
            }

            if (Input.KeyDown(Key.D2))
            {
                _textureIndex = 1;
            }
        }

        #endregion

        #endregion
    }
}
