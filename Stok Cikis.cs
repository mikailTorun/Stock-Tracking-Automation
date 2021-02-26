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
    public partial class Stok_Cikis : Form
    {

        Firma_Listesi firmlist = new Firma_Listesi();

        public Stok_Cikis()
        {
            InitializeComponent();
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
        private void Stok_Cikis_Load(object sender, EventArgs e)
        {
            verileriGoster("Select * from StokCikis");
            Form1 anasayfa = new Form1();
            SqlConnection baglanti = anasayfa.aaa();
            SqlCommand komut = new SqlCommand();
            komut.CommandText = "SELECT *FROM products";
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;

            SqlDataReader dr;

            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr["seri_no"]);
            }
            //burada combobox ile ilgili auto search yaptık
            comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection combData = new AutoCompleteStringCollection();
            getData(combData);
            comboBox1.AutoCompleteCustomSource = combData;
            baglanti.Close();
        }
        public void getData(AutoCompleteStringCollection dataCollection)//burası sadece combobox da ilk harfleri girilen metni tanımak için, autosearch
        {

            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            Form1 anasayfa = new Form1();
            SqlConnection baglan = anasayfa.aaa();
            string sql = "SELECT DISTINCT [seri_no] FROM [products]";



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

        bool urunKontrol(string serino)
        {
            Form1 anasayfa = new Form1();
            SqlConnection baglan = anasayfa.aaa();
            SqlDataAdapter da = new SqlDataAdapter("select * from products where seri_no=@serino", baglan);
            da.SelectCommand.Parameters.AddWithValue("@serino", serino);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Ürün Bulunamadı!!!");
                return false;
            }
            else
            {
                MessageBox.Show("Ürün VAR");
                return true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (urunKontrol(comboBox1.Text) == false) return;

            if(textBox1AdUnvan.Text!="" && textBox2isyeriAdres.Text != "" && textBox7telefon.Text != "" && comboBox1.Text != "" && textBox9.Text != "" && textBox10.Text != "" )
            {
                Form1 anasayfa = new Form1();
                SqlConnection baglan= anasayfa.aaa();
                SqlCommand command = new SqlCommand("Insert into StokCikis(AdiSoyadiUnvani,IsyeriAdresi,Telefon,SeriNo,UrunAdi,CıkısTarihi,MagazaAdiKodu) values(@ad,@adres,@tel,@serino,@urunadi,@tarihi,@magazaadi)",baglan);
                command.Parameters.AddWithValue("@ad", textBox1AdUnvan.Text);
                command.Parameters.AddWithValue("@adres",textBox2isyeriAdres.Text);
                command.Parameters.AddWithValue("@tel", textBox7telefon.Text);
                command.Parameters.AddWithValue("@serino", comboBox1.Text);
                command.Parameters.AddWithValue("@urunadi", textBox9.Text);
                command.Parameters.AddWithValue("@tarihi", dateTimePicker2.Value.ToString("yyyyMMdd"));
                command.Parameters.AddWithValue("@magazaadi", textBox10.Text);
                
                
                    try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Ekleme Başarılı!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ekleme sırasında hata oluştu!!!");
                }
               // baglan.Close();
                // dataGridView1.DataSource="";// var olan elemanları her defasında art arda yazmasın diye yaptım
                verileriGoster("Select * from StokCikis");
            }else
                MessageBox.Show("Lütfen Gerekli Alanları Doldurunuz");


            
        }

        private void temizle()
        {
            textBox1AdUnvan.Clear();
            //
            //
            //
        }

        private void firmaBilgisiGetir(int firmaID)
        {
            temizle();

            Form1 anasayfa = new Form1();
            SqlConnection baglan = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter("select * from FirmName Where ID=@id", baglan);
            da.SelectCommand.Parameters.AddWithValue("@id", firmaID);
            DataTable dt = new DataTable();
            da.Fill(dt);

            //Nesneleri dolduralım
            textBox1AdUnvan.Text = dt.Rows[0]["FirmaAdi"].ToString();
            textBox2isyeriAdres.Text = dt.Rows[0]["IsyeriAdresi"].ToString();
            textBox7telefon.Text = dt.Rows[0]["Telefon"].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Firma_Listesi firmlist = new Firma_Listesi();
            firmlist.firmaID = 0;
            firmlist.ShowDialog();
            //MessageBox.Show(firmlist.firmaID.ToString());
            if (firmlist.firmaID > 0) firmaBilgisiGetir(firmlist.firmaID);
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }
    }
}
