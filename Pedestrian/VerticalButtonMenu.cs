using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine.UI;

namespace Pedestrian
{
    public class VerticalButtonMenu
    {
        IFocusGroup buttons;

        public VerticalButtonMenu(Rectangle screenArea, ButtonType[] buttonTypes)
        {
            var buttonWidth = 120;
            var buttonHeight = 30;
            var buttonSpacing = 10;
            var borderWidth = 2;
            var buttonsX = screenArea.Width / 2 - buttonWidth / 2;
            var buttonsY = screenArea.Width / 2 - 100;

            buttons = new VerticalFocusGroup();

            for (int i = 0, l = buttonTypes.Length; i < l; ++i)
            {
                var buttonType = buttonTypes[i];
                buttons.AddItem(new BorderButton
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
