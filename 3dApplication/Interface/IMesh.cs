using SharpDX;
using SharpDX.Direct3D9;

namespace _3dApplication
{
    public interface IMesh
    {
        void Transform();
        void Render();
    }
}
