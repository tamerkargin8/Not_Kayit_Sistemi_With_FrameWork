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

namespace Not_Kayit_Sistemi_FrameWork
{
    public partial class FrmOgretmenDetay : Form
    {

        int durum;

        public FrmOgretmenDetay()
        {
            InitializeComponent();
        }

        SqlConnection connection = new SqlConnection(@"Data Source=TAMER\SQLEXPRESS;Initial Catalog=DbNoteKayit;Integrated Security=True");

        private void FrmOgretmenDetay_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dbNoteKayitDataSet.TBLDERS' table. You can move, or remove it, as needed.
            connection.Open();
            string query = "SELECT COUNT(*) as toplamOgrenciSayisi,"+
                           "SUM(CASE WHEN durum = 'FALSE' THEN 1 ELSE 0 END) AS kalanOgrenciSayisi " +
                           "FROM TBLDERS";
            SqlCommand command1 = new SqlCommand(query, connection);
            using (SqlDataReader reader = command1.ExecuteReader())
            {
                if (reader.Read())
                {
                    int toplamOgrenciSayisi = Convert.ToInt32(reader["toplamOgrenciSayisi"]);
                    int kalanOgrenciSayisi = Convert.ToInt32(reader["kalanOgrenciSayisi"]);
                    LblGecenSayisi.Text = (toplamOgrenciSayisi-kalanOgrenciSayisi).ToString();
                    LblKalanSayisi.Text = kalanOgrenciSayisi.ToString();
                }
            }
            connection.Close();
            this.tBLDERSTableAdapter.Fill(this.dbNoteKayitDataSet.TBLDERS);
        }

            private void button1_Click(object sender, EventArgs e)
            {
                connection.Open();
                SqlCommand command = new SqlCommand("insert into TBLDERS (OGRNUMARA, OGRAD, OGRSOYAD) values (@P1,@P2,@P3)", connection);
                command.Parameters.AddWithValue("@P1", MskNumara.Text);
                command.Parameters.AddWithValue("@P2", TxtAd.Text);
                command.Parameters.AddWithValue("@P3", TxtSoyad.Text);
                command.ExecuteNonQuery(); // Sorguyu çalıştır
                connection.Close();
                MessageBox.Show("Öğrenci Sisteme Eklendi");
                this.tBLDERSTableAdapter.Fill(this.dbNoteKayitDataSet.TBLDERS); //Otomatik liste doldurma komutu
            }

            private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                int choosen = dataGridView1.SelectedCells[0].RowIndex;

                MskNumara.Text = dataGridView1.Rows[choosen].Cells[1].Value.ToString();
                TxtAd.Text = dataGridView1.Rows[choosen].Cells[2].Value.ToString();
                TxtSoyad.Text = dataGridView1.Rows[choosen].Cells[3].Value.ToString();

                TxtSinav1.Text = dataGridView1.Rows[choosen].Cells[4].Value.ToString();
                TxtSinav2.Text = dataGridView1.Rows[choosen].Cells[5].Value.ToString();
                TxtSinav3.Text = dataGridView1.Rows[choosen].Cells[6].Value.ToString();
            }

            private void btnGuncelle_Click(object sender, EventArgs e)
            {
                double avarage, s1, s2, s3;
                s1 = Convert.ToDouble(TxtSinav1.Text);
                s2 = Convert.ToDouble(TxtSinav2.Text);
                s3 = Convert.ToDouble(TxtSinav3.Text);

                avarage = (s1 + s2 + s3) / 3;
                LblOrtalama.Text = avarage.ToString();

                if (avarage >= 50)
                {
                    durum = 1;
                }
                else
                {
                    durum = 0;
                }


                connection.Open();
                SqlCommand command = new SqlCommand("update TBLDERS set OGRSINAV1=@P1,OGRSINAV2=@P2,OGRSINAV3=@P3, ORTALAMA=@P4,DURUM=@P5 WHERE OGRNUMARA=@P6", connection);
                command.Parameters.AddWithValue("@P1", TxtSinav1.Text);
                command.Parameters.AddWithValue("@P2", TxtSinav2.Text);
                command.Parameters.AddWithValue("@P3", TxtSinav3.Text);
                command.Parameters.AddWithValue("@P4", decimal.Parse(LblOrtalama.Text));
                command.Parameters.AddWithValue("@P5", durum);
                command.Parameters.AddWithValue("@P6", MskNumara.Text);
                MessageBox.Show("Öğrenci Notları Güncellendi");
                command.ExecuteNonQuery();
                connection.Close();
                this.tBLDERSTableAdapter.Fill(this.dbNoteKayitDataSet.TBLDERS);
            }
        }
    }