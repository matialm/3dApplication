using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace _3dApplication
{
    public class Cube : Mesh
    {
        #region Private

        #region Attributes
        private float _scale;
        private Vector3 _angle;
        private Vector3 _position;
        private Vector3 _center;
        #endregion

        #region Methods
        private void LoadProperties()
        {
            _scale = 2;
            _angle = new Vector3(0, 0, 0);
            Stride = Marshal.SizeOf<VertexTexture>();
        }
        private void LoadVertices()
        {
            var vertices = new VertexTexture[24];

            //frente
            vertices[0] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 0.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(0, 1) };
            vertices[1] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 1.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(0, 0) };
            vertices[2] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 1.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(1, 0) };
            vertices[3] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 0.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(1, 1) };

            //contra frente
            vertices[4] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 0.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(0, 1) };
            vertices[5] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 1.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(0, 0) };
            vertices[6] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 1.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(1, 0) };
            vertices[7] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 0.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(1, 1) };

            //lateral izquierdo
            vertices[8] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 0.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(0, 1) };
            vertices[9] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 1.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(0, 0) };
            vertices[10] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 1.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(1, 0) };
            vertices[11] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 0.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(1, 1) };

            //lateral derecho
            vertices[12] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 0.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(0, 1) };
            vertices[13] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 1.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(0, 0) };
            vertices[14] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 1.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(1, 0) };
            vertices[15] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 0.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(1, 1) };

            //techo
            vertices[16] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 1.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(0, 1) };
            vertices[17] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 1.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(0, 0) };
            vertices[18] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 1.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(1, 0) };
            vertices[19] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 1.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(1, 1) };

            //piso
            vertices[20] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 0.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(0, 1) };
            vertices[21] = new VertexTexture { Position = new Vector3(0.0f + _position.X, 0.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(0, 0) };
            vertices[22] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 0.0f + _position.Y, 0.0f + _position.Z), UV = new Vector2(1, 0) };
            vertices[23] = new VertexTexture { Position = new Vector3(1.0f + _position.X, 0.0f + _position.Y, 1.0f + _position.Z), UV = new Vector2(1, 1) };

            VertexCount = vertices.Count();
            VertexBuffer = _device.CreateVertexBuffer(Stride * VertexCount, vertices);
            CalculateCenter(vertices);
        }
        private void LoadIndexs()
        {
            var indexs = new List<int>();

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

            IndexBuffer = _device.CreateIndexBuffer(sizeof(int) * indexs.Count, indexs.ToArray());
            PrimitiveCount = 2 * (int)(Math.Ceiling((double)indexs.Count/4));
        }
        private void LoadVertexDeclaration()
        {
            var vertexElements = new List<VertexElement>();
            vertexElements.Add(new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0));
            vertexElements.Add(new VertexElement(0, (short)Vector3.SizeInBytes, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0));
            vertexElements.Add(VertexElement.VertexDeclarationEnd);

            VertexDeclaration = _device.CreateVertexDeclaration(vertexElements.ToArray());
        }
        private void CalculateCenter(VertexTexture[] vertices)
        {
            var vertex = vertices.OrderByDescending(x => x.Position.Length()).First().Position;
            _center = new Vector3((vertex.X - _position.X) / 2 + _position.X, (vertex.Y - _position.Y) / 2 + _position.Y, (vertex.Z - _position.Z) / 2 + _position.Z);
        }
        #endregion

        #endregion

        #region Public

        #region Attributes

        #endregion

        #region Methods
        public Cube() : this(new int[] { 0, 1, 0 })
        {

        }
        public Cube(int[] position)
        {
            _position = new Vector3(position[0], position[1], position[2]);
            LoadProperties();
            LoadVertices();
            LoadIndexs();
            LoadVertexDeclaration();
            LoadTexture("crate.jpg");
            LoadShaders("Texture.vs", "Texture.ps", "TextureAndTransform", "TexturePixel");
        }
        public override void Transform()
        {
            _angle.X += 0.05f;
            _angle.Y += 0.05f;
            //_angle.Z += 0.05f;

            var rotation = Matrix.RotationX(_angle.X) * Matrix.RotationY(_angle.Y) * Matrix.RotationZ(_angle.Z);
            _world = Matrix.Translation(-1 * _center) * Matrix.Scaling(_scale) * rotation * Matrix.Translation(_center);
            LoadShadersValues();
        }

        #endregion

        #endregion
    }
}
