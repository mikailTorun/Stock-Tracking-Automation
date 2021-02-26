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
    public partial class Arızalı_Gelen_Giden_Bilgileri : Form
    {
        public Arızalı_Gelen_Giden_Bilgileri()
        {
            InitializeComponent();
        }

        private void Arızalı_Gelen_Giden_Bilgileri_Load(object sender, EventArgs e)
        {

            ToolTip Aciklama = new ToolTip();
            Aciklama.ToolTipTitle = "Dikkat!";
            Aciklama.ToolTipIcon = ToolTipIcon.Warning;
            Aciklama.IsBalloon = true;

            Aciklama.SetToolTip(button7, "Sadece SERİ NUMARASI ile arama yapılabilir!!");
            Aciklama.SetToolTip(button8, "Sadece SERİ NUMARASI ile arama yapılabilir!!");


            groupBox1.Visible = false;
            groupBox3.Visible = false;
            
            Form1 anasayfa = new Form1();
            SqlConnection baglanti = anasayfa.aaa();
            SqlCommand komut = new SqlCommand();
            komut.CommandText = "SELECT *FROM FirmName";
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;

            SqlDataReader dr;

            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr["FirmaAdi"]);
                comboBox2.Items.Add(dr["FirmaAdi"]);
            }
            //burada combobox ile ilgili auto search yaptık
            comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection combData = new AutoCompleteStringCollection();
            Stok_Giris stkGris = new Stok_Giris();
            stkGris.getData(combData);
            comboBox1.AutoCompleteCustomSource = combData;
            comboBox2.AutoCompleteCustomSource = combData;
            baglanti.Close();
            verileriGoster("Select * from ArizaliGelenUrun");
            verileriGoster2("Select * from OnarimSonrasiIslem");
        }
        public void verileriGoster(String veri)
        {

           
            Form1 anasayfa = new Form1();
            SqlConnection baglan = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter(veri, baglan);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglan.Close();
        }
        public void verileriGoster2(String veri)
        {

            // Form1 anasayfa = new Form1();
            //         baglantiadresi = anasayfa.adresssql();// sql için baglantı adresi aldım yaptım

            //       SqlConnection baglantiFirmList = new SqlConnection(baglantiadresi);

            //      baglantiFirmList.Open();
            //  SqlConnection baglan = anasayfa.aaa();
            Form1 anasayfa = new Form1();
            SqlConnection baglan = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter(veri, baglan);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
            baglan.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox1.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox3.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && comboBox1.Text != "" && textBox3.Text != "")
            {

                Form1 anasayfa = new Form1();
                SqlConnection baglan = anasayfa.aaa();


                //SqlCommand command = new SqlCommand("Insert into FirmName(FirmaKodu,FirmaAdi) Values ('" + textBox1.Text.ToString() + "' , '" + textBox2.Text.ToString() + "')", baglan);
                SqlCommand command = new SqlCommand("Insert into ArizaliGelenUrun(SeriNo,GeldigiTarih,GeldigiFirma,ArizaAcıklamasi,OnarildigiYer,GonderildigiFirma,GonderildigiTarih) Values (@serino,@geldigitarih,@geldigifirma,@arizaaciklamasi,@onarildigiyer,@gonderilgifirma,@gonderildigitarih)", baglan);
                command.Parameters.AddWithValue("@serino", textBox1.Text);
                command.Parameters.AddWithValue("@geldigitarih", dateTimePicker1.Value.ToString("yyyyMMdd"));
                command.Parameters.AddWithValue("@geldigifirma", comboBox1.Text);
                command.Parameters.AddWithValue("@arizaaciklamasi", textBox3.Text);
                command.Parameters.AddWithValue("@onarildigiyer", textBox2.Text);
                command.Parameters.AddWithValue("@gonderilgifirma", comboBox2.Text);
                command.Parameters.AddWithValue("@gonderildigitarih", dateTimePicker2.Value.ToString("yyyyMMdd"));
                //Bu şekilde kullanabilirsin, daha düzgün ve sağlam olur


                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Ekleme Başarılı!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ekleme sırasında hata oluştu!!!");
                }



                baglan.Close();
                // dataGridView1.DataSource="";// var olan elemanları her defasında art arda yazmasın diye yaptım
                verileriGoster("Select * from ArizaliGelenUrun");

            }
            else
                MessageBox.Show("Lütfen Gerekli Alanları Doldurunuz");
        }
        public static String serino="";
        public static String geldigiFirma="" ;
        public static String tarihi;
        public static char aded = '1';
       

        private void button1_Click(object sender, EventArgs e)
        {
            serino = textBox6.Text;
            geldigiFirma = comboBox1.Text;
            tarihi = dateTimePicker3.Value.ToString("yyyyMMdd");
            Stok_Giris stkgrs = new Stok_Giris();
            stkgrs.Show();
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "" && comboBox3.Text != "" && textBox10.Text != "")
            {

                Form1 anasayfa = new Form1();
                SqlConnection baglan = anasayfa.aaa();


                //SqlCommand command = new SqlCommand("Insert into FirmName(FirmaKodu,FirmaAdi) Values ('" + textBox1.Text.ToString() + "' , '" + textBox2.Text.ToString() + "')", baglan);
                SqlCommand command = new SqlCommand("Insert into OnarimSonrasiIslem(SeriNo,YapilacakIslem,OnarımdaYapilanIslem,DonusTarihi,GonderildigiMagaza,GonderimTarihi) Values (@serino,@yapilacakislem,@onarimdakiislem,@donustarihi,@gonderilecekmagaza,@gonderimtarihi)", baglan);
                command.Parameters.AddWithValue("@serino", textBox6.Text);
                command.Parameters.AddWithValue("@yapilacakislem", comboBox3.Text);
                command.Parameters.AddWithValue("@onarimdakiislem", textBox10.Text);
                command.Parameters.AddWithValue("@donustarihi", dateTimePicker3.Value.ToString("yyyyMMdd"));
                command.Parameters.AddWithValue("@gonderilecekmagaza", textBox8.Text);
                command.Parameters.AddWithValue("@gonderimtarihi", dateTimePicker4.Value.ToString("yyyyMMdd"));
                

                //Bu şekilde kullanabilirsin, daha düzgün ve sağlam olur


                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Ekleme Başarılı!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ekleme sırasında hata oluştu!!!");
                }



                baglan.Close();
                // dataGridView1.DataSource="";// var olan elemanları her defasında art arda yazmasın diye yaptım
                verileriGoster2("Select * from OnarimSonrasiIslem");

            }
            else
                MessageBox.Show("Lütfen Gerekli Alanları Doldurunuz");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                Form1 anasayfa = new Form1();
                SqlConnection baglan = anasayfa.aaa();
                SqlCommand komut = new SqlCommand("Select * from ArizaliGelenUrun where SeriNo like '%" + textBox4.Text + "%' ", baglan);

                SqlDataAdapter da = new SqlDataAdapter(komut);

                DataSet ds = new DataSet();
                da.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];
                baglan.Close();

            }
            else
                MessageBox.Show("Lütfen SERİ NUMARASI ile arama yapınız!!!");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox5.Text != "")
            {
                Form1 anasayfa = new Form1();
                SqlConnection baglan = anasayfa.aaa();
                SqlCommand komut = new SqlCommand("Select * from OnarimSonrasiIslem where SeriNo like '%" + textBox5.Text + "%' ", baglan);

                SqlDataAdapter da = new SqlDataAdapter(komut);

                DataSet ds = new DataSet();
                da.Fill(ds);

                dataGridView2.DataSource = ds.Tables[0];
                baglan.Close();

            }
            else
                MessageBox.Show("Lütfen SERİ NUMARASI ile arama yapınız!!!");
        }

        private void onarimyeriGetir()
        {
            label13.Text = "";

            Form1 anasayfa = new Form1();
            SqlConnection baglan = anasayfa.aaa();
            SqlCommand komut = new SqlCommand("select Top 1 case when OnarildigiYer='' then GonderildigiFirma else OnarildigiYer end " +
            "from ArizaliGelenUrun where SeriNo=@serino Order by GonderildigiTarih desc, ID desc", baglan);
            komut.Parameters.AddWithValue("@serino", textBox6.Text);
            try
            {
                label13.Text = komut.ExecuteScalar().ToString();
            }
            catch //(Exception ex)
            {
                label13.Text = "Onarım yeri bulunamadı !!!";
            }
            

        }

        private void button5_Click(object sender, EventArgs e)
        {
            onarimyeriGetir();    
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                onarimyeriGetir();
            };
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // MessageBox.Show(Properties.Settings.Default.Server);
            // MessageBox.Show(Properties.Settings.Default.Database);
            verileriGoster("Select * from ArizaliGelenUrun");
            verileriGoster2("select * from OnarimSonrasiIslem");
        }
    }
    }

