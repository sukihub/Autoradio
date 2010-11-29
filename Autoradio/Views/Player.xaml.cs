using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Windows.Threading;
using Autoradio.Helpers;

namespace Autoradio.Views
{
    public partial class Player : Page, IModuleInterface
    {
        private StateChangedNotify stateChanged;
        private State state = State.Paused;
        
        private DispatcherTimer timer = new DispatcherTimer();
        private double duration, position;

        private Playlist playlist;

        private Boolean progressFrameDown = false;

        public Player()
        {
            InitializeComponent();

            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (state != State.Playing || progressFrameDown) return;

            timeMinutes.Text = mediaPlayer.Position.Minutes.ToString();
            timeSeconds.Text = mediaPlayer.Position.Seconds.ToString().PadLeft(2, '0');

            position = mediaPlayer.Position.TotalSeconds;

            progressBar.Width = position * 648.0 / duration;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void initialize(StateChangedNotify stateChanged, Playlist playlist)
        {
            this.stateChanged = stateChanged;
            this.playlist = playlist;
        }

        public void playlistHidden()
        {
            int track = playlist.changedTrackID;
            playlist.changedTrackID = -1;

            //ak bol zmeneny playlist, nastavime prehravanie prvej pesnicky (= zmenime skladbu)
            if (playlist.isChanged())
            {
                if (track == -1) track = 0;

                if (playlist.items.Count == 0)
                {
                    emptyPlaylist.Visibility = Visibility.Visible;
                    coverNow.Opacity = 0.0;
                    coverPrevious.Opacity = 0.0;
                    coverNext.Opacity = 0.0;
                    labelInterpret.Opacity = 0.0;
                    labelTitle.Opacity = 0.0;
                }
                else
                {
                    emptyPlaylist.Visibility = Visibility.Collapsed;
                    coverNow.Opacity = 100.0;
                    coverPrevious.Opacity = 100.0;
                    coverNext.Opacity = 100.0;
                    labelInterpret.Opacity = 100.0;
                    labelTitle.Opacity = 100.0;
                }
            }

            //ak bola zmenena skladba
            if (track != -1)
            {
                playlist.current = track;
                ChangeTrack();

                coverPrevious.Source = playlist.GetPrevious().cover;
                coverNow.Source = playlist.items[track].cover;
                coverNext.Source = playlist.GetNext().cover;

                labelTitle.Text = playlist.items[playlist.current].title;
                labelInterpret.Text = playlist.items[playlist.current].artist;
            }
        }

        private void ChangeTrack()
        {
            PlaylistItem current = playlist.items[playlist.current];

            mediaPlayer.Stop();
            mediaPlayer.AutoPlay = (state == State.Playing) ? true : false;
            mediaPlayer.SetSource(current.file.OpenRead());

            duration = current.duration.TotalSeconds;
        }

        private void NextTrack()
        {
            playlist.MoveToNext();
            ChangeTrack();
        }

        private void PreviousTrack()
        {
            playlist.MoveToPrevious();
            ChangeTrack();
        }
        
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            stateChanged(State.TurnedOff);
        }

        private void progressFrame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            progressFrameDown = true;

            progressFrame_MouseMove(sender, e);
        }

        private TimeSpan tmp;

        private void progressFrame_MouseMove(object sender, MouseEventArgs e)
        {
            if (!progressFrameDown) return;

            uint position = (uint)e.GetPosition(progressArea).X - 4;
            progressBar.Width = position;

            tmp = TimeSpan.FromSeconds((double)position * duration / 648.0);
            
            timeMinutes.Text = tmp.Minutes.ToString();
            timeSeconds.Text = tmp.Seconds.ToString().PadLeft(2, '0');
        }

        private void progressFrame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            progressFrameDown = false;

            int position = (int)e.GetPosition(progressArea).X - 4;
            mediaPlayer.Position = tmp; 
        }

        private void coverNext_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NextTrack();

            animNew.Source = playlist.GetNext().cover;
            animNext.Source = coverNext.Source;
            animNow.Source = coverNow.Source;
            animPrevious.Source = coverPrevious.Source;
            //nacitat z playlistu
            //animNew.Source = 

            rotateNext.Begin();

            coverNow.Source = animNext.Source;
            coverPrevious.Source = animNow.Source;
            coverNext.Source = animNew.Source;

            labelTitle.Text = playlist.items[playlist.current].title;
            labelInterpret.Text = playlist.items[playlist.current].artist;
        }

        private void coverPrevious_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PreviousTrack();

            animNext.Source = coverNext.Source;
            animNow.Source = coverNow.Source;
            animPrevious.Source = coverPrevious.Source;
            animNew.Source = playlist.GetPrevious().cover;
            //nacitat z playlistu
            //animNew.Source = 

            rotatePrevious.Begin();

            coverNow.Source = animPrevious.Source;
            coverPrevious.Source = animNew.Source;
            coverNext.Source = animNow.Source;

            labelTitle.Text = playlist.items[playlist.current].title;
            labelInterpret.Text = playlist.items[playlist.current].artist;
        }

        private Boolean coverMouseDown = false;
        private Point startPoint, tmpPoint;

        private void coverNow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            coverMouseDown = true;
            startPoint = e.GetPosition(coverNow);
        }

        private void coverNow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (coverMouseDown) stateChanged(State.PlaylistOn);

            coverMouseDown = false;
        }

        private void coverNow_MouseMove(object sender, MouseEventArgs e)
        {
            if (!coverMouseDown) return;

            //poloha mysky
            tmpPoint = e.GetPosition(coverNow);

            int xDistance = (int)startPoint.X - (int)tmpPoint.X;
            int yDistance = (int)startPoint.Y - (int)tmpPoint.Y;

            if (xDistance > 30)
            {
                coverNext_MouseLeftButtonDown(sender, null);
                coverMouseDown = false;
            }
            else if (xDistance < -30)
            {
                coverPrevious_MouseLeftButtonDown(sender, null);
                coverMouseDown = false;
            }
            else if (yDistance > 70)
            {
                stateChanged(State.TurnedOff);
            }
        }

        private void coverNow_MouseLeave(object sender, MouseEventArgs e)
        {
            coverMouseDown = false;
        }

        private void controlWrapper_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (state == State.Playing)
            {
                state = State.Paused;
                Pause.Begin();
                mediaPlayer.Pause();
            }
            else
            {
                state = State.Playing;
                Play.Begin();
                mediaPlayer.Play();
            }

            stateChanged(state);
        }

        private void controlWrapper_MouseEnter(object sender, MouseEventArgs e)
        {
            PlayMouseEnter.Begin();
        }

        private void controlWrapper_MouseLeave(object sender, MouseEventArgs e)
        {
            PlayMouseLeave.Begin();
        }

        private void mediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            coverNext_MouseLeftButtonDown(null, null);
        }
    }
}
