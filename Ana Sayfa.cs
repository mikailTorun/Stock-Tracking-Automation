using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Stok_Takip_ve_Muhasebe_Programi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        String currentApplicationPath = Application.StartupPath;
        String baglantiAdresiSql;




        private void stokMenusuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void stokGirişToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stok_Giris stkGiris = new Stok_Giris();
            stkGiris.ShowDialog();
        }

        private void canlıSatışToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void firmalarımıGörToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Firma_Listesi firmList = new Firma_Listesi();
            firmList.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
         //   MessageBox.Show(currentApplicationPath);
            StreamReader read = new StreamReader(currentApplicationPath + "\\sqlAdress.txt");
            String satır = read.ReadLine();
            while (satır != null)
            {
                baglantiAdresiSql = satır;
                satır = read.ReadLine();
            }
            SqlConnection baglanti = new SqlConnection(baglantiAdresiSql);
            baglanti.Open();
            
            
         //   MessageBox.Show("baglantı adresi: " +baglantiAdresiSql);
        }
        public String adresssql()
        {
            String adrss = "";
            StreamReader read2 = new StreamReader(currentApplicationPath + "\\sqlAdress.txt");
            String satır2 = read2.ReadLine();
            while (satır2 != null)
            {
                adrss = satır2;
                satır2 = read2.ReadLine();
            }
           // MessageBox.Show("satır2 = "+adrss);
            return adrss;
        }
        public SqlConnection aaa()
        {
            String connectadres = adresssql();
            SqlConnection baglanti = new SqlConnection(connectadres);
            baglanti.Open();
            return baglanti;
        }

        private void stokGüncellemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Arızalı_Gelen_Giden_Bilgileri arizaBilgileri = new Arızalı_Gelen_Giden_Bilgileri();
            arizaBilgileri.Show();
        }

        private void stokÇıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stok_Cikis stokCikisi = new Stok_Cikis();
            stokCikisi.Show();
        }

        private void ürünEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ürünEkle ürün = new ürünEkle();
            ürün.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Form1 anasayfa = new Form1();
                SqlConnection baglan = anasayfa.aaa();
                SqlCommand komut = new SqlCommand("Select * from irsaliyeHareket where seriNo like '%" + textBox1.Text + "%' ", baglan);

                SqlDataAdapter da = new SqlDataAdapter(komut);

                DataSet ds = new DataSet();
                da.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];
                baglan.Close();

            }
            else
                MessageBox.Show("Lütfen SERİ NUMARASI ile arama yapınız!!!");


        }

        private void irsaliyeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IrsaliyeEkrani ie = new IrsaliyeEkrani();
            ie.irsTip = 1;
            ie.Show();
        }

        private void girişİrsaliyesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IrsaliyeEkrani ie = new IrsaliyeEkrani();
            ie.irsTip = 0;
            ie.Show();
        }

        private void arızalıGirişToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IrsaliyeEkrani ie = new IrsaliyeEkrani();
            ie.irsTip = 2;
            ie.Show();


        }

        private void arızalıÇıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IrsaliyeEkrani ie = new IrsaliyeEkrani();
            ie.irsTip = 3;
            ie.Show();
        }

        private void günlükAporlamaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //   UrunRaporları urunRapor = new UrunRaporları();
            // urunRapor.Show();
            Rapor rapor = new Rapor();
            rapor.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 anasayfa = new Form1();
            SqlConnection baglan = anasayfa.aaa();

            //SqlDataAdapter da = new SqlDataAdapter("Select *from products", baglan);
            //DataSet ds = new DataSet();
            //da.Fill(ds);
            //dataGridView1.DataSource = ds.Tables[0];
            //baglan.Close();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter("select ih.irsID, siraNo, case when adet=-1 then 1 END AS 'adet', pcID, ih.seriNo from irsaliyeHareket ih " +
                        "inner join irsaliye ir on ih.irsID =ir.irsID " +
                        "where irsTip=@it", baglan);
            da.SelectCommand.Parameters.AddWithValue("@it", 1);
            // da.SelectCommand.Parameters.AddWithValue("@tip", irsTip);
            // DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
