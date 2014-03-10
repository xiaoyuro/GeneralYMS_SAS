﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeneralYMS_SAS.Views.UserControls
{
    /// <summary>
    /// TitleBar.xaml 的交互逻辑
    /// </summary>
    public partial class TitleBar : UserControl
    {
        public  object ImageSource 
        {
            get { return imgTitle.Source; }
            set { imgTitle.Source = (Visual)value; } 
        }
        public  string TitleText 
        {
            get { return tbTitle.Text; }
            set { tbTitle.Text = value; }
        }
        public  Brush TitleColor 
        {
            get { return tbTitle.Foreground; }
            set { imgTitle.Foreground = value; tbTitle.Foreground = value;}
        }

        public TitleBar()
        {
            InitializeComponent();
        }
    }
}
