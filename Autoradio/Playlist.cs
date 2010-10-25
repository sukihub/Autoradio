using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
//using System.Collections.Generic;

namespace Autoradio
{
    public struct PlaylistItem
    {
        public string uri { get; set; }
        public string radioName { get; set; }
        public string title { get; set; }
        public string artist { get; set; }

        public TimeSpan duration { get; set; }
        public float frequency { get; set; }

        public ImageSource cover { get; set; }
    }

    public class Playlist
    {
        public PlaylistItem[] items;
        public uint current = 0;

        private Random rand = new Random();

        /**
         *  Vrati cislo nasledujucej skladby.
         */
        public uint GetNext()
        {
            return (current + 1) % (uint)items.Length;
        }

        /**
         *  Vrati cislo nasledujucej skladby relativne k zadanej skladbe.
         */
        /*public uint getNext(uint relative)
        {
            return (relative + 1) % (uint)items.Length;
        }*/

        /**
         *  Nastavi current na nasledujucu skladbu.
         */
        public void MoveToNext()
        {
            current = GetNext();
        }

        /**
         *  Vrati cislo predchadzajucej skladby.
         */
        public uint GetPrevious()
        {
            return current == 0 ? 0 : current-1;
        }

        /**
         *  Vrati cislo predchadzajucej skladby relativne k zadanej skladbe.
         */
        /*public uint getPrevious(uint relative)
        {
            return relative == 0 ? 0 : relative - 1;
        }*/

        /**
         *  Nastavi current na predchadzajucu skladbu.
         */
        public void MoveToPrevious()
        {
            current = GetPrevious();
        }

        /**
         *  Otvori novy playlist.
         */
        public void Open()
        {
            items = new PlaylistItem[10];
            int i;
            BitmapImage[] img = new BitmapImage[4];

            img[0] = new BitmapImage(new Uri("cover1.jpg", UriKind.Relative));
            img[1] = new BitmapImage(new Uri("cover2.jpg", UriKind.Relative));
            img[2] = new BitmapImage(new Uri("cover3.jpg", UriKind.Relative));
            img[3] = new BitmapImage(new Uri("cover4.jpg", UriKind.Relative));

            for (i = 0; i < 10; i++)
            {
                items[i].uri = i.ToString() + ". subor";

                items[i].title = "Nazov prehravanej pesnicky";
                items[i].artist = "Super cool interpret";
                
                items[i].radioName = "Radio " + (i+1).ToString();
                items[i].cover = img[rand.Next(4)];
                items[i].duration = new TimeSpan(0, rand.Next(10), rand.Next(60));
                items[i].frequency = (float) Math.Round( (87 + (float)rand.NextDouble() * 21), 1);
            }
        }
    }
}
