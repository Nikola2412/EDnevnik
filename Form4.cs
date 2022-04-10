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

        private void button1_Click(object sender, EventArgs e)
        {
            int ocena = int.Parse(((Button)sender).Text);

            f3.UPISI_U_BAAZU(ocena);
            f3.Show();
            this.Close();
        }
    }
}
