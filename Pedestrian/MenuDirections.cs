using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine.UI;

namespace Pedestrian
{
    /// <summary>
    /// Creates lines of instructional directions displayed on menu.
    /// Each line is created and positioned individually because
    /// font drawing currently does not support center alignment.
    /// </summary>
    public class MenuDirections
    {
        TextBlock directionsLine1, directionsLine2, directionsLine3;


        public MenuDirections(Rectangle displayArea, int yPosition, Color textColor)
        {
            directionsLine1 = new TextBlock
            {
                Text = "HIT GREMLINS FOR POINTS",
                Position = new Vector2(0, yPosition),
                Color = textColor,
            };
            directionsLine1.CenterHorizontal(displayArea.X, displayArea.Width);

            directionsLine2 = new TextBlock
            {
                Text = "USE REVERSE FOR QUICKER",
                Position = new Vector2(0, directionsLine1.Position.Y + TextBlock.Font.LineHeight),
                Color = textColor,
            };
            directionsLine2.CenterHorizontal(displayArea.X, displayArea.Width);

            directionsLine3 = new TextBlock
            {
                Text = "GETAWAY AFTER CRASH",
                Position = new Vector2(0, directionsLine2.Position.Y + TextBlock.Font.LineHeight),
                Color = textColor,
            };
            directionsLine3.CenterHorizontal(displayArea.X, displayArea.Width);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            directionsLine1.Draw(spriteBatch);
            directionsLine2.Draw(spriteBatch);
            directionsLine3.Draw(spriteBatch);
        }
    }
}
