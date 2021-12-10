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

using Microsoft.WindowsAPICodePack.Dialogs;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Price_Tag.Pages
{
    /// <summary>
    /// Логика взаимодействия для PrintPage.xaml
    /// </summary>
    public partial class PrintPage : Page
    {
        public PrintPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int count = 5;
            string OOO_IP_Name = "ИП Кабыткин Александр Игоревич";
            string date = "10.12.2021";
            string productName = "БИСКВИТ ORION CHOCO PIE 6ШТ 180ГР";
            string productCost = "74.00";
            string productType = "Цена за 1 шт.";
            string productID = "656";
            string productCode = "4607084351378";

            string stream = @"F:\PetProjects\Price_Tag\Tags\hui.pdf";

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                stream = dialog.FileName + "/" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".pdf";
                stream.Replace("/", @"\");
                MessageBox.Show(stream);
            }



            // 595 X 842 - A4
            // 208(4) X 166(5)
            FileStream fileStream = File.Create(stream);
            using (Document document = new Document(new iTextSharp.text.Rectangle(842, 595)))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, fileStream);
                document.Open();
                PdfContentByte cb = writer.DirectContent;

                string ttf = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
                BaseFont bf = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(bf, 8);

                // Rectangle
                int r = 0;
                for (int k = 0; k <= 3; k++)
                {
                    for (int i = 0; i <= 2; i++)
                    {
                        if (r >= count) break;
                        cb.Rectangle(5 + 208 * k, document.PageSize.Height - 5 - 166 * i, 208, -166);
                        cb.Stroke();
                        r++;
                    }
                    if (r >= count) break;
                }
                // Barcode
                r = 0;
                for (int i = 0; i <= 3; i++)
                {
                    for (int k = 0; k <= 2; k++)
                    {
                        if (r >= count) break;

                        BarcodeEAN codeEAN = new BarcodeEAN();
                        codeEAN.Code = productCode;

                        iTextSharp.text.Image barcode = codeEAN.CreateImageWithBarcode(cb, null, null);
                        barcode.SetAbsolutePosition(208 * i + 65, document.PageSize.Height - 155 - 166 * k);
                        document.Add(barcode);

                        cb.SetFontAndSize(bf, 10);
                        cb.ShowTextAligned(1, productName, 208 * i + 110, document.PageSize.Height - 35 - 166 * k, 0);
                        cb.SetFontAndSize(bf, 24);
                        cb.ShowTextAligned(1, productCost + " руб.", 208 * i + 109, document.PageSize.Height - 83 - 166 * k, 0);
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(1, productType, 208 * i + 109, document.PageSize.Height - 100 - 166 * k, 0);
                        cb.ShowTextAligned(0, OOO_IP_Name, 208 * i + 10, document.PageSize.Height - 15 - 166 * k, 0);
                        cb.ShowTextAligned(1, "Код: " + productID, 208 * i + 109, document.PageSize.Height - 163 - 166 * k, 0);
                        cb.ShowTextAligned(1, date, 208 * i + 190, document.PageSize.Height - 15 - 166 * k, 0);

                        r++;
                    }
                    if (r >= count) break;
                }
                document.Close();
            }
        }
    }
}
