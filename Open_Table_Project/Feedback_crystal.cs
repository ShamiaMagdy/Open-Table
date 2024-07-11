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
    
    public partial class Feedback_crystal : Form
    {
        CrystalReport2 cr2;
        public string rest_name;
        Manager_Login_frm mngr;
        public Feedback_crystal()
        {
            InitializeComponent();
        }

        private void Feedback_crystal_Load(object sender, EventArgs e)
        {
            cr2 = new CrystalReport2();
            
        }

        private void Feedback_crystal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cr2.SetParameterValue(0, rest_name);
            crystalReportViewer1.ReportSource = cr2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Manager_frm frm = new Manager_frm(mngr);
            frm.ShowDialog();
        }
    }
}
