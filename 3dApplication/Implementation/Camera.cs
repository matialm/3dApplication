using SharpDX;
using SharpDX.DirectInput;
using System;

namespace _3dApplication
{
    public class Camera
    {
        #region Private

        #region Attributes
        public Vector3 _eye;
        public Vector3 _target;
        public Vector3 _up;
        #endregion

        #region Methods
        #endregion

        #endregion

        #region Public

        #region Attributes
        public float Fov { get; set; }
        public float AspectRatio { get; set; }
        public float Znear { get; set; }
        public float Zfar { get; set; }
        public Vector3 Eye { get { return _eye; } }
        public Vector3 Target { get { return _target; } }
        public Vector3 Up { get { return _up; } }
        #endregion

        #region Methods
        public Camera()
        {
            Fov = (float)Math.PI / 4;
            Znear = 8f;
            Zfar = 0f;
            _eye = new Vector3(0, 0, 15);
            _target = new Vector3(0, 0, 0);
            _up = new Vector3(0, 1, 0);
        }

        public void SetSize(float width, float height)
        {
            AspectRatio = width / height;
        }

        public void Update(Input input)
        {
            if (input.IsPressed(Key.S))
            {
                _eye.Z += 0.5f;
            }

            if (input.IsPressed(Key.W))
            {
                _eye.Z -= 0.5f;
            }


            if (input.IsPressed(Key.D))
            {
                _eye.X += 0.5f;
                _target.X += 0.5f;
            }

            if (input.IsPressed(Key.A))
            {
                _eye.X -= 0.5f;
                _target.X -= 0.5f;
            }


            if(input.IsPressed(Key.Up))
            {

            }

            if(input.IsPressed(Key.Down))
            {

            }
        }

        #endregion

        #endregion

    }
}
