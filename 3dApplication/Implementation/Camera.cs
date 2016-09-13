using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dApplication
{
    public class Camera
    {
        #region Private

        #region Attributes
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
        public Vector3 Eye { get; set; }
        public Vector3 Target { get; set; }
        public Vector3 Up { get; set; }
        #endregion

        #region Methods
        public Camera()
        {
            Fov = (float)Math.PI / 4;
            Znear = 8f;
            Zfar = 0f;
            Eye = new Vector3(0, 0, 15);
            Target = new Vector3(0, 0, 0);
            Up = new Vector3(0, 1, 0);
        }

        public void SetSize(float width, float height)
        {
            AspectRatio = width / height;
        }

        #endregion

        #endregion

    }
}
