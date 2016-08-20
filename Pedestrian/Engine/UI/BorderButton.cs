using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine.Graphics;

namespace Pedestrian.Engine.UI
{
    public class BorderButton : Button
    {
        public Color BorderColor { get; set; } = Color.White;
        public int BorderWidth { get; set; } = 1;

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Border.Draw(spriteBatch, Position.ToPoint(), Width, Height, BorderColor);
        }
    }
}
