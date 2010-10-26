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
    public partial class Menu : Page, IModuleInterface
    {
        private StateChangedNotify stateChanged;

        public Menu()
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


        public void fadeIn()
        {
        	allFadeIn.Begin();
        }

        public void fadeOut()
        {
			allFadeOut.Begin();
        }

        private void RDS_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            rdsCheck.IsChecked = !rdsCheck.IsChecked;
        }

    }
}
