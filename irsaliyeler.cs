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
    public partial class irsaliyeler : Form
    {
        public irsaliyeler()
        {
            InitializeComponent();
        }

        public int irsTip;
        public int irsBilgi;

        private void irsaliyeler_Load(object sender, EventArgs e)
        {
            Form1 anasayfa = new Form1();
            SqlConnection con = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter("select irsID, irsOlusturmaTarih, irsEvrakNo, " +
            "case when irstip=0 then 'Giriş İrsaliyesi' when irstip=1 then 'Çıkış İrsaliyesi' else '' end irsTip, " +
            "irsFirma, AdiSoyadi_TicaretUnvani FirmaUnvan,  " +
            "case when irsDurum=0 then 'Geçerli' else 'İptal' end Durum " +
            " from irsaliye  " +
            "left join FirmName on ID=irsFirma  " +
            "Where irstip=@tip", con);
            da.SelectCommand.Parameters.AddWithValue("@tip", irsTip);
            DataTable dTable = new DataTable();
            da.Fill(dTable);
            dataGridView1.DataSource = dTable;
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            irsSec();
        }

        //public int irsid;
        void irsSec()
        {
            int r = dataGridView1.CurrentCell.RowIndex;

            //   frmID = Convert.ToInt32(dataGridView1["ID", r].Value.ToString());

            irsBilgi = Convert.ToInt32(dataGridView1["irsID", r].Value.ToString());
            Close();
        }
    }
}
