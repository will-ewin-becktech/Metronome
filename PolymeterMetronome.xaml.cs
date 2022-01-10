using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Metronome
{
    /// <summary>
    /// Interaction logic for StandardMetronome.xaml
    /// </summary>
    public partial class PolymeterMetronome : UserControl
    {
        private const int LOW_FREQ = 1000;
        private const int DURATION = 10;
        private List<TimedBeeper> _beepers;
        private volatile bool _isRunning = false;

        public PolymeterMetronome()
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

        /// <summary>
        /// wrapper for initialization and child-object begin routines
        /// </summary>
        internal void Start()
        {
            int spm;
            try
            {
                //initialize the settings for the base beat
                spm = int.Parse(txtSPM.Text);
            }
            catch
            {
                MessageBox.Show("That wasn't a number, dummy.");
                return;
            }

            //set up the beepers
            _beepers = new List<TimedBeeper>();
            if (chk1.IsChecked == true)
                _beepers.Add(new TimedBeeper(LOW_FREQ, 1500, DURATION, 1, spm));
            if (chk2.IsChecked == true)
                _beepers.Add(new TimedBeeper(LOW_FREQ, 2000, DURATION, 2, spm));
            if (chk3.IsChecked == true)
                _beepers.Add(new TimedBeeper(LOW_FREQ, 3000, DURATION, 3, spm));
            if (chk4.IsChecked == true)
                _beepers.Add(new TimedBeeper(LOW_FREQ, 4000, DURATION, 4, spm));
            if (chk6.IsChecked == true)
                _beepers.Add(new TimedBeeper(LOW_FREQ, 6000, DURATION, 6, spm));
            if (chk8.IsChecked == true)
                _beepers.Add(new TimedBeeper(LOW_FREQ, 8000, DURATION, 8, spm));

            //start them all at the same time
            var seed = System.DateTime.Now.Second;
            seed += 3;
            _beepers.AsParallel().ForAll(b =>
            {
                new System.Threading.Thread(x => b.Start(seed)).Start();
            });

            _isRunning = true;
            SetUI();
        }

        /// <summary>
        /// wrapper for child-object halt routines
        /// </summary>
        internal void Stop()
        {
            // stop them all
            if (_beepers == null)
                return;
            _beepers.AsParallel().ForAll( b =>
            {
                b.Stop();
            });

            _isRunning = false;
            SetUI();
        }

        private void SetUI()
        {
            //see which way we have just switched it
            if (_isRunning)
            {
                btnClick.Content = "STOP";
                chk1.IsEnabled = false;
                chk2.IsEnabled = false;
                chk3.IsEnabled = false;
                chk4.IsEnabled = false;
                chk6.IsEnabled = false;
                chk8.IsEnabled = false;
                txtSPM.IsEnabled = false;
            }
            else
            {
                btnClick.Content = "START";
                chk1.IsEnabled = true;
                chk2.IsEnabled = true;
                chk3.IsEnabled = true;
                chk4.IsEnabled = true;
                chk6.IsEnabled = true;
                chk8.IsEnabled = true;
                txtSPM.IsEnabled = true;
            }
        }
    }
}
