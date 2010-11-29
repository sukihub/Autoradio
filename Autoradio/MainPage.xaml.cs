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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Autoradio.Views;
using Autoradio.Helpers;

namespace Autoradio
{
    public partial class MainPage : UserControl
    {
        //static public MainPage mainPage;

        //instacia aktualne zobrazenej stranky
        private IModuleInterface current;
        private PlaylistView aboveCurrent;
        private Boolean player = true;
        private Boolean vysunMenu = false;

        private Menu menu;

        //playlist
        private Playlist playlist = new Playlist();

        //URI modulov
        private Uri RadioUri = new Uri("/Views/Radio.xaml", UriKind.Relative);
        private Uri PlayerUri = new Uri("/Views/Player.xaml", UriKind.Relative);
        private Uri PlaylistUri = new Uri("/Views/PlaylistView.xaml", UriKind.Relative);
        private Uri MenuUri = new Uri("/Views/Menu.xaml", UriKind.Relative);

        //pozadia
        BitmapImage backRadio, backPlayer, backPaused;

        //stav prehravaca
        private State state;

        public MainPage()
        {
            InitializeComponent();

            //mainPage = this;

            backRadio = new BitmapImage(new Uri("/Views/BackgroundPurple.png", UriKind.Relative));
            backPlayer = new BitmapImage(new Uri("/Views/BackgroundBlue.png", UriKind.Relative));
            backPaused = new BitmapImage(new Uri("/Views/BackgroundYellow.png", UriKind.Relative));

            BackgroundImage.ImageSource = backPlayer;
            Content.Navigate(PlayerUri);
            AboveContent.Navigate(PlaylistUri);
            Settings.Navigate(MenuUri);
        }

        /**
         *  Event handler kliknutia na OFF tlacitko. 
         */
        private void ButtonOff_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        /**
         *  Event handler volany po dokonceni nacitavania stranky/modulu.
         */
        private void Content_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            current = (IModuleInterface) e.Content;
            current.initialize(stateChanged, playlist);
            current.changedVolume(VolumeSlider.Value / 10);
        }

        private void AboveContent_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            aboveCurrent = (PlaylistView) e.Content;
            aboveCurrent.initialize(stateChanged, playlist);
        }

        /**
         *  Event handler volany pri zmene stavu prehravaca.
         *  
         *  @param newState: Novy stav prehravaca.
         */
        public void stateChanged(State newState)
        {
            this.state = newState;
            ImageSource tmp = null;

            switch (newState)
            { 
                case State.TurnedOff:
                    tmp = (this.player) ? backRadio : backPlayer;
                    Content.Navigate((this.player) ? RadioUri : PlayerUri);              
                    this.player = !this.player;
                    break;

                case State.Paused:
                    tmp = backPaused;
                    break;

                case State.Playing:
                    tmp = (this.player) ? backPlayer : backRadio;
                    break;

                case State.PlaylistOn:
                    aboveCurrent.beforeShow(player);
                    PlaylistShow.Begin();
                    Content.IsHitTestVisible = false;
                    AboveContent.IsHitTestVisible = true;
                    break;

                case State.PlaylistOff:
                    PlaylistHide.Begin();
                    current.playlistHidden();
                    Content.IsHitTestVisible = true;
                    AboveContent.IsHitTestVisible = false;
                    break;
            }

            if (tmp == null) return;
            
            AnimationTmpImage.Source = BackgroundImage.ImageSource;
            AnimationTmpImage.Visibility = Visibility.Visible;
            BackgroundImage.ImageSource = tmp;

            BackgroundSwap.Begin();            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!vysunMenu)
            {
                VysunMenu.Begin();
                vysunMenu = true;
                
                Settings.IsHitTestVisible = true;
                Settings.Visibility = Visibility.Visible;
                
                menu.fadeIn();
            }
            else
            {
                menu.fadeOut();
                ZasunMenu.Begin();
                vysunMenu = false;

                Settings.IsHitTestVisible = false;
                //AboveContent.Visibility = System.Windows.Visibility.Collapsed; 
            }
        }

        private void Settings_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            menu = (Menu)e.Content;
            menu.initialize(stateChanged, playlist);
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (current != null)
                current.changedVolume(e.NewValue / 10); 
        }
    }
}
