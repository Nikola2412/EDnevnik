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
    public partial class Form5 : Form
    {
        public SQLiteConnection conn, conn2;
        Form1 f1;
        
        public Form5()
        {
            InitializeComponent();
        }
        public int index;
        public string ime, username, password, odredi_pol;
        public bool pol;
        int x, y;
        double  n = 0;
        int m;
        List<Button> bu = new List<Button>();
        int strana = 40;
        int border = 20;
        int rw = 5;
        public int petice = 0, cetvorke = 0, trojke = 0, dvojke = 0, jedinice = 0;
        string legenda = "Ocene";
        List<string> opis = new List<string>();

        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {
            f1.Show();
        }

        int a = 100, b = 120;
        public void postavke()
        {
            label2.Text = "Prosek: ";
        }
        private void Form5_Load(object sender, EventArgs e)
        {
            //default podesavanja
            postavke();
            f1 = (Form1)Application.OpenForms[0];
            x = border;
            
            label1.Text = ime;
            conn = f1.conn2;

            chart1.Series.Add(legenda);
            //koliko ocena u jednom redu
            kalkulacija();
            //ucitavanje iz baze
            Ispis();
            //grafs ide ispos ispisa da se sve ocene prov prebroje
            chart();
            //Prosek ucenika
            Prosek();

            //odredjivanje pola za sliku
            if (pol)
                odredi_pol = "musko.jpg";
            else
                odredi_pol = "zensko.jpg";
            pictureBox1.Image = new Bitmap(Image.FromFile(odredi_pol),new Size(a,b));
            //postavljanje dugmica i dodavanje eventa
            foreach (var item in bu)
            {
                this.Controls.Add(item);
                item.Click += new EventHandler(klik);
            }
        }
        private void klik(object sender, EventArgs e)
        {
            int index_ocene = int.Parse(((Button)sender).Name);
            //opis ocene 
            MessageBox.Show(opis[index_ocene]);
        }
        public void kalkulacija()
        {
            m = (this.Width - border - chart1.Width - border) / (strana + rw);
            y = pictureBox1.Location.Y + pictureBox1.Height + border;
            
        }
        public void Ispis()
        {
            conn.Open();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = $"SELECT * FROM Ocena where ID_ucenika = {index}";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                
                int ocena = sqlite_datareader.GetInt16(2);
                bu.Add(new Button
                {
                    Name = n.ToString(),
                    Size = new Size(strana, strana),
                    Location = new Point(x, y),
                    Text = ocena.ToString()
                }) ;
                opis.Add(sqlite_datareader.GetString(4));
                n++;
                //broji se za graf
                if (ocena == 5)
                    petice++;
                else if (ocena == 4)
                    cetvorke++;
                else if (ocena == 3)
                    trojke++;
                else if (ocena == 2)
                    dvojke++;
                else
                    jedinice++;

                x += strana + rw;
                if(n%m==0)
                {
                    y += strana + rw;
                    x = border;
                }
            }
            conn.Close();
        }
        public void chart()
        {
            if (petice != 0)
                chart1.Series[legenda].Points.AddXY("5", petice);
            if (cetvorke != 0)
                chart1.Series[legenda].Points.AddXY("4", cetvorke);
            if (trojke != 0)
                chart1.Series[legenda].Points.AddXY("3", trojke);
            if (dvojke != 0)
                chart1.Series[legenda].Points.AddXY("2", dvojke);
            if (jedinice != 0)
                chart1.Series[legenda].Points.AddXY("1", jedinice);
        }
        public void Prosek()
        {
            //prosek
            double d = (5*petice+ 4*cetvorke+3*trojke+2*dvojke+jedinice)/ n;

            label2.Text += d.ToString("0.00");
        }
    }
}
