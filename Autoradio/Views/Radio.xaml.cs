﻿using System;
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

        private const int PLAY = 1;
        private const int PAUSE = 0;

        private const String pridatFrekText = "Pridať rádiostanicu";
        private const String odobratFrekText = "Odobrať rádiostanicu";

        private bool bolPohyb = false;

        private double aktualnaFrekvencia = 0;
        private double poziciaFrekvencie;

        private Boolean coverMouseDown = false;
        private Point startPoint, tmpPoint;

        private StateChangedNotify stateChanged;

        private Playlist myPlaylist;

        private int stav;

        public Radio()
        {
            InitializeComponent();

            poziciaFrekvencie = 0;

            stav = PLAY;
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

        public void changedVolume(double newVolume)
        {
            myRadioPlayer.Volume = newVolume;
        }

        public void playlistHidden()
        {

            if (myPlaylist.changedTrackID == -1)
                return;

            nazov_stanice.Text = myPlaylist.radioItems[myPlaylist.changedTrackID].radioName;
            aktualnaFrekvencia = myPlaylist.radioItems[myPlaylist.changedTrackID].frequency;
            pridatFrekvenciu.Text = "";

            poziciaFrekvencie = (-1 * (aktualnaFrekvencia - maximalnaFrekvencia)) * dlzkaFrekvencie - (-1 * minPozicia);
            prepocitajFrekvenciu();

            MyDoubleAnimation.To = poziciaFrekvencie;
            Posuv_frekvencie.Begin();
            stopMusic();
            playMusic();

            myPlaylist.changedTrackID = -1;
            
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
            
            for (i=0; i<myPlaylist.radioItems.Count; i++)
            {
                if (Math.Abs(aktualnaFrekvencia - myPlaylist.radioItems[i].frequency) < najblizsie)
                {
                    najblizsie = Math.Abs(aktualnaFrekvencia - myPlaylist.radioItems[i].frequency);
                    index = i;
                }
             }
           
            nazov_stanice.Text = myPlaylist.radioItems[index].radioName;
            aktualnaFrekvencia = myPlaylist.radioItems[index].frequency;
            pridatFrekvenciu.Text = "";

            poziciaFrekvencie = (-1 * (aktualnaFrekvencia - maximalnaFrekvencia)) * dlzkaFrekvencie - (-1 * minPozicia);
            prepocitajFrekvenciu();

            MyDoubleAnimation.To = poziciaFrekvencie;
            Posuv_frekvencie.Begin();

            playMusic();



        }



        private void nextHigherFreq()
        {
            int i, index = -1;
            double najblizsie = 100;

            for (i = 0; i < myPlaylist.radioItems.Count; i++)
            {
                if ((myPlaylist.radioItems[i].frequency > aktualnaFrekvencia) && (Math.Abs(aktualnaFrekvencia - myPlaylist.radioItems[i].frequency) < najblizsie))
                {
                    najblizsie = Math.Abs(aktualnaFrekvencia - myPlaylist.radioItems[i].frequency);
                    index = i;
                }
            }

            if (index == -1)
                return;

            nazov_stanice.Text = myPlaylist.radioItems[index].radioName;
            aktualnaFrekvencia = myPlaylist.radioItems[index].frequency;
            pridatFrekvenciu.Text = "";

            poziciaFrekvencie = (-1 * (aktualnaFrekvencia - maximalnaFrekvencia)) * dlzkaFrekvencie - (-1 * minPozicia);
            prepocitajFrekvenciu();

            MyDoubleAnimation.To = poziciaFrekvencie;
            Posuv_frekvencie.Begin();
            stopMusic();
            playMusic();

        }


        private void prevLowerFreq()
        {
            int i, index = -1;
            double najblizsie = 100;

            for (i = 0; i < myPlaylist.radioItems.Count; i++)
            {
                if ((myPlaylist.radioItems[i].frequency < aktualnaFrekvencia) && (Math.Abs(aktualnaFrekvencia - myPlaylist.radioItems[i].frequency) < najblizsie))
                {
                    najblizsie = Math.Abs(aktualnaFrekvencia - myPlaylist.radioItems[i].frequency);
                    index = i;
                }
            }

            if (index == -1)
                return;

            nazov_stanice.Text = myPlaylist.radioItems[index].radioName;
            aktualnaFrekvencia = myPlaylist.radioItems[index].frequency;
            pridatFrekvenciu.Text = "";

            poziciaFrekvencie = (-1 * (aktualnaFrekvencia - maximalnaFrekvencia)) * dlzkaFrekvencie - (-1 * minPozicia);
            prepocitajFrekvenciu();

            MyDoubleAnimation.To = poziciaFrekvencie;
            Posuv_frekvencie.Begin();
            stopMusic();
            playMusic();
        }


          private void checkRadio()
        {
            int i;

            for (i = 0; i < myPlaylist.radioItems.Count; i++)
            {
                if ((myPlaylist.radioItems[i].frequency == aktualnaFrekvencia ))
                {
                    nazov_stanice.Text = myPlaylist.radioItems[i].radioName;
                    if (pridatFrekvenciu.Text != "")

                    pridatFrekvenciu.Text = "";
                    playMusic();
                    return;
                }
            }

            
            nazov_stanice.Text = "";
            pridatFrekvenciu.Text = "Pridať rádiostanicu";
            stopMusic();
            
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
                    poziciaFrekvencie += (dlzkaFrekvencie / myPlaylist.radioItems.Count);

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
                    poziciaFrekvencie -= (dlzkaFrekvencie / myPlaylist.radioItems.Count);

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
                stopMusic();
                Pause.Begin();
            }
            else
            {
                state = State.Playing;
                playMusic();
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


        

        private void playMusic()
        {
            //myRadioPlayer.Source = new Uri("radioMusic.mp3", UriKind.Relative);
            if (state == State.Playing)
                if (nazov_stanice.Text != "")
                    myRadioPlayer.Play();
            
        }


        private void stopMusic()
        {
           
            myRadioPlayer.Stop();
            stav = PAUSE;
        }

        private void myRadioPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {

        }

        private void PsdLayer1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            /*
            if (nazov_stanice.Text != "")
                playMusic();
             * /
        }

        private void PsdLayer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            /*
            if (stav == PLAY)
            {
                stav = PAUSE;
                stopMusic();
                
            }
            else
            {
                stav = PLAY;
                playMusic();
                
            }
             * */
        }

<<<<<<< HEAD
        private void pridatFrekvenciu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myPlaylist.radioItems = myPlaylist.generateRadioStation((float)aktualnaFrekvencia);
            nazov_stanice.Text = myPlaylist.radioItems[myPlaylist.radioItems.Count-1].radioName;
           
=======
        private void myRadioPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            myRadioPlayer.Stop();
            myRadioPlayer.Play();
>>>>>>> 77bb1d158a11ea81ac8d462ba61619cff5e90f03
        }

    
         
    }
}
