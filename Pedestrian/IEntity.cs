using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Pedestrian
{
    public interface IEntity
    {
        Collider Collider { get; }
        void OnCollisionEnter(IEnumerable<IEntity> entities);
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
