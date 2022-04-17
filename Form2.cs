using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace Dnevnik_2._0
{
    public partial class Form2 : Form
    {
        public SQLiteConnection conn, conn2;
        Form1 f1;
        public Form2()
        {
            InitializeComponent();
        }
        public int id_nastavnika;
        public string username, password;
        public string ucenik, ocene;
        public Size s;

        public List<ucenik> u = new List<ucenik>();
        public List<PictureBox> p = new List<PictureBox>();
        public List<Label> l = new List<Label>();

        public int petice = 0, cetvorke = 0, trojke = 0, dvojke = 0, jedinice = 0;

        int n = 0;

        int border = 10;
        int rw, rh, x, y;
        public int a, b;

        private void button1_Click(object sender, EventArgs e)
        {
            if (chart1.Visible)
                chart1.Hide();
            else
                chart1.Show();
        }

        double nw;
        public void velicina_forme()
        {
            this.Size = new Size(1280, 720);
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            f1 = (Form1)Application.OpenForms[0];

            conn = f1.conn2;
            conn2 = f1.conn3;



            velicina_forme();

            Kalkulacije_broja_ucenika();

            UCITAJ();

            foreach (var pic in p)
            {
                pic.Click += new EventHandler(pictureBox1_Click);
                this.Controls.Add(pic);
            }
            foreach (var label in l)
            {
                this.Controls.Add(label);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Visible)
            {
                dataGridView1.Hide();
                velicina_forme();
            }
            else
            {
                dataGridView1.Show();
                this.Width = dataGridView1.Width;
            }
        }

        public void UCITAJ()
        {

            conn.Open();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader2;
            SQLiteCommand sqlite_cmd2;

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = $"SELECT * FROM Ucenik where ID_nastavnika = {id_nastavnika}";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {

                List<int> o = new List<int>();
                

                int index = sqlite_datareader.GetInt16(0);
                string uc = sqlite_datareader.GetString(1);
                conn2.Open();

                sqlite_cmd2 = conn2.CreateCommand();
                sqlite_cmd2.CommandText = $"SELECT * FROM Ocena Where ID_ucenika = {index}";

                sqlite_datareader2 = sqlite_cmd2.ExecuteReader();
                int srednja = 0;
                dataGridView1.Rows.Add(uc);
                while (sqlite_datareader2.Read())
                {
                    int ocena = sqlite_datareader2.GetInt16(2);
                    o.Add(ocena);
                    srednja += ocena;
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
                    int dt = int.Parse(sqlite_datareader2.GetString(3).Split('-')[1]);
                    dataGridView1.Rows[n].Cells[dt].Value += ocena.ToString()+" ";
                }
                
                Raspodela(uc, sqlite_datareader.GetBoolean(5));
                conn2.Close();
                u.Add(new ucenik(uc, o, srednja, sqlite_datareader.GetInt16(0), sqlite_datareader.GetBoolean(5)));
                x += a + rw;
                n++;
                if (n % nw == 0)
                {
                    y += b + rh;
                    x = 100;
                }
                
            }
            conn.Close();
            pita();
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            Kalkulacije_broja_ucenika();

            ponovna_raspodela_lab();
            ponovna_raspodela_pic();
            
        }
            

        public void Update_pita()
        {
            chart1.Series.Clear();
            chart1.Series.Add("s1");
            chart1.Series["s1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            pita();
        }
        public void pita()
        {
            if (petice != 0)
                chart1.Series["s1"].Points.AddXY("5", petice);
            if (cetvorke != 0)
                chart1.Series["s1"].Points.AddXY("4", cetvorke);
            if (trojke != 0)
                chart1.Series["s1"].Points.AddXY("3", trojke);
            if (dvojke != 0)
                chart1.Series["s1"].Points.AddXY("2", dvojke);
            if (jedinice != 0)
                chart1.Series["s1"].Points.AddXY("1", jedinice);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            f1.Show();
        }

        public void Raspodela(string uc, bool pol)
        {
            string odredi_pol;
            if (pol)
                odredi_pol = "musko.jpg";
            else
                odredi_pol = "zensko.jpg";

            p.Add(new PictureBox
            {
                Name = n.ToString(),
                Size = new Size(a, b),
                Location = new Point(x, y),
                Image = new Bitmap(Image.FromFile(odredi_pol), new Size(a, b)),
                BorderStyle = BorderStyle.Fixed3D
            });
            l.Add(new Label
            {
                Name = n.ToString(),
                Size = new Size(a, rh * 3 / 4),
                Location = new Point(x, y + b),
                Text = uc,
                TextAlign = ContentAlignment.MiddleCenter,
            });


        }
        public void ponovna_raspodela_pic()
        {
            kordinate();
            int i = 0;
            foreach (var pic in p)
            {
                i++;
                pic.Location = new Point(x, y);

                Povecaj_x_y(i);

            }
        }
        public void ponovna_raspodela_lab()
        {
            kordinate();
            int i = 0;
            foreach (var lab in l)
            {
                i++;
                lab.Location = new Point(x, y + b);
                Povecaj_x_y(i);

            }
        }

        public void Povecaj_x_y(int i)
        {
            x += a + rw;
            if (i % nw == 0)
            {
                y += b + rh;
                x = 100;
            }
        }
        public void kordinate()
        {
            x = 100;

            y = border;
        }
        public void Kalkulacije_broja_ucenika()
        {
            int k = 0;

            if (this.VerticalScroll.Visible == true)
            {
                k = System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
            }
            rw = 15; rh = 40;
            a = 100; b = 120;
            double d = (this.Width - border * 2 - a - k) / (a + rw);
            nw = Math.Floor(d);
            if (nw < 1)
                nw = 1;
            kordinate();

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.index = int.Parse(((PictureBox)sender).Name);
            f3.conn = conn2;
            f3.Show();
            this.Hide();

        }
    }
}
