using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pedestrian.Engine.Input
{
    public static class ControllerInputMap
    {
        public static Dictionary<InputCommand, Buttons> Primary = new Dictionary<InputCommand, Buttons>
        {
            { InputCommand.Up, Buttons.DPadUp },
            { InputCommand.Right, Buttons.DPadRight },
            { InputCommand.Down, Buttons.DPadDown },
            { InputCommand.Left, Buttons.DPadLeft },
            { InputCommand.Enter, Buttons.A },
            { InputCommand.Pause, Buttons.Start },
            { InputCommand.Forward, Buttons.RightTrigger },
            { InputCommand.Reverse, Buttons.LeftTrigger },
        };
    }
}
