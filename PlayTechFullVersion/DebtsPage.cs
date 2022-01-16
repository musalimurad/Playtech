using PlayTechFullVersion.Helpers;
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
    public partial class DebtsPage : Form
    {
        PlayTechContext playTechDB =new();

        Dept selectedDept;

        public DebtsPage()
        {
            InitializeComponent();
            
        }

        private void Clear()
        {
            Fullname_tb.Text = default;
            Why_rtb.Text = default;
            TakeMoney_num.Value = 1;
            TakeDate_dtp.Value = DateTime.Now;
        }

        private void Back()
        {
            Clear();
            FillDataGrid();
            Delete_btn.Visible = false;
            Back_btn.Visible = false;
            AddProduct_btn.Visible = true;
        }
     

        private void AddDept()
        {
            string Fullname = Fullname_tb.Text;
            string why = Why_rtb.Text;
            decimal money = TakeMoney_num.Value;
            DateTime date = TakeDate_dtp.Value;

            string[] myArr = { Fullname, why };

            if (Utilities.IsEmpty(myArr))
            {
                Dept dept = new()
                {
                    Fullname = Fullname,
                    TakeWhy = why,
                    TakeMoney = money,
                    TakeDate = date,
                };
                playTechDB.Depts.Add(dept);
                playTechDB.SaveChanges();
                MessageBox.Show("Hesabat əlavə olundu!", "ugurlu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FillDataGrid();
                Clear();

            }
            else
            {
                MessageBox.Show("Bütün boşluqları doldurun!", "diqqət", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }


        private void FillDataGrid()
        {
            MonthCalc_dgv.DataSource = playTechDB.Depts.Select(b => new
            {
                ID = b.Id,
                Ad = b.Fullname,
                Səbəbi = b.TakeWhy,
                Məbləğ = b.TakeMoney,
                Tarix = b.TakeDate


            }).ToList();
            MonthCalc_dgv.Columns[0].Visible = false;
            MonthCalc_dgv.Columns[4].DefaultCellStyle.Format = "dd MMMM yyyy";

            ras_lb.Text = Convert.ToString(playTechDB.Depts.Sum(x=>x.TakeMoney) + " Azn");
        }

        
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            AdminPage adminPage = new();
            adminPage.Show();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void AddProduct_btn_Click(object sender, EventArgs e)
        {
            AddDept();
        }

        private void Back_btn_Click(object sender, EventArgs e)
        {
            Back();
        }

        private void DebtsPage_Load(object sender, EventArgs e)
        {
            FillDataGrid();
        }
        private void Delete()
        {
            playTechDB.Depts.Remove(selectedDept);
            playTechDB.SaveChanges();
            Clear();
            MessageBox.Show(" Məlumat silindi", "Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Delete_btn.Visible = false;
            AddProduct_btn.Visible = true;
            Back_btn.Visible = false;
            FillDataGrid();
        }

        private void MonthCalc_dgv_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int deptID = (int)MonthCalc_dgv.Rows[e.RowIndex].Cells[0].Value;

            selectedDept = playTechDB.Depts.First(x => x.Id == deptID);
            Fullname_tb.Text = selectedDept.Fullname;
            TakeMoney_num.Value = (decimal)selectedDept.TakeMoney;
            TakeDate_dtp.Value =  (DateTime)selectedDept.TakeDate;
            Why_rtb.Text = selectedDept.TakeWhy;
            Delete_btn.Visible = true;
            Back_btn.Visible = true;
            AddProduct_btn.Visible = false;
        }

        private void Delete_btn_Click(object sender, EventArgs e)
        {
            Delete();
        }
    }
}
