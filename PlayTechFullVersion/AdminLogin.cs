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
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void LoginAdmin()
        {
            using var PlayTechDB = new PlayTechContext();
            string Fullname = Fullname_tb.Text;
            string Password = Password_tb.Text;
            string[] MyArr = { Fullname, Password };

            if (Utilities.IsEmpty(MyArr))
            {
                if (PlayTechDB.Admins.Where(x => x.Username == Fullname && x.Password == Password).Count() > 0)
                {
                    this.Hide();
                    AdminPage adminPage = new();
                    adminPage.Show();
                }
                else
                {
                    MessageBox.Show("Ad və ya Şifrə doğru  deyil!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    Fullname_tb.Text = default;
                    Password_tb.Text = default;
                }
            }
            else
            {
                MessageBox.Show("Bütün boşluqları doldurun!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }



        private void AdminLogin_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void Show_pass_Click(object sender, EventArgs e)
        {
            if (Password_tb.PasswordChar == '•')
            {
                Hide_pass.BringToFront();
                Hide_pass.Visible = true;
                Password_tb.PasswordChar = '\0';
            }
        }

        private void Hide_pass_Click(object sender, EventArgs e)
        {
            if (Password_tb.PasswordChar == '\0')
            {
                Show_pass.BringToFront();
                Show_pass.Visible = true;
                Password_tb.PasswordChar = '•';
            }
        }

        private void LoginAdmin_btn_Click(object sender, EventArgs e)
        {
            LoginAdmin();
        }
    }
}
