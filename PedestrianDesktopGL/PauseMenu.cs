using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pedestrian
{
    public class PauseMenu
    {
        VerticalButtonMenu buttons;

        public PauseMenu(Rectangle screenArea)
        {
            var buttonTypes = new ButtonType[] {
                ButtonType.Resume,
                ButtonType.Menu,
                ButtonType.Exit
            };
            buttons = new VerticalButtonMenu(screenArea, buttonTypes, screenArea.Height / 2 - 44);
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
