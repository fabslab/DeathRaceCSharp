using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine.UI;

namespace Pedestrian
{
    public class GameOverMenu
    {
        IFocusGroup buttons;
        CenteredText scores;

        public GameOverMenu(Rectangle screenArea)
        {
            var scoreText = new string[] {
                "1-3 POINTS: SKELETON CHASER",
                "4-10 POINTS: BONE CRACKER",
                "11-20 POINTS: GREMLIN HUNTER",
                "21 or OVER: EXPERT DRIVER"
            };
            scores = new CenteredText(screenArea, scoreText, screenArea.Height / 2, new Color(210, 210, 210));

            var buttonWidth = 120;
            var buttonHeight = 30;
            var buttonSpacing = 30;
            var borderWidth = 2;
            var buttonsWidth = 2 * buttonWidth + buttonSpacing;
            var buttonsX = screenArea.Width / 2 - buttonsWidth / 2;
            var buttonsY = screenArea.Width / 2 - 100;

            buttons = new HorizontalFocusGroup();
            buttons.AddItem(new BorderButton
            {
                BorderWidth = borderWidth,
                Text = "MENU",
                Position = new Vector2(buttonsX, buttonsY),
                Width = buttonWidth,
                Height = buttonHeight,
                Color = Color.Black,
            });
            buttons.AddItem(new BorderButton
            {
                BorderWidth = borderWidth,
                Text = "EXIT",
                Position = new Vector2(buttonsX + buttonWidth + buttonSpacing, buttonsY),
                Width = buttonWidth,
                Height = buttonHeight,
                Color = Color.Black,
            });

            buttons.OnItemSelected = button => {
                var command = (button as Button).Text;
                if (command == "MENU")
                {
                    PedestrianGame.Instance.Events.Emit(GameEvents.LoadMenu, null);
                }
                else if (command == "EXIT")
                {
                    PedestrianGame.Instance.Events.Emit(GameEvents.Exit, null);
                }
            };
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
