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
    public partial class Customer_crystal : Form
    {
        CrystalReport3 cr3;
        public string rest_N;
        Manager_Login_frm mngr;
        public Customer_crystal()
        {
            InitializeComponent();
        }

        private void Customer_crystal_Load(object sender, EventArgs e)
        {
            cr3 = new CrystalReport3();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cr3.SetParameterValue(0, rest_N);
            cr3.SetParameterValue(1, textBox1.Text);
            crystalReportViewer1.ReportSource = cr3;
        }

        private void Customer_crystal_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        private void Customer_crystal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Manager_frm frm = new Manager_frm(mngr);
            frm.ShowDialog();
        }
    }
}
