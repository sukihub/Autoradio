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
    public partial class basic : Page, IModuleInterface
    {
        private StateChangedNotify stateChanged;

        public basic()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            stateChanged(State.Paused);
        }

        /**
         *  Metoda volana hlavnou aplikaciou tesne po vytvoreni stranky.
         *  Hlavna aplikacia moze posielat informacie modulu.
         *  
         *  @param stateChanged: Referencia na metodu volanu pri zmene stavu prehravania.
         */
        public void initialize(StateChangedNotify stateChanged, Playlist playlist)
        {
            this.stateChanged = stateChanged;

            playlist.Open();
            foreach (PlaylistItem item in playlist.items)
            {
                listBox1.Items.Add(item.uri);
            }
        }

    }
}
