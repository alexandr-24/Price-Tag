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
                Manager.Settings settings = JsonConvert.DeserializeObject<Manager.Settings>(File.ReadAllText(@"F:\PetProjects\Price_Tag\settings.json"));
                Manager.SettingsData = new Manager.Settings() { CompanyName = settings.CompanyName, FileStreamString = settings.FileStreamString };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Загрузка продуктов
            try
            {
                List<Product_Class.Product> p = JsonConvert.DeserializeObject<List<Product_Class.Product>>(File.ReadAllText(@"F:\PetProjects\Price_Tag\products.json"));
                Product_Class.ProductsList = p;
            }
            catch
            {

            }

            /* Загрузка продуктов
            Product_Class.Product P1 = new Product_Class.Product { ID = 1, ProductBarcode = "7622201105518", ProductCost = "139.00", ProductName = "Конфеты Milka, из молочного шоколада, с ореховой начинкой, 110 г", ProductType = "Цена за шт." };
            Product_Class.Product P2 = new Product_Class.Product { ID = 2, ProductBarcode = "3858888677633", ProductCost = "50.00", ProductName = "Мороженое Бон Пари Джангли банан 45 мл", ProductType = "Цена за шт." };

            List<Product_Class.Product> products = new List<Product_Class.Product>();
            products.Add(P1);
            products.Add(P2);
            File.WriteAllText(@"F:\PetProjects\Price_Tag\products.json", JsonConvert.SerializeObject(products, Formatting.Indented)); 
            */
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

        
    }
}
