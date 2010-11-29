﻿using System;
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

        private Boolean player = true;

        public PlaylistView()
        {
            InitializeComponent();
        }

        public void changedVolume(double newVolume)
        {
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //playlist.Open();
        }

        public void initialize(StateChangedNotify stateChanged, Playlist playlist)
        {
            this.playlist = playlist;
            this.stateChanged = stateChanged;
            //List.DisplayMemberPath = "uri";
        }

        public void playlistHidden()
        { 
        }

        public void beforeShow(bool player)
        {
            this.player = player;
            if (playlist.current < List.Items.Count) List.SelectedIndex = playlist.current;

            if (player)
            {
                if (Radio.IsChecked == true) Usb.IsChecked = true;
                //List.ItemsSource = playlist.items;
            }
            else
            {
                Radio.IsChecked = true;
                //List.ItemsSource = playlist.radioItems;
            }
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
            ActionBack();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ActionBack();
        }

        //volane pri zruseni playlistu
        private void ActionBack()
        {
            stateChanged(State.PlaylistOff);
        }

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (player)
            {
                stateChanged(State.TurnedOff);
                player = false;
            }
            List.ItemsSource = playlist.radioItems;
        }

        private void Disc_Checked(object sender, RoutedEventArgs e)
        {
            if (!player)
            {
                stateChanged(State.TurnedOff);
                player = true;
            }

            playlist.Open();
            List.ItemsSource = playlist.items;
        }

        private void Usb_Checked(object sender, RoutedEventArgs e)
        {
            if (!player)
            {
                stateChanged(State.TurnedOff);
                player = true;
            }

            //ak je vytvoreny playlist
            if (playlist != null)
            {
                playlist.Open();
                List.ItemsSource = playlist.items;
            }
        }

        private bool mouseDown = false;
        private Point downPoint, movePoint;

        private void List_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
            playlist.changedTrackID = List.SelectedIndex;
            ActionBack();
        }

        private void List_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            downPoint = e.GetPosition(List);
            mouseDown = true;
        }

        private void List_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown) return;

            movePoint = e.GetPosition(List);

            if (movePoint.X <= 10)
            { 
                if (player)
                {
                    playlist.items.RemoveAt(List.SelectedIndex);
                    List.ItemsSource = null;
                    List.ItemsSource = playlist.items;
                }
                else 
                {
                    playlist.radioItems.RemoveAt(List.SelectedIndex);
                    List.ItemsSource = null;
                    List.ItemsSource = playlist.radioItems;
                }

                mouseDown = false;
            }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mouseDown = true;
        }
    }
}
