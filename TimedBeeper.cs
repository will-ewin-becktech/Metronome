using System.Timers;

namespace Metronome
{
    internal class TimedBeeper
    {
        private int _baseFrequency, _altFrequency, _duration, _beat, _tickCount, _spm;
        private Timer _timer;

        /// <summary>
        /// initialize a timed, pulsed, beeper
        /// </summary>
        /// <param name="basefrequency">base beat beep pitch, in Hz</param>
        /// <param name="altFrequency">other beat beep pitch, in Hz</param>
        /// <param name="duration">how long should the beat be</param>
        /// <param name="beat">how many beats per measure</param>
        /// <param name="secondsPerMeasure">exactly what it sounds like</param>
        public TimedBeeper(int basefrequency, int altFrequency, int duration, int beat, int secondsPerMeasure)
        {
            _baseFrequency = basefrequency;
            _altFrequency = altFrequency;
            _duration = duration;
            _beat = beat;
            _spm = secondsPerMeasure;
        }

        /// <summary>
        /// begin execution of the timer
        /// </summary>
        /// <param name="seed">(allegedly) random start parameter</param>
        public void Start(int seed)
        {
            double timeBetweenBeeps = ((double)_spm * 1000.0d) / (double)_beat;
            _timer = new Timer(timeBetweenBeeps);
            _tickCount = 0;
            _timer.Elapsed += _timer_Elapsed;
            while (System.DateTime.Now.Second != seed)
                System.Threading.Thread.Sleep(System.TimeSpan.Zero);
            _timer.Start();
        }

        /// <summary>
        /// halt execution of the timer
        /// </summary>
        public void Stop()
        {
            if (_timer == null)
                return;
            if (!_timer.Enabled)
                return;
            _timer.Stop();
            _timer.Dispose();
        }

        /// <summary>
        /// the "tick" event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //see if it's the base beat or one of the other ones
            if (_tickCount++ % _beat == 0)
                System.Console.Beep(_baseFrequency, _duration);
            else
                System.Console.Beep(_altFrequency, _duration);
        }

        
    }
}
