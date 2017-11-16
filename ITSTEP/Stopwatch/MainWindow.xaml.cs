using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Stopwatch{
 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyNotifyIcon = new System.Windows.Forms.NotifyIcon
            {
                //Icon = new System.Drawing.Icon(@"icon.ico")
            };
            MyNotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(MyNotifyIcon_MouseClick);
        }

        private NotifyIcon MyNotifyIcon;
        DateTime date;
        DateTime stopwath;
        DispatcherTimer timer;
        private int laps = 1;

        private void MyNotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }        
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                MyNotifyIcon.Visible = true;
                MyNotifyIcon.ShowBalloonTip(400, "Секундомер", "Я спрятался в трее. Если что я здесь!!", ToolTipIcon.None);
               
            }
            else if (this.WindowState == WindowState.Normal)
            {
                this.Topmost = true;
                MyNotifyIcon.Visible = false;
                this.ShowInTaskbar = true;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {           
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += timer_Tick;
            btnlap.IsEnabled = false;
            btnreset.IsEnabled = false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            long tick = DateTime.Now.Ticks - date.Ticks;
            stopwath = new DateTime();

            stopwath = stopwath.AddTicks(tick);
            TimerLabel.Text = String.Format("{0:HH:mm:ss:fff}", stopwath);
        }
        private void start(object sender, EventArgs e)
        {
            date = DateTime.Now;
            timer.Start();
            btnstart.IsEnabled = false;
            btnreset.IsEnabled = true;
            btnlap.IsEnabled = true;
        }

        private void reset(object sender, EventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();               
                laps = 1;
                btnlap.IsEnabled = false;
                btnreset.Content = "Reset";
                return;
            }
            stopwath = new DateTime();
            TimerLabel.Text = String.Format("{0:HH:mm:ss:fff}", stopwath);
            lbox.Items.Clear();
            btnreset.IsEnabled = false;
            btnstart.IsEnabled = true;
            btnreset.Content = "Stop";
        }

        private void lap(object sender, EventArgs e)
        {
            lbox.Items.Add(String.Format("{0}-й кург! Время: {1:HH:mm:ss:fff}", laps++, stopwath));
        }

       
    }
}