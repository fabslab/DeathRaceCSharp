using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine;
using Pedestrian.Engine.BitmapFonts;

namespace Pedestrian
{
    public class Scoreboard
    {
        // Duration of game in seconds 
        public int GAME_TIME { get; set; } = 3;
        public int Margin { get; set; }
        public bool IsActive { get; set; } = false;

        BitmapFont font;
        int timeRemaining;
        double timeTracked;
        int[] scores;
        int numPlayers;
        Rectangle displayArea;

        public Scoreboard(int numPlayers, Rectangle displayArea)
        {
            this.numPlayers = numPlayers;
            this.displayArea = displayArea;
            scores = new int[numPlayers];
            font = PedestrianGame.Instance.Content.Load<BitmapFont>("Fonts/munro-edit-font-32px");
            font.LetterSpacing = 5;
            Reset();
            PedestrianGame.Instance.Events.AddObserver(GameEvents.PlayerScored, OnPlayerScored);
        }

        private void OnPlayerScored(IEntity player)
        {
            if (!IsActive)
            {
                return;
            }

            var p = (Player)player;
            var i = (int)p.PlayerIndex;
            if (i <= numPlayers)
            {
                scores[i] = p.Score;
            }
        }

        public void Reset()
        {
            timeRemaining = GAME_TIME;
            timeTracked = 0;
        }

        public void RemoveObservers()
        {
            PedestrianGame.Instance.Events.RemoveObserver(GameEvents.PlayerScored, OnPlayerScored);
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }

            // Scoreboard timer counts down every second
            timeTracked += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeTracked >= 1 && timeRemaining > 0)
            {
                timeTracked -= 1;
                timeRemaining -= 1;
                if (timeRemaining == 0)
                {
                    IsActive = false;
                    PedestrianGame.Instance.Events.Emit(GameEvents.GameOver, null);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (numPlayers >= 1)
            {
                var text = scores[0].ToString("D2");
                var xPosition = displayArea.X + Margin;
                spriteBatch.DrawString(font, text, new Vector2(xPosition, 0), Color.White);
            }
            
            var timeText = timeRemaining.ToString("D2");
            var timeSize = font.GetSize(timeText);
            var timeXPosition = displayArea.Left + (displayArea.Width / 2 - timeSize.Width / 2);
            spriteBatch.DrawString(font, timeText, new Vector2(timeXPosition, 0), Color.White);

            if (numPlayers >= 2)
            {
                var text = scores[1].ToString("D2");
                var textSize = font.GetSize(text);
                var xPosition = displayArea.Right - Margin - textSize.Width;
                spriteBatch.DrawString(font, text, new Vector2(xPosition, 0), Color.White);
            }
        }
    }
}
