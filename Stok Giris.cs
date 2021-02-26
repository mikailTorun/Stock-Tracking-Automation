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
    public partial class Stok_Giris : Form
    {
        public Stok_Giris()
        {
            InitializeComponent();
        }
        public void verileriGoster(String veri)
        {

            Form1 anasayfa = new Form1();
            //         baglantiadresi = anasayfa.adresssql();// sql için baglantı adresi aldım yaptım

            //       SqlConnection baglantiFirmList = new SqlConnection(baglantiadresi);

            //      baglantiFirmList.Open();
            SqlConnection baglan = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter(veri, baglan);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglan.Close();
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            Firma_Listesi firmList = new Firma_Listesi();
            firmList.Show();
        }

        private void Stok_Giris_Load(object sender, EventArgs e)
        {
            textBox5.Text = Arızalı_Gelen_Giden_Bilgileri.aded.ToString();
            textBox1SeriNo.Text = Arızalı_Gelen_Giden_Bilgileri.serino.ToString();
           // dateTimePicker3.Value.ToString= Arızalı_Gelen_Giden_Bilgileri.tarihi.ToString();
            comboBox1.Text = Arızalı_Gelen_Giden_Bilgileri.geldigiFirma.ToString();
            verileriGoster("Select * from products");

            //burada combobox a çekme işlemi yapıldı
            //Baglanti classOfConnection = new Baglanti();
            //SqlConnection baglanti = classOfConnection.myConnection();
            Form1 anasayfa = new Form1();
            SqlConnection baglanti = anasayfa.aaa();
            SqlCommand komut = new SqlCommand();
            komut.CommandText = "SELECT *FROM FirmName";
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;

            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr["FirmaAdi"]);
            }
            //burada combobox ile ilgili auto search yaptık
            comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection combData = new AutoCompleteStringCollection();
            getData(combData);
            comboBox1.AutoCompleteCustomSource = combData;
            baglanti.Close();


            ToolTip Aciklama = new ToolTip();
            Aciklama.ToolTipTitle = "Dikkat!";
            Aciklama.ToolTipIcon = ToolTipIcon.Warning;
            Aciklama.IsBalloon = true;

            Aciklama.SetToolTip(button2, "Sadece ID Numarası ile Silme İşlemi Gerçekleştirilebilir!!");
            Aciklama.SetToolTip(button1, "Tüm Gerekli Alanlar Doldurulmalıdır!!");
            Aciklama.SetToolTip(button5, "Sadece SERİ NUMARASI ile arama yapınız!!");

            for (int i=0; i < dataGridView1.Rows.Count - 1; i++)
            {
                Application.DoEvents();
                DataGridViewCellStyle styl = new DataGridViewCellStyle();
                if (Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value) == 1)
                {
                    //satir arka plan rengini kosullu olarak degistiriyoruz.

                  

                    styl.BackColor = Color.OrangeRed;

                    //yazi rengi beyaz oluyor.

                    styl.ForeColor = Color.White;
                }
            }
            

        }
        public void getData(AutoCompleteStringCollection dataCollection)//burası sadece combobox da ilk harfleri girilen metni tanımak için, autosearch
        {
         
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            Form1 anasayfa = new Form1();
            SqlConnection baglan = anasayfa.aaa();
            string sql = "SELECT DISTINCT [FirmaAdi] FROM [FirmName]";
           
          
               
                command = new SqlCommand(sql, baglan);
                adapter.SelectCommand = command;
                adapter.Fill(ds);
                adapter.Dispose();
                command.Dispose();
                baglan.Close();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    dataCollection.Add(row[0].ToString());
                }
            
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox1SeriNo.Text != "" && textBox5.Text != "")
            {

                Form1 anasayfa = new Form1();
                SqlConnection baglan = anasayfa.aaa();


                //SqlCommand command = new SqlCommand("Insert into FirmName(FirmaKodu,FirmaAdi) Values ('" + textBox1.Text.ToString() + "' , '" + textBox2.Text.ToString() + "')", baglan);
                SqlCommand command = new SqlCommand("Insert into products(seri_no,urun_adi,geldigi_firma,geldigi_tarih,urun_adedi) Values (@serino, @adi,@firmasi,@tarihi,@adedi)", baglan);
                command.Parameters.AddWithValue("@serino", textBox1SeriNo.Text);
                command.Parameters.AddWithValue("@adi", textBox2.Text);
                command.Parameters.AddWithValue("@tarihi", dateTimePicker3.Value.ToString("yyyyMMdd"));//Eğer tarih ekleyeck olsaydın bu şekilde yapabilirdin
                command.Parameters.AddWithValue("@firmasi", comboBox1.Text);
                command.Parameters.AddWithValue("@adedi", textBox5.Text);
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
                verileriGoster("Select * from products");

            }
            else
                MessageBox.Show("Lütfen Gerekli Alanları Doldurunuz");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dateTimePicker3.Value.ToString("yyyyMMdd"));
            verileriGoster("Select * from products");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Lütfen Silinecek Ürünün ID Numarasını giriniz!");
                
            }
            else
            {

                Form1 anasayfa = new Form1();
                SqlConnection baglan = anasayfa.aaa(); ;


                SqlCommand comment = new SqlCommand("delete from products where ID=@productID", baglan);

                comment.Parameters.AddWithValue("@productID", textBox3.Text);

                if (comment.ExecuteNonQuery() == 0)
                {
                    MessageBox.Show("Bu ID Numaralı Bir Ürün Bulunamadı");
                    textBox3.Clear();
                }
                else
                {
                    verileriGoster("Select * from products");
                    baglan.Close();
                    MessageBox.Show(textBox3.Text + " ID Numaralı Ürün Başarıyla Silindi");
                    textBox3.Clear();

                }

            }
        }

        private void Stok_Giris_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Form1 anasayfa = new Form1();
                SqlConnection baglan = anasayfa.aaa();
                SqlCommand komut = new SqlCommand("Select * from products where seri_no like '%" + textBox1.Text + "%' ", baglan);

                SqlDataAdapter da = new SqlDataAdapter(komut);

                DataSet ds = new DataSet();
                da.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];
                baglan.Close();

            }
            else
                MessageBox.Show("Lütfen SERİ NUMARASI ile arama yapınız!!!");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form1 anasayfa = new Form1();
            
            SqlConnection baglan = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter("select * from products", baglan);
            DataSet ds = new DataSet();
            da.Fill(ds,"urunlist");
            dataGridView1.DataSource = ds.Tables[0];
            baglan.Close();

            //Fast Report Dizayn kod
            // create report instance
            FastReport.Report report = new FastReport.Report();
            // load the existing report
            if (!System.IO.File.Exists(Application.StartupPath + @"\Design\RaporUrunList.frx"))
            {
                MessageBox.Show("Dizayn dosyası bulunamadı, lütfen oluşturun!");
            }
            else
            {
                report.Load(Application.StartupPath + @"\Design\RaporUrunList.frx");
            }
            
            // register the dataset
            report.RegisterData(ds);
            // run the report
            report.Design();
            // free resources used by report
            report.Dispose();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form1 anasayfa = new Form1();
            
            SqlConnection baglan = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter("select * from products", baglan);
            DataSet ds = new DataSet();
            da.Fill(ds, "urunlist");
            dataGridView1.DataSource = ds.Tables[0];
            baglan.Close();

            //Fast Report Dizayn kod
            // create report instance
            FastReport.Report report = new FastReport.Report();
            // load the existing report
            if (!System.IO.File.Exists(Application.StartupPath + @"\Design\RaporUrunList.frx"))
            {
                MessageBox.Show("Dizayn dosyası bulunamadı, lütfen oluşturun!");
            }
            else
            {
                report.Load(Application.StartupPath + @"\Design\RaporUrunList.frx");
            }

            // register the dataset
            report.RegisterData(ds);
            // run the report
            report.Print();
            // free resources used by report
            report.Dispose();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form1 anasayfa = new Form1();
           
            SqlConnection baglan = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter("select * from products", baglan);
            DataSet ds = new DataSet();
            da.Fill(ds, "urunlist");
            dataGridView1.DataSource = ds.Tables[0];
            baglan.Close();

            //Fast Report Dizayn kod
            // create report instance
            FastReport.Report report = new FastReport.Report();
            // load the existing report
            if (!System.IO.File.Exists(Application.StartupPath + @"\Design\RaporUrunList.frx"))
            {
                MessageBox.Show("Dizayn dosyası bulunamadı, lütfen oluşturun!");
            }
            else
            {
                report.Load(Application.StartupPath + @"\Design\RaporUrunList.frx");
            }

            // register the dataset
            report.RegisterData(ds);
            // run the report
            report.Show();
            // free resources used by report
            report.Dispose();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form1 anasayfa = new Form1();
            //         baglantiadresi = anasayfa.adresssql();// sql için baglantı adresi aldım yaptım

            //       SqlConnection baglantiFirmList = new SqlConnection(baglantiadresi);

            //      baglantiFirmList.Open();
            SqlConnection baglan = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter("select * from products", baglan);
            DataSet ds = new DataSet();
            da.Fill(ds, "urunlist");
            dataGridView1.DataSource = ds.Tables[0];
            baglan.Close();

            //Fast Report Dizayn kod
            // create report instance
            FastReport.Report report = new FastReport.Report();
            // load the existing report
            if (!System.IO.File.Exists(Application.StartupPath + @"\Design\RaporUrunList.frx"))
            {
                MessageBox.Show("Dizayn dosyası bulunamadı, lütfen oluşturun!");
            }
            else
            {
                report.Load(Application.StartupPath + @"\Design\RaporUrunList.frx");
            }

            // register the dataset
            report.RegisterData(ds);

            report.Prepare();
            FastReport.Export.Pdf.PDFExport export = new FastReport.Export.Pdf.PDFExport();
            report.Export(export, Application.StartupPath + @"\RaporUrunList.pdf");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form1 anasayfa = new Form1();
            //         baglantiadresi = anasayfa.adresssql();// sql için baglantı adresi aldım yaptım

            //       SqlConnection baglantiFirmList = new SqlConnection(baglantiadresi);

            //      baglantiFirmList.Open();
            SqlConnection baglan = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter("select * from products", baglan);
            DataSet ds = new DataSet();
            da.Fill(ds, "urunlist");
            dataGridView1.DataSource = ds.Tables[0];
            baglan.Close();

            //Fast Report Dizayn kod
            // create report instance
            FastReport.Report report = new FastReport.Report();
            // load the existing report
            if (!System.IO.File.Exists(Application.StartupPath + @"\Design\RaporUrunList.frx"))
            {
                MessageBox.Show("Dizayn dosyası bulunamadı, lütfen oluşturun!");
            }
            else
            {
                report.Load(Application.StartupPath + @"\Design\RaporUrunList.frx");
            }

            // register the dataset
            report.RegisterData(ds);

            report.Prepare();
            FastReport.Export.Csv.CSVExport export = new FastReport.Export.Csv.CSVExport();
            report.Export(export, Application.StartupPath + @"\RaporUrunList.csv");
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
