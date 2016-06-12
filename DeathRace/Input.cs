using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace Pedestrian
{
    public interface PlayerInput
    {
        // -1 to 1 indicating how far left or right player is turning (0 is straight, no turn)
        float GetTurnAngleNormalized();

        // -1 to 1 indicating how hard the player is accelerating (negative indicates reverse)
        float GetThrottleValue();
    }

    public class KeyboardInput : PlayerInput
    {
        public enum InputDirections
        {
            Forward,
            Right,
            Reverse,
            Left
        }

        public static Dictionary<InputDirections, Keys> INPUT_MAP_PRIMARY = new Dictionary<InputDirections, Keys>
            {
                { InputDirections.Forward, Keys.W },
                { InputDirections.Right, Keys.D },
                { InputDirections.Reverse, Keys.S },
                { InputDirections.Left, Keys.A }
            };
        public static Dictionary<InputDirections, Keys> INPUT_MAP_SECONDARY = new Dictionary<InputDirections, Keys>
            {
                { InputDirections.Forward, Keys.Up },
                { InputDirections.Right, Keys.Right },
                { InputDirections.Reverse, Keys.Down },
                { InputDirections.Left, Keys.Left }
            };

        private Dictionary<InputDirections, Keys> currentInputMap;

        public KeyboardInput(Dictionary<InputDirections, Keys> inputMap = null)
        {
            currentInputMap = inputMap ?? INPUT_MAP_PRIMARY;
        }

        public float GetTurnAngleNormalized()
        {
            float turn = 0;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(currentInputMap[InputDirections.Right]))
            {
                turn += 1;
            }
            if (keyboardState.IsKeyDown(currentInputMap[InputDirections.Left]))
            {
                turn -= 1;
            }

            return turn;
        }

        public float GetThrottleValue()
        {
            float acceleration = 0;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(currentInputMap[InputDirections.Forward]))
            {
                acceleration += 1;
            }
            if (keyboardState.IsKeyDown(currentInputMap[InputDirections.Reverse]))
            {
                acceleration -= 1;
            }

            return acceleration;
        }
    }
}

