using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pedestrian.Engine.Input
{
    public static class KeyboardInputMap
    {
        public static Dictionary<InputCommand, Keys> GetInputMap(PlayerIndex playerIndex)
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

        public static Dictionary<InputCommand, Keys> Primary = new Dictionary<InputCommand, Keys>
        {
            { InputCommand.Forward, Keys.Up },
            { InputCommand.Right, Keys.Right },
            { InputCommand.Reverse, Keys.Down },
            { InputCommand.Left, Keys.Left },
            { InputCommand.Enter, Keys.Enter },
            { InputCommand.Quit, Keys.Escape },
        };

        public static Dictionary<InputCommand, Keys> Secondary = new Dictionary<InputCommand, Keys>
        {
            { InputCommand.Forward, Keys.W },
            { InputCommand.Right, Keys.D },
            { InputCommand.Reverse, Keys.S },
            { InputCommand.Left, Keys.A },
            { InputCommand.Enter, Keys.Enter },
            { InputCommand.Quit, Keys.Escape }
        };
    }
}
