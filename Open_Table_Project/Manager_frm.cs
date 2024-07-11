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
    public partial class Manager_frm : Form
    {
        Manager_Login_frm manager;
        string ordb = "Data Source=orcl;User id=scott;Password=tiger;";
        OracleConnection conn;
        //OracleDataAdapter adapter;
        //OracleCommandBuilder builder;
        //DataSet ds;
        int rest_id = 0;
        int cat_id = 0;
        int menu_num = 0;
        public string email;
        string r_name;
        OracleDataAdapter adapter;
        OracleCommandBuilder builder;
        DataSet ds;
        public Manager_frm(Manager_Login_frm M_frm)
        {
            InitializeComponent();
            this.manager = M_frm;
        }

        private void Manager_frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Manager_frm_Load(object sender, EventArgs e)
        {
            //****************************get restaurant id**************************************
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select RESTUARANT_ID from MANAGER where MANAGER_E_MAIL=:email";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("email", email);
            OracleDataReader dr= cmd.ExecuteReader();
            while(dr.Read())
            {
                rest_id = Convert.ToInt32(dr[0]);
            }
            dr.Close();
            //MessageBox.Show(rest_id.ToString());
            //*****************************get all times*******************************************
            OracleCommand cmd1 = new OracleCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = "select AVILABLE_TIME from REST_AVILABLE_TIME where REST_ID=:id";
            cmd1.CommandType = CommandType.Text;
            cmd1.Parameters.Add("id", rest_id);
            OracleDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                checkedListBox1.Items.Add(dr1[0].ToString());
            }
            dr1.Close();
            //*****************************get menu number*******************************************
            
            OracleCommand cmd3 = new OracleCommand();
            cmd3.Connection = conn;
            cmd3.CommandText = "select MENU_NUM from RESTAURANT where REST_ID=:id";
            cmd3.CommandType = CommandType.Text;
            cmd3.Parameters.Add("id", rest_id);
            OracleDataReader dr3 = cmd3.ExecuteReader();
            while (dr3.Read())
            {
                menu_num = Convert.ToInt32(dr3[0]);
            }
            dr1.Close();
            //MessageBox.Show(menu_num.ToString());
            //**************************show menu***************************************************
            OracleCommand cmd2 = new OracleCommand();
            cmd2.Connection = conn;
            cmd2.CommandText = @"select CATEGORY_NAME,fi.FOOD_NAME,PRICE_S,PRICE_M,PRICE_L
                                 from CATEGORIES cat,FOOD_ITEM fi, menu_category mc , menu_items mi
                                 where mc.menu_num=:menu_n
                                 and mi.MENU_NUM=:menu_n
                                 and cat.caterogy_id= fi.CATEGORYID
                                 and mc.category_id= fi.CATEGORYID
                                 and fi.food_name= mi.food_name";
            cmd2.CommandType = CommandType.Text;
            cmd2.Parameters.Add("menu_n", menu_num);
            OracleDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                dataGridView2.Rows.Add(dr2[0], dr2[1], dr2[2], dr2[3], dr2[4]);
            }
            dr2.Close();
            //********************************show all categories*************************************
            OracleCommand cmd4 = new OracleCommand();
            cmd4.Connection = conn;
            cmd4.CommandText = "select_categories";
            cmd4.CommandType = CommandType.StoredProcedure;
            cmd4.Parameters.Add("category", OracleDbType.RefCursor, ParameterDirection.Output);
            OracleDataReader dr4 = cmd4.ExecuteReader();
            while (dr4.Read())
            {
                checkedListBox2.Items.Add(dr4[0].ToString());
            }
            dr4.Close();
            //********************************get all tables*************************************
            /* OracleCommand cmd5 = new OracleCommand();
             cmd5.Connection = conn;
             cmd5.CommandText = "select TABLE_NUM,NUM_CHAIRS,AVAILABILITY from TABLES where REST_ID=:r_id";
             cmd5.CommandType = CommandType.Text;
             cmd5.Parameters.Add("r_id",rest_id);
             OracleDataReader dr5 = cmd5.ExecuteReader();
             while (dr5.Read())
             {
                 // comboBox1.Items.Add(dr5[0].ToString());
                 dataGridView1.Rows.Add(dr5[0], dr5[1], dr5[2]);
             }
             dr5.Close();*/
            //*************************get all tables***********************************************

            string cmdstr = "select * from tables where rest_id=:id";
            adapter = new OracleDataAdapter(cmdstr, ordb);
            adapter.SelectCommand.Parameters.Add("id", rest_id);
            ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manager_Login_frm frm = new Manager_Login_frm();
            frm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int n = dataGridView2.SelectedRows.Count;
            string food_Name = "";
            if (n == 1)
            {
                conn = new OracleConnection(ordb);
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "update FOOD_ITEM set PRICE_S=:P_S, PRICE_M=:P_M, PRICE_L=:P_L where FOOD_NAME=:F_N";
                cmd.CommandType = CommandType.Text;
                for (int i = 0; i < n; i++)
                {
                    food_Name = dataGridView2.SelectedRows[i].Cells[1].Value.ToString();
                    cmd.Parameters.Add("P_S", dataGridView2.SelectedRows[i].Cells[2].Value);
                    cmd.Parameters.Add("P_M", dataGridView2.SelectedRows[i].Cells[3].Value);
                    cmd.Parameters.Add("P_L", dataGridView2.SelectedRows[i].Cells[4].Value);
                    cmd.Parameters.Add("F_N", dataGridView2.SelectedRows[i].Cells[1].Value);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Updated Successfully ^_^ ");
                }
            }
            else
                MessageBox.Show("Plaese select one row ");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //----------------------insert time-----------------------------------------------------
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "insert into REST_AVILABLE_TIME values(:R_id,:time)";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("R_id", rest_id);
            cmd.Parameters.Add("time", textBox1.Text);
            cmd.ExecuteNonQuery();
            checkedListBox1.Items.Add(textBox1.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //------------------------delete time----------------------------------------------------------
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "delete from REST_AVILABLE_TIME where REST_ID=:R_id and AVILABLE_TIME=:time";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("R_id", rest_id);
            cmd.Parameters.Add("time", checkedListBox1.SelectedItem);
            cmd.ExecuteNonQuery();
            checkedListBox1.Items.Remove(checkedListBox1.SelectedItem);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //------------------------select category id in specific menu----------------------------------
            List<int> cat_id_Menu = new List<int>();
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select CATEGORY_ID from MENU_CATEGORY where MENU_NUM=:Num";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("Num",menu_num );
            OracleDataReader dr= cmd.ExecuteReader();
            while(dr.Read())
            {
                cat_id_Menu.Add(Convert.ToInt32(dr[0].ToString()));
            }
            //MessageBox.Show(cat_id.ToString());
            //------------------------insert new fooditem ------------------------------------------------
            int n = dataGridView2.SelectedRows.Count;
            string food_Name="";
            if(n==1)
            {
                OracleCommand cmd1 = new OracleCommand();
                cmd1.Connection = conn;
                cmd1.CommandText = "insert into FOOD_ITEM values(:Food_name,:s,:m,:l,:ctgry)";
                cmd1.CommandType = CommandType.Text;
                for(int i=0;i< n;i++)
                {
                    food_Name = dataGridView2.SelectedRows[i].Cells[1].Value.ToString();
                    cmd1.Parameters.Add("Food_name", dataGridView2.SelectedRows[i].Cells[1].Value);
                    cmd1.Parameters.Add("s", dataGridView2.SelectedRows[i].Cells[2].Value);
                    cmd1.Parameters.Add("m", dataGridView2.SelectedRows[i].Cells[3].Value);
                    cmd1.Parameters.Add("l", dataGridView2.SelectedRows[i].Cells[4].Value);
                    cmd1.Parameters.Add("ctgry", cat_id);
                    cmd1.ExecuteNonQuery();
                }
                
                MessageBox.Show("Added Successfully ^_^");
            }
            else
                MessageBox.Show("Plaese select one row ");
            //-----------------------------insert into menu_item---------------------------------------
            OracleCommand cmd2 = new OracleCommand();
            cmd2.Connection = conn;
            cmd2.CommandText = "insert into MENU_ITEMS values(:menu_N,:Food_name)";
            cmd2.CommandType = CommandType.Text;
            cmd2.Parameters.Add("menu_N", menu_num);
            cmd2.Parameters.Add("Food_name", food_Name);
            cmd2.ExecuteNonQuery();
            //------------------------------------------------------------------------------------------
            OracleCommand cmd3 = new OracleCommand();
            cmd3.Connection = conn;
            bool flag = false;
            for(int i=0;i<cat_id_Menu.Count;i++)
            {
                if(cat_id_Menu[i]==cat_id)
                {
                    flag = true;
                }    
            }
            if (flag==false)
            {
                cmd3.CommandText = "insert into MENU_ITEMS values(:menu_N,:Cat_id)";
                cmd3.CommandType = CommandType.Text;
                cmd3.Parameters.Add("menu_N", menu_num);
                cmd3.Parameters.Add("Cat_id", cat_id);
                cmd3.ExecuteNonQuery();
            }
        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select CATEROGY_ID from CATEGORIES where CATEGORY_NAME=:name";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("name", checkedListBox2.SelectedItem);
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cat_id = Convert.ToInt32(dr[0].ToString());
            }
            //MessageBox.Show(cat_id.ToString());
            dataGridView2.Rows.Add(checkedListBox2.SelectedItem);
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int max_id_table = 0, new_id_table = 0;
            //***********************select table id***************************************
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select max(TABLE_NUM) from TABLES ";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr= cmd.ExecuteReader();
            while (dr.Read())
            {
                max_id_table = Convert.ToInt32(dr[0].ToString());
                new_id_table = max_id_table + 1;
            }
            //********************insert new table*****************************************
           /* OracleCommand cmd1 = new OracleCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = "insert into TABLES values(:T_NUM,:CHAIRS,1,:R_ID)";
            cmd1.CommandType = CommandType.Text;
            cmd1.Parameters.Add("T_NUM", new_id_table);
            cmd1.Parameters.Add("CHAIRS", textBox2.Text);
            cmd1.Parameters.Add("R_ID", rest_id);
            cmd1.ExecuteNonQuery();
            //comboBox1.Items.Add(textBox2.Text);
            dataGridView1.Rows.Add(new_id_table.ToString(), textBox2.Text, 1);*/
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int n = dataGridView2.SelectedRows.Count;
            string food_Name = "";
            if (n == 1)
            {
                conn = new OracleConnection(ordb);
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "delete from FOOD_ITEM where FOOD_NAME=:F_N";
                cmd.CommandType = CommandType.Text;
                for (int i = 0; i < n; i++)
                {
                    food_Name = dataGridView2.SelectedRows[i].Cells[1].Value.ToString();
                    cmd.Parameters.Add("F_N", dataGridView2.SelectedRows[i].Cells[1].Value);
                    cmd.ExecuteNonQuery();
                    dataGridView2.Rows.Remove(dataGridView2.SelectedRows[i]);
                    MessageBox.Show("Deleted Successfully ^_^ ");
                }
                OracleCommand cmd1 = new OracleCommand();
                cmd1.Connection = conn;
                cmd1.CommandText = "delete from MENU_ITEMS where FOOD_NAME=:F_N and MENU_NUM=:M_N";
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.Add("F_N", food_Name);
                cmd1.Parameters.Add("M_N", menu_num);
                cmd1.ExecuteNonQuery();
                
            }
            else
                MessageBox.Show("Plaese select one row ");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            /*int n = dataGridView1.SelectedRows.Count;
            if (n == 1)
            {
                conn = new OracleConnection(ordb);
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "update_table_Availability";
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < n; i++)
                {
                    cmd.Parameters.Add("table_num", dataGridView1.SelectedRows[i].Cells[0].Value);
                    cmd.Parameters.Add("avail", dataGridView1.SelectedRows[i].Cells[2].Value);
                    cmd.ExecuteNonQuery();
                }
            }
            else
                MessageBox.Show("Plaese select one row ");*/
            //******************save changes*****************************************************
            builder = new OracleCommandBuilder(adapter);
            adapter.Update(ds.Tables[0]);

        }

        private void showFeedbacksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ////////////////////////send the name of estaurant to the crystalreport2/////////////////////////////
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select REST_NAME from restaurant where rest_id=:id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("id", rest_id);
            OracleDataReader dr= cmd.ExecuteReader();
            while(dr.Read())
            {
                r_name = dr[0].ToString();
            }
            dr.Close();
            this.Visible = false;
            Feedback_crystal fc = new Feedback_crystal();
            fc.rest_name = r_name;
            fc.ShowDialog();
        }

        private void showAllCustomersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select REST_NAME from restaurant where rest_id=:id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("id", rest_id);
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                r_name = dr[0].ToString();
            }
            dr.Close();
            this.Visible = false;
            Customer_crystal cc = new Customer_crystal();
            cc.rest_N = r_name;
            cc.ShowDialog();
        }
    }
}
