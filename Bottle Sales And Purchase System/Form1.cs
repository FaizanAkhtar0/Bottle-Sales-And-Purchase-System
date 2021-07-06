using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bottle_Sales_And_Purchase_System
{
    public partial class Login : Form
    {
        public SqlConnection con = new SqlConnection("Data Source=DESKTOP-CQBGF97\\SQLEXPRESS;Integrated Security=True");
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            try {
                String conString = "select * from db_plastic_management.dbo.Admins where name = '" + this.txt_username.Text + "' AND usr_password = '" + txt_password.Text + "'";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable dt = new DataTable();

                con.Open();
                var dataReader = cmd.ExecuteReader();
                dt.Load(dataReader);
                int count = 0;

                foreach (DataRow row in dt.Rows) {
                    count++;
                }

                if (count == 1){
                    Dashboard db = new Dashboard();
                    db.username = this.txt_username.Text;
                    db.Show();
                    this.Hide();
                    db.Closed += (s, args) => this.Close();
                    db.Show();
                }
                else {
                    MessageBox.Show("You must enter a valid username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                con.Close();
            }
            catch(Exception ex) {
                MessageBox.Show("Unable to connect to Database Server...","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuImageButton5_Click_1(object sender, EventArgs e)
        {
            if (!(txt_username.Text.Equals(""))) {
                MessageBox.Show("Your typed password is: \"" + txt_password.Text + "\"", "Show Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                MessageBox.Show("You must type a password first!","Show Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to quit the application?", "Exit Notice!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(dialogResult == DialogResult.Yes) {
                Application.Exit();
            } else {

            }
        }
    }
}
