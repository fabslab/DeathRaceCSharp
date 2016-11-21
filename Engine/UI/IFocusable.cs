using Microsoft.Xna.Framework.Graphics;
using System;

namespace Pedestrian.Engine.UI
{
    public interface IFocusable
    {
        Action OnFocused { get; set; }
        Action OnFocusRemoved { get; set; }

        void Draw(SpriteBatch spriteBatch);
    }
}
