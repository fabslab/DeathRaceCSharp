using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine.Collision;

namespace Pedestrian.Engine
{
    public interface IEntity
    {
        Collider Collider { get; }
        Vector2 Position { get; }
        bool IsStatic { get; }

        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch);
        void Reset();
    }
}
