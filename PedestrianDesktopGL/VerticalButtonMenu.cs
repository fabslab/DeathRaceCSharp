using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine.BitmapFonts;
using Pedestrian.Engine.UI;

namespace Pedestrian
{
    public class VerticalButtonMenu
    {
        IFocusGroup buttons;

        public VerticalButtonMenu(Rectangle screenArea, ButtonType[] buttonTypes, float yPosition)
        {
            var buttonWidth = 120;
            var buttonHeight = 30;
            var buttonSpacing = 10;
            var borderWidth = 2;
            var buttonsX = screenArea.Width / 2 - buttonWidth / 2;
            var buttonsY = yPosition;

            buttons = new VerticalFocusGroup();

            var font = PedestrianGame.Instance.Content.Load<BitmapFont>("Fonts/munro-edit-font-14px_2");

            for (int i = 0, l = buttonTypes.Length; i < l; ++i)
            {
                var buttonType = buttonTypes[i];
                buttons.AddItem(new BorderButton(font)
                {
                    BorderWidth = borderWidth,
                    Text = buttonType.ToString().ToUpper(),
                    Position = new Vector2(buttonsX, buttonsY + i * (buttonHeight + buttonSpacing)),
                    Width = buttonWidth,
                    Height = buttonHeight,
                    Color = Color.Black,
                    Meta = buttonType
                });
            }

            buttons.OnItemSelected = button => {
                var command = (ButtonType)(button as Button).Meta;
                if (command == ButtonType.Menu)
                {
                    PedestrianGame.Instance.Events.Emit(GameEvents.LoadMenu, null);
                }
                else if (command == ButtonType.Exit)
                {
                    PedestrianGame.Instance.Events.Emit(GameEvents.Exit, null);
                }
                else if (command == ButtonType.Resume)
                {
                    PedestrianGame.Instance.Events.Emit(GameEvents.Resume, null);
                }
            };
        }

        public void Update(GameTime gameTime)
        {
            buttons.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            buttons.Draw(spriteBatch);
        }
    }
}
