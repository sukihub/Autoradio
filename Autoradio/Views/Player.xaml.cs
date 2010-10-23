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

            uint position = (uint)e.GetPosition(progressArea).X - 12;
            progressBar.Width = position;
        }

        private void progressFrame_MouseMove(object sender, MouseEventArgs e)
        {
            if (!progressFrameDown) return;

            uint position = (uint)e.GetPosition(progressArea).X - 12;
            progressBar.Width = position;
        }

        private void progressFrame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            progressFrameDown = false;
        }
    }
}
