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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        Form3 f3;
        private void Form4_Load(object sender, EventArgs e)
        {
            f3 = (Form3)Application.OpenForms[2];
        }
        int ocena;
        private void button1_Click(object sender, EventArgs e)
        {
            ocena = int.Parse(((Button)sender).Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                f3.UPISI_U_BAAZU(ocena, textBox1.Text);
                f3.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Nesto niste izabrali");
            }
        }
    }
}
