using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pedestrian.Engine.Input;
using System.Collections.Generic;

namespace Pedestrian
{
    public class ControllerPlayerInput : IPlayerInput
    {
        PlayerIndex index;
        Dictionary<InputCommand, Buttons[]> currentInputMap;

        public ControllerPlayerInput(Dictionary<InputCommand, Buttons[]> inputMap, PlayerIndex playerIndex)
        {
            currentInputMap = inputMap;
            index = playerIndex;
        }

        public float GetThrottleValue()
        {
            var throttle = 0;
            var controller = GamePad.GetState(index);
            if (!controller.IsConnected) return throttle;

            foreach (var button in currentInputMap[InputCommand.Forward])
            {
                if (controller.IsButtonDown(button))
                {
                    throttle += 1;
                    break;
                }
            }
            foreach (var button in currentInputMap[InputCommand.Reverse])
            {
                if (controller.IsButtonDown(button))
                {
                    throttle -= 1;
                    break;
                }
            }

            return throttle;
        }

        public float GetTurnAngleNormalized()
        {
            float turn = 0;
            var controller = GamePad.GetState(index);
            if (!controller.IsConnected) return turn;

            foreach (var button in currentInputMap[InputCommand.Right])
            {
                if (controller.IsButtonDown(button))
                {
                    turn += 1;
                    break;
                }
            }
            foreach (var button in currentInputMap[InputCommand.Left])
            {
                if (controller.IsButtonDown(button))
                {
                    turn -= 1;
                    break;
                }
            }

            return turn;
        }
    }
}
