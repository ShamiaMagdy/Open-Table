using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;

namespace Open_Table_Project
{
    public partial class feedback_frm : Form
    {
        string ordb = "Data Source=orcl;User id=scott;Password=tiger;";
        OracleConnection conn;
        int cust_id;
        public int area_id1;
        int rest_id;
        int feedback_num;
        public feedback_frm()
        {
            InitializeComponent();
        }

        private void feedback_frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void feedback_frm_Load(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            //////////////////////Show all feedbacks/////////////////////////////////////////////////
            string cmd = "select * from feedback";
            OracleDataAdapter adapter = new OracleDataAdapter(cmd, ordb);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            /////////////////show all areas/////////////////////////////////////////////////////////
            OracleCommand cmd5 = new OracleCommand();
            cmd5.Connection = conn;
            cmd5.CommandText = "select AREA_NAME from AREA ";
            cmd5.CommandType = CommandType.Text;
            OracleDataReader dr5 = cmd5.ExecuteReader();
            while (dr5.Read())
            {
                comboBox2.Items.Add(dr5[0]);
            }
            dr5.Close();
           /* ////////////////get area id/////////////////////////////////////////////////////////////
            OracleCommand cmd4 = new OracleCommand();
            cmd4.Connection = conn;
            cmd4.CommandText = "select AREA_ID from AREA where AREA_NAME=:area";
            cmd4.CommandType = CommandType.Text;
            cmd4.Parameters.Add("area", comboBox2.SelectedItem.ToString());
            OracleDataReader dr4 = cmd4.ExecuteReader();
            while (dr4.Read())
            {
                area_id1 = Convert.ToInt32(dr4[0].ToString());
            }
            dr4.Close();
            ////////////////////get all restaurant//////////////////////////////////////////////////      
            OracleCommand cmd1 = new OracleCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = "select  REST_NAME from restaurant r,LOCATE_AT la where r.REST_ID=la.REST_ID and la.AREA_ID=:a_id ";
            cmd1.CommandType = CommandType.Text;
            cmd1.Parameters.Add("a_id", area_id1);
            OracleDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr[0].ToString());
            }
            dr.Close();
           */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
           
            //////////////////////select customer id/////////////////////////////////////////////
            //int cust_id=0;
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select CUSTOMER_ID from CUSTOMER where CUST_E_MAIL=:email";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("email", textBox2.Text);
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cust_id = Convert.ToInt32(dr[0]);
            }
            dr.Close();
            ////////////////////////get restaurant id/////////////////////////////////////////////
            OracleCommand cmd1 = new OracleCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = "select r.REST_ID from restaurant r, locate_at la where r.rest_id= la.rest_id and la.area_id=:a_id and r.rest_name=:r_name";
            cmd1.CommandType = CommandType.Text;
            cmd1.Parameters.Add("a_id", area_id1);
            cmd1.Parameters.Add("r_name", comboBox1.SelectedItem.ToString());
            OracleDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                rest_id = Convert.ToInt32(dr1[0]);
            }
            dr1.Close();
            /////////////////////get max number of feedbacks////////////////////////////////////////
            OracleCommand cmd2 = new OracleCommand();
            cmd2.Connection = conn;
            cmd2.CommandText = "select max(FEEDBACK_NUM) from feedback";
            cmd2.CommandType = CommandType.Text;
            OracleDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                feedback_num = Convert.ToInt32(dr2[0])+1;
            }
            dr2.Close();
            //////////////////////send a feedback////////////////////////////////////////////////////
            OracleCommand cmd3 = new OracleCommand();
            cmd3.Connection = conn;
            cmd3.CommandText = "insert into feedback values(:cust_id,:feed_num,:service,:cleaness,:food_quality,:rest_id)";
            cmd3.CommandType = CommandType.Text;
            cmd3.Parameters.Add("cust_id", cust_id);
            cmd3.Parameters.Add("feed_num", feedback_num);
            cmd3.Parameters.Add("service", Convert.ToInt32(textBox3.Text));
            cmd3.Parameters.Add("cleaness", Convert.ToInt32(textBox4.Text));
            cmd3.Parameters.Add("food_quality", Convert.ToInt32(textBox5.Text));
            cmd3.Parameters.Add("rest_id", rest_id);
            cmd3.ExecuteNonQuery();
            MessageBox.Show("thanks ^_^");
            ///////////////////////////update datagridview/////////////////////////////////////////////
            string cmdstr = "select * from feedback";
            OracleDataAdapter adapter = new OracleDataAdapter(cmdstr, ordb);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           /* if((Convert.ToInt32( textBox3.Text))<0 && (Convert.ToInt32(textBox3.Text)) >5)
            {
                MessageBox.Show("Please enter number between 0 and 5");
            }*/
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
           /* if ((Convert.ToInt32(textBox4.Text)) < 0 && (Convert.ToInt32(textBox4.Text)) > 5)
            {
                MessageBox.Show("Please enter number between 0 and 5");
            }*/
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
           /* if ((Convert.ToInt32(textBox5.Text)) < 0 && (Convert.ToInt32(textBox5.Text)) > 5)
            {
                MessageBox.Show("Please enter number between 0 and 5");
            }*/
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            conn = new OracleConnection(ordb);
            conn.Open();
            ////////////////get area id/////////////////////////////////////////////////////////////
            OracleCommand cmd4 = new OracleCommand();
            cmd4.Connection = conn;
            cmd4.CommandText = "select AREA_ID from AREA where AREA_NAME=:area";
            cmd4.CommandType = CommandType.Text;
            cmd4.Parameters.Add("area", comboBox2.SelectedItem.ToString());
            OracleDataReader dr4 = cmd4.ExecuteReader();
            while (dr4.Read())
            {
                area_id1 = Convert.ToInt32(dr4[0].ToString());
            }
            dr4.Close();
            ////////////////////get all restaurant in this area//////////////////////////////////////////////////      
            OracleCommand cmd1 = new OracleCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = "select  REST_NAME from restaurant r,LOCATE_AT la where r.REST_ID=la.REST_ID and la.AREA_ID=:a_id ";
            cmd1.CommandType = CommandType.Text;
            cmd1.Parameters.Add("a_id", area_id1);
            OracleDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr[0].ToString());
            }
            dr.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Restaurants res = new Restaurants();
            res.ShowDialog();
        }
    }
}
