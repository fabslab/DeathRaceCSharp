using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pedestrian
{
    public class AnimatedTexture
    {
        int frameCount;
        Texture2D texture;
        float frameDuration;
        int currentFrameIndex;
        float totalElapsed;
        Vector2 origin;

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

        public void UpdateFrame(float elapsed)
        {
            totalElapsed += elapsed;

            if (totalElapsed > frameDuration)
            {
                currentFrameIndex++;
                // Keep the Frame between 0 and the total frames, minus one.
                currentFrameIndex = currentFrameIndex % frameCount;
                totalElapsed -= frameDuration;
            }
        }
        
        public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
        {
            DrawFrame(batch, currentFrameIndex, screenPos);
        }

        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
        {
            var sourcerect = new Rectangle(FrameWidth * frame, 0, FrameWidth, texture.Height);
            batch.Draw(
                texture: texture, 
                position: screenPos, 
                sourceRectangle: sourcerect, 
                color: Color.White,
                origin: origin
            );
        }
    }
}