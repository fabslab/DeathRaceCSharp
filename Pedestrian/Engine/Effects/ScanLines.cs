using Microsoft.Xna.Framework.Graphics;


namespace Pedestrian.Engine.Effects
{
    public class ScanLines : PostProcessor
    {
        EffectParameter
            attenuationParam,
            linesFactorParam;

        float attenuation = 0.2f;
        float linesFactor = 1800;

        bool isContentLoaded = false;

        public float Attenuation
        {
            get { return attenuation; }
            set
            {
                attenuation = value;
                if (isContentLoaded)
                {
                    attenuationParam.SetValue(attenuation);
                }
            }
        }

        public float LinesFactor
        {
            get { return linesFactor; }
            set
            {
                linesFactor = value;
                if (isContentLoaded)
                {
                    linesFactorParam.SetValue(linesFactor);
                }
            }
        }


        public ScanLines(GraphicsDevice graphicsDevice) : base(graphicsDevice) {}


        public override void LoadContent()
        {
            base.LoadContent();

            Effect = PedestrianGame.Instance.Content.Load<Effect>("Shaders/ScanLines");

            attenuationParam = Effect.Parameters["Attenuation"];
            linesFactorParam = Effect.Parameters["LinesFactor"];

            isContentLoaded = true;

            attenuationParam.SetValue(attenuation);
            linesFactorParam.SetValue(linesFactor);
        }
    }
}
