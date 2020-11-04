using DigitalDetector.infra.service;
using DigitalDetector.models;
using DigitalDetector.shared.combo;
using SourceAFIS.Simple;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

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
        public static void CarregarComboNivel(ComboBox cmb)
        {
            cmb.Items.Clear();
            cmb.Items.Add(CriarItemCombo(ENivelUsuario.Nivel_1));
            cmb.Items.Add(CriarItemCombo(ENivelUsuario.Nivel_2));
            cmb.Items.Add(CriarItemCombo(ENivelUsuario.Nivel_3));

            cmb.SelectedIndex = 0;
        }
        private static ComboBoxItem CriarItemCombo(ENivelUsuario nivel)
        {

            return new ComboBoxItem
            {
                Text = nivel.ToString(),
                Value = (int)nivel
            };
        }
        public static BitmapImage Base64StringToBitmap(string base64String)
        {
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(byteBuffer))
            {
                BitmapImage image = new BitmapImage();
                image.StreamSource = ms;
                return image;
            }
        }

        public static Bitmap CriaImagem(string base64)
        {
            byte[] byteBuffer = Convert.FromBase64String(base64);
            Bitmap bmp;
            using (var ms = new MemoryStream(byteBuffer))
            {
                bmp = new Bitmap(ms);
            }
            return bmp;
        }
        public static void RemoverImagem(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
        public static Usuario CompararDigital(string nome, string digital, List<string> comparar)
        {

            var Afis = new AfisEngine();

            Fingerprint Digital_Usuario = new Fingerprint();


            Digital_Usuario.AsBitmap = CriaImagem(digital);
            Person Usuario = new Person();
            Usuario.Fingerprints.Add(Digital_Usuario);
            Afis.Extract(Usuario);


            List<Person> Comparadores = new List<Person>();
            foreach (var item in comparar)
            {
                Fingerprint Digital = new Fingerprint();
                Digital.AsBitmap = CriaImagem(digital);
                Person Comparador = new Person();
                Comparador.Fingerprints.Add(Digital);
                Afis.Extract(Comparador);

                Comparadores.Add(Comparador);
            }

            Person Match = Afis.Identify(Usuario, Comparadores).FirstOrDefault();
            if (Match != null)
            {
                var _repo = new Usuario_Service();
                return _repo.CarregarUsuario(nome);
            }
            else return null;

        }
    }
}
