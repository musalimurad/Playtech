using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlayTechFullVersion
{
    public partial class AdminPage : Form
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddProduct_btn_Click(object sender, EventArgs e)
        {
            AddProduct addProduct = new();
            addProduct.Show();
            this.Close();
        }

        private void AddSeller_btn_Click(object sender, EventArgs e)
        {
            AddSeller addSeller = new();
            addSeller.Show();
            this.Close();
        }

        private void about_btn_Click(object sender, EventArgs e)
        {
            DataBase dataBase = new();
            dataBase.Show();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DailySaleBase dailySaleBase = new();
            dailySaleBase.Show();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AllSaleBase allSaleBase = new();
            allSaleBase.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MonthPayOff monthPayOff = new();
            monthPayOff.Show();
            this.Close();
        }
    }
}
