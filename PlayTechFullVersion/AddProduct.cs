using PlayTechFullVersion.Helpers;
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
    public partial class AddProduct : Form
    {
        public AddProduct()
        {
            InitializeComponent();
        }
            PlayTechContext playTechDB = new();

        #region Find Method

        private int FindCategory(string categoryName)
        {
            using PlayTechContext playTechDB = new();
            Category selectedCategory = playTechDB.Categories.FirstOrDefault(x => x.CategoryName == categoryName);
            if (selectedCategory==null)
            {
                Category newCategory = new()
                {
                    CategoryName = categoryName,
                };
                playTechDB.Categories.Add(newCategory);
                playTechDB.SaveChanges();
                return newCategory.Id;
            }
            return selectedCategory.Id;
        }

        private int FindFirm(string firmName)
        {
            using PlayTechContext playTechDB = new();

            Firm selectedFirm = playTechDB.Firms.FirstOrDefault(x => x.FirmName == firmName);

            if (selectedFirm==null)
            {
                Firm newFirm = new()
                {
                    FirmName = firmName,
                };
                playTechDB.Firms.Add(newFirm);
                playTechDB.SaveChanges();
                return newFirm.Id;
            }
            return selectedFirm.Id;
        }

        #endregion

        #region Fill Method

        private void FillFirmCombo()
        {
            Firmname_cb.Items.AddRange(playTechDB.Firms.Select(x => x.FirmName).ToArray());
        }

        private void FillCategoryCombo()
        {
            Category_cb.Items.AddRange(playTechDB.Categories.Select(x => x.CategoryName).ToArray());
        }

        private void FillDataGrid()
        {
            AddProduct_dgv.DataSource = playTechDB.Products.Select(b => new
            {
                ID = b.Id,
                Ad = b.ProductName,
                Növü = b.Category.CategoryName,
                Firma  = b.Firm.FirmName,
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
        }
        #endregion

        #region Clear Method

        private void Clear()
        {
            ProductName_tb.Text = default;
            Firmname_cb.Text = default;
            Category_cb.Text = default;
            Barcode_tb.Text = default;
            Quantity_num.Value = 1;
            Price_num.Value = default;
            SalePrice_num.Value = 1;
            PublishDate_dtp.Value = DateTime.Now;
        }

        #endregion

        #region Add Product Method

        private void ProductAdd()
        {
            string ProductName = ProductName_tb.Text;
            string FirmName = Firmname_cb.Text;
            string CategoryName = Category_cb.Text;
            string Barcode = Barcode_tb.Text;
            decimal Price = Price_num.Value;
            decimal SalePrice = SalePrice_num.Value;
            DateTime PublishDate = PublishDate_dtp.Value;
            int Count = (int)Quantity_num.Value;

            string[] Myarr = { ProductName, FirmName, CategoryName, Barcode };
            if (Utilities.IsEmpty(Myarr))
            {
                if (Price > 0 && SalePrice > 0)
                {
                    if (Count>0)
                    {
                        int FirmID = FindFirm(FirmName);
                        int CategoryID = FindCategory(CategoryName);
                        Product product = new()
                        {
                            ProductName = ProductName,
                            FirmId = FirmID,
                            CategoryId = CategoryID,
                            BarCode = Barcode,
                            Price = Price,
                            SalePrice = SalePrice,
                            PublishDate = PublishDate,
                            Quantity = Count,
                        };
                        playTechDB.Products.Add(product);
                        playTechDB.SaveChanges();
                        MessageBox.Show("Məhsul əlavə olundu!", "Uğurlu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FillDataGrid();
                        FillCategoryCombo();
                        FillFirmCombo();
                        Clear();
                    }
                    else
                    {
                        MessageBox.Show("məhsul sayı 0 ola bilməz!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                   
                }
                else
                {
                    MessageBox.Show("qiymət 0Azn ola bilməz!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }

            }
            else
            {
                MessageBox.Show(text: "Bütün boşluqları doldurun!", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Hand);
            }
        }

        #endregion

        #region minimize exit button
        private void button4_Click(object sender, EventArgs e)
        {
            
            AdminPage adminPage = new();
            adminPage.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        #endregion

        private void AddProduct_Load(object sender, EventArgs e)
        {
            FillCategoryCombo();
            FillFirmCombo();
            FillDataGrid();
        }

        private void AddProduct_btn_Click(object sender, EventArgs e)
        {
            ProductAdd();
            FillFirmCombo();
            FillCategoryCombo();
            
        }

        #region Delete Edit Method
        Product selectedProduct;
        Category selectedCategory;
        Firm selectedFirm;

        private void AddProduct_dgv_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
            int ProductID = (int)AddProduct_dgv.Rows[e.RowIndex].Cells[0].Value;
            int CategoryID = (int)AddProduct_dgv.Rows[e.RowIndex].Cells[0].Value;
            int FirmID = (int)AddProduct_dgv.Rows[e.RowIndex].Cells[0].Value;
            selectedProduct = playTechDB.Products.First(x => x.Id == ProductID);
            selectedCategory = playTechDB.Categories.FirstOrDefault(x => x.Id == selectedProduct.CategoryId);
            selectedFirm = playTechDB.Firms.FirstOrDefault(x=>x.Id == selectedProduct.FirmId);
            ProductName_tb.Text = selectedProduct.ProductName;
            Quantity_num.Value = (int)selectedProduct.Quantity;
            Price_num.Value = (decimal)selectedProduct.Price;
            SalePrice_num.Value = (decimal)selectedProduct.SalePrice;
            PublishDate_dtp.Value = (DateTime)selectedProduct.PublishDate;
            Barcode_tb.Text = selectedProduct.BarCode;
            Firmname_cb.Text = selectedFirm.FirmName;
            Category_cb.Text = selectedCategory.CategoryName;
            Delete_btn.Visible = true;
            Edit_btn.Visible = true;
            Back_btn.Visible = true;
            AddProduct_btn.Visible = false;
        }

        private void HideButton()
        {
            Delete_btn.Visible = false;
            Edit_btn.Visible = false;
            Back_btn.Visible = false;
            AddProduct_btn.Visible = true;
            Clear();
        }

        private void DeleteButton()
        {
            playTechDB.Products.Remove(selectedProduct);
            playTechDB.SaveChanges();
            Clear();
            HideButton();
            MessageBox.Show(" Məhsul silindi", "Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Information);
            FillDataGrid();
        }
        
        private void EditButton()
        {
            Firm selectedFirm = playTechDB.Firms.FirstOrDefault(x => x.FirmName == Firmname_cb.Text);
            Category selectedCategory = playTechDB.Categories.FirstOrDefault(x => x.CategoryName == Category_cb.Text);

            selectedProduct.ProductName = ProductName_tb.Text;
            selectedProduct.Quantity = (int)Quantity_num.Value;
            selectedProduct.SalePrice = SalePrice_num.Value;
            selectedProduct.Price = Price_num.Value;
            selectedProduct.FirmId= selectedFirm.Id;
            selectedProduct.CategoryId = selectedCategory.Id;
            selectedProduct.BarCode = Barcode_tb.Text;
            selectedProduct.PublishDate = PublishDate_dtp.Value;
            playTechDB.Products.Update(selectedProduct);
            playTechDB.SaveChanges();
            Clear();
            HideButton();
            MessageBox.Show(" Məhsulda dəyişiklik uğurla baş verdi", "Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Information);
            FillDataGrid();

        }

        #endregion

        private void Back_btn_Click(object sender, EventArgs e)
        {
            HideButton();
        }

        private void Delete_btn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(" Məhsulu silmək istəyirsiniz?", "Sual", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
               DeleteButton();
            }
        }

        private void Edit_btn_Click(object sender, EventArgs e)
        {
            EditButton();
        }
    }
}
