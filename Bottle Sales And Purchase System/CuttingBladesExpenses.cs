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
    public partial class CuttingBladesExpenses : Form
    {
        private bool flag = false;
        private double actualPrice = -1D;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public SqlConnection con = new SqlConnection("Data Source=DESKTOP-CQBGF97\\SQLEXPRESS;Integrated Security=True");

        public CuttingBladesExpenses()
        {
            InitializeComponent();
        }

        private void btn_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private bool UserNameValidityCheck()
        {
            try {
                String conString = "select name from db_plastic_management.dbo.CuttingBladesExpenses";
                SqlCommand cmd = new SqlCommand(conString, con);

                con.Open();

                DataTable dt = new DataTable();
                var dataAdapter = cmd.ExecuteReader();
                dt.Load(dataAdapter);

                foreach (DataRow row in dt.Rows) {
                    string username = Convert.ToString(row[0]);
                    if (txt_blade_seller_name.Text.Equals(username)) {
                        con.Close();
                        return true;
                    }
                }
                con.Close();
                return flag;
                
            } catch (Exception ex) {
                MessageBox.Show("Unable to connect to database!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private double convert(String param)
        {
            double returnValue = -1D;
            try {
                returnValue = Convert.ToDouble(param);
                return returnValue;
            } catch (Exception ex) {
                MessageBox.Show("Couldn't convert value: \"" + param + "\", because it contains some other character than numerics/floating point values.\nPlease try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return returnValue;
            }
        }

        private void save_balde_seller_Click(object sender, EventArgs e)
        {
            if (txt_blade_seller_name.Text.Equals("")) {
                MessageBox.Show("Seller name is required for the selected operation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (UserNameValidityCheck()) {
                MessageBox.Show("Username: \"" + txt_blade_seller_name.Text + "\", already exists in the database. \nTry using a different name or add numbers against the name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txt_blade_seller_name.Text = "";
                return;
            } else {

                int quantity = -1;
                double pricePerBlade = -1D;
                try {
                    quantity = Convert.ToInt32(this.txt_blade_quantity.Text);
                }
                catch (Exception ex) {
                    MessageBox.Show("Couldn't convert \"Quantity\" as it contains some other characters than integer values.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.txt_blade_quantity.Text = "";
                    return;
                }

                pricePerBlade = convert(this.txt_price_per_blade.Text);

                if (!(pricePerBlade == -1D)) {
                    actualPrice = (double)quantity * pricePerBlade;

                    try {
                        String conString = "insert into db_plastic_management.dbo.CuttingBladesExpenses(name, quantityBought, price, usr_address, contactNo) values('" + txt_blade_seller_name.Text + "' , '" + quantity + "' , '" + actualPrice + "' , '" + txt_blade_seller_address.Text + "' , '" + txt_blade_seller_contactno.Text + "')";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Insertion of a cutting blade seller was sucessful", "Insertion Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to insert into the Database Server...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else {
                    MessageBox.Show("Couldn't convert price/1blade as it contains some other characters than double or floating point values.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.txt_price_per_blade.Text = "";
                    return;
                }
            }
        }

        private void update_balde_seller_Click(object sender, EventArgs e)
        {
            if (txt_blade_seller_name.Text.Equals("")) {
                MessageBox.Show("Seller name is required for the selected operation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (UserNameValidityCheck()) {

                int quantity = -1;
                double pricePerBlade = -1D;
                try {
                    quantity = Convert.ToInt32(this.txt_blade_quantity.Text);
                } catch (Exception ex) {
                    MessageBox.Show("Couldn't convert \"Quantity\" as it contains some other characters than integer values.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.txt_blade_quantity.Text = "";
                    return;
                }

                pricePerBlade = convert(this.txt_price_per_blade.Text);

                if (!(pricePerBlade == -1D)) {
                    actualPrice = (double)quantity * pricePerBlade;

                    try {
                        String conString = "update db_plastic_management.dbo.CuttingBladesExpenses set quantityBought = '" + txt_blade_quantity.Text + "', price = '" + actualPrice + "', usr_address = '" + txt_blade_seller_address.Text + "', contactNo = '" + txt_blade_seller_contactno.Text + "' where name = '" + txt_blade_seller_name.Text + "'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Updation of the Blade-Seller was sucessful!", "Updation Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to update Blade-Seller Table from the Database Server...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else {
                    MessageBox.Show("Couldn't convert price/1blade as it contains some other characters than double or floating point values.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.txt_price_per_blade.Text = "";
                    return;
                }
            } else {
                MessageBox.Show("No such name: \"" + txt_blade_seller_name.Text + "\", exists in the database to update it!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txt_blade_seller_name.Text = "";
                return;
            }
        }

        private void delete_balde_seller_Click(object sender, EventArgs e)
        {
            if (txt_blade_seller_name.Text.Equals("")) {
                MessageBox.Show("Seller name is required for the selected operation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (UserNameValidityCheck()) {
                try {
                    String conString = "delete from db_plastic_management.dbo.CuttingBladesExpenses where name = '" + txt_blade_seller_name.Text + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Deletion of the Blade-Seller: \"" + txt_blade_seller_name.Text + "\", was sucessful.", "Deletion Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to delete Blade-Seller Table from the Database Server...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No such name: \"" + txt_blade_seller_name.Text + "\", exists in the database to delete it!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txt_blade_seller_name.Text = "";
                return;
            }
        }

        private void CuttingBladesExpenses_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuShadowPanel1_MouseDown(object sender, MouseEventArgs e)
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

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
