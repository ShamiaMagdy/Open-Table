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
using System.Text.RegularExpressions;
namespace Open_Table_Project
{
    public partial class Register_frm : Form
    {
        string ordb = "Data Source=orcl;User id=scott;Password=tiger;";
        OracleConnection conn;
        public Register_frm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //select using procedure
            conn = new OracleConnection(ordb);
            conn.Open();
            string pattern = @"@gmail.com";
            Match result = Regex.Match(textBox3.Text, pattern);//check e_mail
            if (result.Success)
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "get_max_id";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("new_id", OracleDbType.Int32, ParameterDirection.Output);
                cmd.ExecuteNonQuery();
                int max_id = Convert.ToInt32(cmd.Parameters["new_id"].Value.ToString());
                int new_id = max_id + 1;
                //---------------------------------save customer data---------------------------------------
                //insert using prosedure
                OracleCommand cmd1 = new OracleCommand();
                cmd1.Connection = conn;
                cmd1.CommandText = "register_proc";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add("id", new_id.ToString());
                cmd1.Parameters.Add("fn", textBox1.Text);
                cmd1.Parameters.Add("ln", textBox2.Text);
                cmd1.Parameters.Add("email", textBox3.Text);
                cmd1.Parameters.Add("pass", textBox4.Text);
                cmd1.Parameters.Add("tel", Convert.ToInt32(textBox5.Text));
                int r = cmd1.ExecuteNonQuery();
                if (r != -1)
                {
                    restaurant_frm frm = new restaurant_frm();
                    frm.ShowDialog();
                    this.Hide();
                }
            }
            else
                MessageBox.Show("The E_Mail Must be (abc12@gmial.com)");

            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //--------------------------------check e_mail--------------------------------------------
            //select without using where condition
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd2 = new OracleCommand();
            cmd2.Connection = conn;
            cmd2.CommandText = "select CUST_E_MAIL from customer";
            cmd2.CommandType = CommandType.Text;
            OracleDataReader dr = cmd2.ExecuteReader();
            List<string> email_lst = new List<string>();
            while (dr.Read())
            {
                email_lst.Add(dr[0].ToString());
            }
            for (int i = 0; i < email_lst.Count; i++)
            {
                if (email_lst[i] == textBox3.Text)
                {
                    MessageBox.Show("Enetr another e_mail^_^");
                    textBox3.Text = "";
                    break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            this.Visible=false;
            frm.ShowDialog();
            
        }

        private void Register_frm_Load(object sender, EventArgs e)
        {

        }

        private void Register_frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
