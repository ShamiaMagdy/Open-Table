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
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Text.RegularExpressions;
namespace Open_Table_Project
{
    public partial class restaurant_manager_frm : Form
    {
        string ordb = "Data Source=orcl;User id=scott;Password=tiger;";
        OracleConnection conn;
        Manager_Login_frm ml;
        public restaurant_manager_frm()
        {
            InitializeComponent();
        }

        private void restaurant_manager_frm_Load(object sender, EventArgs e)
        {
            //select using procedure ^_^
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select_categories";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("category", OracleDbType.RefCursor, ParameterDirection.Output);
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                checkedListBox1.Items.Add(dr[0].ToString());
            }
            dr.Close();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pattern = @"@gmail.com";
            Match result = Regex.Match(textBox3.Text, pattern);//check e_mail
            if (result.Success)
            {
                //--------get restaurant id------------------------------
                int max_id = 0, new_id = 0, max_id_mngr = 0, new_id_mngr = 0;
                conn = new OracleConnection(ordb);
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select max(REST_ID) from RESTAURANT ";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    max_id = Convert.ToInt32(dr[0].ToString());
                    new_id = max_id + 1;
                }
                //---------get manger id---------------------------------
                OracleCommand cmd4 = new OracleCommand();
                cmd4.Connection = conn;
                cmd4.CommandText = "select max(MANAGER_ID) from MANAGER ";
                cmd4.CommandType = CommandType.Text;
                OracleDataReader dr1 = cmd4.ExecuteReader();
                while (dr1.Read())
                {
                    max_id_mngr = Convert.ToInt32(dr1[0].ToString());
                    new_id_mngr = max_id_mngr + 1;
                }
                //--------get menu id------------------------------
                int max_id_menu=0,new_id_menu=0;
                conn = new OracleConnection(ordb);
                conn.Open();
                OracleCommand cmd12 = new OracleCommand();
                cmd12.Connection = conn;
                cmd12.CommandText = "select max(MENU_NUM) from MENU ";
                cmd12.CommandType = CommandType.Text;
                OracleDataReader dr8 = cmd12.ExecuteReader();
                while (dr8.Read())
                {
                    max_id_menu = Convert.ToInt32(dr8[0].ToString());
                    new_id_menu = max_id_menu + 1;
                }
                //----------------insert restaurant data without using procedure----------------------------------------
                OracleCommand cmd1 = new OracleCommand();
                cmd1.Connection = conn;
                cmd1.CommandText = "insert into  RESTAURANT values(:id,:name,:price,:menu_id)";
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.Add("id", new_id);
                cmd1.Parameters.Add("name", textBox5.Text);
                cmd1.Parameters.Add("price", comboBox1.SelectedItem.ToString());
                cmd1.Parameters.Add("menu_id", new_id_menu);
                cmd1.ExecuteNonQuery();
                //------------------insert manager data without using procedure----------------------------------------
                OracleCommand cmd2 = new OracleCommand();
                cmd2.Connection = conn;
                cmd2.CommandText = "insert into  MANAGER values(:f_name,:l_name,:email,:pass,:rest_id,:mngr_id)";
                cmd2.CommandType = CommandType.Text;
                cmd2.Parameters.Add("f_name", textBox1.Text);
                cmd2.Parameters.Add("l_name", textBox2.Text);
                cmd2.Parameters.Add("email", textBox3.Text);
                cmd2.Parameters.Add("pass", textBox4.Text);
                cmd2.Parameters.Add("rest_id", new_id);
                cmd2.Parameters.Add("mngr_id", new_id_mngr);
                cmd2.ExecuteNonQuery();
                //---------------insert menu number----------------------------------------------------------------------
                OracleCommand cmd13 = new OracleCommand();
                cmd13.Connection = conn;
                cmd13.CommandText = "insert into  MENU values(:menu_id)";
                cmd13.CommandType = CommandType.Text;
                cmd13.Parameters.Add("menu_id", new_id_menu);
                cmd13.ExecuteNonQuery();
                //------------------insert seating option-------------------------------------------------------------
                OracleCommand cmd3 = new OracleCommand();
                cmd3.Connection = conn;
                cmd3.CommandText = "insert into  REST_SEATING values(:rest_id,:seat_op)";
                cmd3.CommandType = CommandType.Text;
                if (checkedListBox2.CheckedItems.Count > 0)
                {
                    for (int i = 0; i < checkedListBox2.CheckedItems.Count; i++)
                    {
                        cmd3.Parameters.Clear();
                        cmd3.Parameters.Add("rest_id", new_id);
                        cmd3.Parameters.Add("seat_op", checkedListBox2.CheckedItems[i].ToString());
                        cmd3.ExecuteNonQuery();
                    }
                }
                else
                    MessageBox.Show("please choose seating option !!");
                //-----------------select category id------------------------------------------------------------------
                //OracleCommand cmd6 = new OracleCommand();
                //cmd6.Connection = conn;
                //cmd6.CommandText = "select CATEROGY_ID from CATEGORIES where CATEGORY_NAME=:name ";
                //cmd6.CommandType = CommandType.Text;
                List<int> cat_id = new List<int>();
                //if (checkedListBox1.CheckedItems.Count > 0)
                //{
                    for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                    {
                    // cmd6.Parameters.Clear();
                    OracleCommand cmd6 = new OracleCommand();
                    cmd6.Connection = conn;
                    cmd6.CommandText = "select CATEROGY_ID from CATEGORIES where CATEGORY_NAME=:name ";
                    cmd6.CommandType = CommandType.Text;
                    cmd6.Parameters.Add("name", checkedListBox1.CheckedItems[i].ToString());
                        OracleDataReader dr2 = cmd6.ExecuteReader();
                        while (dr2.Read())
                        {
                            cat_id.Add(Convert.ToInt32(dr2[0].ToString()));
                            //MessageBox.Show(cat_id[i].ToString());
                        }
                    }
               // }
               
