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
    public partial class UrunlerIrsaliye : Form
    {
        public UrunlerIrsaliye()
        {
            InitializeComponent();
        }

        private void UrunlerIrsaliye_Load(object sender, EventArgs e)
        {
            verileriGoster("select PrID, Urunad, Barkod, isnull(GrupAd,'') GrupAd from Urunler inner join UrunGrup on GrupID = UrunGrubu");
            //verileriGoster("select ih.irsID, siraNo, case when adet=-1 then 1 END AS 'adet', pcID, ih.seriNo from irsaliyeHareket ih " +
            //                "inner join irsaliye ir on ih.irsID =ir.irsID " +
            //                "where irsTip=0");
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

            //urunID = Convert.ToInt32(dataGridView1["PrID", r].Value.ToString());
            urunID = Convert.ToInt32(dataGridView1["prID", r].Value.ToString());


            Close();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            urunSec();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            urunSec();
        }
    }
}
