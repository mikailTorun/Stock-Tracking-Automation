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

namespace Stok_Takip_ve_Muhasebe_Programi
{
    public partial class Rapor : Form
    {
        public Rapor()
        {
            InitializeComponent();
        }
        
        private void Raporx(Int32 frType)
        {
            Form1 anasayfa = new Form1();
            SqlConnection con = anasayfa.aaa();
            string kosulUrun = "";
            string kosulArama = "";
            string kosulGrupKodu = "";
            String tarihKosulu = "";
            kosulGrupKodu = " and GrupAd=" + comboBox4.Text + "";

            if (textBox1.Text.Trim() != "")
            {
                switch (comboBox2.SelectedIndex)
                {
                    case 0:
                        kosulUrun = " and UrunAd like '%" + textBox1.Text + "%' ";
                        break;

                    case 1:
                        kosulUrun = " and PrID = " + textBox1.Text;
                        break;

                    case 2:
                        kosulUrun = "";
                        break;

                    case 3:
                        kosulUrun = " and (Barkod like '%" + textBox1.Text + "%' or Barkod2 like '%" + textBox1.Text + "%') ";
                        break;
                }
            }

            switch (comboBox3.SelectedIndex)
            {
                case 0: kosulArama = " ";  break;
                case 1: kosulArama = " and seriNo=1 "; break;
                case 2: kosulArama = "  and seriNo=0 ";  break;
            }

            //kosulTarihBaslangic = dateTimePicker1.Value.ToString();
            //kosulTarihBitis = dateTimePicker2.Value.ToString();
            tarihKosulu = " and irsOlusturmaTarih Between @tarihBegin and @tarihEnd ";

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            string raporDizayn = "";

            switch (listBox1.SelectedIndex)
            {
                case 0:
                    da = new SqlDataAdapter("Select * from Urunler where 1=1 " + kosulUrun + kosulArama, con);
                    ds = new DataSet();
                    da.Fill(ds,"Urunler");
                    dataGridView1.DataSource = ds.Tables["Urunler"];
                    raporDizayn = "RaporUrunList.frx";
                    break;

                case 1:
                     da = new SqlDataAdapter("Select pcID , UrunAd, sum(abs(adet)) ADET from Urunler  " +
                "inner join irsaliyeHareket on pcID=PrID " +
                "Group By pcID, UrunAd " , con);
                    
                    ds = new DataSet();
                    da.Fill(ds, "UrunlerAdet");
                    dataGridView1.DataSource = ds.Tables["UrunlerAdet"];
                    raporDizayn = "RaporUrunAdetList.frx";
                    break;

                case 2:
                    da = new SqlDataAdapter("Select * from irsaliyeHareket ih inner join irsaliye ir on ir.irsID=ih.irsID " +
                        "inner join Urunler on PrID= pcID where 1=1 " + kosulUrun + kosulArama+ tarihKosulu, con);
                    da.SelectCommand.Parameters.AddWithValue("@tarihBegin",dateTimePicker1.Value.ToString("yyyyMMdd") );
                    da.SelectCommand.Parameters.AddWithValue("@tarihEnd",  dateTimePicker2.Value.ToString("yyyyMMdd") );
                    ds = new DataSet();
                    da.Fill(ds, "UrunlerHareket");
                    dataGridView1.DataSource = ds.Tables["UrunlerHareket"];
                    raporDizayn = "RaporUrunHareketList.frx";
                    break;
            }
            
            FastReport.Report report = new FastReport.Report();
            // load the existing report
            if (!System.IO.File.Exists(Application.StartupPath + @"\Design\" + raporDizayn)) 
            {
                MessageBox.Show("Dizayn dosyası bulunamadı, lütfen oluşturun!");
            }
            else
            {
                report.Load(Application.StartupPath + @"\Design\" + raporDizayn);
            }

            // register the dataset
            report.RegisterData(ds);
            // run the report
            switch (frType)
            {
                case 0:
                    report.Design();
                    break;

                case 1:
                    report.Show();
                    break;
            }
            
            // free resources used by report
            report.Dispose();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Raporx(1);

            //Form1 anasayfa = new Form1();
            //SqlConnection con = anasayfa.aaa();

            //int kosulUrun,//adı, Id, serino ya da barkoda göre kosul, combobox2 deki değerler
            //    kosulArama;// Tüm Ürünler - Serinolu ürünler - Serinolu olmayan ürünler gibi combobox3 deki değerler
                
            //kosulUrun = comboBox2.SelectedIndex;

            //String kelime="";

            //switch (kosulUrun)
            //{
            //    case 0: kelime = "UrunAd"; break;
            //    case 1: kelime = "PrID";break;
            //    case 2: kelime = "seriNo";break;
            //    case 3:kelime = "Barkod"; break;

            //}
            //kosulArama = comboBox3.SelectedIndex;
            
            //switch (listBox1.SelectedIndex)
            //{
            //    case 0:
            //        SqlDataAdapter da = new SqlDataAdapter("Select * from Urunler where "+kelime+" like '%" + textBox1.Text + "%' ", con);
            //        DataTable dt = new DataTable();
            //        da.Fill(dt);
            //        dataGridView1.DataSource = dt;
            //        break;
            //    case 1:
            //        da = new SqlDataAdapter("Select * from Urunler where " + kelime + " like '%" + textBox1.Text + "%' ", con);
            //        dt = new DataTable();
            //        da.Fill(dt);
            //        dataGridView1.DataSource = dt;
            //        break;
            //    case 2:
            //        da = new SqlDataAdapter("Select *from irsaliyeHareket where " + kelime + " like '%" + textBox1.Text + "%' ", con);
            //        dt = new DataTable();
            //        da.Fill(dt);
            //        dataGridView1.DataSource = dt;
            //        break;
            //    case 3:
            //        da = new SqlDataAdapter("Select *from Urunler where " + kelime + " like '%" + textBox1.Text + "%' ", con);
            //        dt = new DataTable();
            //        da.Fill(dt);
            //        dataGridView1.DataSource = dt;
            //        break;

            //}



        }


        void veriGoster(int kosulUrun, int kosulArama, int kosulKelime, String deger)
        {

        }
        private void Rapor_Load(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedValue = "Grup Seçiniz".ToString();

            if (listBox1.SelectedIndex == 2) groupBox4.Visible = true;

            Form1 anasayfa = new Form1();
            SqlConnection baglanti = anasayfa.aaa();

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "SELECT DISTINCT UrunGrubu FROM Urunler ";
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;

            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboBox4.Items.Add(dr["UrunGrubu"]);
            }


        }
        
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
           
        }
        int listboxSelectedindex;
        private void listBox1_Click(object sender, EventArgs e)
        {
            listboxSelectedindex = listBox1.SelectedIndex;
            if (listboxSelectedindex == 2) groupBox4.Visible = true;
            else groupBox4.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Raporx(0);
        }
    }
}
