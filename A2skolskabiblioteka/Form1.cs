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

namespace A2skolskabiblioteka
{
    public partial class Form1 : Form
    {
        SqlConnection konekcija = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\A2.mdf;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
        }
        int ID = 0;
        private void PrikaziULV()
        {
            listView1.Items.Clear();
            string sqlUpit = "SELECT * FROM Autor";
            SqlCommand komanda = new SqlCommand(sqlUpit, konekcija);
            SqlDataAdapter da = new SqlDataAdapter(komanda);
            try
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem listItem = new ListViewItem(row["AutorID"].ToString());
                    listItem.SubItems.Add(row["Ime"].ToString());
                    listItem.SubItems.Add(row["Prezime"].ToString());
                    var dat = DateTime.Parse(row["DatumRodjenja"].ToString());
                    listItem.SubItems.Add(dat.ToString("dd/MM/yyyy"));
                    listView1.Items.Add(listItem);
                }
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

        private void ClearData()
        {
            textBoxSifra.Text = "";
            textBoxIme.Text = "";
            textBoxPrezime.Text = "";
            textBoxRodjen.Text = "";
        }

        private void toolStripButtonAnaliza_Click(object sender, EventArgs e)
        {
            Statistika frm = new Statistika();
            frm.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PrikaziULV();
            
        }


        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            ID = Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text);
            textBoxSifra.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBoxIme.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBoxPrezime.Text = listView1.SelectedItems[0].SubItems[2].Text;
            textBoxRodjen.Text = listView1.SelectedItems[0].SubItems[3].Text;
        }

        private void toolStripButtonBrisanje_Click(object sender, EventArgs e)
        {
            if(textBoxSifra.Text != "")
            {
                try
                {
                    konekcija.Open();
                    string sqlUpit = "Delete Autor WHERE AutorID = @Id";
                    SqlCommand komanda = new SqlCommand(sqlUpit, konekcija);
                    komanda.Parameters.AddWithValue("@Id", textBoxSifra.Text);
                    komanda.ExecuteNonQuery();
                    konekcija.Close();
                    komanda.Dispose();
                    PrikaziULV();
                    ClearData();

                }
                catch(Exception)
                {
                    MessageBox.Show("Doslo je do greske !");
                }
            }
            else
            {
                MessageBox.Show("Izaberite red koji brišete");
            }
        }

        private void toolStripButtonIzlaz_Click(object sender, EventArgs e)
        {
           Application.Exit();
        }

        private void toolStripButtonOAplikaciji_Click(object sender, EventArgs e)
        {
            Uputstvo frm = new Uputstvo();
            frm.Show();
        }
    }
}
