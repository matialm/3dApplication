using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace _3dApplication
{
    class Program
    {
        private static float _lastTick;
        private static float _fps = 100;

        [STAThread]
        static void Main()
        {
            IDevice screen = new DXDevice();
            screen.Show();
            screen.Focus();

            IList<IMesh> meshes = new List<IMesh>();
            meshes.Add(new Cube(screen));
            meshes.Add(new Cube(screen, new int[] { 3, 2, 0 }));

            while (screen.IsAlive)
            {
                if(screen.Accesible)
                {
                    if (screen.Focused)
                    {
                        float currTick = Environment.TickCount;
                        float elapsedtime = currTick - _lastTick;

                        if (elapsedtime > _fps)
                        {
                            foreach (IMesh mesh in meshes)
                            {
                                mesh.Rotate();
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

            screen.Dispose();
        }
    }
}
