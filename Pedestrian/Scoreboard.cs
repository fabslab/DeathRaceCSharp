using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.BitmapFonts;

namespace Pedestrian
{
    public class Scoreboard
    {
        // Duration of game in seconds 
        public const int GAME_TIME = 99;

        BitmapFont font;
        int timeRemaining;
        double timeTracked;
        Player[] players;

        public Scoreboard(Player[] players)
        {
            this.players = players;
            font = PedestrianGame.Instance.Content.Load<BitmapFont>("munro-font01");
            Reset();
        }

        public void Reset()
        {
            timeRemaining = GAME_TIME;
            timeTracked = 0;
        }

        public void Update(GameTime gameTime)
        {
            // Scoreboard timer counts down every second
            timeTracked += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeTracked >= 1 && timeRemaining > 0)
            {
                timeTracked -= 1;
                timeRemaining -= 1;
                if (timeRemaining == 0)
                {
                    Scene.Events.Emit(CoreEvents.GameOver, null);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (players.Length >= 1)
            {
                spriteBatch.DrawString(font, players[0].Score.ToString(), Vector2.Zero, Color.White);
            }

            spriteBatch.DrawString(font, timeRemaining.ToString(), new Vector2(PedestrianGame.VIRTUAL_WIDTH / 2, 0), Color.White);

            if (players.Length >= 2)
            {
                spriteBatch.DrawString(font, players[1].Score.ToString(), new Vector2(PedestrianGame.VIRTUAL_WIDTH - 40, 0), Color.White);
            }
        }
    }
}
