using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pedestrian.Engine.Input
{
    public static class KeyboardInputMap
    {
        public static Dictionary<InputCommand, Keys[]> GetInputMap(PlayerIndex playerIndex)
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

        public static Dictionary<InputCommand, Keys[]> Primary = new Dictionary<InputCommand, Keys[]>
        {
            { InputCommand.Forward, new Keys[] { Keys.Up } },
            { InputCommand.Right, new Keys[] { Keys.Right } },
            { InputCommand.Reverse, new Keys[] { Keys.Down } },
            { InputCommand.Left, new Keys[] { Keys.Left } },
        };

        public static Dictionary<InputCommand, Keys[]> Secondary = new Dictionary<InputCommand, Keys[]>
        {
            { InputCommand.Forward, new Keys[] { Keys.W } },
            { InputCommand.Right, new Keys[] { Keys.D } },
            { InputCommand.Reverse, new Keys[] { Keys.S } },
            { InputCommand.Left, new Keys[] { Keys.A } },
        };

        public static Dictionary<InputCommand, Keys[]> Global = new Dictionary<InputCommand, Keys[]>
        {
            { InputCommand.Up, new Keys[] { Keys.Up } },
            { InputCommand.Down, new Keys[] { Keys.Down } },
            { InputCommand.Left, new Keys[] { Keys.Left } },
            { InputCommand.Right, new Keys[] { Keys.Right } },
            { InputCommand.Enter, new Keys[] { Keys.Enter } },
            { InputCommand.Exit, new Keys[] { Keys.Escape } },
            { InputCommand.Pause, new Keys[] { Keys.Escape } },
        };
    }
}
