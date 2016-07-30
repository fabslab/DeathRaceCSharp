using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.BitmapFonts;

namespace Pedestrian
{
    public class Scoreboard
    {
        // Duration of game in seconds 
        public const int GAME_TIME = 99;
        public bool IsActive { get; set; } = false;

        BitmapFont font;
        int timeRemaining;
        double timeTracked;
        Player[] players;
        int[] scores;

        public Scoreboard(Player[] players)
        {
            this.players = players;
            scores = new int[players.Length];
            font = PedestrianGame.Instance.Content.Load<BitmapFont>("munro-edit-font01");
            font.LetterSpacing = 5;
            Reset();
        }

        public void Reset()
        {
            timeRemaining = GAME_TIME;
            timeTracked = 0;
            UpdateScores();
        }

        private void UpdateScores()
        {
            for (int i = 0, l = players.Length; i < l; ++i)
            {
                scores[i] = players[i].Score;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }

            UpdateScores();

            // Scoreboard timer counts down every second
            timeTracked += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeTracked >= 1 && timeRemaining > 0)
            {
                timeTracked -= 1;
                timeRemaining -= 1;
                if (timeRemaining == 0)
                {
                    IsActive = false;
                    Scene.Events.Emit(CoreEvents.GameOver, null);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (scores.Length >= 1)
            {
                var text = scores[0].ToString("D2");
                var xPosition = Scene.Bounds.X + Scene.SIDEWALK_WIDTH + Scene.BORDER_WIDTH;
                spriteBatch.DrawString(font, text, new Vector2(xPosition, 0), Color.White);
            }
            
            var timeText = timeRemaining.ToString("D2");
            var timeSize = font.GetSize(timeText);
            var timeXPosition = Scene.Bounds.Width / 2 - timeSize.Width / 2;
            spriteBatch.DrawString(font, timeText, new Vector2(timeXPosition, 0), Color.White);

            if (scores.Length >= 2)
            {
                var text = scores[1].ToString("D2");
                var textSize = font.GetSize(text);
                var xPosition = Scene.Bounds.Right - Scene.SIDEWALK_WIDTH - textSize.Width;
                spriteBatch.DrawString(font, text, new Vector2(xPosition, 0), Color.White);
            }
        }
    }
}
