﻿#pragma checksum "C:\Users\stevo\Documents\Visual Studio 2010\Projects\Autoradio\Autoradio\Views\Radio.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B89E3C801838D459BFF408DB4C206D11"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Autoradio.Views {
    
    
    public partial class Radio : System.Windows.Controls.Page {
        
        internal System.Windows.Media.Animation.Storyboard Posuv_frekvencie;
        
        internal System.Windows.Media.Animation.DoubleAnimation MyDoubleAnimation;
        
        internal System.Windows.Media.Animation.Storyboard Play;
        
        internal System.Windows.Media.Animation.Storyboard Pause;
        
        internal System.Windows.Media.Animation.Storyboard PlayMouseEnter;
        
        internal System.Windows.Media.Animation.Storyboard PlayMouseLeave;
        
        internal System.Windows.Media.Animation.Storyboard fadeInPridat;
        
        internal System.Windows.Media.Animation.Storyboard fadeOutPridat;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Image radioImage;
        
        internal System.Windows.Controls.Image image;
        
        internal System.Windows.Controls.Image Gradient;
        
        internal System.Windows.Controls.TextBlock frekvencia_text;
        
        internal System.Windows.Controls.TextBlock nazov_stanice;
        
        internal System.Windows.Controls.Grid controlWrapper;
        
        internal System.Windows.Controls.Canvas play;
        
        internal System.Windows.Controls.Image PsdLayer1;
        
        internal System.Windows.Controls.Canvas pause;
        
        internal System.Windows.Controls.Image PsdLayer;
        
        internal System.Windows.Controls.TextBlock pridatFrekvenciu;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Autoradio;component/Views/Radio.xaml", System.UriKind.Relative));
            this.Posuv_frekvencie = ((System.Windows.Media.Animation.Storyboard)(this.FindName("Posuv_frekvencie")));
            this.MyDoubleAnimation = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("MyDoubleAnimation")));
            this.Play = ((System.Windows.Media.Animation.Storyboard)(this.FindName("Play")));
            this.Pause = ((System.Windows.Media.Animation.Storyboard)(this.FindName("Pause")));
            this.PlayMouseEnter = ((System.Windows.Media.Animation.Storyboard)(this.FindName("PlayMouseEnter")));
            this.PlayMouseLeave = ((System.Windows.Media.Animation.Storyboard)(this.FindName("PlayMouseLeave")));
            this.fadeInPridat = ((System.Windows.Media.Animation.Storyboard)(this.FindName("fadeInPridat")));
            this.fadeOutPridat = ((System.Windows.Media.Animation.Storyboard)(this.FindName("fadeOutPridat")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.radioImage = ((System.Windows.Controls.Image)(this.FindName("radioImage")));
            this.image = ((System.Windows.Controls.Image)(this.FindName("image")));
            this.Gradient = ((System.Windows.Controls.Image)(this.FindName("Gradient")));
            this.frekvencia_text = ((System.Windows.Controls.TextBlock)(this.FindName("frekvencia_text")));
            this.nazov_stanice = ((System.Windows.Controls.TextBlock)(this.FindName("nazov_stanice")));
            this.controlWrapper = ((System.Windows.Controls.Grid)(this.FindName("controlWrapper")));
            this.play = ((System.Windows.Controls.Canvas)(this.FindName("play")));
            this.PsdLayer1 = ((System.Windows.Controls.Image)(this.FindName("PsdLayer1")));
            this.pause = ((System.Windows.Controls.Canvas)(this.FindName("pause")));
            this.PsdLayer = ((System.Windows.Controls.Image)(this.FindName("PsdLayer")));
            this.pridatFrekvenciu = ((System.Windows.Controls.TextBlock)(this.FindName("pridatFrekvenciu")));
        }
    }
}

