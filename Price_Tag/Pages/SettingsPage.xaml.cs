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
using Newtonsoft.Json;
using Microsoft.WindowsAPICodePack.Dialogs;
using Price_Tag.Classes;

namespace Price_Tag.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            LoadSettings();
        }
        private void LoadSettings()
        {
            try
            {
                CompanyNameTB.Text = Manager.SettingsData.CompanyName;
                FileStreamTB.Text = Manager.SettingsData.FileStreamString;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SelectFileStreamButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string stream = dialog.FileName;
                stream.Replace("/", @"\");
                FileStreamTB.Text = stream;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.Settings settings = new Manager.Settings { CompanyName = CompanyNameTB.Text, FileStreamString = FileStreamTB.Text };
            File.WriteAllText(@"F:\PetProjects\Price_Tag\settings.json", JsonConvert.SerializeObject(settings));
            Manager.SettingsData = new Manager.Settings() { CompanyName = settings.CompanyName, FileStreamString = settings.FileStreamString };
            MessageBox.Show("Данные сохранены");
        }
    }
}
