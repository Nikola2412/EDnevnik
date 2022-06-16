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
    public partial class Form8 : Form
    {
        public SQLiteConnection conn;
        public Form8()
        {
            InitializeComponent();
        }
        public string[] s = { "ucenika", "nastavnika","predmet","odeljenje"};
        public string[] pol = {"zenski","muski"};
        public string[] imena = {"Marko", "Milos", "Matija", "Stefan","Vlada" };
        public string[] prezimena = { "Markovic", "Rukavina", "Zdravkovic", "Lazic", "Bogdanov" };
        //public string[] predmeti = {"Informatika","Biologija", "Likovno","Muzicko"};
        //List<Tuple<int, string>> id_odeljenje = new List<Tuple<int, string>>();
        List<int> id_o = new List<int>();
        Form1 f1;
        private void Form8_Load(object sender, EventArgs e)
        {
            foreach (var item in s)
            {
                comboBox1.Items.Add(item);
            }
            foreach (var item in pol)
            {
                comboBox3.Items.Add(item);
            }
            f1 = (Form1)Application.OpenForms[0];
            UCENIK.Hide();
            random();
        }
        public void ucitaj_razrede()
        {
            conn = f1.conn2;
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            conn.Open();
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = $"SELECT razred FROM Odeljenje group by razred";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                comboBox4.Items.Add(sqlite_datareader.GetInt16(0));
            }
            conn.Close();
        }
        public void ucitaj_odeljenja(int razred)
        {
            conn = f1.conn2;
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            conn.Open();
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = $"SELECT * FROM Odeljenje where razred = {razred}";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while(sqlite_datareader.Read())
            {
                int id = sqlite_datareader.GetInt16(0);
                int naziv = sqlite_datareader.GetInt16(2);
                string ime = $"{razred}-{naziv}";
                //id_odeljenje.Add(Tuple.Create(id, ime));
                id_o.Add(id);
                comboBox2.Items.Add(ime);
            }
            conn.Close();
        }
        //ispistuje ucenike/odeljenja i povezuje random
        public void random()
        {
            //Random r = new Random();
            //conn = f1.conn2;
            //SQLiteCommand sqlite_cmd;
            //conn.Open();
            //List<string>postoji = new List<string>();
            //for (int i = 0; i < imena.Length; i++)
            //{
            //    for (int j = 0; j < prezimena.Length; j++)
            //    {
            //        string ucenik, username="", password="";
            //        int index = r.Next(1, 3);
            //        int pol = r.Next(0, 2);
            //        string ime = imena[r.Next(imena.Length)];
            //        string prezime = prezimena[r.Next(prezimena.Length)];
            //        ucenik = ime + " " + prezime;

            //        if (postoji != null && !postoji.Contains(ime + " " + prezime))
            //            username = ime + " " + prezime;
            //        else if (!postoji.Contains(prezime + " " + ime))
            //            username = prezime + " " + ime;
            //        else
            //            continue;
            //        password = "123";
            //        postoji.Add(username);
            //        sqlite_cmd = new SQLiteCommand(String.Format("insert into Ucenik(Ucenik,ID_odeljenja,username,password,pol) values('{0}',{1},'{2}','{3}','{4}');",
            //            ucenik, index, username, password, pol), conn);
            //        sqlite_cmd.ExecuteNonQuery();
            //    }

            //}

            //for (int j = 1; j < 11; j++)
            //{
            //    if (j != 9 || j != 10)
            //    {
            //        sqlite_cmd = new SQLiteCommand(String.Format("insert into Odeljenje(Razred,naziv) values('{0}',{1});",
            //        3, j), conn);
            //        sqlite_cmd.ExecuteNonQuery();
            //    }
            //}
            //for (int i = 0; i < 15; i++)
            //{
            //    foreach (var item in predmeti)
            //    {
            //        sqlite_cmd = new SQLiteCommand(String.Format("insert into Odeljenje_nastavnik(id_odeljenja,Id_nastavnika) values('{0}',{1});",
            //        r.Next(1,41), r.Next(1,3)), conn);
            //        sqlite_cmd.ExecuteNonQuery();
            //    }
            //}
            //sqlite_cmd = new SQLiteCommand(String.Format($"Delete from ocena where id_ucenika>6"), conn);
            //       sqlite_cmd.ExecuteNonQuery();


            //conn.Close();
        }
        public void add_ucenik()
        {
            UCENIK.Show();
            ucitaj_razrede();
            comboBox2.Enabled = false;
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Text = "";
            int razred = int.Parse(comboBox4.Text);
            ucitaj_odeljenja(razred);
            
            comboBox2.Enabled = true;
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox2.SelectedIndex;
            //MessageBox.Show(id_o[index].ToString());
        }
        public void add_nastavnik()
        {
        }
        public void add_predmet()
        {

        }
        public void add_odeljenje()
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (index == 0)
                add_ucenik();
            else if (index == 1)
                add_nastavnik();
            else if (index == 2)
                add_predmet();
            else
                add_odeljenje();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string ucenik, username, password;
            int index = id_o[comboBox2.SelectedIndex];
            int pol = comboBox3.SelectedIndex;
            ucenik = textBox1.Text + " " + textBox2.Text;
            username = textBox4.Text;
            password = textBox5.Text;
            conn = f1.conn2;
            SQLiteCommand sqlite_cmd;
            conn.Open();
            sqlite_cmd = new SQLiteCommand(String.Format("insert into Ucenik(Ucenik,ID_odeljenja,username,password,pol) values('{0}',{1},'{2}','{3}','{4}');", 
                ucenik,index,username,password,pol), conn);
            sqlite_cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void Form8_FormClosing(object sender, FormClosingEventArgs e)
        {
            f1.Show();
        }
    }
}
