using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Autoradio.Helpers;

namespace Autoradio.Views
{
    public partial class Player : Page, IModuleInterface
    {
        private StateChangedNotify stateChanged;
        private State state = State.Playing;

        public Player()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void initialize(StateChangedNotify stateChanged, Playlist playlist)
        {
            this.stateChanged = stateChanged;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            stateChanged(State.TurnedOff);
        }

        private Boolean progressFrameDown = false;

        private void progressFrame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            progressFrameDown = true;

            progressFrame_MouseMove(sender, e);
        }

        private void progressFrame_MouseMove(object sender, MouseEventArgs e)
        {
            if (!progressFrameDown) return;

            uint position = (uint)e.GetPosition(progressArea).X - 4;
            progressBar.Width = position;

            uint minutes = (uint)progressBar.Width / 60;
            uint seconds = (uint)progressBar.Width % 60;
            timeMinutes.Text = minutes.ToString();
            timeSeconds.Text = (seconds < 10 ? "0" : "") + seconds.ToString();
        }

        private void progressFrame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            progressFrameDown = false;
        }

        private void coverNext_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            animNext.Source = coverNext.Source;
            animNow.Source = coverNow.Source;
            animPrevious.Source = coverPrevious.Source;
            //nacitat z playlistu
            //animNew.Source = 

            rotateNext.Begin();

            coverNow.Source = animNext.Source;
            coverPrevious.Source = animNow.Source;
            coverNext.Source = animNew.Source;
        }

        private void coverPrevious_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            animNext.Source = coverNext.Source;
            animNow.Source = coverNow.Source;
            animPrevious.Source = coverPrevious.Source;
            //nacitat z playlistu
            //animNew.Source = 

            rotatePrevious.Begin();

            coverNow.Source = animPrevious.Source;
            coverPrevious.Source = animNew.Source;
            coverNext.Source = animNow.Source;
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
            }
            else
            {
                state = State.Playing;
                Play.Begin();
            }

            stateChanged(state);
        }
    }
}
