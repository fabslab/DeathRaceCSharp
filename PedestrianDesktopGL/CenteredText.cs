using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.UI;
using System.Collections.Generic;

namespace Pedestrian
{
    /// <summary>
    /// Creates lines of instructional directions displayed on menu.
    /// Each line is created and positioned individually because
    /// font drawing currently does not support center alignment.
    /// </summary>
    public class CenteredText
    {
        List<TextBlock> textBlocks = new List<TextBlock>();

        public CenteredText(Rectangle displayArea, string[] lines, int yPosition, Color textColor)
        {
            for (int i = 0, l = lines.Length; i < l; ++i)
            {
                var textBlock = new TextBlock
                {
                    Text = lines[i],
                    Position = new Vector2(0, yPosition + TextBlock.Font.LineHeight * i),
                    Color = textColor,
                };
                textBlock.CenterHorizontal(displayArea.X, displayArea.Width);
                textBlocks.Add(textBlock);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            textBlocks.ForEach(t => t.Draw(spriteBatch));
        }
    }
}
