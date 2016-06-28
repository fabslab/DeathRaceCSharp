using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pedestrian
{
    public interface IEntity
    {
        Rectangle Bounds { get; }
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
