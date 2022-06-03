using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Dnevnik_2._0
{
    public partial class Form7 : Form
    {
        public SQLiteConnection conn;
        public Form7()
        {
            InitializeComponent();
        }
        public int id_ocene;
        string opis;
        Form1 f1;
        public int ocena;
        public void ucitaj()
        {
            f1 = (Form1)Application.OpenForms[0];
            conn = new SQLiteConnection(String.Format($"Data Source={f1.baze};Version=3;"));
            
            conn.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"Select opis from ocena where id_ocene = {id_ocene}";
            SQLiteDataReader data = cmd.ExecuteReader();
            while (data.Read())
                opis = data.GetString(0);
            conn.Close();
            textBox1.Text = opis;
        }
        private void Form7_Load(object sender, EventArgs e)
        {
            button1.Text = ocena.ToString();
            ucitaj();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = $"Delete from ocena where id_ocene = {id_ocene}";
            cmd.ExecuteNonQuery();
            conn.Close();
            Form4 f4 = (Form4)Application.OpenForms[3];
            f4.brisanje();
            this.Close();
        }
        public void potvrdi(int ocena,string s)
        {
            button1.Text= ocena.ToString();
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(String.Format($"update ocena set ocena = {ocena} where id_ocene = {id_ocene}"),conn);
            //SQLiteCommand cmd2 = new SQLiteCommand(String.Format($"update ocena set opis = {s} where id_ocene = {id_ocene}"),conn);
            cmd.ExecuteNonQuery();
            //cmd2.ExecuteNonQuery();
            conn.Close();
            Form4 f4 = (Form4)Application.OpenForms[3];
            f4.update();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            //f5.izmeni(ocena, opis);
            f5.izmeni = true;
            f5.ocena = ocena;
            f5.opis = opis;
            f5.button6.Text = "Potvrdi izmenu";
            f5.ShowDialog();
        }
    }
}
