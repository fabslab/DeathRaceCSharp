using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pedestrian.Engine.Input;
using System.Collections.Generic;

namespace Pedestrian
{
    public class PlayerInput : IPlayerInput
    {
        IEnumerable<IPlayerInput> inputs;

        public PlayerInput(IEnumerable<IPlayerInput> inputs)
        {
            this.inputs = inputs;
        }

        public float GetThrottleValue()
        {
            float throttle = 0;
            foreach (var input in inputs)
            {
                throttle = input.GetThrottleValue();
                if (throttle != 0)
                {
                    return throttle;
                }
            }
            return throttle;
        }

        public float GetTurnAngleNormalized()
        {
            float turnAngle = 0;
            foreach (var input in inputs)
            {
                turnAngle = input.GetTurnAngleNormalized();
                if (turnAngle != 0)
                {
                    return turnAngle;
                }
            }
            return turnAngle;
        }
    }
}
