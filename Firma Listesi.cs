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
    public partial class Firma_Listesi : Form ,Interface1
    {
        public int FrmID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int firmaID;

        // int Interface1.frmID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public int frmID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Firma_Listesi()
        {
            InitializeComponent();
        }

     //   String baglantiadresi;
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
        
        private void Firma_Listesi_Load(object sender, EventArgs e)
        {
            verileriGoster("Select * from FirmName");
            ToolTip Aciklama = new ToolTip();
            Aciklama.ToolTipTitle = "Dikkat!";
            Aciklama.ToolTipIcon = ToolTipIcon.Warning;
            Aciklama.IsBalloon = true;

            Aciklama.SetToolTip(button3, "Sadece Firma Kodu ya da Firma Adı ile arama yapılabilir!!");
            Aciklama.SetToolTip(button2, "Sadece ID Numarası ile silme işlemi gerçekleştirebilirsiniz!!");
            Aciklama.SetToolTip(button4, "Fİrma Listenizi Tümüyle Görmenizi Sağlar!!");
            Aciklama.SetToolTip(button1, "Lütfen Firma Kodu ve Firma Adını Gİriniz, Bu Alanlar Boş Kalamaz. Eğer Firma Kodunuz mevcut Değilse, Bu Alana da Firma Adı Girilmelidir!!");


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2FirmaAdi.Text != "" && textBox1FirmaKodu.Text != "")
            {
                Form1 anasayfa = new Form1();
                SqlConnection baglan = anasayfa.aaa();
               
                SqlCommand command = new SqlCommand("Insert into FirmName(FirmaKodu,FirmaAdi,AdiSoyadi_TicaretUnvani,IsyeriAdresi,Telefon) Values (@firmakodu,@firmadi,@astu,@adres,@tel)", baglan);
                command.Parameters.AddWithValue("@firmakodu", textBox1FirmaKodu.Text);
                command.Parameters.AddWithValue("@firmadi", textBox2FirmaAdi.Text);
                command.Parameters.AddWithValue("@astu", textBox1.Text);
                command.Parameters.AddWithValue("@adres", textBox2.Text);
                command.Parameters.AddWithValue("@tel", textBox7.Text);
     //           command.ExecuteNonQuery();
                
                try {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Ekleme Başarılı!");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Bu Firma Zaten Kayıtlı!!!");
                }
                
                //      baglan.Close();
                // dataGridView1.DataSource="";// var olan elemanları her defasında art arda yazmasın diye yaptım
                verileriGoster("Select * from FirmName");
                textBox1FirmaKodu.Clear();
                textBox2FirmaAdi.Clear();
               
               
            }
            else
            {
                MessageBox.Show("Lütfen Firma Adını ve Firma Kodunu Giriniz!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Lütfen Silinecek Firmanın ID Numarasını giriniz!");
            }
            else
            {

                Form1 anasayfa = new Form1();
                SqlConnection baglan = anasayfa.aaa();


                SqlCommand comment = new SqlCommand("delete from FirmName where ID=@firmID", baglan);
                
                    comment.Parameters.AddWithValue("@firmID", textBox3.Text);

                if (comment.ExecuteNonQuery() == 0)
                {
                    MessageBox.Show("Bu ID Numaralı Bir Firma Bulunamadı");
                    textBox3.Clear();
                }
                else
                {
                    verileriGoster("Select * from FirmName");
                    baglan.Close();
                    MessageBox.Show(textBox3.Text + " ID Numaralı Firma Başarıyla Silindi");
                    textBox3.Clear();

                }
  
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1FirmaKodu.Text != "" )
            {
                Form1 anasayfa = new Form1();
                SqlConnection baglan = anasayfa.aaa();
                SqlCommand komut = new SqlCommand("Select * from FirmName where FirmaKodu like '%" + textBox1FirmaKodu.Text + "%' ", baglan);
               
                SqlDataAdapter da = new SqlDataAdapter(komut);

                // DataSet ds = new DataSet();
                DataTable t = new DataTable();
                da.Fill(t);

                // dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.DataSource = t;
                baglan.Close();
                
            }
           else if (textBox2FirmaAdi.Text != "")
            {
                Form1 anasayfa = new Form1();
                SqlConnection baglan = anasayfa.aaa();
                SqlCommand komut = new SqlCommand("Select * from FirmName where FirmaAdi like '%" + textBox2FirmaAdi.Text + "%' ", baglan);
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                baglan.Close();

            }else
                MessageBox.Show("Lütfen Firma Kodunu ya da Adını Gİriniz!!!");
            

        }

        private void button4_Click(object sender, EventArgs e)
        {
            verileriGoster("Select * from FirmName");
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmSec();
        }

        void frmSec()
        {
            int r = dataGridView1.CurrentCell.RowIndex;

         //   frmID = Convert.ToInt32(dataGridView1["ID", r].Value.ToString());

            firmaID = Convert.ToInt32(dataGridView1["ID", r].Value.ToString());
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
           //int r = dataGridView1.CurrentCell.RowIndex;

           // frmID= Convert.ToInt32(dataGridView1["ID", r].Value.ToString());
           //ClassGenel cg= new ClassGenel();
           //cg.frmID  = Convert.ToInt32(dataGridView1["ID", r].Value.ToString());


           //MessageBox.Show(frmID.ToString());

            //firmaID = Convert.ToInt32(dataGridView1["ID", r].Value.ToString());
            //Close();

            frmSec();

        }
        private int frmid;
        public int frmID
        {
            get { return frmid; }
            set { frmid = Convert.ToInt32(dataGridView1["ID", dataGridView1.CurrentCell.RowIndex].Value.ToString());}
            
        }

        private void Firma_Listesi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5) frmSec();
        }
    }
}
