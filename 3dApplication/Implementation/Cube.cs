using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace _3dApplication
{
    public class Cube : IMesh
    {
        #region Private

        #region Attributes
        private Vector3 _angle;
        private Vector3 _position;
        private Vector3 _center;
        private float _scale;
        #endregion

        #region Methods
        private void LoadProperties()
        {
            _scale = 2;
            _angle = new Vector3(0, 0, 0);
            PrimitiveType = PrimitiveType.TriangleList;
            BaseVertexIndex = 0;
            MinVertexIndex = 0;
            StartIndex = 0;
            Stride = Marshal.SizeOf<VertexTexture>();
            Transformation = Matrix.Identity;
        }
        private void LoadVertices(IDevice device)
        {
            VertexTexture[] vertices = new VertexTexture[24];

            //frente
            vertices[0] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 0.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(0, 1) };
            vertices[1] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 1.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(0, 0) };
            vertices[2] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 1.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(1, 0) };
            vertices[3] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 0.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(1, 1) };

            //contra frente
            vertices[4] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 0.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(0, 1) };
            vertices[5] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 1.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(0, 0) };
            vertices[6] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 1.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(1, 0) };
            vertices[7] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 0.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(1, 1) };

            //lateral izquierdo
            vertices[8] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 0.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(0, 1) };
            vertices[9] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 1.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(0, 0) };
            vertices[10] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 1.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(1, 0) };
            vertices[11] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 0.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(1, 1) };

            //lateral derecho
            vertices[12] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 0.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(0, 1) };
            vertices[13] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 1.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(0, 0) };
            vertices[14] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 1.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(1, 0) };
            vertices[15] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 0.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(1, 1) };

            //techo
            vertices[16] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 1.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(0, 1) };
            vertices[17] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 1.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(0, 0) };
            vertices[18] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 1.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(1, 0) };
            vertices[19] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 1.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(1, 1) };

            //piso
            vertices[20] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 0.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(0, 1) };
            vertices[21] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 0.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(0, 0) };
            vertices[22] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 0.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(1, 0) };
            vertices[23] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 0.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(1, 1) };

            NumVertices = vertices.Count();
            VertexBuffer = device.CreateVertexBuffer(Stride * NumVertices, vertices);
            CalculateCenter(vertices);
        }
        private void LoadIndexs(IDevice device)
        {
            IList<int> indexs = new List<int>();

            //frente
            indexs.Add(0);
            indexs.Add(1);
            indexs.Add(2);
            indexs.Add(0);
            indexs.Add(2);
            indexs.Add(3);

            //contra frente
            indexs.Add(5);
            indexs.Add(4);
            indexs.Add(7);
            indexs.Add(5);
            indexs.Add(7);
            indexs.Add(6);

            //lateral izquierdo
            indexs.Add(8);
            indexs.Add(9);
            indexs.Add(10);
            indexs.Add(8);
            indexs.Add(10);
            indexs.Add(11);

            //lateral derecho
            indexs.Add(12);
            indexs.Add(13);
            indexs.Add(14);
            indexs.Add(12);
            indexs.Add(14);
            indexs.Add(15);

            //techo
            indexs.Add(16);
            indexs.Add(17);
            indexs.Add(18);
            indexs.Add(16);
            indexs.Add(18);
            indexs.Add(19);

            //piso
            indexs.Add(20);
            indexs.Add(21);
            indexs.Add(22);
            indexs.Add(20);
            indexs.Add(22);
            indexs.Add(23);


            IndexBuffer = device.CreateIndexBuffer(sizeof(int) * indexs.Count, indexs.ToArray());
            PrimitiveCount = 2 * (int)(Math.Ceiling((double)indexs.Count/4));
        }
        private void LoadVertexDeclaration(IDevice device)
        {
            IList<VertexElement> vertexElements = new List<VertexElement>();
            vertexElements.Add(new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 0));
            vertexElements.Add(new VertexElement(0, (short)Vector3.SizeInBytes, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0));
            vertexElements.Add(VertexElement.VertexDeclarationEnd);

            VertexDeclaration = device.CreateVertexDeclaration(vertexElements.ToArray());
        }
        private void LoadTexture(IDevice device)
        {
            byte[] data = File.ReadAllBytes(Application.StartupPath + @"\Textures\crate.jpg");
            BaseTexture = device.CreateBaseTexture(data);
        }
        private void CalculateCenter(VertexTexture[] vertices)
        {
            Vector3 vertex = vertices.OrderByDescending(x => x.Position.Length()).First().Position;
            _center = new Vector3((vertex.X - _position.X) / 2 + _position.X, (vertex.Y - _position.Y) / 2 + _position.Y, (vertex.Z - _position.Z) / 2 + _position.Z);
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
        public Cube(IDevice device) : this(device, new int[] { 0, 0, 0 })
        {

        }
        public Cube(IDevice device, int[] position)
        {
            _position = new Vector3(position[0], position[1], position[2]);
            LoadProperties();
            LoadVertices(device);
            LoadIndexs(device);
            LoadVertexDeclaration(device);
            LoadTexture(device);
        }
        public void Rotate()
        {
            _angle.X += 0.05f;
            _angle.Y += 0.05f;
            //_angle.Z += 0.05f;

            var rotation = Matrix.RotationX(_angle.X) * Matrix.RotationY(_angle.Y) * Matrix.RotationZ(_angle.Z);
            Transformation = Matrix.Translation(-1 * _center) * Matrix.Scaling(_scale) * rotation * Matrix.Translation(_center);
        }

        #endregion

        #endregion
    }
}
