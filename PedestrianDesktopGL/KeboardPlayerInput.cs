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
        Dictionary<InputCommand, Keys[]> currentInputMap;

        public KeyboardPlayerInput(Dictionary<InputCommand, Keys[]> inputMap = null)
        {
            currentInputMap = inputMap ?? KeyboardInputMap.Primary;
        }

        public float GetTurnAngleNormalized()
        {
            float turn = 0;
            var keyboardState = Keyboard.GetState();

            foreach (var key in currentInputMap[InputCommand.Right])
            {
                if (keyboardState.IsKeyDown(key))
                {
                    turn += 1;
                    break;
                }
            }
            foreach (var key in currentInputMap[InputCommand.Left])
            {
                if (keyboardState.IsKeyDown(key))
                {
                    turn -= 1;
                    break;
                }
            }

            return turn;
        }

        public float GetThrottleValue()
        {
            float acceleration = 0;
            var keyboardState = Keyboard.GetState();

            foreach (var key in currentInputMap[InputCommand.Forward])
            {
                if (keyboardState.IsKeyDown(key))
                {
                    acceleration += 1;
                    break;
                }
            }
            foreach (var key in currentInputMap[InputCommand.Reverse])
            {
                if (keyboardState.IsKeyDown(key))
                {
                    acceleration -= 1;
                    break;
                }
            }

            return acceleration;
        }
    }
}

