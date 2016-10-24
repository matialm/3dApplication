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

        #endregion

        #region Methods
        private void LoadProperties()
        {
            Stride = Marshal.SizeOf<Vertex>();
        }
        private void LoadVertices()
        {
            var vertices = new Vertex[8];

            //frente
            vertices[0] = new Vertex { Position = new Vector3(-1.0f, -1.0f, 1.0f) };
            vertices[1] = new Vertex { Position = new Vector3(-1.0f, 1.0f, 1.0f) };
            vertices[2] = new Vertex { Position = new Vector3(1.0f, 1.0f, 1.0f) };
            vertices[3] = new Vertex { Position = new Vector3(1.0f, -1.0f, 1.0f) };

            //contra frente
            vertices[4] = new Vertex { Position = new Vector3(-1.0f, -1.0f, -1.0f) };
            vertices[5] = new Vertex { Position = new Vector3(-1.0f, 1.0f, -1.0f) };
            vertices[6] = new Vertex { Position = new Vector3(1.0f, 1.0f, -1.0f) };
            vertices[7] = new Vertex { Position = new Vector3(1.0f, -1.0f, -1.0f) };

            VertexCount = vertices.Count();
            VertexBuffer = _device.CreateVertexBuffer(Stride * VertexCount, vertices);
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
            indexs.Add(7);
            indexs.Add(6);
            indexs.Add(5);
            indexs.Add(7);
            indexs.Add(5);
            indexs.Add(4);

            //lateral izquierdo
            indexs.Add(4);
            indexs.Add(5);
            indexs.Add(1);
            indexs.Add(4);
            indexs.Add(1);
            indexs.Add(0);

            //lateral derecho
            indexs.Add(3);
            indexs.Add(2);
            indexs.Add(6);
            indexs.Add(3);
            indexs.Add(6);
            indexs.Add(7);

            //techo
            indexs.Add(1);
            indexs.Add(5);
            indexs.Add(6);
            indexs.Add(1);
            indexs.Add(6);
            indexs.Add(2);

            //piso
            indexs.Add(4);
            indexs.Add(0);
            indexs.Add(3);
            indexs.Add(4);
            indexs.Add(3);
            indexs.Add(7);

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
            LoadShadersValues();
        }
        #endregion

        #endregion
    }
}
