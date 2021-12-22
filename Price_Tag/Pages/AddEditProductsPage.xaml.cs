using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Newtonsoft.Json;
using Price_Tag.Classes;

namespace Price_Tag.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditProductsPage.xaml
    /// </summary>
    public partial class AddEditProductsPage : Page
    {
        private Product_Class.Product currentProduct = new Product_Class.Product();
        private bool isAdd;
        private int index;
        public AddEditProductsPage(Product_Class.Product product)
        {
            InitializeComponent();
            if (product != null)
            {
                isAdd = false;
                currentProduct = product;
                if (currentProduct.ProductType == "Цена за шт.")
                    RB1.IsChecked = true;
                else if (currentProduct.ProductType == "Цена за кг.")
                    RB2.IsChecked = true;
                ProductIdTB.IsEnabled = false;
                index = Product_Class.ProductsList.FindIndex(x => x.ID == product.ID);
            }
            else isAdd = true;

            DataContext = currentProduct;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Content = new ProductsPage();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка корректность данных
                string errors = "";

                // Код
                if (ProductIdTB.Text.Any(char.IsLetter) || ProductIdTB.Text.Length == 0)
                {
                    errors += "Код продукта указан не корректно!\n";
                }
                // Название
                if (ProductNameTB.Text.Trim().Length == 0)
                {
                    errors += "Введите название продукта!\n";
                }
                // Цена
                if (ProductCostTB.Text.Any(char.IsLetter) || ProductCostTB.Text.Length == 0)
                {
                    errors += "Цена указана некорректно!\n";
                }
                // Тип товара
                if (RB1.IsChecked == false && RB2.IsChecked == false)
                {
                    errors += "Выберете тип товара!\n";
                }
                // Штрих-код
                if (ProductBarcodeTB.Text.Any(char.IsLetter) || (ProductBarcodeTB.Text.Length != 8 && ProductBarcodeTB.Text.Length != 13))
                {
                    errors += "Штрих-код указан некорректно!";
                }
                // Вывод ошибок
                if (errors.Length > 0)
                {
                    MessageBox.Show(errors);
                }
                else
                {
                    if (isAdd == true)
                    {
                        currentProduct.ID = Convert.ToInt32(ProductIdTB.Text);
                        currentProduct.ProductName = ProductNameTB.Text;
                        currentProduct.ProductCost = ProductCostTB.Text;
                        currentProduct.ProductType = RB1.IsChecked == true ? "Цена за шт." : "Цена за кг.";
                        currentProduct.ProductBarcode = ProductBarcodeTB.Text;
                        Product_Class.ProductsList.Add(currentProduct);
                        File.WriteAllText(@"products.json", JsonConvert.SerializeObject(Product_Class.ProductsList, Formatting.Indented));
                        MessageBox.Show("Продукт добавлен!");
                    }
                    else
                    {
                        currentProduct.ID = Convert.ToInt32(ProductIdTB.Text);
                        currentProduct.ProductName = ProductNameTB.Text;
                        currentProduct.ProductCost = ProductCostTB.Text;
                        currentProduct.ProductType = RB1.IsChecked == true ? "Цена за шт." : "Цена за кг.";
                        currentProduct.ProductBarcode = ProductBarcodeTB.Text;
                        Product_Class.ProductsList[index] = currentProduct;
                        File.WriteAllText(@"products.json", JsonConvert.SerializeObject(Product_Class.ProductsList, Formatting.Indented));
                        MessageBox.Show("Продукт изменен!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