                //-----------------insert cuisine----------------------------------------------------------------------
                OracleCommand cmd5 = new OracleCommand();
                cmd5.Connection = conn;
                cmd5.CommandText = "insert into  RESTAURANT_CATEGORY values(:category_id,:rest_id) ";
                cmd5.CommandType = CommandType.Text;
                for (int i = 0; i < cat_id.Count; i++)
                {
                    cmd5.Parameters.Clear();
                    cmd5.Parameters.Add("category_id", cat_id[i]);
                    cmd5.Parameters.Add("rest_id", new_id);
                    cmd5.ExecuteNonQuery();
                }
                //------------------insert into menu category------------------------------------------------------------
                OracleCommand cmd15 = new OracleCommand();
                cmd15.Connection = conn;
                cmd15.CommandText = "insert into  MENU_CATEGORY values(:MENU_NUM,:CATEGORY_ID) ";
                cmd15.CommandType = CommandType.Text;
                for (int i = 0; i < cat_id.Count; i++)
                {
                    cmd15.Parameters.Clear();
                    cmd15.Parameters.Add("MENU_NUM", new_id_menu);
                    cmd15.Parameters.Add("CATEGORY_ID", cat_id[i]);
                    cmd15.ExecuteNonQuery();
                }
                //------------------get all areas------------------------------------------------------------------------
                OracleCommand cmd7 = new OracleCommand();
                cmd7.Connection = conn;
                cmd7.CommandText = "select AREA_NAME from AREA";
                cmd7.CommandType = CommandType.Text;
                OracleDataReader dr4 = cmd7.ExecuteReader();
                List<string> area_list = new List<string>();
                while (dr4.Read())
                {
                    area_list.Add(dr4[0].ToString());
                }
                bool flag = false;
                for (int i = 0; i < area_list.Count; i++)
                {
                    if (area_list[i] == textBox6.Text)
                    {
                        OracleCommand cmd8 = new OracleCommand();
                        cmd8.Connection = conn;
                        int area_id = 0;
                        cmd8.CommandText = "select AREA_ID from AREA where AREA_NAME=:area";
                        cmd8.CommandType = CommandType.Text;
                        cmd8.Parameters.Add("area", textBox6.Text);
                        OracleDataReader dr5 = cmd8.ExecuteReader();
                        while (dr5.Read())
                        {
                            area_id = Convert.ToInt32(dr5[0].ToString());
                        }
                        //------------------insert area--------------------------------------------------------------
                        OracleCommand cmd9 = new OracleCommand();
                        cmd9.Connection = conn;
                        cmd9.CommandText = "insert into  LOCATE_AT values(:rest_id,:area_id)";
                        cmd9.CommandType = CommandType.Text;
                        cmd9.Parameters.Add("rest_id", new_id);
                        cmd9.Parameters.Add("area_id", area_id);
                        cmd9.ExecuteNonQuery();
                        flag = true;
                    }
                    else
                        continue;
                }
                int max_id_area = 0, new_id_area = 0;
                if (flag == false)
                {//--------------------get area id ----------------------------------------------------------
                    OracleCommand cmd11 = new OracleCommand();
                    cmd11.Connection = conn;
                    cmd11.CommandText = "select max(AREA_ID) from AREA ";
                    cmd11.CommandType = CommandType.Text;
                    OracleDataReader dr6 = cmd11.ExecuteReader();
                    while (dr6.Read())
                    {
                        max_id_area = Convert.ToInt32(dr6[0].ToString());
                        new_id_area = max_id_area + 1;
                    }
                    //-------------------------insert new area ------------------------------------------------
                    OracleCommand cmd8 = new OracleCommand();
                    cmd8.Connection = conn;
                    cmd8.CommandText = "insert into AREA values(:AREA_ID,:name) ";
                    cmd8.CommandType = CommandType.Text;
                    cmd8.Parameters.Add("AREA_ID", new_id_area);
                    cmd8.Parameters.Add("name", textBox6.Text);
                    cmd8.ExecuteNonQuery();
                    //------------------insert area--------------------------------------------------------------
                    OracleCommand cmd9 = new OracleCommand();
                    cmd9.Connection = conn;
                    cmd9.CommandText = "insert into  LOCATE_AT values(:rest_id,:area_id)";
                    cmd9.CommandType = CommandType.Text;
                    cmd9.Parameters.Add("rest_id", new_id);
                    cmd9.Parameters.Add("area_id", new_id_area);
                    cmd9.ExecuteNonQuery();
                }
                this.Visible = false;
                Manager_frm frm = new Manager_frm(ml);
                frm.email = textBox3.Text;
                frm.ShowDialog();
            }
            else
                MessageBox.Show("The E_Mail Must be (abc12@gmial.com)");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 frm = new Form1();
            frm.ShowDialog();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd2 = new OracleCommand();
            cmd2.Connection = conn;
            cmd2.CommandText = "select MANAGER_E_MAIL from MANAGER";
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

        private void restaurant_manager_frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
