﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

using Price_Tag.Classes;
using Price_Tag.Pages;

using Microsoft.WindowsAPICodePack.Dialogs;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Price_Tag
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Manager.MainFrame = MainFrame;
            Manager.MainFrame.Content = new HomePage();
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Content = new HomePage();
            Grid.SetRow(SelectedButtonFrame, 0);
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Content = new SettingsPage();
            Grid.SetRow(SelectedButtonFrame, 3);
        }
    }
}
