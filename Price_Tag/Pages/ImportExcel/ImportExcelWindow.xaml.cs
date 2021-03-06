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
using System.Windows.Shapes;
using Price_Tag.Classes;

namespace Price_Tag.Pages.ImportExcel
{
    /// <summary>
    /// Логика взаимодействия для ImportExcelWindow.xaml
    /// </summary>
    public partial class ImportExcelWindow : Window
    {
        Page2 page2;
        public ImportExcelWindow()
        {
            InitializeComponent();
            Manager.ImportFrame = ImportFrame;
            Manager.ImportFrame.Content = new Page1();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Manager.ImportPage2 != null)
            {
                Manager.ImportPage2.CloseExcel();
            }
        }
    }
}
