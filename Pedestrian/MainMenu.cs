using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedestrian
{
    public class MainMenu
    {
        Texture2D marquee;
        Rectangle displayArea;
        Vector2 position;

        public MainMenu(Rectangle displayArea)
        {
            this.displayArea = displayArea;
        }

        public void LoadContent()
        {
            marquee = PedestrianGame.Instance.Content.Load<Texture2D>("marquee01");
            var xPosition = displayArea.Left + (displayArea.Width / 2 - marquee.Width / 2);
            position = new Vector2(xPosition, 0);
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(marquee, position, Color.White);
        }
    }
}
