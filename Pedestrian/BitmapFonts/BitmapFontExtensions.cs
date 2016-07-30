using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.TextureAtlases;

namespace Pedestrian.BitmapFonts
{
    public static class BitmapFontExtensions
    {
        public static void DrawString(this SpriteBatch spriteBatch, BitmapFont bitmapFont, string text, Vector2 position, Color color)
        {
            var dx = position.X;
            var dy = position.Y;

            for (var i = 0; i < text.Length; ++i)
            {
                var character = text[i];
                var fontRegion = bitmapFont.GetCharacterRegion(character);

                if (fontRegion != null)
                {
                    var charPosition = new Vector2(dx + fontRegion.XOffset, dy + fontRegion.YOffset);

                    spriteBatch.Draw(fontRegion.TextureRegion, charPosition, color);

                    if (i != text.Length - 1)
                    {
                        dx += fontRegion.XAdvance + bitmapFont.LetterSpacing;
                    }
                    else
                    {
                        dx += fontRegion.XOffset + fontRegion.Width;
                    }
                }

                if (character == '\n')
                {
                    dy += bitmapFont.LineHeight;
                    dx = position.X;
                }
            }
        }

        public static void DrawString(this SpriteBatch spriteBatch, BitmapFont bitmapFont, string text, Vector2 position, Color color, int wrapWidth)
        {
            var dx = position.X;
            var dy = position.Y;
            var sentences = text.Split(new[] {'\n'}, StringSplitOptions.None);

            foreach (var sentence in sentences)
            {
                var words = sentence.Split(new[] { ' ' }, StringSplitOptions.None);

                for (var i = 0; i < words.Length; ++i)
                {
                    var word = words[i];
                    var size = bitmapFont.GetStringRectangle(word, Vector2.Zero);

                    if (i != 0 && dx + size.Width >= wrapWidth)
                    {
                        dy += bitmapFont.LineHeight;
                        dx = position.X;
                    }

                    DrawString(spriteBatch, bitmapFont, word, new Vector2(dx, dy), color);
                    dx += size.Width;
                    if (i != words.Length - 1)
                    {
                        dx += bitmapFont.GetCharacterRegion(' ').XAdvance + bitmapFont.LetterSpacing;
                    }
                    else
                    {
                        dx += bitmapFont.GetCharacterRegion(' ').Width;
                    }
                }

                dx = position.X;
                dy += bitmapFont.LineHeight;
            }
        }
    }
}