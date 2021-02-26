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
    public partial class UrunRaporları : Form
    {
        public UrunRaporları()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable();
        private void button2_Click(object sender, EventArgs e)
        {
            Form1 anasayfa = new Form1();
            SqlConnection baglan = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter("select i.irsID, irsOlusturmaTarih, irsEvrakNo, irsFirma, FirmaAdi, " +
                "pcID, UrunAd, case when irstip=0 then 'Giriş' when irstip=1 then 'Çıkış' when irstip=2 then 'Arızalı Giriş' " +
                "when irstip=3 then 'Arızalı Çıkış' end irsTip, " +
                "case when adet=-1 then 1 else 1 end adet " +
                "from irsaliyeHareket ih  " +
                "inner join irsaliye i on i.irsID=ih.irsID " +
                "inner join FirmName on irsFirma=ID  " +
                "inner join Urunler u on u.prID=ih.pcID  " +
                "Where ih.seriNo=@serino  " +
                "Order by irsOlusturmaTarih, irsID ", baglan);

              da.SelectCommand.Parameters.AddWithValue("@serino", textBox3.Text);
             dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 anasayfa = new Form1();
            SqlConnection baglan = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter("select i.irsID, irsOlusturmaTarih, irsEvrakNo, irsFirma, FirmaAdi, " +
                "pcID, UrunAd, case when irstip=0 then 'Giriş' when irstip=1 then 'Çıkış' when irstip=2 then 'Arızalı Giriş' " +
                "when irstip=3 then 'Arızalı Çıkış' end irsTip, " +
                "case when adet=-1 then 1 else 1 end adet " +
                "from irsaliyeHareket ih  " +
                "inner join irsaliye i on i.irsID=ih.irsID " +
                "inner join FirmName on irsFirma=ID  " +
                "inner join Urunler u on u.prID=ih.pcID  " +
                "Where UrunAd=@ad  " +
                "Order by irsOlusturmaTarih, irsID ", baglan);

            da.SelectCommand.Parameters.AddWithValue("@ad", textBox1.Text);
             dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 anasayfa = new Form1();
            SqlConnection baglan = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter("Select * from Urunler",baglan);
             dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 anasayfa = new Form1();
            SqlConnection baglan = anasayfa.aaa();

            SqlDataAdapter da = new SqlDataAdapter("Select pcID , UrunAd, sum(abs(adet)) from Urunler " +
                "inner join irsaliyeHareket on pcID=PrID " +
                "Group By pcID, UrunAd " +
                "Having UrunAd=@ad",baglan);
            da.SelectCommand.Parameters.AddWithValue("@ad",textBox2.Text);
             dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }
    }
}
