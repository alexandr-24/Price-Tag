# Price Tag
**Программа для печати ценников. Позволяет добавить или импортировать из Excel товары и подготовить файл .pdf с ценниками к печати.**
## Оглавление
1. [Настройки](#Настройки)
2. [Добавление продуктов](#Добавление-продуктов)
    1. [Добавление вручную](#Добавление-вручную)
    2. [Импорт из Excel](#Импорт-из-Excel)
3. [Печать ценников](#Печать-ценников)
4. [Скрины приложения](#Скрины-приложения)

## Настройки
В настройки входит название предприятия и путь сохранения .pdf файлов. Хранение настроек реализовано через **.json**:
```json
{"CompanyName":"ИП Василий Пупкин","FileStreamString":"E:\\PetProjects\\Price_Tag\\Tags"}
```

В настройках пользователь может указать название предприятия и **путь сохранения .pdf файлов:**
```C#
using Microsoft.WindowsAPICodePack.Dialogs;
```
```C#
CommonOpenFileDialog dialog = new CommonOpenFileDialog();
dialog.IsFolderPicker = true;
if (dialog.ShowDialog() == CommonFileDialogResult.Ok) 
{
    string stream = dialog.FileName;
    stream.Replace("/", @"\");
    FileStreamTB.Text = stream;
}
```
А затем сохранить изменения:
```C#
using Newtonsoft.Json;
```
```C#
Manager.Settings settings = new Manager.Settings { CompanyName = CompanyNameTB.Text, FileStreamString = FileStreamTB.Text };
File.WriteAllText(@"settings.json", JsonConvert.SerializeObject(settings));
Manager.SettingsData = new Manager.Settings() { CompanyName = settings.CompanyName, FileStreamString = settings.FileStreamString };
MessageBox.Show("Данные сохранены");
```

## Добавление продуктов
Прежде чем напечатать ценники, пользователь должен добавить продукты в программу.<br/>
Хранение продуктов реализовано через **.json**:
```json
[
  {
    "ID": 102,
    "ProductName": "Мороженое Бон Пари Джангли банан 45 мл",
    "ProductCost": "50.49",
    "ProductType": "Цена за кг.",
    "ProductBarcode": "3858888677633"
  },
  {
    "ID": 103,
    "ProductName": "Вода Bon Aqua, 0.5л",
    "ProductCost": "48.00",
    "ProductType": "Цена за шт.",
    "ProductBarcode": "40822426"
  }
]
```
Для работы с продуктами в приложении есть **класс Product_Class.cs**
```C#
public class Product_Class
{
    public class Product
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string ProductCost { get; set; }
        public string ProductType { get; set; }
        public string ProductBarcode { get; set; }
    }
    public static List<Product> ProductsList { get; set; }
}
```
### Добавление вручную
При добавлении товара вручную пользователя переносит на страницу AddEditProductsPage.xaml.<br/>
После ввода корректных данных продукт добавляется в наш .json файл с продуктами и **сохраняется с помощью библиотеки Newtonsoft.Json**:
```C#
using Newtonsoft.Json;
using Price_Tag.Classes;
```
```C#
Product_Class.ProductsList.Add(currentProduct);
File.WriteAllText(@"products.json", JsonConvert.SerializeObject(Product_Class.ProductsList, Formatting.Indented));
```
### Импорт из Excel
#### Страница 1
Пользователь указывает путь к файлу и указывает является ли первая строка названием столбца:
```C#
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
```
#### Страница 2
Работа с Excel реализована через **Microsoft.Office.Interop.Excel**<br/>
На второй странице пользователь сопоставляет столбцы для корректного импорта.<br/>
<details>
<summary>Для этого в TextBox'ы подгружаются все названия столбцов:</summary>

```C#
using Excel = Microsoft.Office.Interop.Excel;
```
```C#
private bool FirstLineIsColumnN;

private Excel.Application xlApp;
private Excel.Workbook xlWorkbook;
private Excel._Worksheet xlWorksheet;
private Excel.Range xlRange;

public Page2(string ExcelPath, bool FirstLineIsColumnName) {
    InitializeComponent();
    FirstLineIsColumnN = FirstLineIsColumnName;
    try {
      xlApp = new Excel.Application();
      xlWorkbook = xlApp.Workbooks.Open(ExcelPath);
      xlWorksheet = xlWorkbook.Sheets[1];
      xlRange = xlWorksheet.UsedRange;

      List < string > list = new List < string > ();
      if (FirstLineIsColumnName == true) {
        int i = 1;
        while (xlRange.Cells[1, i] != null && xlRange.Cells[1, i].Value2 != null) {
          list.Add(xlRange.Cells[1, i].Value2.ToString());
          i++;
        }
      } 
      else 
      {
        int i = 1;
        char n = 'A';
        while (xlRange.Cells[1, i] != null && xlRange.Cells[1, i].Value2 != null) {
          list.Add(Convert.ToString(n));
          n += (char) 1;
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
```
</details>
После корректного сопоставления продукты импортируются:

```C#
int i;

int CodeChar;
int NameChar;
int CostChar;
int TypeChar;
int BarcodeChar;

if (FirstLineIsColumnN == true) {
  i = 2;
  CodeChar = Convert.ToInt32(CodeCB.SelectedIndex + 1);
  NameChar = Convert.ToInt32(NameCB.SelectedIndex + 1);
  CostChar = Convert.ToInt32(CostCB.SelectedIndex + 1);
  TypeChar = Convert.ToInt32(TypeCB.SelectedIndex + 1);
  BarcodeChar = Convert.ToInt32(BarcodeCB.SelectedIndex + 1);
} 
else {
  i = 1;
  CodeChar = Convert.ToInt32(Convert.ToChar(CodeCB.SelectedItem) - Convert.ToChar('A')) + 1;
  NameChar = Convert.ToInt32(Convert.ToChar(NameCB.SelectedItem) - Convert.ToChar('A')) + 1;
  CostChar = Convert.ToInt32(Convert.ToChar(CostCB.SelectedItem) - Convert.ToChar('A')) + 1;
  TypeChar = Convert.ToInt32(Convert.ToChar(TypeCB.SelectedItem) - Convert.ToChar('A')) + 1;
  BarcodeChar = Convert.ToInt32(Convert.ToChar(BarcodeCB.SelectedItem) - Convert.ToChar('A')) + 1;
}
try {
  while (xlRange.Cells[NameChar, i + 1] != null && xlRange.Cells[NameChar, i + 1].Value2 != null) {
    Product_Class.Product product = new Product_Class.Product();
    product.ID = Convert.ToInt32(xlRange.Cells[i, CodeChar].Value2);
    product.ProductName = xlRange.Cells[i, NameChar].Value2;
    product.ProductCost = Convert.ToString(xlRange.Cells[i, CostChar].Value2);
    product.ProductType = xlRange.Cells[i, TypeChar].Value2;
    product.ProductBarcode = Convert.ToString(xlRange.Cells[i, BarcodeChar].Value2);
    Product_Class.ProductsList.Add(product);
    i++;
  }
  File.WriteAllText(@ "products.json", JsonConvert.SerializeObject(Product_Class.ProductsList, Formatting.Indented));
  MessageBox.Show("Импорт прошёл успешно!");
} 
catch (Exception ex) 
{
  MessageBox.Show(ex.Message);
}
```

## Печать ценников
Для записи в .pdf используется библиотека **iTextSharp**:
```C#
using System.IO;
using Price_Tag.Classes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
```
```C#
private void Print(List < ProductsToPrint > productsToPrintsList) {
    try {
      if (Manager.SettingsData.FileStreamString == "") {
        throw new Exception("Не указан путь сохранения файлов!");
      }
      string stream = Manager.SettingsData.FileStreamString;
      stream += "/" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".pdf";
      stream.Replace("/", @"\");

        int count = productsToPrintsList.Count;

        // 595 X 842 - A4
        // 208(4) X 166(5)
        FileStream fileStream = File.Create(stream); using(Document document = new Document(new iTextSharp.text.Rectangle(842, 595))) {
          PdfWriter writer = PdfWriter.GetInstance(document, fileStream);
          document.Open();
          PdfContentByte cb = writer.DirectContent;

          string ttf = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
          BaseFont bf = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
          cb.SetColorFill(BaseColor.BLACK);
          cb.SetFontAndSize(bf, 8);

          // Price tag
          int r = 0;
          while (r < count) {
            if (r != 0) document.NewPage();
            cb.SetColorFill(BaseColor.BLACK);
            cb.SetFontAndSize(bf, 8);
            for (int i = 0; i <= 3; i++) {
              for (int k = 0; k <= 2; k++) {
                if (r >= count) break;

                cb.Rectangle(5 + 208 * i, document.PageSize.Height - 5 - 166 * k, 208, -166);
                cb.Stroke();

                BarcodeEAN codeEAN = new BarcodeEAN();
                if (Convert.ToString(productsToPrintsList[r].ProductBarcode).Length == 13)
                  codeEAN.CodeType = Barcode.EAN13;
                else codeEAN.CodeType = Barcode.EAN8;
                codeEAN.Code = Convert.ToString(productsToPrintsList[r].ProductBarcode);

                iTextSharp.text.Image barcode = codeEAN.CreateImageWithBarcode(cb, null, null);
                if (Convert.ToString(productsToPrintsList[r].ProductBarcode).Length == 13)
                  barcode.SetAbsolutePosition(208 * i + 65, document.PageSize.Height - 155 - 166 * k);
                else barcode.SetAbsolutePosition(208 * i + 85, document.PageSize.Height - 155 - 166 * k);
                document.Add(barcode);

                cb.SetFontAndSize(bf, 10);
                string line1 = "";
                string line2 = "";
                if (productsToPrintsList[r].ProductName.Length > 30) {
                  int lastEnter = 0;
                  for (int j = 0; j < 35; j++) {
                    if (productsToPrintsList[r].ProductName[j] == ' ') {
                      lastEnter = j;
                    }
                  }
                  for (int j = 0; j < lastEnter; j++) {
                    line1 += productsToPrintsList[r].ProductName[j];
                  }
                  while (lastEnter < productsToPrintsList[r].ProductName.Length) {
                    line2 += productsToPrintsList[r].ProductName[lastEnter];
                    lastEnter++;
                  }
                } else {
                  line1 = productsToPrintsList[r].ProductName;
                }

                cb.ShowTextAligned(1, line1, 208 * i + 110, document.PageSize.Height - 35 - 166 * k, 0);
                cb.ShowTextAligned(1, line2, 208 * i + 110, document.PageSize.Height - 50 - 166 * k, 0);
                cb.SetFontAndSize(bf, 24);
                cb.ShowTextAligned(1, productsToPrintsList[r].ProductCost + " руб.", 208 * i + 109, document.PageSize.Height - 83 - 166 * k, 0);
                cb.SetFontAndSize(bf, 8);
                cb.ShowTextAligned(1, productsToPrintsList[r].ProductType, 208 * i + 109, document.PageSize.Height - 100 - 166 * k, 0);
                cb.ShowTextAligned(0, Manager.SettingsData.CompanyName, 208 * i + 10, document.PageSize.Height - 15 - 166 * k, 0);
                cb.ShowTextAligned(1, "Код: " + productsToPrintsList[r].ID, 208 * i + 109, document.PageSize.Height - 163 - 166 * k, 0);
                cb.ShowTextAligned(1, System.DateTime.Now.ToString("dd.MM.yyyy"), 208 * i + 190, document.PageSize.Height - 15 - 166 * k, 0);

                r++;
              }
              if (r >= count) break;
            }
          }
          document.Close();
          MessageBox.Show("Ценники напечатаны!");
          Process.Start(Manager.SettingsData.FileStreamString);
        }
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }
```

## Скрины приложения
### Главная страница
    
![image](https://user-images.githubusercontent.com/71275256/174962840-f2a18d93-18ac-48b8-9b67-6677495b0a26.png)

### Товары
* **Все товары**
    
![image](https://user-images.githubusercontent.com/71275256/174962957-0d824350-db2d-4974-a4cc-b71c0be89422.png)

* **Добавление товаров**

![image](https://user-images.githubusercontent.com/71275256/174963108-d6b1e8ab-6542-43d1-ad57-0d3c8acc51a1.png)

* **Изменение товаров**

![image](https://user-images.githubusercontent.com/71275256/174963191-3a4dbb95-db55-4342-b0ee-ea97b26ffd54.png)

* **Импорт из Excel**

![image](https://user-images.githubusercontent.com/71275256/174963326-cbef3c39-d20b-4966-b6e9-ae568720fc25.png)
![image](https://user-images.githubusercontent.com/71275256/174963548-482e48de-a0cb-4042-a188-5d098d23f057.png)
![image](https://user-images.githubusercontent.com/71275256/174963504-50effe79-c4b5-4f0a-9a21-f0356a316db9.png)
![image](https://user-images.githubusercontent.com/71275256/174963634-e703c548-1f13-445b-8d81-fa67cd037caf.png)
    
### Печать ценников
    
![image](https://user-images.githubusercontent.com/71275256/174963743-0ece9dbe-8e41-41ac-886b-dd964e05a54a.png)
![image](https://user-images.githubusercontent.com/71275256/174963853-6121c4be-f79d-4a94-9ab7-09082dd6ed5c.png)
![image](https://user-images.githubusercontent.com/71275256/174963953-bbec349f-8234-4e2d-a06b-d8683d146fb1.png)

### Настройки
    
![image](https://user-images.githubusercontent.com/71275256/174964010-b462e42e-b92f-4a96-a804-5586f2f2e732.png)

