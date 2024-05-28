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
using System.Windows.Forms.DataVisualization.Charting;

namespace A2skolskabiblioteka
{
    public partial class Statistika : Form
    {
        SqlConnection konekcija = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\A2.mdf;Integrated Security=True");

        public Statistika()
        {
            InitializeComponent();
        }

        private void Statistika_Load(object sender, EventArgs e)
        {
            PopuniCmb();
        }

        private void PopuniCmb()
        {
            String strSQL = "Select DISTINCT AutorID, (Ime + ' ' + Prezime) AS ImePrezime FROM Autor";
            SqlCommand komanda = new SqlCommand(strSQL, konekcija);
            SqlDataAdapter da = new SqlDataAdapter(komanda);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
                comboBoxAutor.DataSource = ds.Tables[0];
                comboBoxAutor.DisplayMember = "ImePrezime";
                comboBoxAutor.ValueMember = "AutorID";
                comboBoxAutor.SelectedItem = null;
                comboBoxAutor.DropDownStyle = ComboBoxStyle.DropDownList;

            }
            catch (Exception)
            {
                MessageBox.Show("Doslo je do greske !");
            }
            finally
            {
                da.Dispose();
                komanda.Dispose();
            }  
        }

        private void btnPrikazi_Click(object sender, EventArgs e)
        {
            string sql = "SELECT YEAR(DatumUzimanja) as Godina, " +
                "COUNT(DatumUzimanja) as Broj " +
                "FROM Na_Citanju, Knjiga, Napisali " +
                "WHERE Na_Citanju.KnjigaID = Knjiga.KnjigaID " +
                "AND Knjiga.KnjigaID = Napisali.KnjigaID " +
                "AND Napisali.AutorID = @param1 " +
                "AND Year(DatumUzimanja) BETWEEN @param3 AND @param2  " +
                "GROUP BY YEAR(DatumUzimanja)";
            SqlCommand komanda = new SqlCommand(sql, konekcija);
            komanda.Parameters.AddWithValue("@param1", comboBoxAutor.SelectedValue);
            komanda.Parameters.AddWithValue("@param2", DateTime.Now.Year);
            komanda.Parameters.AddWithValue("@param3", DateTime.Now.AddYears((int)-numericUpDown1.Value).Year);
            SqlDataAdapter da = new SqlDataAdapter(komanda);
            try
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                chart1.DataSource = dt;
                chart1.Series[0].XValueMember = "Godina";
                chart1.Series[0].YValueMembers = "Broj";
                chart1.Series[0].IsValueShownAsLabel = false;
                chart1.Series[0].ChartType = SeriesChartType.Line;
                chart1.Series[0].Color = Color.Red;
                dataGridView1.DataSource = dt;

            }
            catch(Exception)
            {
                MessageBox.Show("Doslo je do greske !");
            }
            finally
            {
                da.Dispose();
                komanda.Dispose();
            }
        }

        private void btnIzadji_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
