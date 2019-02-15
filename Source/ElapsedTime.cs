using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using KC.WPF_Kanban.Utils;

namespace KC.WPF_Kanban
{
    /// <summary>
    /// Displays the time elapsed since a given point in time
    /// </summary>
    public class ElapsedTime : Control
    {
        #region Override DP Metadata

        static ElapsedTime()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ElapsedTime), new FrameworkPropertyMetadata(typeof(ElapsedTime)));
        }

        #endregion

        /// <summary>
        /// Gets or sets the point in time, from which the elapsed time is calculated
        /// </summary>
        public DateTime Time
        {
            get { return (DateTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(DateTime), typeof(ElapsedTime),
                new FrameworkPropertyMetadata(DateTime.MinValue, new PropertyChangedCallback(OnTimeChanged)));

        private static void OnTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ElapsedTime t)
            {
                t.ConvertTimeStr();
            }
        }

        /// <summary>
        /// Gets or sets the elapsed time as short string
        /// </summary>
        public string ElapsedTimeStr
        {
            get { return (string)GetValue(ElapsedTimeStrProperty); }
            set { SetValue(ElapsedTimeStrProperty, value); }
        }
        public static readonly DependencyProperty ElapsedTimeStrProperty =
            DependencyProperty.Register("ElapsedTimeStr", typeof(string), typeof(ElapsedTime),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Calculates the elapsed time string
        /// </summary>
        private void ConvertTimeStr()
        {
            _timer?.Stop();
            if (Time == DateTime.MinValue)
            {
                ElapsedTimeStr = string.Empty;
                return;
            }
            TimeSpan duration = DateTime.UtcNow - Time.ToUniversalTime();
            ElapsedTimeStr = duration.AsShortStr();

            SetTimerInterval(duration);
        }


        private DispatcherTimer _timer;

        /// <summary>
        /// Sets the update timer to a fitting interval
        /// </summary>
        private void SetTimerInterval(TimeSpan duration)
        {
            if (_timer==null)
            {
                _timer = new DispatcherTimer();
                _timer.Tick += this._timer_Tick;
            }

            // Set the best interval for updating the control
            if (duration.TotalSeconds <= 60)
            {
                if (_timer.Interval.TotalSeconds != 1)
                {
                    _timer.Interval = TimeSpan.FromSeconds(1);
                }
            }
            else if (duration.TotalMinutes <= 10)
            {
                if (_timer.Interval.TotalSeconds != 6)
                {
                    _timer.Interval = TimeSpan.FromSeconds(6);
                }
            }
            else if (duration.TotalMinutes <= 60)
            {
                if (_timer.Interval.TotalMinutes != 1)
                {
                    _timer.Interval = TimeSpan.FromMinutes(1);
                }
            }
            else if (duration.TotalHours <= 10)
            {
                if (_timer.Interval.TotalMinutes != 6)
                {
                    _timer.Interval = TimeSpan.FromMinutes(6);
                }
            }
            else if (duration.TotalHours <= 240)
            {
                if (_timer.Interval.TotalHours != 1)
                {
                    _timer.Interval = TimeSpan.FromHours(1);
                }
            }
            else
            {
                if (_timer.Interval.TotalDays != 1)
                {
                    _timer.Interval = TimeSpan.FromDays(1);
                }
            }
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e) => ConvertTimeStr();
    }
}
