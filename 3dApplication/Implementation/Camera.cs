using SharpDX;
using SharpDX.DirectInput;
using System;

namespace _3dApplication
{
    public class Camera
    {
        #region Private

        #region Attributes
        private static Camera _instance = null;

        private Vector3 _position;
        private Vector3 _rotation;
        private Vector3 _lookAt;
        private Vector3 _up;
        private float _fov;
        private float _zNear;
        private float _zFar;
        #endregion

        #region Methods
        #endregion

        #endregion

        #region Public

        #region Attributes
        public Matrix Projection { get; set; }
        public Matrix View { get; set; }
        public Vector3 Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
            }
        }
        public static Camera Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Camera();
                }

                return _instance;
            }
        }
        #endregion

        #region Methods
        private Camera()
        {

            _position = new Vector3(0, 1, -15);
            _lookAt = new Vector3(0, 0, 1);
            _up = new Vector3(0, 1, 0);

            _rotation = new Vector3(0, 0, 0);
            _fov = (float)Math.PI / 4;
            _zNear = 8f;
            _zFar = 0f;

            View = Matrix.Identity;
        }
        public void SetSize(float width, float height)
        {
            var aspectRatio = width / height;
            Projection = Matrix.PerspectiveFovLH(_fov, aspectRatio, _zNear, _zFar);
        }
        public void Update()
        {
            var input = Input.Instance;

            if (input.KeyDown(Key.S))
            {
                _position.Z -= 0.5f;
            }

            if (input.KeyDown(Key.W))
            {
                _position.Z += 0.5f;
            }

            if (input.KeyDown(Key.D))
            {
                _position.X += 0.5f;
            }

            if (input.KeyDown(Key.A))
            {
                _position.X -= 0.5f;
            }

            if (input.KeyDown(Key.Up))
            {
                _position.Y += 0.5f;
            }

            if (input.KeyDown(Key.Down))
            {
                _position.Y -= 0.5f;
            }

            if (input.KeyDown(Key.Right))
            {
                _rotation.Y += 1f * (float)(Math.PI / 180);
            }

            if (input.KeyDown(Key.Left))
            {
                _rotation.Y -= 1f * (float)(Math.PI / 180);
            }

            if (input.KeyDown(Key.PageDown))
            {
                _rotation.X += 1f * (float)(Math.PI / 180);
            }

            if (input.KeyDown(Key.PageUp))
            {
                _rotation.X -= 1f * (float)(Math.PI / 180);
            }


            var transformation = View;

            var rotation = Matrix3x3.RotationYawPitchRoll(_rotation.Y, _rotation.X, 0);
            var lookAt = Vector3.Transform(_lookAt, rotation);
            var up = Vector3.Transform(_up, rotation);
            var right = Vector3.Cross(up, lookAt);
            var position = new Vector3(-Vector3.Dot(_position, right), -Vector3.Dot(_position, up), -Vector3.Dot(_position, lookAt));

            transformation.Column1 = new Vector4(right, position.X);
            transformation.Column2 = new Vector4(up, position.Y);
            transformation.Column3 = new Vector4(lookAt, position.Z);

            View = transformation;
        }
        #endregion

        #endregion
    }
}
