using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
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
            Camera camera = new Camera();
            IDevice screen = new DXDevice(camera);
            screen.Show();
            screen.Focus();
            Input input = new Input(screen.Handle);
            screen.Input = input;

            IList<IMesh> meshes = new List<IMesh>();
            meshes.Add(new HeightMap(screen));
            meshes.Add(new Cube(screen));
            //meshes.Add(new Cube(screen, new int[] { 3, 2, 0 }));

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
                            camera.Update(input);
                            foreach (IMesh mesh in meshes)
                            {
                                mesh.Transform();
                            }
                            _lastTick = currTick;
                        }
                        screen.Render(camera, meshes);
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
