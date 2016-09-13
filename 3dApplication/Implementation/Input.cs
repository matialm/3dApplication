using SharpDX.DirectInput;
using System;

namespace _3dApplication
{
    public class Input
    {
        #region Private

        #region Attributes
        private Keyboard _keyboard;
        private Mouse _mouse;
        #endregion

        #region Methods
        #endregion

        #endregion

        #region Public

        #region Attributes
        #endregion

        #region Methods
        public Input(IntPtr handler)
        {
            DirectInput directInput = new DirectInput();
            _keyboard = new Keyboard(directInput);
            _keyboard.SetCooperativeLevel(handler, CooperativeLevel.Background | CooperativeLevel.NonExclusive);
            _keyboard.Acquire();

            _mouse = new Mouse(directInput);
            _mouse.SetCooperativeLevel(handler, CooperativeLevel.Background | CooperativeLevel.NonExclusive);
            _mouse.Acquire();
        }

        public bool IsPressed(Key key)
        {
            return _keyboard.GetCurrentState().IsPressed(key);
        }

        #endregion

        #endregion
    }
}
