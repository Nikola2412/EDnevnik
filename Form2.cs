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
    public partial class Form2 : Form
    {
        public SQLiteConnection conn,conn2;
        public Form2()
        {
            InitializeComponent();
        }
        public int id_nastavnika;
        public int id_predmeta;
        public string username;
        Form1 f1;
        Form3 f3;

        public List<Odeljenja> o = new List<Odeljenja>();
        public List<PictureBox> p = new List<PictureBox>();
        public List<Label> l = new List<Label>();


        int border = 10;
        int rw, rh, x, y;
        public int a, b;

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

            label1.Text = username;

            velicina_forme();

            Kalkulacije_broja_ucenika();

            sakupi_odeljenja();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int k = comboBox1.SelectedIndex;
            string od_raz = comboBox1.Text;
            //MessageBox.Show(od_raz);
            foreach (var item in l)
            {
                if (item.Text == od_raz)
                    forma3(int.Parse(item.Name),od_raz);
            }
        }

        public void Postavi()
        {
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
        public void kordinate()
        {
            dx();

            y = border;
        }
        public void Kalkulacije_broja_ucenika()
        {
            kordinate();
            rw = 15; rh = 40;
            a = 100; b = 120;
            double d = (this.Width - x - a) / (a + rw);
            nw = Math.Floor(d);
            if (nw < 1)
                nw = 1;
            

        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            Kalkulacije_broja_ucenika();

            ponovna_raspodela_lab();
            ponovna_raspodela_pic();
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
                x = Math.Max(comboBox1.Width + comboBox1.Location.X, label1.Width + label1.Location.X) + 20;
            }
        }
        //test
        //setting u buducnosti
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"{x}-{nw}-{this.Size}");
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            f1.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int id = int.Parse(((PictureBox)sender).Name);
            forma3(id, l[id-1].Text);
        }
        public void sakupi_odeljenja()
        {
            conn.Open();

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;

            SQLiteDataReader sqlite_datareader2;
            SQLiteCommand sqlite_cmd2;

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = $"SELECT * FROM Odeljenje_nastavnik where Id_nastavnika = {id_nastavnika}";

            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
            {
                int index = sqlite_datareader.GetInt16(0);

                conn2.Open();

                sqlite_cmd2 = conn2.CreateCommand();
                sqlite_cmd2.CommandText = $"SELECT * FROM Odeljenje Where Id_odeljenja = {index}";
                sqlite_datareader2 = sqlite_cmd2.ExecuteReader();

                int n = 1;

                while (sqlite_datareader2.Read())
                {
                    int id = sqlite_datareader2.GetInt16(0);
                    int raz = sqlite_datareader2.GetInt16(1);
                    int ime = sqlite_datareader2.GetInt16(2);
                    o.Add(new Odeljenja(id));

                    comboBox1.Items.Add(raz_ime(raz, ime));
                    Raspodela(id, raz_ime(raz,ime));

                    x += a + rw;
                    n++;

                    //kad ce da prelomi u novi red
                    if (n % nw == 0)
                    {
                        y += b + rh;
                        dx();
                    }
                }
                conn2.Close();
            }
            conn.Close();

            Postavi();

        }
        public void dx()
        {
            x = Math.Max(comboBox1.Width + comboBox1.Location.X, label1.Width + label1.Location.X) + 20;
        }
        public void Raspodela(int id,string odeljenje)
        {
            //dadavanje pictueboxa i labla
            p.Add(new PictureBox {
                Name = id.ToString(),
                Size = new Size(a, b),
                Location = new Point(x, y),
                Image = new Bitmap(Image.FromFile("nesto.jpg"), new Size(a, b)),
                BorderStyle = BorderStyle.Fixed3D
            });
            l.Add(new Label {
                Name = id.ToString(),
                Size = new Size(a, rh * 3 / 4),
                Location = new Point(x, y + b),
                Text = odeljenje,
                TextAlign = ContentAlignment.MiddleCenter,
            });

        }
        public void forma3(int id,string ime)
        {
            f3 = new Form3();
            //MessageBox.Show(o[0].id_odeljenja.ToString());
            f3.Text = ime;
            int n = 0;
            foreach (var item in o)
            {
                if (item.id_odeljenja == id)
                {
                    f3.id_odeljenja = item.id_odeljenja;
                    f3.id_predmeta = id_predmeta;
                    f3.ind = n;
                    f3.Show();
                    this.Hide();
                }
                n++;
            }
            
;       }
        
        public string raz_ime(int a,int b)
        {
            return $"{a}-{b}";
        }
    }
}
