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
    public partial class DailySaleBase : Form
    {
        PlayTechContext playTechDB = new();

        public DailySaleBase()
        {
            InitializeComponent();
        }

        private void FillDataGrid()
        {
            
            AddProduct_dgv.DataSource = playTechDB.OrderItems.Where(m => m.Product.ProductName.Contains(Product_cb.Text)).Select(b => new 
            {
                
                ID = b.Id,
                Ad = b.Product.ProductName,
                Firma = b.Product.Firm.FirmName,
                Barkod = b.Product.BarCode,
                SatılanSay = b.Quantity,
                ÜmumiSatışQiyməti = b.IsRefunded == true ? b.ItemPrice - (b.RefundedCount * b.Product.SalePrice) : b.ItemPrice,
                BirədədMəhsulQiyməti = b.Product.SalePrice,
                GəlişQiyməti = b.Product.Price,
                SatışTarixi = b.DailySaleDate,
                

            }).ToList();
            AddProduct_dgv.Columns[0].Visible = false;
            dateTime_lb.Text = Convert.ToString(DateTime.Now);

                AllPrice_lb.Text = Convert.ToString(playTechDB.OrderItems.Sum(x => x.IsRefunded == true ? x.ItemPrice - x.RefundedCount * x.Product.SalePrice : x.ItemPrice) + "Azn");
                TopPrice_lb.Text = Convert.ToString(playTechDB.OrderItems.Sum(x => Math.Floor((decimal)(x.IsRefunded == true ? (x.ItemPrice - (x.RefundedCount * x.Product.SalePrice)) - (x.Quantity * x.Product.Price) : x.ItemPrice - (x.Quantity * x.Product.Price)))) + "Azn");
            
            for (int i = 0; i < AddProduct_dgv.RowCount; i++)
            {
                int Quantity = (int)AddProduct_dgv.Rows[i].Cells[4].Value;

                if (Quantity <= 0)
                {
                    AddProduct_dgv.Rows[i].DefaultCellStyle.ForeColor = Color.DarkOrange;
                }

            }
        }

        private void FillProductCombo()
        {
            Product_cb.Items.AddRange(playTechDB.Products.Select(x => x.ProductName).ToArray());
        }
       

        private void button8_Click(object sender, EventArgs e)
        {
            
            AdminPage adminPage = new();
            adminPage.Show();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        
        private void DailySaleBase_Load(object sender, EventArgs e)
        {
            FillDataGrid();
            FillProductCombo();
           
        }

        #region search and excel method

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

        private void about_btn_Click(object sender, EventArgs e)
        {
            DataObject copyData = AddProduct_dgv.GetClipboardContent();

            if (copyData != null)
            {
                Clipboard.SetDataObject(copyData);
            }

            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();

            xlapp.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook xlWbook;
            Microsoft.Office.Interop.Excel.Worksheet xlSheet;
            object missedData = System.Reflection.Missing.Value;
            xlWbook = xlapp.Workbooks.Add(missedData);

            xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWbook.Worksheets.get_Item(1);
            Microsoft.Office.Interop.Excel.Range xlr = (Microsoft.Office.Interop.Excel.Range)xlSheet.Cells[1, 1];

            xlr.Select();
            xlSheet.PasteSpecial(xlr, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }

        #endregion
    }
}
