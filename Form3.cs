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
        public Form3()
        {
            InitializeComponent();
        }
        public SQLiteConnection conn;
        public int index;
        int y;
        int x;
        int rw = 10;
        Form2 f2;
        Form4 f4;
        string ime;
        int a, b;
        List<Button> buttons=new List<Button>();
        int strana = 40;
        List<int>o=new List<int>();
        int border = 20; 

        private void Form3_Load(object sender, EventArgs e)
        {
            f2 = (Form2)Application.OpenForms[1];
            x_y();
            a = f2.a;
            b= f2.b;
            pictureBox1.Size = new Size(a, b);
            CITAJ();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            f4 = new Form4();
            f4.Show();
            this.Hide();
        }
        public void UPISI_U_BAAZU(int ocena)
        {
            SQLiteCommand cmd;
            conn = f2.conn2;

            try
            {
                conn.Open();
                //MessageBox.Show("Konskcija je: " + conn.State.ToString());
                int i = f2.u[index].id;
                cmd = new SQLiteCommand(String.Format("insert into Ocena(ID_ucenika,Ocena) values('{0}',{1});", i, ocena), conn);
                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                conn.Close();
                f2.u[index].ocene.Add(ocena);
                CITAJ();
                if (ocena == 5)
                    f2.petice++;
                else if (ocena == 4)
                    f2.cetvorke++;
                else if (ocena == 3)
                    f2.trojke++;
                else if (ocena == 2)
                    f2.dvojke++;
                else
                    f2.jedinice++;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            index++;
            CITAJ();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            index--;
            CITAJ();
        }

        public void CITAJ()
        {
            brisi();

            if (index == f2.u.Count() - 1)
                button2.Enabled = false;
            if (index == 0)
                button1.Enabled = false;
            if(index !=0)
                button1.Enabled = true;
            if (index != f2.u.Count() - 1)
                button2.Enabled = true;
            
            pictureBox1.Image = f2.p[index].Image;
            o = f2.u[index].ocene;
            ime = f2.u[index].UCENIK;
            label1.Text = ime;

            kalkulacija();
        }
        public void kalkulacija()
        {
            double m = (2*ClientRectangle.Width/3 - strana) / (strana + rw);

            postavi(Math.Floor(m));
        }
        public void postavi(double m)
        {
            //MessageBox.Show(m.ToString());

            int n = 1;
            foreach (var item in o)
            {
                buttons.Add(new Button
                {
                    Name = n.ToString(),
                    Size = new Size(strana, strana),
                    Text = item.ToString(),
                    Location = new Point(x, y)

                });
                x += strana + rw;
                if(n%m==0)
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
        public void x_y()
        {
            x = ClientRectangle.Width / 3;
            y = border + strana/2;
        }
        public void brisi()
        {
            x_y();
            foreach (var item in buttons)
            {
                this.Controls.Remove(item);
            }
            buttons.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            f2.Show();
            f2.Update_pita();
            this.Close();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            f2.Show();
        }

        private void klik(object sender, EventArgs e)
        {
            MessageBox.Show(((Button)sender).Location.ToString());
        }
    }
}
