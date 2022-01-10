using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace Metronome
{
    /// <summary>
    /// Interaction logic for StandardMetronome.xaml
    /// </summary>
    public partial class StandardMetronome : UserControl
    {
        private const int HIGH_FREQ = 2500;
        private const int LOW_FREQ = 1000;
        private const int DURATION = 10;
        private const int MS_PER_MINUTE = 60000;
        private Timer _timer;
        private int _tickCount = 0, _beat = 1;
        private volatile bool _isRunning = false;

        public StandardMetronome()
        {
            InitializeComponent();
        }
        private void btnClick_Click(object sender, RoutedEventArgs e)
        {
            if (!_isRunning)
                Start();
            else
                Stop();
        }

        internal void Start()
        {
            int bpm, frequencyInMs;
            try
            {
                //initialize the settings
                bpm = int.Parse(txtBPM.Text);
                frequencyInMs = MS_PER_MINUTE / bpm;
                _beat = int.Parse(txtBeat.Text);

            }
            catch
            {
                MessageBox.Show("That wasn't a number, dummy.");
                return;
            }

            //reset the counter
            _tickCount = 0;
            _timer = new Timer(frequencyInMs);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
            _isRunning = true;
            SetUI();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_tickCount++ % _beat == 0)
                System.Console.Beep(LOW_FREQ, DURATION);
            else
                System.Console.Beep(HIGH_FREQ, DURATION);
        }

        internal void Stop()
        {
            // stop and destroy the timer
            if (_timer == null)
                return;
            if (!_timer.Enabled)
                return;
            _timer.Stop();
            _timer.Dispose();
            _isRunning = false;
            SetUI();
        }

        private void SetUI()
        {
            //see which way we have just switched it
            if (_isRunning)
            {
                btnClick.Content = "STOP";
                txtBeat.IsEnabled = false;
                txtBPM.IsEnabled = false;
            }
            else
            {
                btnClick.Content = "START";
                txtBeat.IsEnabled = true;
                txtBPM.IsEnabled = true;
            }
        }
    }
}
