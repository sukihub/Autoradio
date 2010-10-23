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
        //instacia aktualne zobrazenej stranky
        private IModuleInterface current;

        //playlist
        private Playlist playlist = new Playlist();

        //URI modulov
        private Uri RadioUri = new Uri("/Views/Radio.xaml", UriKind.Relative);
        private Uri PlayerUri;

        //stav prehravaca
        private State state;

        public MainPage()
        {
            InitializeComponent();
            PlayerUri = Content.Source;
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
        }

        /**
         *  Event handler volany pri zmene stavu prehravaca.
         *  
         *  @param newState: Novy stav prehravaca.
         */
        public void stateChanged(State newState)
        {
            this.state = newState;

            //if (newState != State.TurnedOff) return;

            ImageSource tmp = AnimationTmpImage.Source;

            AnimationTmpImage.Source = BackgroundImage.ImageSource;
            AnimationTmpImage.Visibility = Visibility.Visible;
            AnimationTmpImage.Opacity = 1.0;

            BackgroundImage.ImageSource = tmp;

            BackgroundSwap.Begin();

            Content.Navigate(RadioUri);
        }
    }
}
