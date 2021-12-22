using System;
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
using Newtonsoft.Json;

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
            // Загрузка названия компании и путь к сохранению файла
            try
            {
                Manager.Settings settings = JsonConvert.DeserializeObject<Manager.Settings>(File.ReadAllText(@"settings.json"));
                Manager.SettingsData = new Manager.Settings() { CompanyName = settings.CompanyName, FileStreamString = settings.FileStreamString };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Загрузка продуктов
            try
            {
                List<Product_Class.Product> p = JsonConvert.DeserializeObject<List<Product_Class.Product>>(File.ReadAllText(@"products.json", Encoding.UTF8));
                Product_Class.ProductsList = p;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Content = new HomePage();
            Grid.SetRow(SelectedButtonFrame, 0);
        }
        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Content = new ProductsPage();
            Grid.SetRow(SelectedButtonFrame, 1);
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Content = new PrintPage();
            Grid.SetRow(SelectedButtonFrame, 2);
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Content = new SettingsPage();
            Grid.SetRow(SelectedButtonFrame, 3);
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Content = new InfoPage();
            Grid.SetRow(SelectedButtonFrame, 5);
        }
    }
}
