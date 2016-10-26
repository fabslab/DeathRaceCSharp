using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Pedestrian.Engine
{
    public static class Timers
    {
        static List<Timer> timers = new List<Timer>();

        public static Timer GetTimer(int msTime)
        {
            var timer = new Timer(msTime);
            timers.Add(timer);
            return timer;
        }

        public static void Update(GameTime gameTime)
        {
            timers.ForEach(timer => timer.Update(gameTime));
        }

        public static void RemoveTimer(Timer timer)
        {
            timers.Remove(timer);
        }
    }

    public class Timer
    {
        int maxTime;
        int currentTime = 0;
        bool ended = false;

        public bool Paused { get; set; } = false;
        public Action OnTimerEnd { get; set; }
        public int RemainingTime { get { return maxTime - currentTime; } }

        public Timer(int msTime)
        {
            maxTime = msTime;
        }

        public void Reset()
        {
            ended = false;
            currentTime = 0;
        }

        public void End()
        {
            if (ended) return;

            ended = true;
            currentTime = maxTime;
            OnTimerEnd?.Invoke();
        }

        public void Update(GameTime gameTime)
        {
            if (ended || Paused) return;

            currentTime += gameTime.ElapsedGameTime.Milliseconds;
            if (currentTime >= maxTime)
            {
                ended = true;
                currentTime = maxTime;
                OnTimerEnd?.Invoke();
            }
        }
    }
}
