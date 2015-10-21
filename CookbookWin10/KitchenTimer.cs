using System;
using Windows.UI.Xaml;

namespace CookbookWin10
{
    class KitchenTimer
    {
        private int minutes;
        private int seconds;
        private DispatcherTimer stopwatch;

        private EventArgs eventArgs = null;
        public event TickHandler Tick;
        public delegate void TickHandler(KitchenTimer kitchenTimer, EventArgs e);
        public event TimerDoneHandler TimeDone;
        public delegate void TimerDoneHandler(KitchenTimer kitchenTimer, EventArgs e);


        public KitchenTimer()
        {
            minutes = seconds = 0;
            stopwatch = new DispatcherTimer();
            stopwatch.Tick += Stopwatch_Tick;
            stopwatch.Interval = new TimeSpan(0, 0, 0, 1);
        }

        public void startKitchenTimer()
        {
            stopwatch.Start();
        }
        private void Stopwatch_Tick(object sender, object e)
        {
            if ((seconds - 1) < 0)
            {
                seconds = 59;
                if ((minutes - 1) >= 0)
                {
                    minutes--;
                }
            }
            else
            {
                seconds--;
            }

            if (minutes <= 0 && seconds <= 0)
            {
                seconds = minutes = 0;                
                stopwatch.Stop();                
                TimeDone(this, eventArgs);
            }
            Tick(this, eventArgs);            
        }        

        public void incrementSeconds()
        {
            seconds = ((seconds + 1 >= 60) ? 0 : seconds + 1);
            Tick(this, eventArgs);
        }
        public void incrementMinutes()
        {
            minutes = ((minutes + 1 > 99) ? 0 : minutes + 1);
            Tick(this, eventArgs);
        }
        public void decrementSeconds()
        {
            seconds = ((seconds - 1 < 0) ? 59 : seconds - 1);
            Tick(this, eventArgs);
        }
        public void decrementMinutes()
        {
            minutes = ((minutes - 1 < 0) ? 99 : minutes - 1);
            Tick(this, eventArgs);
        }
        public int getMinutes()
        {
            return minutes;
        }
        public void setMinutes(int minutes)
        {
            this.minutes = minutes;
        }
        public int getSeconds()
        {
            return seconds;
        }
        public void setSeconds(int seconds)
        {
            this.seconds = seconds;
        }
    }
}
