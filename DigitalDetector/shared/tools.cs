using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DigitalDetector.shared
{
    public static class Tools
    {
        public static string CarregarDigital(PictureBox pbDigital)
        {
            Bitmap bitmap = null;
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pbDigital.ImageLocation = openFile.FileName;
                pbDigital.SizeMode = PictureBoxSizeMode.StretchImage;

                bitmap = new Bitmap(openFile.FileName);
            }
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);

            return Convert.ToBase64String(ms.ToArray()); ;
        }
    }
}
