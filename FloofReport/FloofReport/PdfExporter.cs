using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace FloofReport
{
    public static class PdfExporter
    {
        public static void ExportToPdf(FrameworkElement element, string filename)
        {
            // Convert the WPF UI element to a bitmap image
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            int dpi = 96;
            double width = element.ActualWidth;
            double height = element.ActualHeight;

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)Math.Ceiling(width * dpi / 96.0), (int)Math.Ceiling(height * dpi / 96.0), dpi, dpi, PixelFormats.Pbgra32);
            bmp.Render(element);

            // Create a PDF document
            PdfDocument document = new PdfDocument();

            // Create a new page and add it to the document
            PdfPage page = document.AddPage();
            page.Width = width * 72 / 96.0;
            page.Height = height * 72 / 96.0;

            // Create an XImage from the bitmap image and draw it on the PDF page
            using (XGraphics gfx = XGraphics.FromPdfPage(page))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    encoder.Save(stream);

                    XImage img = XImage.FromStream(stream);
                    gfx.DrawImage(img, 0, 0, page.Width, page.Height);
                }
            }
            // Save the document to a file
            document.Save(filename);
        }
    }
}
