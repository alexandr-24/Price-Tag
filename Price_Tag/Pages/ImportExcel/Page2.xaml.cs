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
using Excel = Microsoft.Office.Interop.Excel;

namespace Price_Tag.Pages.ImportExcel
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        private bool FirstLineIsColumnN;

        private Excel.Application xlApp;
        private Excel.Workbook xlWorkbook;
        private Excel._Worksheet xlWorksheet;
        private Excel.Range xlRange;

        public Page2(string ExcelPath, bool FirstLineIsColumnName)
        {
            InitializeComponent();
            Manager.ImportPage2 = this;
            FirstLineIsColumnN = FirstLineIsColumnName;
            try
            {
                xlApp = new Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(ExcelPath);
                xlWorksheet = xlWorkbook.Sheets[1];
                xlRange = xlWorksheet.UsedRange;

                List<string> list = new List<string>();
                if (FirstLineIsColumnName == true)
                {
                    int i = 1;
                    while(xlRange.Cells[1, i] != null && xlRange.Cells[1, i].Value2 != null)
                    {
                        list.Add (xlRange.Cells[1, i].Value2.ToString());
                        i++;
                    }
                }
                else
                {
                    int i = 1;
                    char n = 'A';
                    while (xlRange.Cells[1, i] != null && xlRange.Cells[1, i].Value2 != null)
                    {
                        list.Add(Convert.ToString(n));
                        n += (char)1;
                        i++;
                    }
                }
                CodeCB.ItemsSource = list;
                NameCB.ItemsSource = list;
                CostCB.ItemsSource = list;
                TypeCB.ItemsSource = list;
                BarcodeCB.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int i;

            int CodeChar;
            int NameChar;
            int CostChar;
            int TypeChar;
            int BarcodeChar;

            if (FirstLineIsColumnN == true)
            {
                i = 2;
                CodeChar = Convert.ToInt32(CodeCB.SelectedIndex + 1);
                NameChar = Convert.ToInt32(NameCB.SelectedIndex + 1);
                CostChar = Convert.ToInt32(CostCB.SelectedIndex + 1);
                TypeChar = Convert.ToInt32(TypeCB.SelectedIndex + 1);
                BarcodeChar = Convert.ToInt32(BarcodeCB.SelectedIndex + 1);
            }
            else
            {
                i = 1;
                CodeChar = Convert.ToInt32(Convert.ToChar(CodeCB.SelectedItem) - Convert.ToChar('A')) + 1;
                NameChar = Convert.ToInt32(Convert.ToChar(NameCB.SelectedItem) - Convert.ToChar('A')) + 1;
                CostChar = Convert.ToInt32(Convert.ToChar(CostCB.SelectedItem) - Convert.ToChar('A')) + 1;
                TypeChar = Convert.ToInt32(Convert.ToChar(TypeCB.SelectedItem) - Convert.ToChar('A')) + 1;
                BarcodeChar = Convert.ToInt32(Convert.ToChar(BarcodeCB.SelectedItem) - Convert.ToChar('A')) + 1;
            }
            try
            {
                while (xlRange.Cells[NameChar, i + 1] != null && xlRange.Cells[NameChar, i + 1].Value2 != null)
                {
                    Product_Class.Product product = new Product_Class.Product();
                    product.ID = Convert.ToInt32(xlRange.Cells[i, CodeChar].Value2);
                    product.ProductName = xlRange.Cells[i, NameChar].Value2;
                    product.ProductCost = Convert.ToString (xlRange.Cells[i, CostChar].Value2);
                    product.ProductType = xlRange.Cells[i, TypeChar].Value2;
                    product.ProductBarcode = Convert.ToString (xlRange.Cells[i, BarcodeChar].Value2);
                    Product_Class.ProductsList.Add(product);
                    i++;
                }
                File.WriteAllText(@"products.json", JsonConvert.SerializeObject(Product_Class.ProductsList, Formatting.Indented));
                MessageBox.Show("Импорт прошёл успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void CloseExcel()
        {
            xlWorkbook.Close(false, null, null);
            xlApp.Quit();
        }
    }
}
