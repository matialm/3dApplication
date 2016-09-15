using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Color = SharpDX.Color;

namespace _3dApplication
{
    public class HeightMap : IMesh
    {
        #region Private
        private int _height;
        private int _width;
        private Vector3 _center;
        #region Attributes

        #endregion

        #region Methods
        private void LoadProperties()
        {
            PrimitiveType = PrimitiveType.TriangleList;
            BaseVertexIndex = 0;
            MinVertexIndex = 0;
            StartIndex = 0;
            Stride = Marshal.SizeOf<VertexColored>();
            Transformation = Matrix.Identity;
        }
        private void LoadVertices(IDevice device)
        {
            Image image = Image.FromFile(Application.StartupPath + @"\HeightMaps\Map01.bmp");
            Bitmap bitmap = new Bitmap(image);

            _height = image.Height;
            _width = image.Width;


            HeightMapElement[,] heightmap = new HeightMapElement[_height, _width];

            for (int z = 0; z < _height; z++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var color = bitmap.GetPixel(x, z);
                    float height = (float)Math.Sqrt(Math.Pow(color.R, 2) + Math.Pow(color.G, 2) + Math.Pow(color.B, 2) + Math.Pow(color.A, 2));
                    heightmap[z, x] = new HeightMapElement { Height = height / 30.0f, Color = Color.FromRgba(color.ToArgb()) };
                }
            }

            image.Dispose();
            bitmap.Dispose();

            int index = 0;
            VertexColored[] vertices = new VertexColored[_height * _width];
            for(int z = 0; z < _height; z++)
            {
                for(int x = 0; x < _width; x++)
                {
                    vertices[index] = new VertexColored { Position = new Vector3(x, heightmap[z, x].Height, z), Color = heightmap[z, x].Color };
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
            for (int z = 0; z < (_height-1); z++)
            {
                for (int x = 0; x < (_width-1); x++)
                {
                    indexs[index++] = x + _width * z;
                    indexs[index++] = x + _width * (z+1);
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
            vertexElements.Add(new VertexElement(0, (short)Vector3.SizeInBytes, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0));
            vertexElements.Add(VertexElement.VertexDeclarationEnd);

            VertexDeclaration = device.CreateVertexDeclaration(vertexElements.ToArray());
        }
        private void LoadTexture(IDevice device)
        {
            //byte[] data = File.ReadAllBytes(Application.StartupPath + @"\Textures\TerrainTest.jpg");
            //BaseTexture = device.CreateBaseTexture(data);
            BaseTexture = null;
        }
        private void CalculateCenter()
        {
            _center = new Vector3((float)(_width / 2), 0, (float)(_height / 2));
        }
        #endregion

        #endregion

        #region Public

        #region Attributes
        public VertexDeclaration VertexDeclaration { get; set; }
        public PrimitiveType PrimitiveType { get; set; }
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public BaseTexture BaseTexture { get; set; }
        public Matrix Transformation { get; set; }
        public int BaseVertexIndex { get; set; }
        public int MinVertexIndex { get; set; }
        public int NumVertices { get; set; }
        public int StartIndex { get; set; }
        public int PrimitiveCount { get; set; }
        public int Stride { get; set; }
        #endregion

        #region Methods
        public HeightMap(IDevice device)
        {
            LoadProperties();
            LoadVertices(device);
            LoadIndexs(device);
            LoadVertexDeclaration(device);
            LoadTexture(device);
            CalculateCenter();
        }
        public void Transform()
        {
            Transformation = Matrix.Translation(-1*_center);
        }

        #endregion

        #endregion
    }
}
