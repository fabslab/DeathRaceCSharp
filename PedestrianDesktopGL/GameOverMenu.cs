using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine.UI;

namespace Pedestrian
{
    public class GameOverMenu
    {
        VerticalButtonMenu buttons;
        CenteredText scores;


        public GameOverMenu(Rectangle screenArea)
        {
            var scoreText = new string[] {
                "1-3 POINTS: SKELETON CHASER",
                "4-10 POINTS: BONE CRACKER",
                "11-20 POINTS: GREMLIN HUNTER",
                "21 or OVER: EXPERT DRIVER"
            };
            scores = new CenteredText(screenArea, scoreText, screenArea.Height / 2 - 60, new Color(210, 210, 210));

            var buttonTypes = new ButtonType[] {
                ButtonType.Menu,
                ButtonType.Exit
            };
            buttons = new VerticalButtonMenu(screenArea, buttonTypes, screenArea.Height / 2);
        }

        public void Update(GameTime gameTime)
        {
            buttons.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            scores.Draw(spriteBatch);
            buttons.Draw(spriteBatch);
        }
    }
}
