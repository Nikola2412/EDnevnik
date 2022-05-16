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
    public partial class Form3 : Form
    {

        public SQLiteConnection conn,conn2;
        public Form3()
        {
            InitializeComponent();
        }

        double nw;
        int rw, rh, x, y;
        public int a, b;
        int n = 0;
        int border=10;

        Form2 f2;
        public int id_odeljenja;
        public int ind;

        public List<PictureBox> p = new List<PictureBox>();
        public List<Label> l = new List<Label>();
       

        private void Form3_Load(object sender, EventArgs e)
        {
            
            f2 = (Form2)Application.OpenForms[1];
            this.label1.Text = f2.label1.Text;
            conn = f2.conn;
            conn2 = f2.conn2;

            Kalkulacije_broja_ucenika();

            ucitaj();

            foreach (var pic in p)
            {
                pic.Click += new EventHandler(pictureBox1_Click);
                this.Controls.Add(pic);
            }
            //postavlja label sa imenom ucenika
            foreach (var label in l)
            {
                this.Controls.Add(label);
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        public void kordinate()
        {
            x = Math.Max(button1.Width + button1.Location.X, label1.Width + label1.Location.X) + 20; ;

            y = border;
        }
        public void Kalkulacije_broja_ucenika()
        {
            int k = 0;
            kordinate();
            if (this.VerticalScroll.Visible == true)
            {
                k = System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
            }
            rw = 15; rh = 40;
            a = 100; b = 120;
            double d = (this.Width - x - a - k) / (a + rw);
            nw = Math.Floor(d);
            if (nw < 1)
                nw = 1;
            

        }

        public void ucitaj()
        {
            conn.Open();

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader2;
            SQLiteCommand sqlite_cmd2;

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = $"SELECT * FROM Ucenik where ID_odeljenja = {id_odeljenja}";


            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                //tuple ocena,opis ocene
                List<Tuple<int, string>> o = new List<Tuple<int, string>>();


                int index = sqlite_datareader.GetInt16(0);
                string uc = sqlite_datareader.GetString(1);
                conn2.Open();
                //konekcija za tabelu ocene

                //komanda za citanje
                sqlite_cmd2 = conn2.CreateCommand();
                sqlite_cmd2.CommandText = $"SELECT * FROM Ocena Where ID_ucenika = {index}";

                sqlite_datareader2 = sqlite_cmd2.ExecuteReader();
                int srednja = 0;

                //cita ocene i broj da li su 5, 4, 3, 2 ili 1 da bi se uradila pita 
                while (sqlite_datareader2.Read())
                {
                    int ocena = sqlite_datareader2.GetInt16(2);
                    string opis = sqlite_datareader2.GetString(4);
                    o.Add(Tuple.Create(ocena, opis));
                    srednja += ocena;
                    //if (ocena == 5)
                    //    petice++;
                    //else if (ocena == 4)
                    //    cetvorke++;
                    //else if (ocena == 3)
                    //    trojke++;
                    //else if (ocena == 2)
                    //    dvojke++;
                    //else
                    //    jedinice++;
                    //izdvaja mesec iz datuma
                    //int dt = int.Parse(sqlite_datareader2.GetString(3).Split('-')[1]);
                    //dataGridView1.Rows[n].Cells[dt].Value += ocena.ToString()+" ";
                    //dataGridView1.Rows[n].Cells[dt].Value += ocena.ToString() + " ";
                }
                //raspodela ucenika po ekranu
                Raspodela(uc, sqlite_datareader.GetBoolean(5));
                conn2.Close();

                //dodaje ucenika u klasu

                f2.o[ind].u.Add(new ucenik(uc, o, srednja, sqlite_datareader.GetInt16(0), sqlite_datareader.GetBoolean(5)));

                //dataGridView1.Rows[n].Cells["prosek"].Value = u[n].srednja;
                //pomera u desno
                x += a + rw;
                n++;

                //kad ce da prelomi u novi red
                if (n % nw == 0)
                {
                    y += b + rh;
                    x = 100;
                }

            }
            conn.Close();
        }
        public void Raspodela(string uc, bool pol)
        {
            //rasporedjuje ucenike

            string odredi_pol;
            //pos
            if (pol)
                odredi_pol = "musko.jpg";
            else
                odredi_pol = "zensko.jpg";


            //dadavanje pictueboxa i labla
            p.Add(new PictureBox {
                Name = n.ToString(),
                Size = new Size(a, b),
                Location = new Point(x, y),
                Image = new Bitmap(Image.FromFile(odredi_pol), new Size(a, b)),
                BorderStyle = BorderStyle.Fixed3D
            });
            l.Add(new Label {
                Name = n.ToString(),
                Size = new Size(a, rh * 3 / 4),
                Location = new Point(x, y + b),
                Text = uc,
                TextAlign = ContentAlignment.MiddleCenter,
            });


        }
    }
}
