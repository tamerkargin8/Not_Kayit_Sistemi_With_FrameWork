using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Not_Kayit_Sistemi_FrameWork
{
    public partial class FrmOgrenciDetay : Form
    {
        public FrmOgrenciDetay()
        {
            InitializeComponent();
        }
        public string numara;
        SqlConnection connection =  new SqlConnection(@"Data Source=TAMER\SQLEXPRESS;Initial Catalog=DbNoteKayit;Integrated Security=True");

        ////Data Source=TAMER\SQLEXPRESS;Initial Catalog=DbNoteKayit;Integrated Security=True
        private void FrmOgrenciDetay_Load(object sender, EventArgs e)
        {
            lblNumara.Text = numara;
            connection.Open();
            SqlCommand command = new SqlCommand("Select * From TBLDERS where OGRNUMARA=@p1", connection);
            command.Parameters.AddWithValue("@p1", numara);
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                lblAdSoyad.Text = dataReader[2].ToString() + "  " + dataReader[3].ToString(); // sql listesinde gözüken sütunlarına göre hangi sütundaysa onu yazdık
                lblSinav1.Text = dataReader[4].ToString(); 
                lblSinav2.Text = dataReader[5].ToString(); 
                lblSinav3.Text = dataReader[6].ToString(); 
                lblOrtalama.Text = dataReader[7].ToString();

                if (dataReader[8].ToString() == "True")
                {
                    lblDurum.Text = "Geçti";
                }
                else
                {
                    lblDurum.Text = "Kaldı";
                }
            }
            connection.Close();
        }
    }
}
