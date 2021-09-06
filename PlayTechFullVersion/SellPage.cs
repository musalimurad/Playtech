using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PlayTechFullVersion.Models;


namespace PlayTechFullVersion
{
    public partial class SellPage : Form
    {
        public SellPage()
        {
            InitializeComponent();
        }

       
        PlayTechContext playTechDB = new();

        private void FillProductCombo()
        {
            Product_cb.Items.AddRange(playTechDB.Products.Select(x => x.ProductName).ToArray());
        }
     

        private void FillDataGrid()
        {
            AddProduct_dgv.DataSource = playTechDB.Products.Where(m => m.ProductName.Contains(Product_cb.Text)).Select(b => new
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
                    AddProduct_dgv.Rows[i].DefaultCellStyle.ForeColor = Color.Orange;
                }

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SellerPage sellerPage = new();
            sellerPage.Show();
            this.Close();
           
        }

        private void SellPage_Load(object sender, EventArgs e)
        {
            FillProductCombo();
            FillDataGrid();

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

        Product selectedProduct;
        private void AddProduct_dgv_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int ProID = (int)AddProduct_dgv.Rows[e.RowIndex].Cells[0].Value;
            selectedProduct = playTechDB.Products.FirstOrDefault(x => x.Id == ProID);
            if (selectedProduct.Quantity > 0)
            {
                Product_cb.Text = selectedProduct.ProductName;
            }
        }

        private void add_btn_Click(object sender, EventArgs e)
        {
            if (Product_cb.Text != "")
            {
                if (Quantity_num.Value != 0)
                {
                    panel2.Visible = true;

                    string Proname = Product_cb.Text + "-" +  Quantity_num.Value;
                    Tag_clb.Items.Add(Proname, true);
                    //if (!Tag_clb.Items.Contains(Proname.Substring(0, Proname.LastIndexOf("-"))))
                    //{

                    //}
                    Product_cb.Text = default;
                    Quantity_num.Value = 1;
                }
                else
                {
                    MessageBox.Show(text: " say 0 ola bilməz!", caption: "Diqqət", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
                }

            }
            else
            {
                MessageBox.Show(text: "Xahiş edirik məhsul seçin sonra satış edin", caption: "Diqqət", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
            }
        }

        private void Tag_clb_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Selected = Tag_clb.SelectedIndex;
            if (Selected != -1)
            {
                Tag_clb.Items.RemoveAt(Selected);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Tag_clb.Items.Clear();

            Product_cb.Text = default;
            Quantity_num.Value = 1;
            panel2.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string result = "";
            string ProCount = "";
            //Order order = new()
            //{
            //    OrderCode = Guid.NewGuid().ToString(),
            //    SaleDate = DateTime.Now
            //};

            for (int i = 0; i < Tag_clb.Items.Count; i++)
            {
                string ProductItem = Tag_clb.Items[i].ToString();
                string productName = ProductItem.Substring(0, ProductItem.LastIndexOf("-"));
                int count = Convert.ToInt32(ProductItem[(ProductItem.LastIndexOf("-") + 1)..]);

                Product selectedPro = playTechDB.Products.First(x => x.ProductName == productName);
             
                if (selectedPro.Quantity>=count)
                {

                   playTechDB.OrderItems.Add(new OrderItem()
                    {
                        ProductId = selectedPro.Id,
                        Quantity = count,
                        DailySaleDate = DateTime.Now,
                        ItemPrice = selectedPro.SalePrice * count,

                    });


                    
                    selectedPro.Quantity -= count;
                    //order.TotalPrice = order.OrderItems.Sum(x => x.ItemPrice * x.Quantity);
                     //playTechDB.Add();

                    playTechDB.SaveChanges();

                    result += ($"Məhsul adı: {selectedPro.ProductName}, Say: {count}, Qiymət: {selectedPro.SalePrice * count} AZN ");
                    MessageBox.Show(result);

                   

                    FillDataGrid();
                }
                else
                {
                    ProCount = $"{selectedPro.ProductName} bazadakı mövcud sayı {selectedPro.Quantity} ədəddir. {count} ədəd satış ola bilməz!";
                    MessageBox.Show(ProCount, "Diqqət", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }



            }
            Product_cb.Text = default;
            Quantity_num.Value = 1;
            Tag_clb.Items.Clear();
            panel2.Visible = false;
        }

       
    }
}
