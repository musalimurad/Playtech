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
    public partial class DataBase : Form
    {
        PlayTechContext playTechDB = new();
        public DataBase()
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
            AddProduct_dgv.DataSource = playTechDB.Products.Where(m => m.ProductName.Contains(Product_cb.Text) && m.Firm.FirmName.Contains(Firm_cb.Text)).Select(b => new
            {
                ID = b.Id,
                Ad = b.ProductName,
                Növü = b.Category.CategoryName,
                Firma = b.Firm.FirmName,
                Barkod = b.BarCode,
                Say = b.Quantity,
                GəlişQiyməti = b.Price,
                SatışQiyməti = b.SalePrice,
                GəlişTarixi = b.PublishDate


            }).ToList();
            AddProduct_dgv.Columns[0].Visible = false;
            AddProduct_dgv.Columns[8].DefaultCellStyle.Format = "dd MMMM yyyy";

            for (int i = 0; i < AddProduct_dgv.RowCount; i++)
            {
                int Quantity = (int)AddProduct_dgv.Rows[i].Cells[5].Value;
                if (Quantity <= 0)
                {
                    AddProduct_dgv.Rows[i].DefaultCellStyle.ForeColor = Color.Orange;
                }

            }
            playTechDB.SaveChanges();
        }
        #endregion

        #region Exit minimize method
        private void button6_Click(object sender, EventArgs e)
        {
            AdminPage adminPage = new();
            adminPage.Show();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        #endregion

        private void DataBase_Load(object sender, EventArgs e)
        {
            FillDataGrid();
            FillFirmCombo();
            FillProductCombo();
        }

        #region search Filter Method

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
        #endregion

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
    }
}
