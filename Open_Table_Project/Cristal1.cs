using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.Shared;

namespace Open_Table_Project
{
    public partial class Cristal1 : Form
    {
        CrystalReport1 cr;
        public Cristal1()
        {
            InitializeComponent();
        }

        private void Cristal1_Load(object sender, EventArgs e)
        {
            cr = new CrystalReport1();
            foreach (ParameterDiscreteValue v in cr.ParameterFields[0].DefaultValues)
                comboBox1.Items.Add(v.Value);
        }

        private void Cristal1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cr.SetParameterValue(0, comboBox1.Text);
            crystalReportViewer1.ReportSource = cr;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Form1 frm = new Form1();
            frm.ShowDialog();
        }
    }
}
