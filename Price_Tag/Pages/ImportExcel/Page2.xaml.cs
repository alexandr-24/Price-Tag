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

namespace Price_Tag.Pages.ImportExcel
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2(string ExcelPath, bool FirstLineIsColumnName)
        {
            InitializeComponent();
            try
            {
                WorkBook workbook = WorkBook.Load(ExcelPath);
                WorkSheet sheet = workbook.WorkSheets.First();
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
    }
}
