using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pedestrian.Engine.Input
{
    public static class ControllerInputMap
    {
        public static Dictionary<InputCommand, Buttons> Primary = new Dictionary<InputCommand, Buttons>
        {
            { InputCommand.Forward, Buttons.RightTrigger },
            { InputCommand.Right, Buttons.LeftTrigger },
            { InputCommand.Reverse, Buttons.DPadDown },
            { InputCommand.Left, Buttons.DPadLeft }
        };
    }
}
