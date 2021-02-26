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
using Microsoft.VisualBasic;

namespace Stok_Takip_ve_Muhasebe_Programi
{
    public partial class Urunler : Form
    {
        public Urunler()
        {
            InitializeComponent();
        }

        private void Urunler_Load(object sender, EventArgs e)
        {
            verileriGoster("select PrID, Urunad, Barkod, isnull(GrupAd,'') GrupAd from Urunler inner join UrunGrup on GrupID = UrunGrubu");
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
        public int urunID;
        void urunSec()
        {
            int r = dataGridView1.CurrentCell.RowIndex;

            urunID = Convert.ToInt32(dataGridView1["PrID", r].Value.ToString());

            //Form1 anasayfa = new Form1();
            //SqlConnection baglan = anasayfa.aaa();

            //SqlDataAdapter da = new SqlDataAdapter("Select seriNo from Urunler where PrID =@urunid", baglan);
            //da.SelectCommand.Parameters.AddWithValue("urunid",urunID);
            //DataTable dt = new DataTable();
            //da.Fill(dt);
            //Boolean seriNoSorgu = Convert.ToBoolean(dt.Rows[0]["seriNo"]);
            //if (seriNoSorgu == true)
            //{
            //    string IsimGirisi = Interaction.InputBox("Seri No Girişi", "Seri No Giriniz.", "", 0, 0);
            //    MessageBox.Show("Girilen isim: " + IsimGirisi);
            //}

            //   urunID = Convert.ToInt32(dataGridView1["PrID", r].Value.ToString());
            Close();
        }
       


        private void button1_Click(object sender, EventArgs e)
        {

            urunSec();
        }
    }
}
