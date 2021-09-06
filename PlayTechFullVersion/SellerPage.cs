using PlayTechFullVersion.Models;
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
    public partial class SellerPage : Form
    {
        PlayTechContext playTechDB = new();
        public SellerPage()
        {
            InitializeComponent();
        }

        #region Fill Method

        private void FillFirmCombo()
        {
            Firm_cb.Items.AddRange(playTechDB.Firms.Select(x => x.FirmName).ToArray());
        }

        private void FillProductCombo()
        {
            Product_cb.Items.AddRange(playTechDB.Products.Select(x => x.ProductName).ToArray());
        }

        private void FillDataGrid()
        {
            AddProduct_dgv.DataSource = playTechDB.Products.Where(m => m.ProductName.Contains(Product_cb.Text) &&
            m.Firm.FirmName.Contains(Firm_cb.Text)).Select(b => new
            {
                ID = b.Id,
                Ad = b.ProductName,
                Firma = b.Firm.FirmName,
                Barkod = b.BarCode,
                Say = b.Quantity,
                SatışQiyməti = b.SalePrice,


            }).ToList();
            AddProduct_dgv.Columns[0].Visible = false;

            for (int i = 0; i < AddProduct_dgv.RowCount; i++)
            {
                int Quantity = (int)AddProduct_dgv.Rows[i].Cells[4].Value;
                if (Quantity <= 0)
                {
                    AddProduct_dgv.Rows[i].DefaultCellStyle.ForeColor = Color.DarkOrange;
                    
                }

            }
        }
        #endregion

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void about_btn_Click(object sender, EventArgs e)
        {
            SellPage sellPage = new();
            sellPage.Show();
            this.Close();
        }

        private void SellerPage_Load(object sender, EventArgs e)
        {
            FillDataGrid();
            FillFirmCombo();
            FillProductCombo();
        }

        private void Product_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDataGrid();

        }

        private void Firm_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDataGrid();

        }

        private void Product_cb_KeyDown(object sender, KeyEventArgs e)
        {
            FillDataGrid();

        }

        private void Product_cb_KeyPress(object sender, KeyPressEventArgs e)
        {
            FillDataGrid();

        }

        private void Product_cb_KeyUp(object sender, KeyEventArgs e)
        {
            FillDataGrid();

        }

        private void Firm_cb_KeyDown(object sender, KeyEventArgs e)
        {
            FillDataGrid();

        }

        private void Firm_cb_KeyPress(object sender, KeyPressEventArgs e)
        {
            FillDataGrid();

        }

        private void Firm_cb_KeyUp(object sender, KeyEventArgs e)
        {
            FillDataGrid();

        }

        private void AddProduct_dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
