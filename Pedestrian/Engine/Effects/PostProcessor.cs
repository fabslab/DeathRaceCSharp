using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pedestrian.Engine.Effects
{
    /// <summary>
    /// Inherit from this class when using the generic PostProcessor is not adequate.
    /// </summary>
    public abstract class PostProcessor
    {
        public SamplerState SamplerState { get; set; } = SamplerState.LinearClamp;

        protected SpriteBatch spriteBatch;
        protected GraphicsDevice graphicsDevice;
        protected Effect Effect { get; set; }


        public PostProcessor(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }


        public virtual void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public virtual void Process(RenderTarget2D source, RenderTarget2D destination)
        {
            DrawFullscreenQuad(source, destination, Effect);
        }

        /// <summary>
        /// Helper for drawing a texture into a rendertarget, using
        /// a custom shader to apply postprocessing effects.
        /// </summary>
        protected void DrawFullscreenQuad(Texture2D texture, RenderTarget2D renderTarget, Effect effect)
        {
            Rectangle destinationRectangle;
            if (renderTarget == null)
            {
                var pp = graphicsDevice.PresentationParameters;
                destinationRectangle = new Rectangle(0, 0, pp.BackBufferWidth, pp.BackBufferHeight);
            }
            else
            {
                destinationRectangle = new Rectangle(0, 0, renderTarget.Width, renderTarget.Height);
            }
            graphicsDevice.SetRenderTarget(renderTarget);
            DrawFullscreenQuad(texture, destinationRectangle, effect);
        }

        /// <summary>
        /// Helper for drawing a texture into the current rendertarget,
        /// using a custom shader to apply postprocessing effects.
        /// </summary>
        protected void DrawFullscreenQuad(Texture2D texture, Rectangle destinationRectangle, Effect effect)
        {
            spriteBatch.Begin(0, BlendState.Opaque, SamplerState, DepthStencilState.None, RasterizerState.CullNone, effect);
            spriteBatch.Draw(texture, destinationRectangle, Color.White);
            spriteBatch.End();
        }

    }
}
