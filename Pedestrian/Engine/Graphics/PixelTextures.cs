using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedestrian.Engine.Graphics
{
    public class PixelTextures
    {
        public static PixelTextures Instance { get; private set; }

        public Texture2D WhitePixel { get; private set; }

        GraphicsDevice graphicsDevice;

        public PixelTextures(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            WhitePixel = new Texture2D(graphicsDevice, 1, 1);
            WhitePixel.SetData(new Color[1] { Color.White });
            Instance = this;
        }

        public void Unload()
        {
            WhitePixel.Dispose();
        }
    }
}
