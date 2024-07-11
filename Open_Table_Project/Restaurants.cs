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
    public partial class Restaurants : Form
    {
        string ordb = "Data Source=orcl;User id=scott;Password=tiger;";
        OracleConnection conn;
        public string time;
        public string num_of_people;
        public string date;
        public string city;
        int area_id;
        int rest_id;
        int cust_id;
        int table_num;
        public Restaurants()
        {
            InitializeComponent();
        }

        private void Restaurants_Load(object sender, EventArgs e)
        {
            //*********************select all categories into checklistbox1**********************************
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
            //***************************************select area id*******************************************************
            OracleCommand cmd8 = new OracleCommand();
            cmd8.Connection = conn;
            //int area_id = 0;
            cmd8.CommandText = "select AREA_ID from AREA where AREA_NAME=:area";
            cmd8.CommandType = CommandType.Text;
            cmd8.Parameters.Add("area", city);
            OracleDataReader dr5 = cmd8.ExecuteReader();
            while (dr5.Read())
            {
                area_id = Convert.ToInt32(dr5[0].ToString());
            }
            //feedback_frm frm = new feedback_frm();
            //frm.area_id = this.area_id;
            //****************************************show all restaurant**************************************************
            string cmdstr = @"select DISTINCT REST_NAME 
                              from RESTAURANT Rest ,REST_AVILABLE_TIME RT ,TABLES T ,LOCATE_AT LA
                              where rest.rest_id= rt.rest_id
                              and rest.rest_id= t.rest_id
                              and rest.rest_id= la.rest_id
                              and rt.avilable_time=:time
                              and la.area_id=:ar_id
                              and t.num_chairs=:chairs
                              and t.availability=1";
            OracleDataAdapter adapter = new OracleDataAdapter(cmdstr, conn);
            adapter.SelectCommand.Parameters.Add("time", time);
            adapter.SelectCommand.Parameters.Add("ar_id", area_id);
            adapter.SelectCommand.Parameters.Add("chairs", num_of_people);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

        }

        private void Restaurants_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
               
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ////////////////////get all category_id//////////////////////////////////////////////////
            List<int> cat_id = new List<int>();
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                OracleCommand cmd6 = new OracleCommand();
                cmd6.Connection = conn;
                cmd6.CommandText = "select CATEROGY_ID from CATEGORIES where CATEGORY_NAME=:name ";
                cmd6.CommandType = CommandType.Text;
                cmd6.Parameters.Add("name", checkedListBox1.CheckedItems[i].ToString());
                OracleDataReader dr2 = cmd6.ExecuteReader();
                while (dr2.Read())
                {
                    cat_id.Add(Convert.ToInt32(dr2[0].ToString()));
                }
            }
            ////////////////////get all restaurant (cuisine)////////////////////////////////////////
            int n1 = checkedListBox1.CheckedItems.Count;
            int n2 = checkedListBox2.CheckedItems.Count;
            int n3 = checkedListBox3.CheckedItems.Count;
            if (n1 > 0 && n2 == 0 && n3 == 0)
            {
                for (int i = 0; i < n1; i++)
                {
                    string cmdstr = @"select DISTINCT REST_NAME 
                                  from RESTAURANT Rest ,REST_AVILABLE_TIME RT ,TABLES T ,LOCATE_AT LA,RESTAURANT_CATEGORY rc 
                                  where rest.rest_id= rt.rest_id
                                  and rest.rest_id= t.rest_id
                                  and rest.rest_id= la.rest_id
                                  and rc.restaurant_id= rest.rest_id
                                  and rt.avilable_time=:time
                                  and la.area_id=:ar_id
                                  and t.num_chairs=:chairs
                                  and t.availability=1
                                  and rc.category_id=:cat_id";
                    OracleDataAdapter adapter = new OracleDataAdapter(cmdstr, conn);
                    adapter.SelectCommand.Parameters.Add("time", time);
                    adapter.SelectCommand.Parameters.Add("ar_id", area_id);
                    adapter.SelectCommand.Parameters.Add("chairs", num_of_people);
                    adapter.SelectCommand.Parameters.Add("cat_id", cat_id[i]);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                }
            }
            //////////////////////////////search by(seating option)///////////////////////////////////////////
            else if (n1 ==0 && n2 > 0 && n3 == 0)
            {
                for (int i = 0; i < n2; i++)
                {
                    string cmdstr = @"select DISTINCT REST_NAME 
                                  from RESTAURANT Rest ,REST_AVILABLE_TIME RT ,TABLES T ,LOCATE_AT LA,RESTAURANT_CATEGORY rc ,REST_SEATING r_seat
                                  where rest.rest_id= rt.rest_id
                                  and rest.rest_id= t.rest_id
                                  and rest.rest_id= la.rest_id
                                  and r_seat.REST_ID=rest.rest_id
                                  and rt.avilable_time=:time
                                  and la.area_id=:ar_id
                                  and t.num_chairs=:chairs
                                  and t.availability=1
                                  and r_seat.SEATING_OPTION=:seat";
                    OracleDataAdapter adapter = new OracleDataAdapter(cmdstr, conn);
                    adapter.SelectCommand.Parameters.Add("time", time);
                    adapter.SelectCommand.Parameters.Add("ar_id", area_id);
                    adapter.SelectCommand.Parameters.Add("chairs", num_of_people);
                    adapter.SelectCommand.Parameters.Add("seat", checkedListBox2.CheckedItems[i].ToString());
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                }
            }
            ///////////////////////////////////search by (price)//////////////////////////////////////////////
            else if (n1 == 0 && n2 == 0 && n3 > 0)
            {
                for (int i = 0; i < n3; i++)
                {
                    string cmdstr = @"select DISTINCT REST_NAME 
                                  from RESTAURANT Rest ,REST_AVILABLE_TIME RT ,TABLES T ,LOCATE_AT LA,RESTAURANT_CATEGORY rc ,REST_SEATING r_seat
                                  where rest.rest_id= rt.rest_id
                                  and rest.rest_id= t.rest_id
                                  and rest.rest_id= la.rest_id
                                  and rt.avilable_time=:time
                                  and la.area_id=:ar_id
                                  and t.num_chairs=:chairs
                                  and t.availability=1
                                  and rest.range_price=:price";
                    OracleDataAdapter adapter = new OracleDataAdapter(cmdstr, conn);
                    adapter.SelectCommand.Parameters.Add("time", time);
                    adapter.SelectCommand.Parameters.Add("ar_id", area_id);
                    adapter.SelectCommand.Parameters.Add("chairs", num_of_people);
                    adapter.SelectCommand.Parameters.Add("price", checkedListBox3.CheckedItems[i].ToString());
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                }
            }
            ///////////////////////////////////search by (cuisine && seating option)//////////////////////////////////////////////
            else if (n1 > 0 && n2 > 0 && n3 > 0)
            {
                for (int i = 0; i < n1; i++)
                {
                    string cmdstr = @"select DISTINCT REST_NAME 
                                  from RESTAURANT Rest ,REST_AVILABLE_TIME RT ,TABLES T ,LOCATE_AT LA,RESTAURANT_CATEGORY rc ,REST_SEATING r_seat
                                  where rest.rest_id= rt.rest_id
                                  and rest.rest_id= t.rest_id
                                  and rest.rest_id= la.rest_id
                                  and rc.restaurant_id= rest.rest_id
                                  and r_seat.REST_ID=rest.rest_id
                                  and rt.avilable_time=:time
                                  and la.area_id=:ar_id
                                  and t.num_chairs=:chairs
                                  and t.availability=1
                                  and rc.category_id=:cat_id
                                  and rest.range_price=:price";
                    OracleDataAdapter adapter = new OracleDataAdapter(cmdstr, conn);
                    adapter.SelectCommand.Parameters.Add("time", time);
                    adapter.SelectCommand.Parameters.Add("ar_id", area_id);
                    adapter.SelectCommand.Parameters.Add("chairs", num_of_people);
                    adapter.SelectCommand.Parameters.Add("cat_id", cat_id[i]);
                    adapter.SelectCommand.Parameters.Add("price", checkedListBox3.CheckedItems[i].ToString());
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                }
            }
            ///////////////////////////////////all restaurant/////////////////////////////////////////////////
            if (n1==0 &&n2==0 && n3==0)
            {
                string cmdstr = @"select DISTINCT REST_NAME 
                              from RESTAURANT Rest ,REST_AVILABLE_TIME RT ,TABLES T ,LOCATE_AT LA
                              where rest.rest_id= rt.rest_id
                              and rest.rest_id= t.rest_id
                              and rest.rest_id= la.rest_id
                              and rt.avilable_time=:time
                              and la.area_id=:ar_id
                              and t.num_chairs=:chairs
                              and t.availability=1";
                OracleDataAdapter adapter = new OracleDataAdapter(cmdstr, conn);
                adapter.SelectCommand.Parameters.Add("time", time);
                adapter.SelectCommand.Parameters.Add("ar_id", area_id);
                adapter.SelectCommand.Parameters.Add("chairs", num_of_people);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            restaurant_frm rest = new restaurant_frm();
            rest.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int n = dataGridView1.SelectedRows.Count;
            if (n == 1)
            {
                ////////////////////select max number of reservation////////////////////////////
                int max_res_id = 0, new_res_id = 0;
                conn = new OracleConnection(ordb);
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select max(RESERVATION_NO) from RESERVE";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    max_res_id = Convert.ToInt32(dr[0]);
                    new_res_id = max_res_id + 1;
                }
                dr.Close();
                //////////////////////select customer id/////////////////////////////////////////////
                //int cust_id=0;
                OracleCommand cmd1 = new OracleCommand();
                cmd1.Connection = conn;
                cmd1.CommandText = "select CUSTOMER_ID from CUSTOMER where CUST_E_MAIL=:email";
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.Add("email", textBox3.Text);
                OracleDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    cust_id = Convert.ToInt32(dr1[0]);
                }
                dr1.Close();
                //////////////////////select restaurant id/////////////////////////////////////////////
                //int rest_id=0;
                OracleCommand cmd2 = new OracleCommand();
                cmd2.Connection = conn;
                cmd2.CommandText = "select REST_ID from RESTAURANT where REST_NAME=:name";
                cmd2.CommandType = CommandType.Text;
                for (int i = 0; i < n; i++)
                {
                    cmd2.Parameters.Add("name", dataGridView1.Rows[i].Cells[0].Value);
                    OracleDataReader dr2 = cmd2.ExecuteReader();
                    while (dr2.Read())
                    {
                        rest_id = Convert.ToInt32(dr2[0]);
                    }
                    dr2.Close();
                }
                // MessageBox.Show(rest_id.ToString());
                //////////////////////select table number///////////////////////////////////////////////////////////////////////////
                conn = new OracleConnection(ordb);
                conn.Open();
                int table_num = 0;
                OracleCommand cmd3 = new OracleCommand();
                cmd3.Connection = conn;
                cmd3.CommandText = "select TABLE_NUM from TABLES where REST_ID=:Rest_id and NUM_CHAIRS=:chairs and AVAILABILITY=1";
                cmd3.CommandType = CommandType.Text;
                cmd3.Parameters.Add("Rest_id", rest_id);
                cmd3.Parameters.Add("chairs", num_of_people);
                OracleDataReader dr3 = cmd3.ExecuteReader();
                while (dr3.Read())
                {
                    table_num = Convert.ToInt32(dr3[0]);
                }
                dr3.Close();
                /////////////////////////select number of reservations ////////////////////////////////////////////////////////////////
                int new_numberOfPoints = 0,points=0;
                OracleCommand cmd6 = new OracleCommand();
                cmd6.Connection = conn;
                cmd6.CommandText = "select POINTS_NUM from CUSTOMER where CUST_E_MAIL=:email";
                cmd6.CommandType = CommandType.Text;
                cmd6.Parameters.Add("email", textBox3.Text);
                OracleDataReader dr4 = cmd6.ExecuteReader();
                while(dr4.Read())
                {
                    points = Convert.ToInt32(dr4[0]);
                    new_numberOfPoints = points + 1;
                }
                dr4.Close();
                //////////////////////insert new reservation///////////////////////////////////////////////////////////////////////////
                OracleCommand cmd4 = new OracleCommand();
                cmd4.Connection = conn;
                cmd4.CommandText = "insert_new_reservation";
                cmd4.CommandType = CommandType.StoredProcedure;
                cmd4.Parameters.Add("cust_id", cust_id);
                cmd4.Parameters.Add("table_num", table_num);
                cmd4.Parameters.Add("reserve_num", new_res_id);
                cmd4.Parameters.Add("people_num", num_of_people);
                cmd4.Parameters.Add("Date",Convert.ToDateTime(date));//error here!!!!!!!!!!!!
                cmd4.Parameters.Add("Time", time);
                cmd4.ExecuteNonQuery();
                ///////////////////////update number of reservations in customer table///////////////////////////////////////////////////
                OracleCommand cmd5 = new OracleCommand();
                cmd5.Connection = conn;
                cmd5.CommandText = "update CUSTOMER set POINTS_NUM=:new_numberOfPoints where CUST_E_MAIL=:email";
                cmd5.CommandType = CommandType.Text;
                cmd5.Parameters.Add("new_numberOfPoints", new_numberOfPoints);
                cmd5.Parameters.Add("email", textBox3.Text);
                cmd5.ExecuteNonQuery();
                //////////////////set availability to 0 when customer make reservation ///////////////////////////////////////////////////
                OracleCommand cmd7 = new OracleCommand();
                cmd7.Connection = conn;
                //cmd7.CommandText = "update TABLES set AVAILABILITY=:avail where TABLE_NUM=:t_num";
                cmd7.CommandText = "update_table";
                cmd7.CommandType = CommandType.StoredProcedure;
                //cmd7.Parameters.Add("avail", 0);
                cmd7.Parameters.Add("t_num", table_num);
                cmd7.ExecuteNonQuery();

                if(new_numberOfPoints>2)
                {
                    MessageBox.Show("you has a discount ^_^");
                }
            }
            else
                MessageBox.Show("Please select one restaurant ^_^");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int n = dataGridView1.SelectedRows.Count;
            if (n == 1)
            {
                Menu_frm frm = new Menu_frm();
                this.Visible = false;
                //frm.restaurant_id = rest_id;
                frm.ShowDialog();
            }
            else
                MessageBox.Show("Please select one restaurant ^_^");
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            feedback_frm frm = new feedback_frm();
            frm.area_id1 = area_id;
            frm.ShowDialog();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            panel3.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text == " ") || (textBox2.Text == " ") || (textBox3.Text == " "))
            {
                MessageBox.Show("Please fill the textbox ");
            }
            else
            {
                /////////////////////////////select cust id///////////////////////////////////////////////////////
                conn = new OracleConnection(ordb);
                conn.Open();
                OracleCommand cmd1 = new OracleCommand();
                cmd1.Connection = conn;
                cmd1.CommandText = "select CUSTOMER_ID from CUSTOMER where CUST_E_MAIL=:email";
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.Add("email", textBox3.Text);
                OracleDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    cust_id = Convert.ToInt32(dr1[0]);
                }
                dr1.Close();
                /////////////////////select table number to update availability //////////////////////////////////
                int table_num = 0;
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select TABLE_NUM from RESERVE where cust_id=:id";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("id", cust_id);
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    table_num = Convert.ToInt32(dr[0]);
                }
                /////////////////////////////update availability/////////////////////////////////////////////////
                OracleCommand cmd2 = new OracleCommand();
                cmd2.Connection = conn;
                cmd2.CommandText = "update TABLES set availability=1 WHERE table_num=:t_id";
                cmd2.CommandType = CommandType.Text;
                cmd2.Parameters.Add("t_id", table_num);
                cmd2.ExecuteNonQuery();
                ///////////////////////////cancel reservation////////////////////////////////////////////////////
                OracleCommand cmd3 = new OracleCommand();
                cmd3.Connection = conn;
                cmd3.CommandText = "cancel_reservation";
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.Add("c_id", cust_id);
                cmd3.ExecuteNonQuery();
                ///////////////////////////add negative point to this customer -_- ////////////////////////////////
                OracleCommand cmd4 = new OracleCommand();
                cmd4.Connection = conn;
                cmd4.CommandText = "update CUSTOMER set negative_points= negative_points+1, points_num= points_num-1 WHERE customer_id=:c_id";
                cmd4.CommandType = CommandType.Text;
                cmd4.Parameters.Add("c_id", cust_id);
                cmd4.ExecuteNonQuery();
                MessageBox.Show("Reservation Cancelled -_-");
            }
           
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
