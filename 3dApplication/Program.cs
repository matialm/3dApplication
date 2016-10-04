using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace _3dApplication
{
    class Program
    {
        private static float _lastTick;
        private static float _fps = 1000f/60f;

        [STAThread]
        static void Main()
        {
            Camera camera = Camera.Instance();
            IDevice screen = DXDevice.Instance();
            screen.Show();
            screen.Focus();
            Input input = Input.Instance();

            IList<IMesh> meshes = new List<IMesh>();

            var heightMap = new HeightMap();
            var skybox = new Skybox();
            skybox.SetSize(heightMap.GetWidth());

            meshes.Add(skybox);
            meshes.Add(heightMap);
            meshes.Add(new Cube());
            //meshes.Add(new Cube(new int[] { 3, 2, 0 }));

            while (screen.IsAlive)
            {
                if(screen.Accesible)
                {
                    if (screen.Focused)
                    {
                        float currTick = Environment.TickCount;
                        float elapsedtime = currTick - _lastTick;

                        input.Update();
                        screen.CaptureInput();

                        if (elapsedtime > _fps)
                        {
                            camera.Update();
                            foreach (IMesh mesh in meshes)
                            {
                                mesh.Transform();
                            }
                            _lastTick = currTick;
                        }

                        screen.Render(meshes);
                    }
                    else
                        Thread.Sleep(100);
                }

                Application.DoEvents();
            }

            input.Dispose();
            screen.Dispose();
        }
    }
}
