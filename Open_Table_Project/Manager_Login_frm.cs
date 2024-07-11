using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Open_Table_Project
{
    public partial class Manager_Login_frm : Form
    {
        string ordb = "Data Source=orcl;User id=scott;Password=tiger;";
        OracleConnection conn;
        public Manager_Login_frm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 frm = new Form1();
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //select using bind variable
            int count = 0;
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select count(*) from MANAGER where MANAGER_E_MAIL=:email and MANAGER_PASSWORD=:pass";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("email", textBox1.Text);
            cmd.Parameters.Add("pass", textBox2.Text);
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                count = Convert.ToInt32(dr[0].ToString());
            }
            if (count == 1)
            {
                this.Visible = false;
                Manager_frm frm = new Manager_frm(this);
                frm.email = textBox1.Text;
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please enter correct email or password");
                textBox1.Text = "";
                textBox2.Text = "";
            }

        }

        private void Manager_Login_frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
