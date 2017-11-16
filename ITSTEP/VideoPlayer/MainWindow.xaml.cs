using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace VideoPlayer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            playList.ItemsSource = videos;
            mediaPlayer.MediaOpened += mediaPlayer_MediaOpened;
            sliderSeek.Visibility = Visibility.Hidden;
        }
        
        private List<VideoItem> videos = new List<VideoItem>();
        DispatcherTimer timer;

        #region Medialement
        private void openFile_Click(object sender, RoutedEventArgs e)
        {       
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Video|*.avi;*.mp4;*.flv;*wmv|All|*.*";
            fileDialog.Multiselect = true;
           
            bool? result = fileDialog.ShowDialog(this);
            if (result.HasValue && (result.Value == true))
            {
                videos.Clear();
                string[] filePathLoad = fileDialog.FileNames;

                foreach (var item in filePathLoad)
                    videos.Add(new VideoItem(item));

                playList.Items.Refresh();
                timer = new DispatcherTimer{Interval = TimeSpan.FromMilliseconds(1000)};
                timer.Tick += timer_Tick;
                timer.Start();
                playList.SelectedItem = videos[0];
                mediaPlayer.Play();
                sliderSeek.Visibility = Visibility.Visible;
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            sliderSeek.Value = mediaPlayer.Position.TotalSeconds;
        }
        private void mediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            int current = playList.SelectedIndex;
            DateTime time = new DateTime();
            
            sliderSeek.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            sliderSeek.Value = 0;
            videos[current].TotalTime = time.AddSeconds(sliderSeek.Maximum);
            this.Title = videos[current].NameFile;            
        }
        private void mediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            int max = videos.Count;
            int current = playList.SelectedIndex;
            if (current < max-1)
                playList.SelectedItem = videos[++current];
        }
        private void mediaPlayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show("Ошибка");
        }
        #endregion
        #region RemoteBar
        private void playVideo_Click(object sender, RoutedEventArgs e)
        {
            if (playList.SelectedIndex!=-1)
            {
                sliderSeek.Visibility = Visibility.Visible;
                mediaPlayer.Play();
            }
        }
        private void pauseVideo_Click(object sender, RoutedEventArgs e) => mediaPlayer.Pause();
        private void stopVideo_Click(object sender, RoutedEventArgs e)
        {
            sliderSeek.Visibility = Visibility.Hidden;
            mediaPlayer.Stop();
        }
        private void muteVideo_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.IsMuted == false)
            {
                mediaPlayer.IsMuted = true;               
                return;
            }
            mediaPlayer.IsMuted = false;
        }
        #endregion
        private void historyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object current = playList.SelectedItem;
            if (current != null)
            {
                VideoItem item = current as VideoItem;
                mediaPlayer.Source = new Uri(item.FilePath);
            }
        }
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DateTime time = new DateTime();
            time = time.AddSeconds(mediaPlayer.Position.TotalSeconds);            
            lblTime.Content = String.Format("{0:HH:mm:ss}", time);
        }
        private void slider_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {                
            mediaPlayer.Pause();
            timer.Stop();
        }
        private void slider_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TimeSpan ts = new TimeSpan();           
            ts = TimeSpan.FromSeconds(sliderSeek.Value);
            mediaPlayer.Position = ts;
            timer.Start();
            mediaPlayer.Play();            
        }
        private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = e.NewValue;    
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}