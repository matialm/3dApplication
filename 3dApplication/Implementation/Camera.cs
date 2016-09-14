using SharpDX;
using SharpDX.DirectInput;
using System;

namespace _3dApplication
{
    public class Camera
    {
        #region Private

        #region Attributes
        private Vector3 _eye;
        private Vector3 _target;
        private Vector3 _up;
        private float _fov;
        private float _aspectRatio;
        private float _zNear;
        private float _zFar;

        private Vector3 _position;
        private Vector3 _angle;
        #endregion

        #region Methods
        #endregion

        #endregion

        #region Public

        #region Attributes
        public Matrix Projection { get; set; }
        public Matrix View { get; set; }
        #endregion

        #region Methods
        public Camera()
        {
            _position = new Vector3(0, 0, 0);
            _angle = new Vector3(0, 0, 0);
            _fov = (float)Math.PI / 4;
            _zNear = 8f;
            _zFar = 0f;
            _eye = new Vector3(0, 10, 15);
            _target = new Vector3(0, 0, 0);
            _up = new Vector3(0, 1, 0);
            View = Matrix.Identity;
            View = Matrix.Identity;
        }

        public void SetSize(float width, float height)
        {
            _aspectRatio = width / height;
        }

        public void Update(Input input)
        {
            if (input.IsPressed(Key.S))
            {
                _position.Z -= 0.5f;
            }

            if (input.IsPressed(Key.W))
            {
                _position.Z += 0.5f;
            }

            if (input.IsPressed(Key.D))
            {
                _position.X -= 0.5f;
            }

            if (input.IsPressed(Key.A))
            {
                _position.X += 0.5f;
            }

            if (input.IsPressed(Key.Up))
            {
                _position.Y += 0.5f;
                //_angle.X -= 1f * (float)(Math.PI/180);
            }

            if (input.IsPressed(Key.Down))
            {
                _position.Y -= 0.5f;
                //_angle.X += 1f * (float)(Math.PI / 180);
            }

            if (input.IsPressed(Key.Right))
            {
                _angle.Y += 1f * (float)(Math.PI / 180);
            }

            if (input.IsPressed(Key.Left))
            {
                _angle.Y -= 1f * (float)(Math.PI / 180);
            }

            Projection = Matrix.PerspectiveFovLH(_fov, _aspectRatio, _zNear, _zFar);
            View = Matrix.RotationYawPitchRoll(_angle.Y, _angle.X, 0) * Matrix.Translation(_position) * Matrix.LookAtLH(_eye, _target, _up);
        }

        #endregion

        #endregion

    }
}
