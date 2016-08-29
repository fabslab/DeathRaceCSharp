using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pedestrian.Engine.Input
{
    public static class GlobalInput
    {
        private static KeyboardState keyboardState, lastKeyboardState;
        private static GamePadState gamepadState, lastGamepadState;

        public static void Update()
        {
            lastKeyboardState = keyboardState;
            lastGamepadState = gamepadState;

            keyboardState = Keyboard.GetState();
            gamepadState = GamePad.GetState(PlayerIndex.One);
        }

        /// <summary>
        /// Returns whether the command was just input (changed from released to pressed).
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static bool WasCommandEntered(InputCommand command)
        {
            Keys key;
            if (KeyboardInputMap.Global.TryGetValue(command, out key))
            {
                if (lastKeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key))
                {
                    return true;
                }
            }

            Buttons button;
            if (ControllerInputMap.Primary.TryGetValue(command, out button))
            {
                if (lastGamepadState.IsButtonUp(button) && gamepadState.IsButtonDown(button))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
