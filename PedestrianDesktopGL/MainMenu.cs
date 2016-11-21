using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine.BitmapFonts;
using Pedestrian.Engine.UI;

namespace Pedestrian
{
    public class MainMenu
    {
        Texture2D marquee;
        Vector2 marqueePosition;
        Rectangle displayArea;
        IFocusGroup buttons;
        CenteredText directions;

        public MainMenu(Rectangle displayArea)
        {
            this.displayArea = displayArea;
        }

        public void LoadContent()
        {
            marquee = PedestrianGame.Instance.Content.Load<Texture2D>("marquee-8bit");
            var marqueeXPosition = displayArea.Left + (displayArea.Width / 2 - marquee.Width / 2);
            marqueePosition = new Vector2(marqueeXPosition, 0);

            var buttonWidth = 120;
            var buttonHeight = 30;
            var buttonSpacing = 30;
            var borderWidth = 2;
            var buttonsWidth = 2 * buttonWidth + buttonSpacing;
            var buttonsX = displayArea.Width / 2 - buttonsWidth / 2;
            var buttonsY = marquee.Height + 12;

            var font = PedestrianGame.Instance.Content.Load<BitmapFont>("Fonts/munro-edit-font-14px_2");

            buttons = new HorizontalFocusGroup();
            buttons.AddItem(new BorderButton(font)
            {
                BorderWidth = borderWidth,
                Text = "1 PLAYER",
                Position = new Vector2(buttonsX, buttonsY),
                Width = buttonWidth,
                Height = buttonHeight,
                Meta = 1
            });
            buttons.AddItem(new BorderButton(font)
            {
                BorderWidth = borderWidth,
                Text = "2 PLAYERS",
                Position = new Vector2(buttonsX + buttonWidth + buttonSpacing, buttonsY),
                Width = buttonWidth,
                Height = buttonHeight,
                Meta = 2
            });

            buttons.OnItemSelected = button => {
                PedestrianGame.Instance.NumPlayers = (int)(button as Button).Meta;
                PedestrianGame.Instance.Events.Emit(GameEvents.GameStart, null);
            };

            var directionsText = new string[] {
                "HIT GREMLINS FOR POINTS",
                "USE REVERSE FOR QUICKER",
                "GETAWAY AFTER CRASH",
            };
            directions = new CenteredText(displayArea, directionsText, buttonsY + 50, new Color(210, 210, 210));
        }

        public void Update(GameTime gameTime)
        {
            buttons.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(marquee, marqueePosition, Color.White);
            buttons.Draw(spriteBatch);
            directions.Draw(spriteBatch);
        }
    }
}
