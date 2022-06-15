using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Dnevnik_2._0
{
    public partial class Form2 : Form
    {
        public SQLiteConnection conn, conn2;
        public Form2()
        {
            InitializeComponent();
        }
        Random r = new Random();
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
        Size s;
        public void velicina_forme()
        {
            this.Size = s;
        }
        //int broj_slika;
        //string slike = "./pozadine/";
        private void Form2_Load(object sender, EventArgs e)
        {
            f1 = (Form1)Application.OpenForms[0];
            s = new Size(1280, 720);

            conn = f1.conn2;
            conn2 = f1.conn3;

            label1.Text = username;

            //if(!Directory.Exists(slike))
            //    Directory.CreateDirectory(slike);

            //broj_slika = Directory.GetFiles(slike).Length;
            //// MessageBox.Show(broj_slika.ToString());
            //if (broj_slika == 0)
            //{
            //    ProcessStartInfo p = new ProcessStartInfo(@".\Generisanje_Slika_csharp.exe");
            //    Process.Start(p);
            //    System.Threading.Thread.Sleep(3000);
            //}

            velicina_forme();
            //MessageBox.Show(this.Size.ToString());

            Kalkulacije_broja_ucenika();

            sakupi_odeljenja();
            pozicija_dugmadi();
            if (l[l.Count - 1].Location.Y + l[l.Count - 1].Height > s.Height)
            {
                this.AutoScroll = true;
            }
            moze = true;
        }
        bool moze = false;
        public void pozicija_dugmadi()
        {
            button6.Location = new Point(this.Width - 2 * button6.Width, button6.Location.Y);
            button7.Location = new Point((int)(button6.Location.X - 1.5 * button7.Width), button6.Location.Y);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int k = comboBox1.SelectedIndex;
            string od_raz = comboBox1.Text;
            //MessageBox.Show(od_raz);
            foreach (var item in l)
            {
                if (item.Text == od_raz)
                    forma3(int.Parse(item.Name), od_raz);
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

            y = border + panel1.Height;
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
            if (!moze)
                return;

            Kalkulacije_broja_ucenika();
            pozicija_dugmadi();
            ponovna_raspodela_lab();
            ponovna_raspodela_pic();
            s = this.Size;
            if (l[l.Count - 1].Location.Y + l[l.Count - 1].Height > s.Height)
            {
                this.AutoScroll = true;
            }
            else
            {
                this.AutoScroll = false;
            }
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
            MessageBox.Show($"U izradi je");
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            f1.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int id = int.Parse(((PictureBox)sender).Name);
            foreach (var item in l)
            {
                if (item.Name == id.ToString())
                    forma3(id, item.Text);
            }
        }

        public void sakupi_odeljenja()
        {
            conn.Open();

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;

            SQLiteDataReader sqlite_datareader2;
            SQLiteCommand sqlite_cmd2;

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = $"SELECT * FROM Odeljenje_nastavnik join Odeljenje using (id_odeljenja) where Id_nastavnika = {id_nastavnika} order by razred,naziv";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            int n = 1;
            while (sqlite_datareader.Read())
            {
                int index = sqlite_datareader.GetInt16(0);

                conn2.Open();

                sqlite_cmd2 = conn2.CreateCommand();
                sqlite_cmd2.CommandText = $"SELECT * FROM Odeljenje Where Id_odeljenja = {index}";
                sqlite_datareader2 = sqlite_cmd2.ExecuteReader();



                while (sqlite_datareader2.Read())
                {
                    int id = sqlite_datareader2.GetInt16(0);
                    int raz = sqlite_datareader2.GetInt16(1);
                    int ime = sqlite_datareader2.GetInt16(2);
                    o.Add(new Odeljenja(id));

                    comboBox1.Items.Add(raz_ime(raz, ime));
                    Raspodela(id, raz_ime(raz, ime));

                    x += a + rw;


                    //kad ce da prelomi u novi red

                    if (n % nw == 0)
                    {
                        y += b + rh;
                        dx();
                    }
                    n++;
                }
                conn2.Close();
            }
            conn.Close();

            Postavi();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }

        }
        bool dragging = false;
        Point dragCursorPoint;
        Point dragFormPoint;

        public void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            
            dragFormPoint = this.Location;
        }

        public void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                if (this.WindowState == FormWindowState.Maximized)
                {

                    if (dragCursorPoint.Y < e.Y + 10)
                        this.WindowState = FormWindowState.Normal;
                }
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        public void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
        bool drag2 = false;
        Point t;
        bool naMestuV = false;
        bool naMestuH = false;
        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            t = e.Location;
            if (naMestuH || naMestuV)
            {
                drag2 = true;
            }
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
            drag2 = false;
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                return;
            if ((e.X >= this.Width - 5 - SystemInformation.VerticalScrollBarWidth || e.X <= 5) && e.Y>panel1.Height)
            {
                Cursor = Cursors.VSplit;
                naMestuH = true;
                naMestuV = false;
            }
            else if (e.Y >= this.Height - 5)
            {
                Cursor = Cursors.HSplit;
                naMestuV = true;
                naMestuH = false;
            }
            else
                Cursor = Cursors.Default;
            if (!drag2) return;

            if (naMestuH)
                this.Width += e.X - t.X;
            if (naMestuV)
                this.Height += e.Y - t.Y;
            t = e.Location;

        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public void dx()
        {
            x = Math.Max(comboBox1.Width + comboBox1.Location.X, label1.Width + label1.Location.X) + 20;
        }
        public Bitmap CreateBitmapImage(string sImageText)
        {
            Bitmap objBmpImage = new Bitmap(1000, 5000);

            int intWidth = 0;
            int intHeight = 0;

            // Create the Font object for the image text drawing.
            System.Drawing.Font objFont = new System.Drawing.Font("Arial", 64, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);

            // Create a graphics object to measure the text's width and height.
            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            // This is where the bitmap size is determined.
            intWidth += (int)objGraphics.MeasureString(sImageText, objFont).Width;
            intHeight += (int)objGraphics.MeasureString(sImageText, objFont).Height;

            // Create the bmpImage again with the correct size for the text and font.
            objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));


            // Add the colors to the new bitmap.
            objGraphics = Graphics.FromImage(objBmpImage);

            // Set Background color

            objGraphics.Clear(System.Drawing.Color.White);
            objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;



            objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; //  <-- This is the correct value to use. ClearTypeGridFit is better yet!
            objGraphics.DrawString(sImageText, objFont, new SolidBrush(System.Drawing.Color.Black), 0, 0, StringFormat.GenericDefault);

            objGraphics.Flush();

            return (objBmpImage);
        }
        public void Raspodela(int id, string odeljenje)
        {
            //dadavanje pictueboxa i labla
            p.Add(new PictureBox {
                Name = id.ToString(),
                Size = new Size(a, b),
                Location = new Point(x, y),
                Image = new Bitmap(CreateBitmapImage(odeljenje), new Size(a, b)),
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
        public void forma3(int id, string ime)
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
        }

        public string raz_ime(int a, int b)
        {
            return $"{a}-{b}";
        }
    }
}
