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
    public partial class AddSeller : Form
    {
        public AddSeller()
        {
            InitializeComponent();
        }
            PlayTechContext playTechDB = new();


        #region Fill Method

        private void FillDataGrid()
        {
            Seller_dgv.DataSource = playTechDB.Users.Select(b => new
            {
                ID = b.Id,
                Ad = b.Fullname,
                IstifadəçiAdı = b.Username,
                Telefon = b.Phone,
                Şifrə = b.Password,

            }).ToList();
            Seller_dgv.Columns[0].Visible = false;
            playTechDB.SaveChanges();

        }

        #endregion

        #region Method AddSeller, Method Clear

        private void Clear()
        {
            Name_tb.Text = default;
            User_tb.Text = default;
            Phone_tb.Text = default;
            Password_tb.Text = default;
            RePassword_tb.Text = default;
        }

        private void SellerAdd()
        {
            using var playTech = new PlayTechContext();

            string Fullname = Name_tb.Text;
            string Username = User_tb.Text;
            string Phone = Phone_tb.Text;
            string Password = Password_tb.Text;
            string RePassword = RePassword_tb.Text;
            string[] Myarr = { Fullname, Username, Phone, Password, RePassword };
            if (Utilities.IsEmpty(Myarr))
            {
                if (Password.Length >= 6 && RePassword.Length >= 6)
                {
                    if (Password == RePassword)
                    {

                        Models.User selectedUser = playTechDB.Users.FirstOrDefault(x => x.Phone == Phone);
                        if (selectedUser == null)
                        {
                            playTechDB.Users.Add(new User()
                            {
                                Fullname = Fullname,
                                Username = Username,
                                Phone = Phone,
                                Password = Password,
                                RePassword = RePassword
                            });
                            playTech.SaveChanges();
                            MessageBox.Show("Əlavə olundu!", "Uğurlu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FillDataGrid();
                            Clear();
                        }
                        else
                        {
                            MessageBox.Show("Bu telefon nömrəsi ilə artıq qeydiyyatdan keçilib!", "Diqqət", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        }
                    }
                    else
                    {
                        MessageBox.Show("Şifrələr eyni deyil!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
                else
                {
                    MessageBox.Show("Şifrə uzunluğu ən az 6 simvoldan ibarət olmalıdır!", "Diqqət", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Bütün boşluqları doldurun!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

       

        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminPage adminPage = new();
            adminPage.Show();
        }

        private void Hidepass_btn_Click(object sender, EventArgs e)
        {
            if (Password_tb.PasswordChar == '\0' && RePassword_tb.PasswordChar == '\0')
            {
                Showpass_btn.BringToFront();
                Showpass_btn.Visible = true;
                Password_tb.PasswordChar = '•';
                RePassword_tb.PasswordChar = '•';
            }
        }

        private void Showpass_btn_Click(object sender, EventArgs e)
        {
            if (Password_tb.PasswordChar == '•' && RePassword_tb.PasswordChar == '•')
            {
                Hidepass_btn.BringToFront();
                Hidepass_btn.Visible = true;
                Password_tb.PasswordChar = '\0';
                RePassword_tb.PasswordChar = '\0';

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            AdminPage adminPage = new();
            adminPage.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void AddSeller_Load(object sender, EventArgs e)
        {
            FillDataGrid();
        }

        private void RegisterAdmin_btn_Click(object sender, EventArgs e)
        {
            SellerAdd();
            FillDataGrid();
        }

        User selectedUser;

        private void Seller_dgv_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int UserID = (int)Seller_dgv.Rows[e.RowIndex].Cells[0].Value;
            selectedUser = playTechDB.Users.First(x => x.Id == UserID);

            Name_tb.Text = selectedUser.Fullname;
            User_tb.Text = selectedUser.Username;
            Phone_tb.Text = selectedUser.Phone;
            Password_tb.Text = selectedUser.Password;
            RePassword_tb.Text = selectedUser.RePassword;

            Edit_btn.Visible = true;
            Delete_btn.Visible = true;
            Back_btn.Visible = true;
            RegisterAdmin_btn.Visible = false;
        }

        private void HideButton()
        {
            Edit_btn.Visible = false;
            Delete_btn.Visible = false;
            Back_btn.Visible = false;
            RegisterAdmin_btn.Visible = true;
        }

        private void DeleteButton()
        {
            playTechDB.Users.Remove(selectedUser);
            playTechDB.SaveChanges();
            Clear();
            HideButton();
            FillDataGrid();
        }

        private void Delete_btn_Click(object sender, EventArgs e)
        {
            DeleteButton();
            Clear();
            FillDataGrid();
        }

        private void Back_btn_Click(object sender, EventArgs e)
        {
            HideButton();
            Clear();
        }
        
        private void EditButton()
        {
            selectedUser.Fullname = Name_tb.Text;
            selectedUser.Username = User_tb.Text;
            selectedUser.Phone = Phone_tb.Text;
            selectedUser.Password = Password_tb.Text;
            selectedUser.RePassword = RePassword_tb.Text;
           
            playTechDB.Users.Update(selectedUser);
            playTechDB.SaveChanges();
            Clear();
            HideButton();
            MessageBox.Show(" Istifadəçi dəyişiklik uğurla baş verdi", "Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Information);
            FillDataGrid();

        }

        private void Edit_btn_Click(object sender, EventArgs e)
        {
            EditButton();
        }
    }
}
