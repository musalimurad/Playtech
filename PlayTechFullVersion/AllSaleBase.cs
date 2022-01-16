using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Task.Models;
using WinFormsAppModel.Models;

namespace PlayTechFullVersion
{
    public partial class AllSaleBase : Form
    {
        PlayTechContext playTechDB;
        OrderItem selectedOrder;
        public AllSaleBase()
        {
            InitializeComponent();
            playTechDB = new();
            FillDataGrid();
        }
        private void FillProductCombo()
        {
            Product_cb.Items.AddRange(playTechDB.Products.Where(x=>x.IsDelete==false).Select(x => x.ProductName).ToArray());
        }
        private void FillDataGrid()
        {
            AddProduct_dgv.DataSource = playTechDB.OrderItems
                .Where(m => m.Product.ProductName.Contains(Product_cb.Text)
                && m.DailySaleDate.Value.Day==PublishDate_dtp.Value.Day && m.DailySaleDate.Value.Month == PublishDate_dtp.Value.Month && m.DailySaleDate.Value.Year == PublishDate_dtp.Value.Year).Select(b => new
                {
                    ID = b.Id,
                    Ad = b.Product.ProductName,
                    Firma = b.Product.Firm.FirmName,
                    Barkod = b.Product.BarCode,
                    SatılanSay = b.Quantity,
                    ÜmumiSatış = b.IsRefunded==true? b.ItemPrice - (b.RefundedCount * b.Product.SalePrice) : b.ItemPrice,
                    SatışQiyməti = b.Product.SalePrice,
                    MayaQiyməti = b.Product.Price,
                    SatışTarixi = b.DailySaleDate,
                    SatışNövü = b.SaleType.Type,
                    KreditMüddəti = b.CreditMonth.Month,
                    SatışVəziyyəti=!b.IsRefunded?"Satılıb":$"{b.RefundedCount} ədəd Geri Qaytarıldı"
                }).ToList();
            AddProduct_dgv.Columns[0].Visible = false;
            AllPrice_lb.Text = Convert.ToString(playTechDB.OrderItems.Where(x=> x.DailySaleDate.Value.Day==  PublishDate_dtp.Value.Day && x.DailySaleDate.Value.Month==PublishDate_dtp.Value.Month && x.DailySaleDate.Value.Year==PublishDate_dtp.Value.Year).Sum(x => x.IsRefunded==true? x.ItemPrice- x.RefundedCount * x.Product.SalePrice : x.ItemPrice) + "  Azn");
            TopPrice_lb.Text = Convert.ToString(playTechDB.OrderItems.Where(x=> x.DailySaleDate.Value.Day == PublishDate_dtp.Value.Day && x.DailySaleDate.Value.Month == PublishDate_dtp.Value.Month && x.DailySaleDate.Value.Year == PublishDate_dtp.Value.Year).Sum(x => (decimal)(x.IsRefunded==true? (x.ItemPrice - (x.RefundedCount * x.Product.SalePrice)) - (x.Quantity* x.Product.Price) : x.ItemPrice - (x.Quantity * x.Product.Price))) + "  Azn");
            rasxod_lb.Text = Convert.ToString((playTechDB.OrderItems.Where(x => x.DailySaleDate.Value.Day == PublishDate_dtp.Value.Day && x.DailySaleDate.Value.Month == PublishDate_dtp.Value.Month && x.DailySaleDate.Value.Year == PublishDate_dtp.Value.Year).Sum(x => x.IsRefunded == true ? x.ItemPrice - x.RefundedCount * x.Product.SalePrice : x.ItemPrice)) - (playTechDB.OrderItems.Where(x => x.DailySaleDate.Value.Day == PublishDate_dtp.Value.Day && x.DailySaleDate.Value.Month == PublishDate_dtp.Value.Month && x.DailySaleDate.Value.Year == PublishDate_dtp.Value.Year).Sum(x => (decimal)(x.IsRefunded == true ? (x.ItemPrice - (x.RefundedCount * x.Product.SalePrice)) - (x.Quantity * x.Product.Price) : x.ItemPrice - (x.Quantity * x.Product.Price)))) + "  Azn");
            for (int i = 0; i < AddProduct_dgv.RowCount; i++)
            {
                int Quantity = (int)AddProduct_dgv.Rows[i].Cells[4].Value;

                if (Quantity <= 0)
                {
                    AddProduct_dgv.Rows[i].DefaultCellStyle.ForeColor = Color.DarkOrange;
                }

            }
            playTechDB.SaveChanges();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            
            AdminPage adminPage = new();
            adminPage.Show();
            this.Close();
        }


        private void button11_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int quantity = (int)Quantity_num.Value;
           
            if (quantity <= selectedOrder.Quantity && quantity!=0)
            {
                selectedOrder.IsRefunded = true;
                selectedOrder.RefundedCount = quantity;
                selectedOrder.Quantity -= quantity;
                selectedOrder.Product.Quantity += quantity;

                playTechDB.SaveChanges();
                MessageBox.Show($"{selectedOrder.Product.ProductName} adlı məhsuldan {selectedOrder.RefundedCount} ədəd geri qaytarıldı", "Geri qaytarılma", MessageBoxButtons.OK, MessageBoxIcon.Information);
                panel2.Visible = false;
                FillDataGrid();
            }
            else
            {
                MessageBox.Show("Geri qaytarılan məhsul sayı satılmış məhsul sayını keçə bilməz və ya qaytarılan məhsul sayı 0 ola bilməz!", "Diqqət", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
           
        }

        private void AddProduct_dgv_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            panel2.Visible = true;
            int orderItemId = (int)AddProduct_dgv.Rows[e.RowIndex].Cells[0].Value;
            selectedOrder = playTechDB.OrderItems.Include(x => x.Product).FirstOrDefault(x => x.Id == orderItemId);
            ProductName_tb.Text = selectedOrder.Product.ProductName;
            
        }

        private void PublishDate_dtp_ValueChanged(object sender, EventArgs e)
        {
            DateTime publishDate = PublishDate_dtp.Value;
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

        private void AllSaleBase_Load(object sender, EventArgs e)
        {
            FillDataGrid();
            FillProductCombo();
            PublishDate_dtp.Value = DateTime.Now;
        }

        private void Product_cb_SelectedIndexChanged(object sender, EventArgs e)
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

        private void ProductName_tb_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
