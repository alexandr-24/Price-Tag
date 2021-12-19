using IronXL;
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
using Price_Tag.Classes;
using Newtonsoft.Json;
using System.IO;

namespace Price_Tag.Pages.ImportExcel
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        private bool FirstLineIsColumnN;
        WorkBook workbook;
        WorkSheet sheet;
        public Page2(string ExcelPath, bool FirstLineIsColumnName)
        {
            InitializeComponent();
            FirstLineIsColumnN = FirstLineIsColumnName;
            try
            {
                workbook = WorkBook.Load(ExcelPath);
                sheet = workbook.WorkSheets.First();
                // Загрузка первой строки в ComboBox
                if (FirstLineIsColumnName == true)
                {
                    char n = 'A';
                    while (sheet[Convert.ToString(n) + Convert.ToString(1)].StringValue != "")
                    {
                        n += (char)1;
                    }
                    n -= (char)1;
                    var firstLine = sheet["A1:" + Convert.ToString(n) + "1"].ToList();
                    CodeCB.ItemsSource = firstLine;
                    NameCB.ItemsSource = firstLine;
                    CostCB.ItemsSource = firstLine;
                    TypeCB.ItemsSource = firstLine;
                    BarcodeCB.ItemsSource = firstLine;
                }
                else
                {
                    char n = 'A';
                    List<string> lines = new List<string>();
                    while (sheet[Convert.ToString(n) + Convert.ToString(1)].StringValue != "")
                    {
                        lines.Add(Convert.ToString(n));
                        n += (char)1;
                    }
                    CodeCB.ItemsSource = lines;
                    NameCB.ItemsSource = lines;
                    CostCB.ItemsSource = lines;
                    TypeCB.ItemsSource = lines;
                    BarcodeCB.ItemsSource = lines;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int i;

            char CodeChar;
            char NameChar;
            char CostChar;
            char TypeChar;
            char BarcodeChar;

            if (FirstLineIsColumnN == true)
            {
                i = 2;
                CodeChar = Convert.ToChar('A' + CodeCB.SelectedIndex);
                NameChar = Convert.ToChar('A' + NameCB.SelectedIndex);
                CostChar = Convert.ToChar('A' + CostCB.SelectedIndex);
                TypeChar = Convert.ToChar('A' + TypeCB.SelectedIndex);
                BarcodeChar = Convert.ToChar('A' + BarcodeCB.SelectedIndex);
            }
            else
            {
                i = 1;
                CodeChar = Convert.ToChar(CodeCB.SelectedItem);
                NameChar = Convert.ToChar(NameCB.SelectedItem);
                CostChar = Convert.ToChar(CostCB.SelectedItem);
                TypeChar = Convert.ToChar(TypeCB.SelectedItem);
                BarcodeChar = Convert.ToChar(BarcodeCB.SelectedItem);
            }
            while (sheet["A" + Convert.ToString(i)].StringValue != "")
            {
                Product_Class.Product product = new Product_Class.Product();
                product.ID = Convert.ToInt32(sheet[Convert.ToString(CodeChar) + Convert.ToString(i)].StringValue);
                product.ProductName = sheet[Convert.ToString(NameChar) + Convert.ToString(i)].StringValue;
                product.ProductCost = sheet[Convert.ToString(CostChar) + Convert.ToString(i)].StringValue;
                product.ProductType = sheet[Convert.ToString(TypeChar) + Convert.ToString(i)].StringValue;
                product.ProductBarcode = sheet[Convert.ToString(BarcodeChar) + Convert.ToString(i)].StringValue;
                Product_Class.ProductsList.Add(product);
                i++;
            }
            File.WriteAllText(@"products.json", JsonConvert.SerializeObject(Product_Class.ProductsList, Formatting.Indented));
            MessageBox.Show("Импорт прошел успешно!");
        }
    }
}
