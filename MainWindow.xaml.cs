using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Threading;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlayButton(object sender, RoutedEventArgs e)
        {
            videoClip.Play();
            if (timer != null)
                timer.Start();
        }

        private void PauseButton(object sender, RoutedEventArgs e)
        {
            videoClip.Pause();
            if (timer != null)
                timer.Stop();
        }

        private void StopButton(object sender, RoutedEventArgs e)
        {
            videoClip.Stop();
            if (timer != null)
                timer.Stop();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            videoClip.ScrubbingEnabled = true;
            videoClip.Stop();
        }

        private void videoClip_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimerSlider.Maximum = videoClip.NaturalDuration.TimeSpan.TotalSeconds;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            totalTimeOfVideo.Content = videoClip.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimerSlider.Value = videoClip.Position.TotalSeconds;
        }

        private void videoClip_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        private void TimerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            videoClip.Position = TimeSpan.FromSeconds(TimerSlider.Value);
        }

        private void TimerSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            videoClip.Pause();
            if (timer != null)
                timer.Stop();
        }

        private void TimerSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            videoClip.Play();
            if (timer != null)
                timer.Start();
        }

        private void BrowseButton(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fd = new OpenFileDialog();
                fd.Filter = "MP3 Files (*.mp3)|*.mp3|MP4 File (*.mp4)|*.mp4|3GP File (*.3gp)|*.3gp|Audio File (*.wma)|*.wma|MOV File (*.mov)|*.mov|AVI File (*.avi)|*.avi|Flash Video(*.flv)|*.flv|Video File (*.wmv)|*.wmv|MPEG-2 File (*.mpeg)|*.mpeg|WebM Video (*.webm)|*.webm|All files (*.*)|*.*";
                fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                fd.ShowDialog();
                string filename = fd.FileName;
                if (filename != "")
                {
                    Uri u = new Uri(filename);
                    videoClip.Source = u;
                }
                else
                {
                    System.Windows.MessageBox.Show("No Selection", "Empty");
                }
            }
            catch (Exception e1)
            {
                System.Console.WriteLine("Error Text: " + e1.Message);
            }
        }

        private void SkipForwardButton(object? sender, EventArgs e)
        {
            if (timer != null)
            {
                videoClip.Position += TimeSpan.FromSeconds(10);
                if (videoClip.Position >= videoClip.NaturalDuration.TimeSpan)
                {
                    videoClip.Position -= TimeSpan.FromMilliseconds(100);
                }
            }
        }

        private void SkipBackwardButton(object? sender, EventArgs e)
        {
            if (timer != null)
            {
                videoClip.Position -= TimeSpan.FromSeconds(10);
                if (videoClip.Position <= videoClip.NaturalDuration.TimeSpan)
                {
                    videoClip.Position += TimeSpan.FromMilliseconds(100);
                }
            }
        }
    }
}