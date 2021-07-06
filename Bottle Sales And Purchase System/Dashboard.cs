using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bottle_Sales_And_Purchase_System
{
    public partial class Dashboard : Form
    {

        private bool flag_night_mode = false;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public String username;

        public Dashboard()
        {
            InitializeComponent();
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            this.lbl_panel.Visible = false;
            this.setting_pnl.Visible = false;
            this.btn_admin.Normalcolor = Color.BlueViolet;
            this.btn_settings.Normalcolor = Color.DeepPink;
        }

        private void btn_settings_Click(object sender, EventArgs e)
        {
            this.btn_settings.Normalcolor = Color.BlueViolet;
            this.btn_admin.Normalcolor = Color.DeepPink;
            this.setting_pnl.Visible = true;
        }

        private void bunifuiOSSwitch1_OnValueChange(object sender, EventArgs e)
        {
            if(bunifuiOSSwitch1.Value == false) {
                this.BackColor = Color.DarkGray;
                this.setting_content_pnl.BorderColor = Color.Gray;
                flag_night_mode = true;
            }
            else {
                this.BackColor = Color.White;
                flag_night_mode = false;
            }
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            this.setting_pnl.Visible = false;
            this.lbl_username.Text = "Loggen in: " + username.ToString() + "\nStatus | ADMIN";
        }

        private void bunifuFlatButton1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Closed += (s, args) => this.Close();
            login.Show();
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to Logout(Yes) or Quit(No)?\nWarning: Quiting without saving changes may occur loss of unsaved data.", "Dashboard", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes) {
                this.Hide();
                Login login = new Login();
                login.Closed += (s, args) => this.Close();
                login.Show();
            } else {
                Application.Exit();
            }
        }

        private void Dashboard_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuGradientPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void setting_pnl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void lbl_panel_MouseDown(object sender, MouseEventArgs e)
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

        private void admin_purchase_Click(object sender, EventArgs e)
        {
            Purchase purchase = new Purchase(ref this.flag_night_mode);
            purchase.Show();
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to open \"Loader-Employees(Yes)\" or \"Separator-Employees(No)\" or \"Crusher-Employees(Cancel)\" Management Form?", "Select", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if(dialogResult == DialogResult.Yes) {
                EmployeeManagement employee = new EmployeeManagement(ref this.flag_night_mode);
                employee.Show();
            } else if (dialogResult == DialogResult.No) {
                SeparatorEmployee separatorEmployee = new SeparatorEmployee(ref this.flag_night_mode);
                separatorEmployee.Show();
            } else {
                CrusherEmployeeManagement crusherEmployee = new CrusherEmployeeManagement(ref this.flag_night_mode);
                crusherEmployee.Show();
            }
        }

        private void admin_sales_Click(object sender, EventArgs e)
        {
            SalesManagement sales = new SalesManagement(ref this.flag_night_mode);
            sales.Show();
        }

        private void bunifuTileButton4_Click(object sender, EventArgs e)
        {
            Payroll payroll = new Payroll(ref this.flag_night_mode);
            payroll.Show();
        }

        private void bunifuTileButton2_Click(object sender, EventArgs e)
        {
            MiscelleniousManagement miscellenious = new MiscelleniousManagement(ref this.flag_night_mode);
            miscellenious.Show();
        }

        private void bunifuTileButton3_Click(object sender, EventArgs e)
        {
            ProfitLoss profitLoss = new ProfitLoss(ref this.flag_night_mode);
            profitLoss.Show();
        }
    }
}
