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
    public partial class HomePage : Form
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Proqramdan çıxış etmək istəyirsiniz?", "Sual", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                ActiveForm.Close();
            }
        }
        int Mouse_X;
        int Mouse_Y;
        bool Move;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Mouse_X = e.X;
            Mouse_Y = e.Y;
            Move = true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move)
            {
                SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Move = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;

        }

        private void admin_btn_Click(object sender, EventArgs e)
        {
           
            AdminLogin adminLogin = new();
            adminLogin.Show();
        }

        private void seller_btn_Click(object sender, EventArgs e)
        {
            SellerLogin sellerLogin = new();
            sellerLogin.Show();
            
        }
    }
}
