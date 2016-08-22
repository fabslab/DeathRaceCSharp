using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine.BitmapFonts;

namespace Pedestrian.Engine.UI
{
    public class TextBlock
    {
        public Color Color { get; set; } = Color.White;
        public string Text { get; set; } = "";
        public Vector2 Position { get; set; }
        public static BitmapFont Font { get; } = PedestrianGame.Instance.Content.Load<BitmapFont>("Fonts/munro-edit-font-12px");

        static TextBlock()
        {
            Font.LetterSpacing = 1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Position, Color);
        }

        public void CenterHorizontal(int containerX, int containerWidth)
        {
            var textSize = Font.GetSize(Text);

            // account for first characters x offset when centering
            var firstLetter = Font.GetCharacterRegion(Text[0]);
            var textWidth = textSize.Width + firstLetter.XOffset;

            Position = new Vector2(containerWidth / 2 - textWidth / 2, Position.Y);
        }

        public void SetY(float y)
        {
            Position = new Vector2(Position.X, y);
        }
    }
}
