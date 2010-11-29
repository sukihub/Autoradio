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
        public string[] RADIA = { "Express", "Okey", "SRo", "Lumen", "Europa", "Jemne melodie", "Radio FM", "Radio Z", "Radio Suki", "Twist" };
        
        public List<PlaylistItem> items;
        public List<PlaylistItem> radioItems;

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
            radioOpen();

            openDialog.Filter = "MP3 (.mp3)|*.mp3";
            openDialog.Multiselect = true;


        }

        /**
         *  Vrati cislo nasledujucej skladby.
         */
        public PlaylistItem GetNext()
        {
            return items[(current + 1) % items.Count];
        }

        /**
         *  Nastavi current na nasledujucu skladbu.
         */
        public void MoveToNext()
        {
            current = (current + 1) % items.Count;
        }

        /**
         *  Vrati cislo predchadzajucej skladby.
         */
        public PlaylistItem GetPrevious()
        {
            return (current == 0) ? items[items.Count-1] : items[current - 1];
        }

        /**
         *  Nastavi current na predchadzajucu skladbu.
         */
        public void MoveToPrevious()
        {
            current = (current == 0) ? items.Count-1 : current - 1;
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

            BitmapImage[] img = new BitmapImage[1];

            img[0] = new BitmapImage(new Uri("default_song.png", UriKind.Relative));
            
           

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
                    item.title = f.Name.Substring(0, f.Name.Length < 20 ? f.Name.Length : 20 );
                    item.artist = "Unknown";
                    item.cover = img[0];
                    item.duration = new TimeSpan(0, 0, 0);
                }

                item.radioName = "Radio " + (items.Count + 1).ToString();
                item.frequency = (float) Math.Round( (87 + (float)rand.NextDouble() * 21), 1);

                items.Add(item);
            }

            this.current = 0;
        }

        public void radioOpen()
        {
            radioItems = new List<PlaylistItem>();
            PlaylistItem item;

            BitmapImage img = new BitmapImage();
            img = new BitmapImage(new Uri("default_radio.png", UriKind.Relative));

            for (int i = 0; i < RADIA.Length; i++)
            {
                item = new PlaylistItem();


                item.cover = img;
                item.radioName = RADIA[i];
                item.frequency = (float)Math.Round((87 + (float)rand.NextDouble() * 21), 1);
                item.artist = item.frequency.ToString();
                item.title = item.radioName;

                radioItems.Add(item);
            }
        }
    }
}
