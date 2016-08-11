using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine;
using Pedestrian.Engine.Collision;
using System.Collections.Generic;

namespace Pedestrian
{
    public class GameArea : IEntity
    {
        public Collider Collider { get; }
        public Vector2 Position { get; }
        public bool IsStatic { get; } = true;
        public int SidewalkWidth { get; set; } = 32;
        public int BorderWidth { get; set; } = 2;
        Vector2 sidewalkLine1Position, sidewalkLine2Position;

        Rectangle borderRect, bounds;
        AreaEntity leftSidwalk, rightSidewalk;

        public GameArea(Rectangle area, List<IEntity> entities)
        {
            borderRect = area;

            // bounds is the area inside the border
            bounds = new Rectangle(
                area.X + BorderWidth,
                area.Y + BorderWidth,
                area.Width - 2 * BorderWidth,
                area.Height - 2 * BorderWidth
            );

            Collider = new ContainerCollider(bounds);

            var leftSidewalkArea = new Rectangle(bounds.X, bounds.Y, SidewalkWidth, bounds.Height);
            var rightSidewalkArea = new Rectangle(bounds.Right - SidewalkWidth, bounds.Y, SidewalkWidth, bounds.Height);

            sidewalkLine1Position = new Vector2(leftSidewalkArea.Right - BorderWidth, leftSidewalkArea.Y);
            sidewalkLine2Position = new Vector2(rightSidewalkArea.X, leftSidewalkArea.Y);

            leftSidwalk = new AreaEntity(leftSidewalkArea, ColliderCategory.Sidewalk);
            rightSidewalk = new AreaEntity(rightSidewalkArea, ColliderCategory.Sidewalk);
            entities.Add(leftSidwalk);
            entities.Add(rightSidewalk);
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
        public void Update(GameTime gameTime) { }
    }
}
