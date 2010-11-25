using System;
using System.Collections.Generic;
using System.Globalization;
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
        private State state = State.Playing;

        private const int minPozicia = -413;
        private const int maxPozicia = 455;
        private const double minimalnaFrekvencia = 87;
        private const double maximalnaFrekvencia = 108;
        private const double pocetFrekvencii = maximalnaFrekvencia - minimalnaFrekvencia;
        private const double dlzkaFrekvencie = (maxPozicia + (-1 * minPozicia)) / pocetFrekvencii;

        private const String pridatFrekText = "Pridať rádiostanicu";
        private const String odobratFrekText = "Odobrať rádiostanicu";

        private bool bolPohyb = false;

        private double aktualnaFrekvencia = 0;
        private double poziciaFrekvencie;

        private Boolean coverMouseDown = false;
        private Point startPoint, tmpPoint;

        private StateChangedNotify stateChanged;

        private Playlist myPlaylist;

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
            myPlaylist = playlist;
            //myPlaylist.Open();
            searchForClosestRadio();
        }

        public void playlistHidden()
        {
        }

        private void prepocitajFrekvenciu()
        {
            aktualnaFrekvencia = (double)((poziciaFrekvencie + -1 * minPozicia) / dlzkaFrekvencie);
            aktualnaFrekvencia = maximalnaFrekvencia - aktualnaFrekvencia;

            frekvencia_text.Text = String.Format(NumberFormatInfo.InvariantInfo, "{0:0.0}", aktualnaFrekvencia);
        }

   

        private void searchForClosestRadio()
        {
            int i,  index = 0;
            double najblizsie = 100;
            
            for (i=0; i<10; i++)
            {
                if (Math.Abs(aktualnaFrekvencia - myPlaylist.items[i].frequency) < najblizsie)
                {
                    najblizsie = Math.Abs(aktualnaFrekvencia - myPlaylist.items[i].frequency);
                    index = i;
                }
             }
           
            nazov_stanice.Text = myPlaylist.items[index].radioName;
            aktualnaFrekvencia = myPlaylist.items[index].frequency;
            pridatFrekvenciu.Text = "Odobrať rádiostanicu";

            poziciaFrekvencie = (-1 * (aktualnaFrekvencia - maximalnaFrekvencia)) * dlzkaFrekvencie - (-1 * minPozicia);
            prepocitajFrekvenciu();

            MyDoubleAnimation.To = poziciaFrekvencie;
            Posuv_frekvencie.Begin();

        }



        private void nextHigherFreq()
        {
            int i, index = -1;
            double najblizsie = 100;

            for (i = 0; i < 10; i++)
            {
                if ((myPlaylist.items[i].frequency > aktualnaFrekvencia) && (Math.Abs(aktualnaFrekvencia - myPlaylist.items[i].frequency) < najblizsie))
                {
                    najblizsie = Math.Abs(aktualnaFrekvencia - myPlaylist.items[i].frequency);
                    index = i;
                }
            }

            if (index == -1)
                return;

            nazov_stanice.Text = myPlaylist.items[index].radioName;
            aktualnaFrekvencia = myPlaylist.items[index].frequency;
            pridatFrekvenciu.Text = "Odobrať rádiostanicu";

            poziciaFrekvencie = (-1 * (aktualnaFrekvencia - maximalnaFrekvencia)) * dlzkaFrekvencie - (-1 * minPozicia);
            prepocitajFrekvenciu();

            MyDoubleAnimation.To = poziciaFrekvencie;
            Posuv_frekvencie.Begin();

        }


        private void prevLowerFreq()
        {
            int i, index = -1;
            double najblizsie = 100;

            for (i = 0; i < 10; i++)
            {
                if ((myPlaylist.items[i].frequency < aktualnaFrekvencia) && (Math.Abs(aktualnaFrekvencia - myPlaylist.items[i].frequency) < najblizsie))
                {
                    najblizsie = Math.Abs(aktualnaFrekvencia - myPlaylist.items[i].frequency);
                    index = i;
                }
            }

            if (index == -1)
                return;

            nazov_stanice.Text = myPlaylist.items[index].radioName;
            aktualnaFrekvencia = myPlaylist.items[index].frequency;
            pridatFrekvenciu.Text = "Odobrať rádiostanicu";

            poziciaFrekvencie = (-1 * (aktualnaFrekvencia - maximalnaFrekvencia)) * dlzkaFrekvencie - (-1 * minPozicia);
            prepocitajFrekvenciu();

            MyDoubleAnimation.To = poziciaFrekvencie;
            Posuv_frekvencie.Begin();
        }


          private void checkRadio()
        {
            int i;

            for (i = 0; i < 10; i++)
            {
                if ((myPlaylist.items[i].frequency == aktualnaFrekvencia ))
                {
                    nazov_stanice.Text = myPlaylist.items[i].radioName;
                    if (pridatFrekvenciu.Text != "")

                    pridatFrekvenciu.Text = "Odobrať rádiostanicu";
                    return;
                }
            }

            nazov_stanice.Text = "";
            pridatFrekvenciu.Text = "Pridať rádiostanicu";
            
        }


        private void radioDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            coverMouseDown = true;
            startPoint = e.GetPosition(radioImage);
        }

        private void radioDown_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            coverMouseDown = false;
            
            //ak bolo len kliknute
            if (bolPohyb == false)
            {
                //na lavu polku
                if (startPoint.X < 320)
                {
                    poziciaFrekvencie += (dlzkaFrekvencie / 10);

                    if (poziciaFrekvencie < minPozicia)
                        poziciaFrekvencie = minPozicia;

                    MyDoubleAnimation.To = poziciaFrekvencie;

                    Posuv_frekvencie.Begin();
                    prepocitajFrekvenciu();

                    //skonroluje ci je na danej frekvencii radio
                    checkRadio();
                }
                //na pravu polku
                else
                {
                    poziciaFrekvencie -= (dlzkaFrekvencie / 10);

                    if (poziciaFrekvencie < minPozicia)
                        poziciaFrekvencie = minPozicia;

                    MyDoubleAnimation.To = poziciaFrekvencie;
                    
                    Posuv_frekvencie.Begin();
                    prepocitajFrekvenciu();

                    //skonroluje ci je na danej frekvencii radio
                    checkRadio();
                }
            }
            else
            {
                bolPohyb = false;
            }
        }

        private void radioDown_MouseMove(object sender, MouseEventArgs e)
        {
            if (!coverMouseDown) return;
            bolPohyb = true;

            //poloha mysky
            tmpPoint = e.GetPosition(radioImage);

            int xDistance = (int)startPoint.X - (int)tmpPoint.X;
            int yDistance = (int)startPoint.Y - (int)tmpPoint.Y;    


            if (xDistance > 30)
            {
                nextHigherFreq();
                coverMouseDown = false;
            }
            else if (xDistance < -30)
            {
                prevLowerFreq();
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

        private void frekvencia_text_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            stateChanged(State.PlaylistOn);
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

        private void controlWrapper_MouseEnter(object sender, MouseEventArgs e)
        {
            PlayMouseEnter.Begin();
        }

        private void controlWrapper_MouseLeave(object sender, MouseEventArgs e)
        {
            PlayMouseLeave.Begin();
        }

        


    
    }
}
