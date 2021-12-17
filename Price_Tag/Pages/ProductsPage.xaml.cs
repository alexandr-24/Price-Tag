using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Price_Tag.Classes;

using IronXL;

namespace Price_Tag.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            UpdateDG();
        }
        private void UpdateDG()
        {
            DG.ItemsSource = Product_Class.ProductsList.ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Content = new AddEditProductsPage(null);
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Content = new AddEditProductsPage((sender as Button).DataContext as Product_Class.Product);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var productsToRemove = DG.SelectedItems.Cast<Product_Class.Product>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {productsToRemove.Count()} элементов?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    for (int i = 0; i < productsToRemove.Count; i++)
                    {
                        Product_Class.ProductsList.Remove(productsToRemove[i]);
                    }
                    File.WriteAllText(@"F:\PetProjects\Price_Tag\products.json", JsonConvert.SerializeObject(Product_Class.ProductsList, Formatting.Indented));
                    UpdateDG();
                    MessageBox.Show("Продукты удалены!");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }

            }
        }
        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("Excel", "*.xls;*.xlsx"));
            dialog.Multiselect = false;
            
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string stream = dialog.FileName;
                stream.Replace("/", @"\");
                MessageBox.Show(stream);
                ImportFromExcel(stream, true);
            }
        }
        private void ImportFromExcel(string ExcelPath, bool FirstLineIsColumnName)
        {
            WorkBook workbook = WorkBook.Load(ExcelPath);
            WorkSheet sheet = workbook.WorkSheets.First();


            // Загрузка первой строки в ComboBox
            if (FirstLineIsColumnName == true)
            {
                char n = 'A';
                while (sheet[Convert.ToString(n) + Convert.ToString(1)].StringValue != "")
                {
                    MessageBox.Show(sheet[Convert.ToString(n) + Convert.ToString(1)].StringValue);
                    n += (char)1;
                }
                n -= (char)1;
                var firstLine = sheet["A1:" + Convert.ToString(n) + "1"].ToList();
                CB.ItemsSource = firstLine;
            }
            else
            {
                char n = 'A';
                List<string> lines = new List<string>();
                while (sheet[Convert.ToString(n) + Convert.ToString(1)].StringValue != "")
                {
                    MessageBox.Show(sheet[Convert.ToString(n) + Convert.ToString(1)].StringValue);
                    lines.Add(Convert.ToString(n));
                    n += (char)1;
                }
                var firstLine = lines;
                CB.ItemsSource = firstLine;
            }
            
            

            /*
            int k;
            if (FirstLineIsColumnName == true) k = 2;
            else k = 1;

            while (sheet["A" + k].StringValue != "")
            {
                MessageBox.Show(sheet["B" + k].StringValue);
                k++;
            }
            */
        }
    }
}
