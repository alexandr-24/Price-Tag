using Microsoft.WindowsAPICodePack.Dialogs;
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
using System.Threading;


using Excel = Microsoft.Office.Interop.Excel;

using IronXL;

namespace Price_Tag.Pages.ImportExcel
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void SelectFileStreamButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("Excel", "*.xls;*.xlsx"));
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string stream = dialog.FileName;
                stream.Replace("/", @"\");
                FileStreamTB.Text = stream;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            bool isOK = true;
            try
            {
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(FileStreamTB.Text);
                xlWorkbook.Close(false, null, null);
                xlApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                isOK = false;
            }
            if (isOK) 
            {
                // Анимация загрузки не работает - не получается реализовать многопоточность
                Rect.Visibility = Visibility.Visible;
                GifImg.Visibility = Visibility.Visible;

                Manager.ImportFrame.Content = new Page2(FileStreamTB.Text, FirstLineIsNameCB.IsChecked == true ? true : false);
            }
        }
    }
}
