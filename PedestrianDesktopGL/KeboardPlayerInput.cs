using Microsoft.Xna.Framework.Input;
using Pedestrian.Engine.Input;
using System.Collections.Generic;

namespace Pedestrian
{
    /// <summary>
    /// Input specific to gameplay. Instance to be created for each player.
    /// </summary>
    public class KeyboardPlayerInput : IPlayerInput
    {
        Dictionary<InputCommand, Keys> currentInputMap;

        public KeyboardPlayerInput(Dictionary<InputCommand, Keys> inputMap = null)
        {
            currentInputMap = inputMap ?? KeyboardInputMap.Primary;
        }

        public float GetTurnAngleNormalized()
        {
            float turn = 0;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(currentInputMap[InputCommand.Right]))
            {
                turn += 1;
            }
            if (keyboardState.IsKeyDown(currentInputMap[InputCommand.Left]))
            {
                turn -= 1;
            }

            return turn;
        }

        public float GetThrottleValue()
        {
            float acceleration = 0;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(currentInputMap[InputCommand.Forward]))
            {
                acceleration += 1;
            }
            if (keyboardState.IsKeyDown(currentInputMap[InputCommand.Reverse]))
            {
                acceleration -= 1;
            }

            return acceleration;
        }
    }
}

