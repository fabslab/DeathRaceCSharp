using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pedestrian.Engine
{
    public class AnimatedTexture
    {
        int frameCount;
        Texture2D texture;
        float frameDuration;
        int currentFrameIndex;
        float totalElapsed;
        Vector2 origin;

        public Color Color { get; set; } = Color.White;
        public int FrameWidth { get; set; }
        public int FrameHeight {
            get
            {
                return texture.Height;
            }
        }

        public void Load(string asset, int frameCount, int frameDuration)
        {
            texture = PedestrianGame.Instance.Content.Load<Texture2D>(asset);
            this.frameCount = frameCount;
            this.frameDuration = frameDuration;
            FrameWidth = texture.Width / frameCount;
            origin = new Vector2(FrameWidth / 2, texture.Height / 2);
            currentFrameIndex = 0;
            totalElapsed = 0;
        }

        public void Update(GameTime time)
        {
            totalElapsed += (float)time.ElapsedGameTime.TotalMilliseconds;

            if (totalElapsed > frameDuration)
            {
                currentFrameIndex++;
                // Keep the Frame between 0 and the total frames, minus one.
                currentFrameIndex = currentFrameIndex % frameCount;
                totalElapsed -= frameDuration;
            }
        }
        
        public void Draw(SpriteBatch batch, Vector2 screenPos)
        {
            Draw(batch, screenPos, SpriteEffects.None);
        }

        public void Draw(SpriteBatch batch, Vector2 screenPos, SpriteEffects spriteEffect)
        {
            Draw(batch, currentFrameIndex, screenPos, spriteEffect);
        }

        public void Draw(SpriteBatch batch, int frame, Vector2 screenPos, SpriteEffects spriteEffect)
        {
            var frameRectangle = new Rectangle(FrameWidth * frame, 0, FrameWidth, texture.Height);
            batch.Draw(
                texture: texture,
                position: screenPos,
                sourceRectangle: frameRectangle,
                color: Color,
                origin: origin,
                effects: spriteEffect,
                scale: 1,
                layerDepth: 0,
                rotation: 0
            );
        }
    }
}