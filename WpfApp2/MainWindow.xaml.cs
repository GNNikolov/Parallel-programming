﻿using System.Windows;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private void onClickListener(object sender, RoutedEventArgs e)
        {
            var mLock = new livelock.LiveLock(this);
            mLock.showLock(true);

        }
    }


}
