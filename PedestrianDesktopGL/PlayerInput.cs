using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pedestrian.Engine.Input;

namespace Pedestrian
{
    public class PlayerInput : IPlayerInput
    {
        PlayerIndex playerIndex;
        ControllerPlayerInput controller;
        KeyboardPlayerInput keys;

        public PlayerInput(PlayerIndex playerIndex = PlayerIndex.One)
        {
            this.playerIndex = playerIndex;
            controller = new ControllerPlayerInput(playerIndex);
            keys = new KeyboardPlayerInput(KeyboardInputMap.GetInputMap(playerIndex));
        }

        public float GetThrottleValue()
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return controller.GetThrottleValue();
            }
            else
            {
                return keys.GetThrottleValue();
            }
        }

        public float GetTurnAngleNormalized()
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return controller.GetTurnAngleNormalized();
            }
            else
            {
                return keys.GetTurnAngleNormalized();
            }
        }
    }
}
