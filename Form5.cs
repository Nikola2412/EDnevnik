using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dnevnik_2._0
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        Form4 f4;
        List<Button> b = new List<Button>();
        public bool izmeni = false;
        private void Form4_Load(object sender, EventArgs e)
        {
            f4 = (Form4)Application.OpenForms[3];
            b.Add(button1);
            b.Add(button2);
            b.Add(button3);
            b.Add(button4);
            b.Add(button5);
            if (izmeni)
            {
                izmena();
            }
        }
        public int ocena;
        public string opis;
        private void button1_Click(object sender, EventArgs e)
        {

            //detektuje koje dugme je pritinusto
            ocena = int.Parse(((Button)sender).Text);
            foreach (var item in b)
            {
                item.BackColor = DefaultBackColor;
                item.ForeColor = DefaultForeColor;
            }
            ((Button)sender).BackColor = Color.Gray;
            ((Button)sender).ForeColor = Color.Black;
        }
        public void izmena()
        {
            textBox1.Text = opis;
            //MessageBox.Show(ocena.ToString());
            b[ocena-1].BackColor = Color.Gray;
            b[ocena-1].ForeColor = Color.Black;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            //salje ocenu i opis ocene
            if (textBox1.Text != "")
            {
                if (!izmeni)
                {
                    f4.UPISI_U_BAAZU(ocena, textBox1.Text);
                    f4.Show();
                }
                else
                {
                    Form7 f7 = (Form7)Application.OpenForms["Form7"];
                    f7.potvrdi(ocena, textBox1.Text);
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Nesto niste izabrali");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
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
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        public void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
    }
}
