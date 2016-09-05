using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pedestrian.Engine.Input
{
    public static class ControllerInputMap
    {
        public static Dictionary<InputCommand, Buttons[]> Primary = new Dictionary<InputCommand, Buttons[]>
        {
            { InputCommand.Up, new Buttons[] { Buttons.DPadUp } },
            { InputCommand.Right, new Buttons[] { Buttons.DPadRight, Buttons.LeftThumbstickRight } },
            { InputCommand.Down, new Buttons[] { Buttons.DPadDown } },
            { InputCommand.Left, new Buttons[] { Buttons.DPadLeft, Buttons.LeftThumbstickLeft } },
            { InputCommand.Enter, new Buttons[] { Buttons.A } },
            { InputCommand.Pause, new Buttons[] { Buttons.Start } },
            { InputCommand.Forward, new Buttons[] { Buttons.RightTrigger } },
            { InputCommand.Reverse, new Buttons[] { Buttons.LeftTrigger } },
        };
    }
}
