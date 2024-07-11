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
    public partial class restaurant_frm : Form
    {
        string ordb = "Data Source=orcl;User id=scott;Password=tiger;";
        OracleConnection conn;
        public restaurant_frm()
        {
            InitializeComponent();
        }

        private void restaurant_frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void restaurant_frm_Load(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select AREA_NAME from AREA ";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                comboBox4.Items.Add(dr[0]);
            }
        }

        private void restaurant_frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Restaurants res = new Restaurants();
            res.time = comboBox1.SelectedItem.ToString();
            res.num_of_people = comboBox2.SelectedItem.ToString();
            //res.date = Convert.ToDateTime(dateTimePicker1.Text);
            label5.Text = dateTimePicker1.Text;
            res.date = label5.Text;
            res.city = comboBox4.SelectedItem.ToString();
            res.ShowDialog();
        }
    }
}
