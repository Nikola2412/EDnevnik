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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        public SQLiteConnection conn,conn2;
        public int index;
        int y;
        int x;
        int rw = 10;
        Form3 f3;
        Form2 f2;
        string ime;
        int a, b;
        List<Button> buttons = new List<Button>();
        int strana = 40;
        List<Tuple<int,int>> o = new List<Tuple<int,int>>();
        int border = 20;
        public int ind;//index odeljenja
        public int id_predmeta;
        public int id_poslednje_ocene;

        private void Form3_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(f2.o[ind].u.Count().ToString());
            //MessageBox.Show(DateTime.Now.ToString("yyyy-M-d"));
            f3 = (Form3)Application.OpenForms[2];
            f2 = (Form2)Application.OpenForms[1];
            conn = f3.conn2;
            conn2 = f3.conn2;
            najveci_id_ocene();
            x_y();
            a = f3.a;
            b = f3.b;
            pictureBox1.Size = new Size(a, b);
            CITAJ();
        }
        public void najveci_id_ocene()
        {
            SQLiteCommand cmd=new SQLiteCommand(String.Format("Select max(id_ocene) from Ocena"),conn);
            conn.Open();
            id_poslednje_ocene = Convert.ToInt32(cmd.ExecuteScalar())+1;
            conn.Close();
            //MessageBox.Show(id_poslednje_ocene.ToString());
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.ShowDialog();
            //this.Hide();
        }
        public void UPISI_U_BAAZU(int ocena, string opis)
        {
            SQLiteCommand cmd;

            try
            {
                conn.Open();
                //MessageBox.Show("Konskcija je: " + conn.State.ToString());
                int i = f2.o[ind].u[index].id;
                cmd = new SQLiteCommand(String.Format("insert into Ocena(ID_ucenika,id_predmeta,Ocena,Datum_unosa,Opis) values('{0}',{1},'{2}','{3}','{4}');", i,id_predmeta, ocena, DateTime.Now.ToString("yyyy-M-d"), opis), conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                //kad sve zavrsi
                //zatvara konekciju
                conn.Close();
                //dodaje u klacu tog ucenika
                f2.o[ind].u[index].id_Ocena_opis.Add(Tuple.Create(id_poslednje_ocene,ocena, opis));
                //ponovo ucitava
                CITAJ();
                if (ocena == 5)
                    f3.petice++;
                else if (ocena == 4)
                    f3.cetvorke++;
                else if (ocena == 3)
                    f3.trojke++;
                else if (ocena == 2)
                    f3.dvojke++;
                else
                    f3.jedinice++;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            o.Clear();
            index++;
            CITAJ();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            o.Clear();
            index--;
            CITAJ();
        }

        public void CITAJ()
        {

            //restartuje
            brisi();


            //prvobitni ili sledeci uvenik
            if (index == f2.o[ind].u.Count() - 1)
                button2.Enabled = false;
            if (index == 0)
                button1.Enabled = false;
            if (index != 0)
                button1.Enabled = true;
            if (index != f2.o[ind].u.Count() - 1)
                button2.Enabled = true;

            pictureBox1.Image = f3.p[index].Image;
            //dajaje u listu ocena
            foreach (Tuple<int,int, string> ocena in f2.o[ind].u[index].id_Ocena_opis)
            {
                o.Add(Tuple.Create(ocena.Item1,ocena.Item2));
            }
            ime = f2.o[ind].u[index].UCENIK;
            label1.Text = ime;


            kalkulacija();
        }
        //koliko ocena dda postavi u jednom redu
        public void kalkulacija()
        {
            double m = (2 * ClientRectangle.Width / 3 - strana) / (strana + rw);

            postavi(Math.Floor(m));
        }
        public void postavi(double m)
        {
            //MessageBox.Show(m.ToString());

            int n = 1;
            //unosenje i postavnjanje
            foreach (var item in o)
            {
                buttons.Add(new Button {
                    Name = n.ToString(),
                    Size = new Size(strana, strana),
                    Text = item.Item2.ToString(),
                    Location = new Point(x, y)

                });
                x += strana + rw;
                if (n % m == 0)
                {
                    x = ClientRectangle.Width / 3;
                    y += strana + rw;
                }
                n++;
            }
            foreach (var item in buttons)
            {
                item.Click += new EventHandler(klik);
                this.Controls.Add(item);
            }
        }
        //Default x y
        public void x_y()
        {
            x = ClientRectangle.Width / 3;
            y = border + strana / 2;
        }
        //Restart koda
        public void brisi()
        {
            x_y();
            foreach (var item in buttons)
            {
                this.Controls.Remove(item);
            }
            o.Clear();
            buttons.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            f3.Show();
            this.Close();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            f3.Show();
        }
        public void brisanje()
        {
            f2.o[ind].u[index].id_Ocena_opis.RemoveAt(i);
            CITAJ();
        }
        public void update()
        {
            f2.o[ind].u.Clear();
            f3.ucitaj();
            CITAJ();
            //f2.o[ind].u[index].id_Ocena_opis[i].Item3 = s;
        }
        int i;
        private void klik(object sender, EventArgs e)
        {
            //izvlaci opis
            i = int.Parse(((Button)sender).Name)-1;
            Form7 f7 = new Form7();
            f7.id_ocene = o[i].Item1;
            f7.ocena = o[i].Item2;
           
            f7.ShowDialog();
            //MessageBox.Show(o[i].Item1.ToString() + " " + o[i].Item2.ToString());
            //MessageBox.Show(f2.o[ind].u[index].id_Ocena_opis[i - 1].Item3);
        }
    }
}
