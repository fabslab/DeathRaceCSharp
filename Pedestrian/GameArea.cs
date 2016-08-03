using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pedestrian
{
    public class GameArea : IEntity
    {
        public Collider Collider { get; }
        public Vector2 Position { get; }
        public int SidewalkWidth { get; set; } = 32;
        public int BorderWidth { get; set; } = 2;

        Rectangle borderRect;

        public GameArea(Rectangle area)
        {
            borderRect = area;
            var bounds = new Rectangle(
                area.X + BorderWidth,
                area.Y + BorderWidth,
                area.Width - 2 * BorderWidth,
                area.Height - 2 * BorderWidth
            );
            Collider = new ContainerCollider(bounds);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Border.Draw(
                spriteBatch,
                borderRect,
                Color.White,
                BorderWidth
            );
            
            var dashedLineLength = borderRect.Height - BorderWidth;
            var sidewalkLine1Position = new Vector2(SidewalkWidth, borderRect.Top + BorderWidth);
            var sidewalkLine2Position = new Vector2(borderRect.Right - SidewalkWidth - BorderWidth, borderRect.Top + BorderWidth);
            DashedLine.Draw(spriteBatch, sidewalkLine1Position, dashedLineLength, BorderWidth, Color.White);
            DashedLine.Draw(spriteBatch, sidewalkLine2Position, dashedLineLength, BorderWidth, Color.White);
        }

        public void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch) {}
        public void Update(GameTime gameTime) { }
    }
}
