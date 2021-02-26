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
    public partial class ürünEkle : Form
    {
        public ürünEkle()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 ansayfa = new Form1();
            SqlConnection baglan = ansayfa.aaa();

            int stkID;
            SqlCommand command = new SqlCommand("Insert into Urunler(UrunAd,UrunGrubu,Barkod,Barkod2,seriNo) values (@ad,@grup,@brkod,@brkod2,@seri) " + 
            "Select Scope_identity()", baglan);
            command.Parameters.AddWithValue("@ad",textBox3.Text);
            command.Parameters.AddWithValue("@grup", comboBox1.SelectedValue);
            command.Parameters.AddWithValue("@brkod", textBox2.Text);
            command.Parameters.AddWithValue("@brkod2", textBox4.Text);
            command.Parameters.AddWithValue("@seri",checkBox1.Checked);
            try
           {
                stkID = int.Parse(command.ExecuteScalar().ToString());
                label6.Text = stkID.ToString();
                MessageBox.Show("Ekleme Başarılı!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Bu ürün grubu zaten oluşturulmuş!!!");
                MessageBox.Show(ex.Message);
            }

           
        }

        private void ürünEkle_Load(object sender, EventArgs e)
        {
            Form1 anasayfa = new Form1();
            SqlConnection baglanti = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter("Select * From UrunGrup",baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "GrupAd";
            comboBox1.ValueMember = "GrupID";
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Urunler u = new Urunler();
            u.urunID = 0;
            u.ShowDialog();
            //MessageBox.Show(firmlist.firmaID.ToString());
            if (u.urunID > 0) urunGetir(u.urunID);


           
        }
        
        private void temizle()
        {
            textBox3.Clear();
            comboBox1.Text = "";
            textBox2.Clear();
            textBox4.Clear();
            checkBox1.Checked = false;
        }

        private void urunGetir(int id)
        {
            temizle();

            Form1 anasayfa = new Form1();
            SqlConnection baglan = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter("select * from Urunler Where PrID=@prid", baglan);
            da.SelectCommand.Parameters.AddWithValue("@prid", id);
            DataTable dt = new DataTable();
            da.Fill(dt);

            //Nesneleri dolduralım
            textBox3.Text = dt.Rows[0]["UrunAd"].ToString();
            comboBox1.SelectedValue = dt.Rows[0]["UrunGrubu"].ToString();
            textBox2.Text = dt.Rows[0]["Barkod"].ToString();
            textBox4.Text = dt.Rows[0]["Barkod2"].ToString();
            checkBox1.Checked = Convert.ToBoolean(  dt.Rows[0]["seriNo"]);
            label6.Text = dt.Rows[0]["PrID"].ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void label7_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 anasayfa = new Form1();
            SqlConnection baglan = anasayfa.aaa();

            SqlCommand command = new SqlCommand("Insert into UrunGrup (GrupAd) values (@grupad) ", baglan);
            command.Parameters.AddWithValue("@grupad", textBox1.Text);
            if (textBox1.Text != "")
            {
                command.ExecuteNonQuery();
                ürünEkle üE = new ürünEkle();
                this.Hide();
                üE.Show();
            }

            else MessageBox.Show("Ekleme alanı boş bırakılamaz");
            
        }
    }
}
