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
    public class Skybox : Mesh
    {
        #region Private

        #region Attributes
        private float _scale;
        private float _widthTextureOffset = (1f / 4f);
        private float _heightTextureOffset = (1f / 3f);
        private Vector3 _center;
        #endregion

        #region Methods
        private void LoadProperties()
        {
            _scale = 20;
            Stride = Marshal.SizeOf<VertexTexture>();
        }
        private void LoadVertices()
        {
            var vertices = new VertexTexture[24];

            //frente
            vertices[0] = new VertexTexture { Position = new Vector3(0.0f, 0.0f, 1.0f), UV = new Vector2(_widthTextureOffset, 2 * _heightTextureOffset) };
            vertices[1] = new VertexTexture { Position = new Vector3(0.0f, 1.0f, 1.0f), UV = new Vector2(_widthTextureOffset, _heightTextureOffset) };
            vertices[2] = new VertexTexture { Position = new Vector3(1.0f, 1.0f, 1.0f), UV = new Vector2(2 * _widthTextureOffset, _heightTextureOffset) };
            vertices[3] = new VertexTexture { Position = new Vector3(1.0f, 0.0f, 1.0f), UV = new Vector2(2 * _widthTextureOffset, 2 * _heightTextureOffset) };

            //contra frente
            vertices[4] = new VertexTexture { Position = new Vector3(1.0f, 0.0f, 0.0f), UV = new Vector2(3 * _widthTextureOffset, 2 * _heightTextureOffset) };
            vertices[5] = new VertexTexture { Position = new Vector3(1.0f, 1.0f, 0.0f), UV = new Vector2(3 * _widthTextureOffset, _heightTextureOffset) };
            vertices[6] = new VertexTexture { Position = new Vector3(0.0f, 1.0f, 0.0f), UV = new Vector2(4 * _widthTextureOffset, _heightTextureOffset) };
            vertices[7] = new VertexTexture { Position = new Vector3(0.0f, 0.0f, 0.0f), UV = new Vector2(4 * _widthTextureOffset, 2 * _heightTextureOffset) };

            //lateral izquierdo
            vertices[8] = new VertexTexture { Position = new Vector3(0.0f, 0.0f, 0.0f), UV = new Vector2(0, 2 * _heightTextureOffset) };
            vertices[9] = new VertexTexture { Position = new Vector3(0.0f, 1.0f, 0.0f), UV = new Vector2(0, _heightTextureOffset) };
            vertices[10] = new VertexTexture { Position = new Vector3(0.0f, 1.0f, 1.0f), UV = new Vector2(_widthTextureOffset, _heightTextureOffset) };
            vertices[11] = new VertexTexture { Position = new Vector3(0.0f, 0.0f, 1.0f), UV = new Vector2(_widthTextureOffset, 2 * _heightTextureOffset) };

            //lateral derecho
            vertices[12] = new VertexTexture { Position = new Vector3(1.0f, 0.0f, 1.0f), UV = new Vector2(2 * _widthTextureOffset, 2 * _heightTextureOffset) };
            vertices[13] = new VertexTexture { Position = new Vector3(1.0f, 1.0f, 1.0f), UV = new Vector2(2 * _widthTextureOffset, _heightTextureOffset) };
            vertices[14] = new VertexTexture { Position = new Vector3(1.0f, 1.0f, 0.0f), UV = new Vector2(3 * _widthTextureOffset, _heightTextureOffset) };
            vertices[15] = new VertexTexture { Position = new Vector3(1.0f, 0.0f, 0.0f), UV = new Vector2(3 * _widthTextureOffset, 2 * _heightTextureOffset) };

            //techo
            vertices[16] = new VertexTexture { Position = new Vector3(0.0f, 1.0f, 1.0f), UV = new Vector2(_widthTextureOffset, _heightTextureOffset) };
            vertices[17] = new VertexTexture { Position = new Vector3(0.0f, 1.0f, 0.0f), UV = new Vector2(_widthTextureOffset, 0) };
            vertices[18] = new VertexTexture { Position = new Vector3(1.0f, 1.0f, 0.0f), UV = new Vector2(2 * _widthTextureOffset, 0) };
            vertices[19] = new VertexTexture { Position = new Vector3(1.0f, 1.0f, 1.0f), UV = new Vector2(2 * _widthTextureOffset, _heightTextureOffset) };

            //piso
            vertices[20] = new VertexTexture { Position = new Vector3(0.0f, 0.0f, 0.0f), UV = new Vector2(_widthTextureOffset, 3 * _heightTextureOffset) };
            vertices[21] = new VertexTexture { Position = new Vector3(0.0f, 0.0f, 1.0f), UV = new Vector2(_widthTextureOffset, 2 * _heightTextureOffset) };
            vertices[22] = new VertexTexture { Position = new Vector3(1.0f, 0.0f, 1.0f), UV = new Vector2(2 * _widthTextureOffset, 2 * _heightTextureOffset) };
            vertices[23] = new VertexTexture { Position = new Vector3(1.0f, 0.0f, 0.0f), UV = new Vector2(2 * _widthTextureOffset, 3 * _heightTextureOffset) };

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
            indexs.Add(4);
            indexs.Add(5);
            indexs.Add(6);
            indexs.Add(4);
            indexs.Add(6);
            indexs.Add(7);

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
            PrimitiveCount = 2 * (int)(Math.Ceiling((double)indexs.Count / 4));
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
            _center = new Vector3(vertex.X / 2, vertex.Y / 2, vertex.Z / 2);
        }
        #endregion

        #endregion

        #region Public

        #region Attributes

        #endregion

        #region Methods
        public Skybox()
        {
            LoadProperties();
            LoadVertices();
            LoadIndexs();
            LoadTexture("Skybox.jpg");
            LoadVertexDeclaration();
            LoadShaders("Skybox.vs", "Skybox.ps", "TextureAndTransform", "TexturePixel");
        }
        public override void Transform()
        {
            _world = Matrix.Translation(-1 * _center) * Matrix.Scaling(_scale) * Matrix.Translation(_center);
            LoadShadersValues();
        }
        public void SetSize(int mapWidth)
        {
            _scale = 10 * mapWidth;
        }
        #endregion

        #endregion
    }
}
