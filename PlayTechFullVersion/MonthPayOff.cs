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
    public partial class MonthPayOff : Form
    {
        public MonthPayOff()
        {
            InitializeComponent();
        }


        PlayTechContext playTechDB = new();

        private void Clear()
        {
            Fullname_tb.Text = default;
            MonthCalcDate_dtp.Value = DateTime.Now;
            WorkerSalary_num.Value = 1;
            FloorMoney_num.Value = 1;
            CommunalMoney_num.Value = 1;
            MonthPrice_num.Value = 1;
            MonthMoney_num.Value = 1;
        }

        private void AddMonthCalc()
        {
            string Fullname = Fullname_tb.Text;
            decimal MonthMoney = MonthMoney_num.Value;
            decimal WorkerSalary = WorkerSalary_num.Value;
            decimal FloorMoney = FloorMoney_num.Value;
            decimal CommunalMoney = CommunalMoney_num.Value;
            decimal MonthPrice = MonthPrice_num.Value;
            DateTime MonthCalcDate = MonthCalcDate_dtp.Value;

            string[] myarr = { Fullname };

            if (Utilities.IsEmpty(myarr))
            {
                Expense expense = new()
                {
                    Fullname = Fullname,
                    AllMoney = MonthMoney,
                    WorkerSalary = WorkerSalary,
                    FloorPrice = FloorMoney,
                    Communal = CommunalMoney,
                    Price = MonthPrice,
                    CalcDate = MonthCalcDate
                };
                playTechDB.Expenses.Add(expense);
                playTechDB.SaveChanges();
                MessageBox.Show("Aylıq hesabat əlavə olundu!", "ugurlu", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            MonthCalc_dgv.DataSource = playTechDB.Expenses.Select(b => new
            {
                ID = b.Id,
                Ad = b.Fullname,
                AylıqMəbləğ = b.AllMoney,
                AylıqGəlir = b.Price,
                İşçiMaaşı = b.WorkerSalary,
                ObyektÖdənişi = b.FloorPrice,
                KomunalÖdəniş = b.Communal,
                HesabatTarixi = b.CalcDate


            }).ToList();
            MonthCalc_dgv.Columns[0].Visible = false;
            MonthCalc_dgv.Columns[7].DefaultCellStyle.Format = "dd MMMM yyyy";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AdminPage adminPage = new();
            adminPage.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void AddProduct_btn_Click(object sender, EventArgs e)
        {
            AddMonthCalc();
        }

        private void MonthPayOff_Load(object sender, EventArgs e)
        {
            FillDataGrid();
        }

        Expense SelectedExpense;

        private void Delete()
        {
            playTechDB.Expenses.Remove(SelectedExpense);
            playTechDB.SaveChanges();
            Clear();
            MessageBox.Show(" Məlumat silindi", "Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Delete_btn.Visible = false;
            AddProduct_btn.Visible = true;
            FillDataGrid();
        }
        private void Delete_btn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(" Hesabatı silmək istəyirsiniz?", "Sual", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Delete();
                Back_btn.Visible = false;
            }
        }

        private void MonthCalc_dgv_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int ExID = (int)MonthCalc_dgv.Rows[e.RowIndex].Cells[0].Value;

            SelectedExpense = playTechDB.Expenses.First(x => x.Id ==ExID);
            Fullname_tb.Text = SelectedExpense.Fullname;
            MonthMoney_num.Value = SelectedExpense.AllMoney;
            MonthPrice_num.Value = SelectedExpense.Price;
            FloorMoney_num.Value = SelectedExpense.FloorPrice;
            WorkerSalary_num.Value = SelectedExpense.WorkerSalary;
            CommunalMoney_num.Value = SelectedExpense.Communal;
            MonthCalcDate_dtp.Value = SelectedExpense.CalcDate;
            Delete_btn.Visible = true;
            Back_btn.Visible = true;
            AddProduct_btn.Visible = false;
        }

        private void Back_btn_Click(object sender, EventArgs e)
        {
            Clear();
            FillDataGrid();
            Delete_btn.Visible = false;
            Back_btn.Visible = false;
            AddProduct_btn.Visible = true;
        }
    }
}
