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
    public partial class PlaylistView : Page, IModuleInterface
    {
        private Playlist playlist;
        private StateChangedNotify stateChanged;

        public PlaylistView()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void initialize(StateChangedNotify stateChanged, Playlist playlist)
        {
            this.playlist = playlist;
            this.stateChanged = stateChanged;

            playlist.Open();

            List.ItemsSource = playlist.items;
            //List.DisplayMemberPath = "uri";
        }

        private void Repeat_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Shuffle_Click(object sender, RoutedEventArgs e)
        {
            //Shuffle.IsChecked = !Shuffle.IsChecked;
        }

        private void BackArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //stateChanged(State.PlaylistOff);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            stateChanged(State.PlaylistOff);
        }
    }
}
