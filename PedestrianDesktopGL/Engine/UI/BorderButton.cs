using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine.Graphics;

namespace Pedestrian.Engine.UI
{
    public class BorderButton : Button
    {
        public Color BorderColor { get; set; }
        public int BorderWidth { get; set; } = 1;

        Color defaultColor = new Color(100, 100, 100);


        public BorderButton()
        {
            BorderColor = defaultColor;
            OnFocused = () =>
            {
                BorderColor = Color.White;
            };
            OnFocusRemoved = () =>
            {
                BorderColor = defaultColor;
            };
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Border.Draw(spriteBatch, Position.ToPoint(), Width, Height, BorderColor, BorderWidth);
        }
    }
}
