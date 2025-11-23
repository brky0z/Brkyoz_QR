using System;
using System.Drawing; // Resim işlemleri
using System.Windows.Forms;
using ZXing; // Golcü (QR kütüphanesi)

namespace QR_Okuyucu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        // ---------------------------------------------------------
        // 1. BUTON: RESİM SEÇ (Dosyadan)
        // ---------------------------------------------------------
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosyaAc = new OpenFileDialog();
            dosyaAc.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp|Tüm Dosyalar|*.*";
            dosyaAc.Title = "QR Kodlu SS'i Seç Kral";

            if (dosyaAc.ShowDialog() == DialogResult.OK)
            {
                // Seçilen resmi alıp ortak golcüye (QR_Coz) paslıyoruz
                QR_Coz(Image.FromFile(dosyaAc.FileName));
            }
        }

        // ---------------------------------------------------------
        // 2. BUTON: YAPIŞTIR (Panodan / Ctrl+V)
        // ---------------------------------------------------------
        private void btnYapistir_Click(object sender, EventArgs e)
        {
            // Panoda resim var mı?
            if (Clipboard.ContainsImage())
            {
                // Varsa resmi alıp ortak golcüye paslıyoruz
                QR_Coz(Clipboard.GetImage());
            }
            else
            {
                MessageBox.Show("Kral panoda resim yok. Önce bir SS al (PrintScreen).", "Hafıza Boş");
            }
        }

        // ---------------------------------------------------------
        // ORTAK GOLCÜ (Teknik Direktörün Taktik Alanı)
        // İki buton da işi buraya yıkar, burası çözer.
        // ---------------------------------------------------------
        private void QR_Coz(Image gelenResim)
        {
            try
            {
                // 1. Resmi ekrana bas
                pictureBox1.Image = gelenResim;

                // 2. Okumaya başla
                BarcodeReader okuyucu = new BarcodeReader();
                var sonuc = okuyucu.Decode((Bitmap)gelenResim);

                // 3. Sonuç var mı?
                if (sonuc != null)
                {
                    textBox1.Text = sonuc.Text;

                    // Link kontrolü (Ofsayt taktiği)
                    string link = sonuc.Text.ToLower();
                    if (link.StartsWith("http") || link.StartsWith("www") || link.Contains(".com"))
                    {
                        // Tarayıcıyı aç
                        System.Diagnostics.Process.Start(sonuc.Text);
                    }
                    else
                    {
                        // Link değilse sadece mesaj ver
                        MessageBox.Show("QR Okundu: " + sonuc.Text);
                    }
                }
                else
                {
                    textBox1.Text = "QR Bulunamadı.";
                    MessageBox.Show("Resim var ama içinde QR kod yok kral.", "Ofsayt");
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Bir hata oldu kaptan: " + hata.Message);
            }
        }
    }
}