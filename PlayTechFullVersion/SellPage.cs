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
    public partial class SellPage : Form
    {
        public SellPage()
        {
            InitializeComponent();
        }

       
        PlayTechContext playTechDB = new();


        private void FillProductCombo()
        {
            Product_cb.Items.AddRange(playTechDB.Products.Where(x=>x.IsDelete==false).Select(x => x.ProductName).ToArray());
        }
     

        private void FillDataGrid()
        {
            AddProduct_dgv.DataSource = playTechDB.Products.Where(m => m.IsDelete==false && m.ProductName.Contains(Product_cb.Text)).Select(b => new
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
        private void FillPayment()
        {
            payMethod_cb.Items.AddRange(playTechDB.SaleTypes.Select(x => x.Type).ToArray());
        }

        private void FillCreditMonth()
        {
            installment_cb.Items.AddRange(playTechDB.CreditMonths.Select(x => x.Month).ToArray());
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
            FillPayment();
            FillCreditMonth();
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
                    if(payMethod_cb.Text != "")
                    {
                        if(payMethod_cb.Text == "Kredit")
                        {
                            if (installment_cb.Text != "")
                            {
                                panel2.Visible = true;

                                string Proname = Product_cb.Text + "-" + Quantity_num.Value;
                                Tag_clb.Items.Add(Proname, true);

                                Product_cb.Text = default;
                                Quantity_num.Value = 1;
                            }
                            else
                            {
                                MessageBox.Show(text: " Kredit müddətini seçin!", caption: "Diqqət", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            panel2.Visible = true;

                            string Proname = Product_cb.Text + "-" + Quantity_num.Value;
                            Tag_clb.Items.Add(Proname, true);

                            Product_cb.Text = default;
                            Quantity_num.Value = 1;
                        }
                      
                    }
                    else
                    {
                        MessageBox.Show(text: " Ödəniş üsulunu seçin!", caption: "Diqqət", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
                    }
                   
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

        private void Clear()
        {
            Tag_clb.Items.Clear();

            Product_cb.Text = default;
            payMethod_cb.Text = default;
            Quantity_num.Value = 1;
            panel2.Visible = false;
            panel3.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string result = "";
            string ProCount = "";
            

            for (int i = 0; i < Tag_clb.Items.Count; i++)
            {
                string ProductItem = Tag_clb.Items[i].ToString();
                string productName = ProductItem.Substring(0, ProductItem.LastIndexOf("-"));
                int count = Convert.ToInt32(ProductItem[(ProductItem.LastIndexOf("-") + 1)..]);
                Product selectedPro = playTechDB.Products.First(x => x.ProductName == productName);
                string type = payMethod_cb.Text;
                string creditMonth = installment_cb.Text;
                if (selectedPro.Quantity>=count)
                {
                    SaleType selectedType = playTechDB.SaleTypes.First(x => x.Type == type);

                    if (type == "Kredit")
                    {
                        CreditMonth selectedCreditMonth = playTechDB.CreditMonths.First(x => x.Month == creditMonth);

                        playTechDB.OrderItems.Add(new OrderItem()
                        {
                            ProductId = selectedPro.Id,
                            Quantity = count,
                            DailySaleDate = DateTime.Now,
                            ItemPrice = selectedPro.SalePrice * count,
                            SaleTypeId = selectedType.Id,
                            CreditMonthId = selectedCreditMonth.Id,

                        });
                        result = ($"Məhsul adı: {selectedPro.ProductName}, Say: {count}, Kreditlə satış olundu! ");
                        MessageBox.Show(result);
                    }
                    else
                    {
                        playTechDB.OrderItems.Add(new OrderItem()
                        {
                            ProductId = selectedPro.Id,
                            Quantity = count,
                            DailySaleDate = DateTime.Now,
                            ItemPrice = selectedPro.SalePrice * count,
                            SaleTypeId = selectedType.Id,
                            //CreditMonthId = selectedCreditMonth.Id,

                        });
                        result = ($"Məhsul adı: {selectedPro.ProductName}, Say: {count}, Qiymət: {selectedPro.SalePrice * count} AZN");
                        MessageBox.Show(result);
                    }
                   


                    
                    selectedPro.Quantity -= count;


                    playTechDB.SaveChanges();

                   

                   

                    FillDataGrid();
                }
                else
                {
                    ProCount = $"{selectedPro.ProductName} bazadakı mövcud sayı {selectedPro.Quantity} ədəddir. {count} ədəd satış ola bilməz!";
                    MessageBox.Show(ProCount, "Diqqət", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }



            }
            Clear();
        }

        private void payMethod_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (payMethod_cb.Text=="Kredit")
            {
                panel3.Visible = true;
            }
            else
            {
                panel3.Visible = false;
            }
        }
    }
}
