using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pedestrian
{
    public class ControllerInput : IPlayerInput
    {
        GamePadState controller;

        public bool IsConnected => controller.IsConnected;

        public ControllerInput(PlayerIndex playerIndex = PlayerIndex.One)
        {
            controller = GamePad.GetState(playerIndex);
        }

        public float GetThrottleValue()
        {
            if (!IsConnected) return 0;

            var throttle = 0;

            if (controller.Triggers.Right >= 0.8)
            {
                throttle += 1;
            }
            if (controller.Triggers.Left >= 0.8)
            {
                throttle -= 1;
            }

            return throttle;
        }

        public float GetTurnAngleNormalized()
        {
            if (!IsConnected) return 0;

            float turn = 0;

            if (controller.DPad.Right == ButtonState.Pressed)
            {
                turn += 1;
            }
            if (controller.DPad.Left == ButtonState.Pressed)
            {
                turn -= 1;
            }

            return turn;
        }
    }
}
