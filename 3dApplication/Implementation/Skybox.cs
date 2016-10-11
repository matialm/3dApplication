using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace _3dApplication
{
    public class Skybox : Mesh
    {
        #region Private

        #region Attributes
        private float _scale;
        private Vector3 _center;
        #endregion

        #region Methods
        private void LoadProperties()
        {
            _scale = 20;
            Stride = Marshal.SizeOf<Vertex>();
        }
        private void LoadVertices()
        {
            var vertices = new Vertex[24];

            //frente
            vertices[0] = new Vertex { Position = new Vector3(0.0f, 0.0f, 1.0f) };
            vertices[1] = new Vertex { Position = new Vector3(0.0f, 1.0f, 1.0f) };
            vertices[2] = new Vertex { Position = new Vector3(1.0f, 1.0f, 1.0f) };
            vertices[3] = new Vertex { Position = new Vector3(1.0f, 0.0f, 1.0f) };

            //contra frente
            vertices[4] = new Vertex { Position = new Vector3(1.0f, 0.0f, 0.0f) };
            vertices[5] = new Vertex { Position = new Vector3(1.0f, 1.0f, 0.0f) };
            vertices[6] = new Vertex { Position = new Vector3(0.0f, 1.0f, 0.0f) };
            vertices[7] = new Vertex { Position = new Vector3(0.0f, 0.0f, 0.0f) };

            //lateral izquierdo
            vertices[8] = new Vertex { Position = new Vector3(0.0f, 0.0f, 0.0f) };
            vertices[9] = new Vertex { Position = new Vector3(0.0f, 1.0f, 0.0f) };
            vertices[10] = new Vertex { Position = new Vector3(0.0f, 1.0f, 1.0f) };
            vertices[11] = new Vertex { Position = new Vector3(0.0f, 0.0f, 1.0f) };

            //lateral derecho
            vertices[12] = new Vertex { Position = new Vector3(1.0f, 0.0f, 1.0f) };
            vertices[13] = new Vertex { Position = new Vector3(1.0f, 1.0f, 1.0f) };
            vertices[14] = new Vertex { Position = new Vector3(1.0f, 1.0f, 0.0f) };
            vertices[15] = new Vertex { Position = new Vector3(1.0f, 0.0f, 0.0f) };

            //techo
            vertices[16] = new Vertex { Position = new Vector3(0.0f, 1.0f, 1.0f) };
            vertices[17] = new Vertex { Position = new Vector3(0.0f, 1.0f, 0.0f) };
            vertices[18] = new Vertex { Position = new Vector3(1.0f, 1.0f, 0.0f) };
            vertices[19] = new Vertex { Position = new Vector3(1.0f, 1.0f, 1.0f) };

            //piso
            vertices[20] = new Vertex { Position = new Vector3(0.0f, 0.0f, 0.0f) };
            vertices[21] = new Vertex { Position = new Vector3(0.0f, 0.0f, 1.0f) };
            vertices[22] = new Vertex { Position = new Vector3(1.0f, 0.0f, 1.0f) };
            vertices[23] = new Vertex { Position = new Vector3(1.0f, 0.0f, 0.0f) };

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
            vertexElements.Add(VertexElement.VertexDeclarationEnd);

            VertexDeclaration = _device.CreateVertexDeclaration(vertexElements.ToArray());
        }
        private void CalculateCenter(Vertex[] vertices)
        {
            var vertex = vertices.OrderByDescending(x => x.Position.Length()).First().Position;
            _center = new Vector3(vertex.X / 2, vertex.Y / 2, vertex.Z / 2);
        }

        private new void LoadShadersValues()
        {
            base.LoadShadersValues();
            VertexShaderValues.Add(new ShaderConstant { StartRegister = 12, Values = Camera.Instance.Position.ToArray() });
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
            LoadCubeTexture("Skybox.dds");
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
