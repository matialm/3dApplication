using SharpDX.DirectInput;
using System;

namespace _3dApplication
{
    public class Input
    {
        #region Private

        #region Attributes
        private static Input _instance = null;        
        private Keyboard _keyboard;
        private Mouse _mouse;
        private bool[] _lastKeyboardState;
        private bool[] _actualKeyboardState;
        #endregion

        #region Methods
        #endregion

        #endregion

        #region Public

        #region Attributes
        #endregion

        #region Methods
        public static Input Instance()
        {
            if(_instance == null)
            {
                _instance = new Input();
            }

            return _instance;
        }
        private Input()
        {
            DirectInput directInput = new DirectInput();
            _keyboard = new Keyboard(directInput);
            _keyboard.SetCooperativeLevel(DXDevice.Instance().Handle, CooperativeLevel.Background | CooperativeLevel.NonExclusive);
            _keyboard.Acquire();

            _mouse = new Mouse(directInput);
            _mouse.SetCooperativeLevel(DXDevice.Instance().Handle, CooperativeLevel.Background | CooperativeLevel.NonExclusive);
            _mouse.Acquire();

            int[] values = (int[])Enum.GetValues(typeof(Key));
            int max = values[values.Length - 1];
            _actualKeyboardState = new bool[max];
            _lastKeyboardState = new bool[max];

            for (int i = 0; i < max; i++)
            {
                _actualKeyboardState[i] = false;
                _lastKeyboardState[i] = false;
            }
        }
        public void Update()
        {
            Array.Copy(_actualKeyboardState, _lastKeyboardState, _lastKeyboardState.Length);

            for (int i = 0; i < _actualKeyboardState.Length; i++)
            {
                _actualKeyboardState[i] = _keyboard.GetCurrentState().IsPressed((Key)(i + 1));
            }
        }
        public bool KeyDown(Key key)
        {
            return _actualKeyboardState[(int)(key - 1)];
        }
        public bool KeyUp(Key key)
        {
            return !_actualKeyboardState[(int)(key - 1)] && _lastKeyboardState[(int)(key - 1)];
        }
        public bool KeyPress(Key key)
        {
            return _actualKeyboardState[(int)(key - 1)] && !_lastKeyboardState[(int)(key - 1)];
        }
        public void Dispose()
        {
            _keyboard.Unacquire();
            _keyboard.Dispose();
            _mouse.Unacquire();
            _mouse.Dispose();
        }
        #endregion

        #endregion
    }
}
