using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pedestrian.Engine
{
    public class KeyboardInputMap
    {
        public static Dictionary<InputDirection, Keys> GetInputMap(PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                return Primary;
            }
            else
            {
                return Secondary;
            }
        }

        public static Dictionary<InputDirection, Keys> Primary = new Dictionary<InputDirection, Keys>
            {
                { InputDirection.Forward, Keys.W },
                { InputDirection.Right, Keys.D },
                { InputDirection.Reverse, Keys.S },
                { InputDirection.Left, Keys.A }
            };

        public static Dictionary<InputDirection, Keys> Secondary = new Dictionary<InputDirection, Keys>
            {
                { InputDirection.Forward, Keys.Up },
                { InputDirection.Right, Keys.Right },
                { InputDirection.Reverse, Keys.Down },
                { InputDirection.Left, Keys.Left }
            };
    }
}
