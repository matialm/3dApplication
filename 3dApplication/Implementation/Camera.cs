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
        private Camera()
        {
            _position = new Vector3(0, 1, -15);
            _lookAt = new Vector3(0, 0, 1);
            _up = new Vector3(0, 1, 0);

            _rotation = new Vector3(0, 0, 0);
            _fov = (float)Math.PI / 4;
            _zNear = 1f;
            _zFar = 200f;
        }
        private void UpdateRotation()
        {
            var input = Input.Instance;
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
        }
        private void UpdateTranslation(Vector3 lookAt, Vector3 right, Vector3 up)
        {
            var input = Input.Instance;
            if (input.KeyDown(Key.S))
            {
                _position -= 0.5f * lookAt;
            }

            if (input.KeyDown(Key.W))
            {
                _position += 0.5f * lookAt;
            }

            if (input.KeyDown(Key.D))
            {
                _position += 0.5f * right;
            }

            if (input.KeyDown(Key.A))
            {
                _position -= 0.5f * right;
            }

            if (input.KeyDown(Key.Up))
            {
                _position += 0.5f * up;
            }

            if (input.KeyDown(Key.Down))
            {
                _position -= 0.5f * up;
            }
        }
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
        public void SetSize(float width, float height)
        {
            var aspectRatio = width / height;
            var projection = Matrix.Zero;

            var distance = 1 / (float)Math.Tan(_fov / 2);

            projection.Column1 = new Vector4(distance/aspectRatio, 0, 0, 0);
            projection.Column2 = new Vector4(0, distance, 0, 0);
            projection.Column3 = new Vector4(0, 0, _zFar / (_zFar - _zNear), (-1 * _zNear * _zFar) / (_zFar - _zNear));
            projection.Column4 = new Vector4(0, 0, 1, 0);

            Projection = projection;
        }
        public void Update()
        {
            UpdateRotation();

            var pitch = Matrix.RotationX(_rotation.X);
            var yaw = Matrix.RotationY(_rotation.Y);
            var lookAt = Vector3.TransformNormal(_lookAt, pitch * yaw);
            var up = Vector3.TransformNormal(_up, pitch * yaw);
            var right = Vector3.Cross(up, lookAt);
            var lookAtReference = Vector3.TransformNormal(_lookAt, yaw);

            UpdateTranslation(lookAtReference, right, _up);

            var view = Matrix.Identity;
            view.Column1 = new Vector4(right, -Vector3.Dot(_position, right));
            view.Column2 = new Vector4(up, -Vector3.Dot(_position, up));
            view.Column3 = new Vector4(lookAt, -Vector3.Dot(_position, lookAt));

            View = view;
        }
        #endregion

        #endregion
    }
}
