using Microsoft.Xna.Framework.Input;
using Pedestrian.Engine;
using System.Collections.Generic;

namespace Pedestrian
{
    public class KeyboardInput : IPlayerInput
    {
        Dictionary<InputDirection, Keys> currentInputMap;

        public KeyboardInput(Dictionary<InputDirection, Keys> inputMap = null)
        {
            currentInputMap = inputMap ?? KeyboardInputMap.Primary;
        }

        public float GetTurnAngleNormalized()
        {
            float turn = 0;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(currentInputMap[InputDirection.Right]))
            {
                turn += 1;
            }
            if (keyboardState.IsKeyDown(currentInputMap[InputDirection.Left]))
            {
                turn -= 1;
            }

            return turn;
        }

        public float GetThrottleValue()
        {
            float acceleration = 0;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(currentInputMap[InputDirection.Forward]))
            {
                acceleration += 1;
            }
            if (keyboardState.IsKeyDown(currentInputMap[InputDirection.Reverse]))
            {
                acceleration -= 1;
            }

            return acceleration;
        }
    }
}

