using SharpDX;
using SharpDX.Direct3D9;
using System.Collections.Generic;
using System.Linq;

namespace _3dApplication
{
    public class Cube : IMesh
    {
        #region Private

        #region Attributes
        private Vector3 _angle;
        private Vector3 _position;
        private Vector3 _center;
        #endregion

        #region Methods
        private void LoadProperties()
        {
            _angle = new Vector3(0, 0, 0);
            PrimitiveType = PrimitiveType.TriangleList;
            BaseVertexIndex = 0;
            MinVertexIndex = 0;
            StartIndex = 0;
            Stride = (Vector4.SizeInBytes + 4);
            Transformation = Matrix.Identity;
        }
        private void LoadVertices(IDevice device)
        {
            IList<Vertex> vertices = new List<Vertex>();

            vertices.Add(new Vertex { Color = Color.Blue, Position = new Vector4(0.0f + _position.X, 0.0f + _position.Y, 2.0f + _position.Z, 1.0f) });
            vertices.Add(new Vertex { Color = Color.Red, Position = new Vector4(0.0f + _position.X, 2.0f + _position.Y, 2.0f + _position.Z, 1.0f) });
            vertices.Add(new Vertex { Color = Color.Magenta, Position = new Vector4(2.0f + _position.X, 2.0f + _position.Y, 2.0f + _position.Z, 1.0f) });
            vertices.Add(new Vertex { Color = Color.Yellow, Position = new Vector4(2.0f + _position.X, 0.0f + _position.Y, 2.0f + _position.Z, 1.0f) });
            vertices.Add(new Vertex { Color = Color.Green, Position = new Vector4(0.0f + _position.X, 0.0f + _position.Y, 0.0f + _position.Z, 1.0f) });
            vertices.Add(new Vertex { Color = Color.Gray, Position = new Vector4(0.0f + _position.X, 2.0f + _position.Y, 0.0f + _position.Z, 1.0f) });
            vertices.Add(new Vertex { Color = Color.Green, Position = new Vector4(2.0f + _position.X, 2.0f + _position.Y, 0.0f + _position.Z, 1.0f) });
            vertices.Add(new Vertex { Color = Color.Pink, Position = new Vector4(2.0f + _position.X, 0.0f + _position.Y, 0.0f + _position.Z, 1.0f) });

            NumVertices = vertices.Count;
            VertexBuffer = device.CreateVertexBuffer(Stride * NumVertices, vertices.ToArray());
            CalculateCenter(vertices);
        }
        private void LoadIndexs(IDevice device)
        {
            IList<int> indexs = new List<int>();

            //frente
            indexs.Add(0);
            indexs.Add(1);
            indexs.Add(3);
            indexs.Add(3);
            indexs.Add(1);
            indexs.Add(2);

            //contra frente
            indexs.Add(5);
            indexs.Add(4);
            indexs.Add(6);
            indexs.Add(6);
            indexs.Add(4);
            indexs.Add(7);

            //lateral izquierdo
            indexs.Add(4);
            indexs.Add(5);
            indexs.Add(0);
            indexs.Add(0);
            indexs.Add(5);
            indexs.Add(1);

            //lateral derecho
            indexs.Add(3);
            indexs.Add(2);
            indexs.Add(7);
            indexs.Add(7);
            indexs.Add(2);
            indexs.Add(6);

            //techo
            indexs.Add(1);
            indexs.Add(5);
            indexs.Add(2);
            indexs.Add(2);
            indexs.Add(5);
            indexs.Add(6);

            //piso
            indexs.Add(4);
            indexs.Add(0);
            indexs.Add(7);
            indexs.Add(7);
            indexs.Add(0);
            indexs.Add(3);

            IndexBuffer = device.CreateIndexBuffer(sizeof(int) * indexs.Count, indexs.ToArray());
            PrimitiveCount = (indexs.Count / 6) * 2;
        }
        private void LoadVertexDeclaration(IDevice device)
        {
            IList<VertexElement> vertexElements = new List<VertexElement>();
            vertexElements.Add(new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 0));
            vertexElements.Add(new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0));
            vertexElements.Add(VertexElement.VertexDeclarationEnd);

            VertexDeclaration = device.CreateVertexDeclaration(vertexElements.ToArray());
        }
        private void CalculateCenter(IEnumerable<Vertex> vertices)
        {
            Vector4 vertex = vertices.OrderByDescending(x => x.Position.Length()).First().Position;
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
        }
        public void Rotate()
        {
            _angle.X += 0.05f;
            _angle.Y += 0.05f;
            _angle.Z += 0.05f;

            var rotation = Matrix.RotationX(_angle.X) * Matrix.RotationY(_angle.Y) * Matrix.RotationZ(_angle.Z);
            Transformation = Matrix.Translation(-1 * _center) * rotation * Matrix.Translation(_center);
        }
        #endregion

        #endregion
    }
}
