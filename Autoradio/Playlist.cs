using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TagLib;
//using System.Collections.Generic;

namespace Autoradio
{
    public struct PlaylistItem
    {
        public FileInfo file { get; set; }
        public string radioName { get; set; }
        public string title { get; set; }
        public string artist { get; set; }

        public TimeSpan duration { get; set; }
        public float frequency { get; set; }

        public ImageSource cover { get; set; }
    }

    public class Playlist
    {
        public List<PlaylistItem> items;
        public int current = 0;

        private Random rand = new Random();
        //OpenFileDialog
        private OpenFileDialog openDialog = new OpenFileDialog();

        //nastavi sa na true pri nacitani noveho playlistu
        private bool changed = false;
        //nastavi sa na cislo skladby, ak bola v playliste nejake vybrata
        public int changedTrackID = -1;

        public Playlist()
        {
            //openDialog.Filter = "MP3 Files (.mp3)|*.mp3";
            openDialog.Multiselect = true;
        }

        /**
         *  Vrati cislo nasledujucej skladby.
         */
        public int GetNext()
        {
            return (current + 1) % items.Count;
        }

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
        public int GetPrevious()
        {
            return current == 0 ? 0 : current-1;
        }

        /**
         *  Nastavi current na predchadzajucu skladbu.
         */
        public void MoveToPrevious()
        {
            current = GetPrevious();
        }

        /**
         *  Vrati true ak bol playlist zmeneny od posledneho zavolania.
         */
        public bool isChanged()
        {
            if (changed)
            {
                changed = false;
                return true;
            }

            return false;
        }

        /**
         *  Otvori novy playlist.
         */
        public void Open()
        {
            if (openDialog.ShowDialog() == false) return;

            changed = true;

            items = new List<PlaylistItem>();
            PlaylistItem item;
            TagLib.File id3File;

            MemoryStream memory;
            BitmapImage bmp;

            BitmapImage[] img = new BitmapImage[4];

            img[0] = new BitmapImage(new Uri("cover1.jpg", UriKind.Relative));
            img[1] = new BitmapImage(new Uri("cover2.jpg", UriKind.Relative));
            img[2] = new BitmapImage(new Uri("cover3.jpg", UriKind.Relative));
            img[3] = new BitmapImage(new Uri("cover4.jpg", UriKind.Relative));

            foreach(FileInfo f in openDialog.Files)
            {                
                item = new PlaylistItem();
                id3File = TagLib.File.Create(new TagLib.File.StreamFileAbstraction(f.OpenRead(), f.Name));

                item.file = f;

                if (id3File.Tag.TagTypes.HasFlag(TagLib.TagTypes.Id3v2))
                {
                    item.title = id3File.Tag.Title;
                    item.artist = id3File.Tag.FirstPerformer;
                    item.duration = id3File.Properties.Duration;

                    if (id3File.Tag.Pictures.Length > 0)
                    {
                        memory = new MemoryStream(id3File.Tag.Pictures[0].Data.Data);
                        bmp = new BitmapImage();
                        bmp.SetSource(memory);
                        item.cover = bmp;
                    }
                    else
                    {
                        item.cover = img[0];
                    }
                }
                else
                {
                    item.title = f.Name.Substring(0, 20);
                    item.artist = "Unknown";
                    item.cover = img[0];
                    item.duration = new TimeSpan(0, 0, 0);
                }

                item.radioName = "Radio " + (items.Count + 1).ToString();
                item.frequency = (float) Math.Round( (87 + (float)rand.NextDouble() * 21), 1);

                items.Add(item);
            }
        }
    }
}
