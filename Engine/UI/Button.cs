using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine.BitmapFonts;
using Pedestrian.Engine.Graphics.Shapes;
using System;

namespace Pedestrian.Engine.UI
{
    public class Button : IFocusable
    {
        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Text { get; set; } = "";
        public Color Color { get; set; }
        public Action OnFocused { get; set; }
        public Action OnFocusRemoved { get; set; }
        public object Meta { get; set; }

        BitmapFont font;


        public Button(BitmapFont fontAsset)
        {
            font = fontAsset;
            font.LetterSpacing = 2;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var area = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            RectangleShape.Draw(spriteBatch, area, Color);
            
            var textPosition = GetTextPosition();
            spriteBatch.DrawString(font, Text, textPosition, Color.White);
        }

        private Vector2 GetTextPosition()
        {
            if (Text == "")
            {
                return Position;
            }

            var textSize = font.GetSize(Text);

            // account for first characters x offset when centering
            var firstLetter = font.GetCharacterRegion(Text[0]);
            var textWidth = textSize.Width + firstLetter.XOffset;

            var xPosition = Position.X + (Width / 2 - textWidth / 2);
            var yPosition = Position.Y + (Height / 2 - font.LineHeight / 2);
            return new Vector2(xPosition, yPosition);
        }
    }
}
