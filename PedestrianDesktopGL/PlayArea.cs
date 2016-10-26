using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine;
using Pedestrian.Engine.Collision;
using Pedestrian.Engine.Graphics;
using System.Collections.Generic;

namespace Pedestrian
{
    public class PlayArea : IEntity
    {
        public static PlayArea Instance { get; private set; }

        public Collider Collider { get; }
        public Vector2 Position { get; }
        public bool IsStatic { get; } = true;
        public int SidewalkWidth { get; private set; }
        public int BorderWidth { get; private set; }
        Vector2 sidewalkLine1Position, sidewalkLine2Position;
        public Rectangle Bounds { get; set; }

        Rectangle borderRect;

        public PlayArea(Rectangle area, int sidewalkWidth, int borderWidth)
        {
            Instance = this;
            borderRect = area;
            SidewalkWidth = sidewalkWidth;
            BorderWidth = borderWidth;

            // bounds is the area inside the border
            Bounds = new Rectangle(
                area.X + BorderWidth,
                area.Y + BorderWidth,
                area.Width - 2 * BorderWidth,
                area.Height - 2 * BorderWidth
            );

            Collider = new ContainerCollider(Bounds, ColliderCategory.GameBounds, ColliderCategory.All);

            sidewalkLine1Position = new Vector2(Bounds.X + SidewalkWidth - BorderWidth, Bounds.Y);
            sidewalkLine2Position = new Vector2(Bounds.Right - SidewalkWidth, Bounds.Y);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Border.Draw(
                spriteBatch,
                borderRect,
                Color.White,
                BorderWidth
            );

            var dashedLineHeight = borderRect.Height - BorderWidth;
            DashedLine.Draw(spriteBatch, sidewalkLine1Position, dashedLineHeight, BorderWidth, Color.White);
            DashedLine.Draw(spriteBatch, sidewalkLine2Position, dashedLineHeight, BorderWidth, Color.White);
        }

        public void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch) {}
        public void Update(GameTime gameTime) {}
        public void Unload() { }
    }
}
