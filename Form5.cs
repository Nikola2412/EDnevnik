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
        private void Form4_Load(object sender, EventArgs e)
        {
            f4 = (Form4)Application.OpenForms[3];
        }
        int ocena;
        private void button1_Click(object sender, EventArgs e)
        {

            //detektuje koje dugme je pritinusto
            ocena = int.Parse(((Button)sender).Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //salje ocenu i opis ocene
            if (textBox1.Text != "")
            {
                f4.UPISI_U_BAAZU(ocena, textBox1.Text);
                f4.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Nesto niste izabrali");
            }
        }
    }
}
