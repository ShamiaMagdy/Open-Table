using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Open_Table_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Register_frm frm = new Register_frm();
            this.Visible = false;
            frm.ShowDialog();
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login_frm frm = new Login_frm();
            this.Visible = false;
            frm.ShowDialog();
        
        }

        private void forRestaurantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
           
        }

        private void registerNewRestaurantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            restaurant_manager_frm frm = new restaurant_manager_frm();
            this.Visible = false;
            frm.ShowDialog();
        }

        private void loginToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manager_Login_frm frm = new Manager_Login_frm();
            frm.ShowDialog();
        }

        private void showMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Cristal1 cr = new Cristal1();
            cr.ShowDialog();
        }
    }
}
