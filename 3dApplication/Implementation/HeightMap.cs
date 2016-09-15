﻿using SharpDX;
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

        #region Attributes
        private int _height;
        private int _width;
        private Vector3 _center;
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
                    heightmap[z, x] = height / 30.0f;
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

        private void LoadVertexDeclaration(IDevice device)
        {
            IList<VertexElement> vertexElements = new List<VertexElement>();
            vertexElements.Add(new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0));
            vertexElements.Add(new VertexElement(0, (short)Vector3.SizeInBytes, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0));
            vertexElements.Add(VertexElement.VertexDeclarationEnd);

            VertexDeclaration = device.CreateVertexDeclaration(vertexElements.ToArray());
        }
        private void LoadTexture(IDevice device)
        {
            byte[] data = File.ReadAllBytes(Application.StartupPath + @"\Textures\TerrainTest.jpg");
            BaseTexture = device.CreateBaseTexture(data);
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
            LoadHeightMap(device);
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
