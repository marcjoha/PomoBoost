using System;

namespace PomoBoost
{
    class PomodoroTimer
    {
        private TimeSpan timeSpan;
        private TimeSpan timeRemaining;

        public PomodoroTimer(TimeSpan ts)
        {
            this.timeSpan = ts;
            this.timeRemaining = ts;
        }

        public void Tick(TimeSpan ts)
        {
            this.timeRemaining = this.timeRemaining.Subtract(ts);
        }

        public void Reset()
        {
            this.timeRemaining = this.timeSpan;
        }

        public bool IsZero()
        {
            return this.timeRemaining == TimeSpan.Zero;
        }

        public TimeSpan GetTimeRemaining()
        {
            return this.timeRemaining;
        }

        public TimeSpan GetTimeSpan()
        {
            return this.timeSpan;
        }

        public override String ToString()
        {
            return this.timeRemaining.Minutes.ToString().PadLeft(2, '0') + ":" + this.timeRemaining.Seconds.ToString().PadLeft(2, '0');
        }

    }
}
