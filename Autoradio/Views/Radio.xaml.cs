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
    public partial class Radio : Page, IModuleInterface
    {
        private const int minPozicia = -413;
        private const int maxPozicia = 455;
        private const double minimalnaFrekvencia = 87;
        private const double maximalnaFrekvencia = 108;
        private const double pocetFrekvencii = maximalnaFrekvencia - minimalnaFrekvencia;
        private const double dlzkaFrekvencie = (maxPozicia + (-1 * minPozicia)) / pocetFrekvencii;

        private double aktualnaFrekvencia;
        private int poziciaFrekvencie;

        private StateChangedNotify stateChanged;
        

        public Radio()
        {
            InitializeComponent();

            poziciaFrekvencie = 0;
            prepocitajFrekvenciu();
            
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void initialize(StateChangedNotify stateChanged, Playlist playlist)
        {
            this.stateChanged = stateChanged;
        }


        private void prepocitajFrekvenciu()
        {
            aktualnaFrekvencia = (double)((poziciaFrekvencie + -1 * minPozicia) / dlzkaFrekvencie);
            aktualnaFrekvencia = maximalnaFrekvencia - aktualnaFrekvencia;
            frekvencia_text.Text = aktualnaFrekvencia.ToString();
        }


        private void button_left_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            poziciaFrekvencie -= 50;

            if (poziciaFrekvencie < minPozicia)
                poziciaFrekvencie = minPozicia;

            MyDoubleAnimation.To = poziciaFrekvencie;
            Posuv_frekvencie.Begin();
            prepocitajFrekvenciu();
               
		}

        private void button_right_Click(object sender, RoutedEventArgs e)
        {
            poziciaFrekvencie += 50;

            if (poziciaFrekvencie > maxPozicia)
                poziciaFrekvencie = maxPozicia;

            MyDoubleAnimation.To = poziciaFrekvencie;
            Posuv_frekvencie.Begin();
            prepocitajFrekvenciu();
            
        }

    }
}
