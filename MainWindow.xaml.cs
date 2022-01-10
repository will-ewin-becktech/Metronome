using System.Timers;
using System.Windows;

namespace Metronome
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //make sure to clean up any active things on the way out
            this.ucStandard.Stop();
            this.ucPolymeter.Stop();
        }
    }
}
