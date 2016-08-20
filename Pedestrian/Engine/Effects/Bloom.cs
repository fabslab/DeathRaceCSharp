using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pedestrian.Engine.Effects
{
    public class Bloom : PostProcessor
    {
        public BloomSettings Settings
        {
            get { return settings; }
            set { if (value != settings) UpdateSettings(value); }
        }

        /// <summary>
        /// Scales render targets used for blurring. Can be decreased safely to 
        /// minimize fillrate costs because blurring does not require high resolution
        /// Defaults to 0.5 (of width and height) of main back buffer.
        /// </summary>
        public float RenderTargetScale { get; set; } = 0.5f;

        BloomSettings settings;

        Effect bloomExtractEffect;
        Effect bloomCombineEffect;
        Effect gaussianBlurEffect;

        RenderTarget2D renderTarget1;
        RenderTarget2D renderTarget2;

        EffectParameter
            bloomExtractThresholdParam,
            bloomIntensityParam,
            bloomBaseIntensityParam,
            bloomSaturationParam,
            bloomBaseSaturationParam,
            bloomBaseTextureParam,
            blurWeightsParam,
            blurOffsetsParam;

        bool isContentLoaded = false;


        public Bloom(GraphicsDevice graphicsDevice) : base(graphicsDevice) {}

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            bloomExtractEffect = PedestrianGame.Instance.Content.Load<Effect>("Shaders/BloomExtract");
            bloomCombineEffect = PedestrianGame.Instance.Content.Load<Effect>("Shaders/BloomCombine");
            gaussianBlurEffect = PedestrianGame.Instance.Content.Load<Effect>("Shaders/GaussianBlur");

            bloomExtractThresholdParam = bloomExtractEffect.Parameters["BloomThreshold"];

            bloomIntensityParam = bloomCombineEffect.Parameters["BloomIntensity"];
            bloomBaseIntensityParam = bloomCombineEffect.Parameters["BaseIntensity"];
            bloomSaturationParam = bloomCombineEffect.Parameters["BloomSaturation"];
            bloomBaseSaturationParam = bloomCombineEffect.Parameters["BaseSaturation"];
            bloomBaseTextureParam = bloomCombineEffect.Parameters["BaseTexture"];

            blurWeightsParam = gaussianBlurEffect.Parameters["SampleWeights"];
            blurOffsetsParam = gaussianBlurEffect.Parameters["SampleOffsets"];

            isContentLoaded = true;

            UpdateSettings(settings);

            // Look up the resolution and format of our main backbuffer to create render targets
            PresentationParameters pp = graphicsDevice.PresentationParameters;
            float width = pp.BackBufferWidth * RenderTargetScale;
            float height = pp.BackBufferHeight * RenderTargetScale;
            SurfaceFormat format = pp.BackBufferFormat;
            renderTarget1 = new RenderTarget2D(graphicsDevice, (int)width, (int)height, false, format, DepthFormat.None);
            renderTarget2 = new RenderTarget2D(graphicsDevice, (int)width, (int)height, false, format, DepthFormat.None);
        }


        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        public void Unload()
        {
            renderTarget1.Dispose();
            renderTarget2.Dispose();
        }

        /// <summary>
        /// This is where it all happens. Grabs a scene that has already been rendered,
        /// and uses postprocess magic to add a glowing bloom effect over the top of it.
        /// </summary>
        public override void Process(RenderTarget2D source, RenderTarget2D destination)
        {
            graphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;

            // Pass 1: draw the scene into rendertarget 1, using a
            // shader that extracts only the brightest parts of the image.
            DrawFullscreenQuad(source, renderTarget1, bloomExtractEffect);

            // Pass 2: draw from rendertarget 1 into rendertarget 2,
            // using a shader to apply a horizontal gaussian blur filter.
            SetBlurEffectParameters(1.0f / renderTarget1.Width, 0);
            DrawFullscreenQuad(renderTarget1, renderTarget2, gaussianBlurEffect);

            // Pass 3: draw from rendertarget 2 back into rendertarget 1,
            // using a shader to apply a vertical gaussian blur filter.
            SetBlurEffectParameters(0, 1.0f / renderTarget1.Height);
            DrawFullscreenQuad(renderTarget2, renderTarget1, gaussianBlurEffect);

            // Pass 4: draw both rendertarget 1 and the original scene
            // image into the destination target, using a shader that
            // combines them to produce the final bloomed result.
            bloomBaseTextureParam.SetValue(source);
            DrawFullscreenQuad(renderTarget1, destination, bloomCombineEffect);
        }

        /// <summary>
		/// Updates the Settings configuration and sets the 
        /// parameters used by the bloom and blur shaders.
		/// </summary>
		/// <param name="settings">Settings.</param>
        void UpdateSettings(BloomSettings settings)
        {
            this.settings = settings;

            if (!isContentLoaded) return;

            bloomExtractThresholdParam.SetValue(settings.BloomThreshold);

            bloomIntensityParam.SetValue(settings.BloomIntensity);
            bloomBaseIntensityParam.SetValue(settings.BaseIntensity);
            bloomSaturationParam.SetValue(settings.BloomSaturation);
            bloomBaseSaturationParam.SetValue(settings.BaseSaturation);
        }

        /// <summary>
        /// Computes sample weightings and texture coordinate offsets
        /// for one pass of a separable gaussian blur filter.
        /// </summary>
        void SetBlurEffectParameters(float dx, float dy)
        {
            // Look up how many samples our gaussian blur effect supports.
            var sampleCount = blurWeightsParam.Elements.Count;

            // Create temporary arrays for computing our filter settings.
            var sampleWeights = new float[sampleCount];
            var sampleOffsets = new Vector2[sampleCount];

            // The first sample always has a zero offset.
            sampleWeights[0] = ComputeGaussian(0);
            sampleOffsets[0] = Vector2.Zero;

            // Maintain a sum of all the weighting values.
            var totalWeights = sampleWeights[0];

            // Add pairs of additional sample taps, positioned
            // along a line in both directions from the center.
            for (int i = 0, l = sampleCount / 2; i < l; ++i)
            {
                // Store weights for the positive and negative taps.
                var weight = ComputeGaussian(i + 1);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                // To get the maximum amount of blurring from a limited number of
                // pixel shader samples, we take advantage of the bilinear filtering
                // hardware inside the texture fetch unit. If we position our texture
                // coordinates exactly halfway between two texels, the filtering unit
                // will average them for us, giving two samples for the price of one.
                // This allows us to step in units of two texels per sample, rather
                // than just one at a time. The 1.5 offset kicks things off by
                // positioning us nicely in between two texels.
                var sampleOffset = i * 2 + 1.5f;

                var delta = new Vector2(dx, dy) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            // Normalize the list of sample weightings, so they will always sum to one.
            for (int i = 0, l = sampleWeights.Length; i < l; ++i)
            {
                sampleWeights[i] /= totalWeights;
            }

            // Tell the effect about our new filter settings.
            blurWeightsParam.SetValue(sampleWeights);
            blurOffsetsParam.SetValue(sampleOffsets);
        }

        /// <summary>
        /// Evaluates a single point on the gaussian falloff curve.
        /// Used for setting up the blur filter weightings.
        /// </summary>
        float ComputeGaussian(float n)
        {
            float theta = Settings.BlurAmount;

            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }
    }
}
