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
    public partial class Menu_frm : Form
    {
        public int restaurant_id;
        string ordb = "Data Source=orcl;User id=scott;Password=tiger;";
        OracleConnection conn;
        public Menu_frm()
        {
            InitializeComponent();
        }

        private void Menu_frm_Load(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            OracleDataAdapter adapter1 = new OracleDataAdapter("select * from CATEGORIES", ordb);
            adapter1.Fill(ds, "category");

            OracleDataAdapter adapter2 = new OracleDataAdapter("select * from FOOD_ITEM", ordb);
            adapter2.Fill(ds, "food");

            DataRelation r = new DataRelation("fc", ds.Tables[0].Columns["CATEROGY_ID"], ds.Tables[1].Columns["CATEGORYID"]);
            ds.Relations.Add(r);

            BindingSource bs_master = new BindingSource(ds, "category");
            BindingSource bs_child = new BindingSource(bs_master, "fc");

            dataGridView2.DataSource = bs_master;
            dataGridView1.DataSource = bs_child;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Restaurants res = new Restaurants();
            res.ShowDialog();
        }

        private void Menu_frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
