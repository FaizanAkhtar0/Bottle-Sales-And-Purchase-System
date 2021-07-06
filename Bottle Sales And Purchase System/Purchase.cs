using System;
using System.Collections;
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
    public partial class Purchase : Form
    {
        private int ID = -1;
        private double actualPrice = -1D, actualLoadingPrice = -1D, actualDieselPrice = -1D;

        public bool flag_night_mode;

        public SqlConnection con = new SqlConnection("Data Source=DESKTOP-CQBGF97\\SQLEXPRESS;Integrated Security=True");

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public Purchase(ref bool param)
        {
            InitializeComponent();
            this.flag_night_mode = param;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuShadowPanel3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuShadowPanel4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuShadowPanel5_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void Purchase_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private double convert(String param) {
            double returnValue = -1D;
            try {
                returnValue = Convert.ToDouble(param);
                return returnValue;
            }catch(Exception ex) {
                MessageBox.Show("Couldn't convert value: \"" + param + "\", because it contains some other character than numerics/floating point values.\nPlease try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return returnValue;
            }
        }
        private DataTable grid_table = new DataTable();

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!(filter_seller_name.Text.Equals(""))) {
                try {
                         
                } catch(Exception ex) {

                }
            }
        }

        private void update_seller_Click(object sender, EventArgs e)
        {
            // using Local variables, will be deleted on this function ending.
            double quantity = -1D, price = -1D, loadingPrice = -1D, dieselQuantity = -1D, dieselPrice = -1D, advance = -1D, foodPrice = -1D;

            // Fields conversion checks, notifications and obtaining converted values.
            quantity = convert(txt_seller_quantity.Text);
            price = convert(txt_seller_price.Text);
            dieselQuantity = convert(txt_diesel_quantity.Text);
            dieselPrice = convert(txt_diesel_price.Text);
            loadingPrice = convert(txt_loading_price.Text);
            advance = convert(txt_seller_advance.Text);
            foodPrice = convert(txt_food_price.Text);

            // Applying field checks to ensure data integrity & error removal.
            if (!(quantity == -1D) && !(price == -1D) && !(dieselQuantity == -1D) && !(dieselPrice == -1D) && !(loadingPrice == -1D) && !(advance == -1D) && !(foodPrice == -1D)) {
                actualPrice = quantity * price;
                actualLoadingPrice = quantity * loadingPrice;
                actualDieselPrice = dieselQuantity * dieselPrice;
            } else {
                if (quantity == -1D) {
                    txt_seller_quantity.Text = "";
                } else if (price == -1D) {
                    txt_seller_price.Text = "";
                } else if (dieselQuantity == -1D) {
                    txt_diesel_quantity.Text = "";
                } else if (dieselPrice == -1D) {
                    txt_diesel_price.Text = "";
                } else if (loadingPrice == -1D) {
                    txt_loading_price.Text = "";
                } else if (advance == -1D) {
                    txt_seller_advance.Text = "";
                } else if (foodPrice == -1D) {
                    txt_food_price.Text = "";
                } else if ((advance == -1D) && (foodPrice == -1D)) {
                    txt_seller_advance.Text = "";
                    txt_food_price.Text = "";
                } else {
                    txt_seller_quantity.Text = "";
                    txt_seller_price.Text = "";
                    txt_diesel_quantity.Text = "";
                    txt_diesel_price.Text = "";
                    txt_loading_price.Text = "";
                    txt_seller_advance.Text = "";
                    txt_food_price.Text = "";
                }
                return;
            }

            // Update KabariaSeller table for basic information about seller.
            if (!(this.ID == -1)) {
                try {
                    String conString = "update db_plastic_management.dbo.KabariaSeller set name = '" + txt_seller_name.Text + "', quantityBought = '" + txt_seller_quantity.Text + "', price = '" + actualPrice + "', usr_address = '" + txt_seller_address.Text + "', advance_money = '" + txt_seller_advance.Text + "' where ks_id = '" + this.ID + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                } catch (Exception ex) {
                    MessageBox.Show("Unable to update Seller from the Database Server...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Update BuyingExpenses table for Expenses provided to buy from this seller.
                try {
                    String conString = "update db_plastic_management.dbo.BuyingExpenses set diesel_quantity = '" + txt_diesel_quantity.Text + "', diesel_price = '" + actualDieselPrice + "', food_price = '" + foodPrice + "', loading_price = '" + actualLoadingPrice + "' where b_id = '" + this.ID + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Updation of the Seller was sucessful!", "Updation Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refresh();
                } catch (Exception ex) {
                    MessageBox.Show("Unable to update BuyingExpenses for the Seller from the Database Server...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                MessageBox.Show("You must select a seller 1st from the view below in order to update it!", "Invalid Action", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Resetting values for later use of the variables.
            actualDieselPrice = -1D;
            actualPrice = -1D;
            actualLoadingPrice = -1D;
            populateLoadingPrice();
        }

        private void delete_seller_Click(object sender, EventArgs e)
        {
            try {
                String conString = "delete from db_plastic_management.dbo.KabariaSeller where ks_id = '" + this.ID + "'";
                SqlCommand cmd = new SqlCommand(conString, con);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Deletion of the seller was sucessful", "Deletion Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                refresh();
            } catch (Exception ex) {
                MessageBox.Show("Unable to delete Seller from the Database Server.\nPlease check database server connectivity...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            populateLoadingPrice();
        }

        private void filter_seller_name_TextChanged(object sender, EventArgs e)
        {
            try {
                String conString = "select * from db_plastic_management.dbo.KabariaSeller where name like '" + filter_seller_name.Text + "%'";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable data = new DataTable();
                con.Open();
                data.Load(cmd.ExecuteReader());

                seller_view.DataSource = data;
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            filter_quantity.Text = "";
            filter_price.Text = "";
            filter_address.Text = "";
            filter_advance_payment.Text = "";
        }

        private void filter_quantity_TextChanged(object sender, EventArgs e)
        {
            if ((filter_seller_name.Text.Equals(""))) {
                try {
                    String conString = "select * from db_plastic_management.dbo.KabariaSeller where quantityBought like '" + filter_quantity.Text + "%'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    DataTable data = new DataTable();
                    con.Open();
                    data.Load(cmd.ExecuteReader());

                    seller_view.DataSource = data;
                    con.Close();
                } catch (Exception ex) {
                    MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                if (!(filter_seller_name.Text.Equals(""))) {
                    try {
                        String conString = "select * from db_plastic_management.dbo.KabariaSeller where quantityBought like '" + filter_quantity.Text + "%' AND name like '" + filter_seller_name.Text + "%'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        DataTable data = new DataTable();
                        con.Open();
                        data.Load(cmd.ExecuteReader());

                        seller_view.DataSource = data;
                        con.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else {
                    MessageBox.Show("You must use the name filter first!", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            filter_price.Text = "";
            filter_address.Text = "";
            filter_advance_payment.Text = "";
        }

        private void filter_price_TextChanged(object sender, EventArgs e)
        {
            if ((filter_seller_name.Text.Equals("")) && (filter_quantity.Text.Equals(""))) {
                try {
                    String conString = "select * from db_plastic_management.dbo.KabariaSeller where price like '" + filter_price.Text + "%'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    DataTable data = new DataTable();
                    con.Open();
                    data.Load(cmd.ExecuteReader());

                    seller_view.DataSource = data;
                    con.Close();
                } catch (Exception ex) {
                    MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                if (!(filter_seller_name.Text.Equals("")) && (filter_quantity.Text.Equals(""))) {
                    try {
                        String conString = "select * from db_plastic_management.dbo.KabariaSeller where price like '" + filter_price.Text + "%' AND name like '" + filter_seller_name.Text + "%'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        DataTable data = new DataTable();
                        con.Open();
                        data.Load(cmd.ExecuteReader());

                        seller_view.DataSource = data;
                        con.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else if (!(filter_quantity.Text.Equals("")) && (filter_seller_name.Text.Equals(""))) {
                    try {
                        String conString = "select * from db_plastic_management.dbo.KabariaSeller where price like '" + filter_price.Text + "%' AND quantityBought like '" + filter_quantity.Text + "%'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        DataTable data = new DataTable();
                        con.Open();
                        data.Load(cmd.ExecuteReader());

                        seller_view.DataSource = data;
                        con.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else {
                    try {
                        String conString = "select * from db_plastic_management.dbo.KabariaSeller where price like '" + filter_price.Text + "%' AND name like '" + filter_seller_name.Text + "%' AND quantityBought like '" + filter_quantity.Text + "%'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        DataTable data = new DataTable();
                        con.Open();
                        data.Load(cmd.ExecuteReader());

                        seller_view.DataSource = data;
                        con.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            filter_address.Text = "";
            filter_advance_payment.Text = "";
        }

        private void filter_address_TextChanged(object sender, EventArgs e)
        {
            if ((filter_seller_name.Text.Equals("")) && (filter_quantity.Text.Equals("")) && (filter_price.Text.Equals(""))) {
                try {
                    String conString = "select * from db_plastic_management.dbo.KabariaSeller where usr_address like '" + filter_address.Text + "%'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    DataTable data = new DataTable();
                    con.Open();
                    data.Load(cmd.ExecuteReader());

                    seller_view.DataSource = data;
                    con.Close();
                } catch (Exception ex) {
                    MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                if (!(filter_seller_name.Text.Equals("")) && (filter_quantity.Text.Equals("")) && (filter_price.Text.Equals(""))) {
                    try {
                        String conString = "select * from db_plastic_management.dbo.KabariaSeller where usr_address like '" + filter_address.Text + "%' AND name like '" + filter_seller_name.Text + "%'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        DataTable data = new DataTable();
                        con.Open();
                        data.Load(cmd.ExecuteReader());

                        seller_view.DataSource = data;
                        con.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else if (!(filter_quantity.Text.Equals("")) && (filter_seller_name.Text.Equals("")) && (filter_price.Text.Equals(""))) {
                    try {
                        String conString = "select * from db_plastic_management.dbo.KabariaSeller where usr_address like '" + filter_address.Text + "%' AND quantityBought like '" + filter_quantity.Text + "%'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        DataTable data = new DataTable();
                        con.Open();
                        data.Load(cmd.ExecuteReader());

                        seller_view.DataSource = data;
                        con.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else if (!(filter_price.Text.Equals("")) && (filter_seller_name.Text.Equals("")) && (filter_quantity.Text.Equals(""))) {
                    try {
                        String conString = "select * from db_plastic_management.dbo.KabariaSeller where usr_address like '" + filter_address.Text + "%' AND price like '" + filter_price.Text + "%'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        DataTable data = new DataTable();
                        con.Open();
                        data.Load(cmd.ExecuteReader());

                        seller_view.DataSource = data;
                        con.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else {
                    try {
                        String conString = "select * from db_plastic_management.dbo.KabariaSeller where usr_address like '" + filter_address.Text + "%' AND name like '" + filter_seller_name.Text + "%' AND quantityBought like '" + filter_quantity.Text + "%' AND price like '" + filter_price.Text + "%'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        DataTable data = new DataTable();
                        con.Open();
                        data.Load(cmd.ExecuteReader());

                        seller_view.DataSource = data;
                        con.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            filter_advance_payment.Text = "";
        }

        private void filter_advance_payment_TextChanged(object sender, EventArgs e)
        {
            if ((filter_seller_name.Text.Equals("")) && (filter_quantity.Text.Equals("")) && (filter_price.Text.Equals("")) && (filter_address.Text.Equals(""))) {
                try {
                    String conString = "select * from db_plastic_management.dbo.KabariaSeller where advance_money like '" + filter_advance_payment.Text + "%'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    DataTable data = new DataTable();
                    con.Open();
                    data.Load(cmd.ExecuteReader());

                    seller_view.DataSource = data;
                    con.Close();
                } catch (Exception ex) {
                    MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                if (!(filter_seller_name.Text.Equals("")) && (filter_quantity.Text.Equals("")) && (filter_price.Text.Equals("")) && (filter_address.Text.Equals(""))) {
                    try {
                        String conString = "select * from db_plastic_management.dbo.KabariaSeller where advance_money like '" + filter_advance_payment.Text + "%' AND name like '" + filter_seller_name.Text + "%'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        DataTable data = new DataTable();
                        con.Open();
                        data.Load(cmd.ExecuteReader());

                        seller_view.DataSource = data;
                        con.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else if (!(filter_quantity.Text.Equals("")) && (filter_seller_name.Text.Equals("")) && (filter_price.Text.Equals("")) && (filter_address.Text.Equals(""))) {
                    try {
                        String conString = "select * from db_plastic_management.dbo.KabariaSeller advance_money like '" + filter_advance_payment.Text + "%' AND quantityBought like '" + filter_quantity.Text + "%'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        DataTable data = new DataTable();
                        con.Open();
                        data.Load(cmd.ExecuteReader());

                        seller_view.DataSource = data;
                        con.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else if (!(filter_price.Text.Equals("")) && (filter_seller_name.Text.Equals("")) && (filter_quantity.Text.Equals("")) && (filter_address.Text.Equals(""))) {
                    try {
                        String conString = "select * from db_plastic_management.dbo.KabariaSeller advance_money like '" + filter_advance_payment.Text + "%' AND price like '" + filter_price.Text + "%'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        DataTable data = new DataTable();
                        con.Open();
                        data.Load(cmd.ExecuteReader());

                        seller_view.DataSource = data;
                        con.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else if (!(filter_address.Text.Equals("")) && (filter_seller_name.Text.Equals("")) && (filter_quantity.Text.Equals("")) && (filter_price.Text.Equals(""))) {
                    try {
                        String conString = "select * from db_plastic_management.dbo.KabariaSeller where advance_money like '" + filter_advance_payment.Text + "%' AND usr_address like '" + filter_address.Text + "%'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        DataTable data = new DataTable();
                        con.Open();
                        data.Load(cmd.ExecuteReader());

                        seller_view.DataSource = data;
                        con.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else {
                    try {
                        String conString = "select * from db_plastic_management.dbo.KabariaSeller where advance_money like '" + filter_advance_payment.Text + "%' AND name like '" + filter_seller_name.Text + "%' AND quantityBought like '" + filter_quantity.Text + "%' AND price like '" + filter_price.Text + "%' AND usr_address like '" + filter_address.Text + "%'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        DataTable data = new DataTable();
                        con.Open();
                        data.Load(cmd.ExecuteReader());

                        seller_view.DataSource = data;
                        con.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void seller_view_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try {
                this.ID =  Convert.ToInt32(seller_view.CurrentRow.Cells[0].Value.ToString());
                this.txt_seller_name.Text = seller_view.CurrentRow.Cells[1].Value.ToString();
                double seller_Quantity = Convert.ToDouble(seller_view.CurrentRow.Cells[2].Value.ToString());
                this.txt_seller_quantity.Text = seller_view.CurrentRow.Cells[2].Value.ToString();
                double pricePerQuantity = (Convert.ToDouble(seller_view.CurrentRow.Cells[3].Value.ToString())) / (Convert.ToDouble(seller_view.CurrentRow.Cells[2].Value.ToString()));
                this.txt_seller_price.Text = pricePerQuantity.ToString();
                this.txt_seller_address.Text = seller_view.CurrentRow.Cells[4].Value.ToString();
                this.txt_seller_advance.Text = seller_view.CurrentRow.Cells[5].Value.ToString();

                try {
                    String conString = "select * from  db_plastic_management.dbo.BuyingExpenses where b_id = '" + this.ID + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    DataTable dt_buyingExpenses = new DataTable();

                    con.Open();
                    dt_buyingExpenses.Load(cmd.ExecuteReader());
                    foreach(DataRow row in dt_buyingExpenses.Rows) {
                        txt_diesel_quantity.Text = row[2].ToString();
                        double pricePerLitter = (Convert.ToDouble(row[3].ToString())) / (Convert.ToDouble(row[2].ToString()));
                        txt_diesel_price.Text = pricePerLitter.ToString();
                        txt_food_price.Text = row[4].ToString();
                        double loadingpricePerKg = (Convert.ToDouble(row[5].ToString())) / (seller_Quantity);
                        txt_loading_price.Text = loadingpricePerKg.ToString();
                    }
                    con.Close();
                } catch (Exception ex) {
                    MessageBox.Show("Unable to fetch and convert values from BuyingExpenses of a seller from the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } catch(Exception ex) {
                MessageBox.Show("Conversion of values failed!", "Conversion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void seller_view_refresh_Click(object sender, EventArgs e)
        {
            refreshSellerView();
        }

        private void refreshSellerView()
        {
            try {
                String conString = "select * from db_plastic_management.dbo.KabariaSeller where advance_money like '" + filter_advance_payment.Text + "%' AND name like '" + filter_seller_name.Text + "%' AND quantityBought like '" + filter_quantity.Text + "%' AND price like '" + filter_price.Text + "%' AND usr_address like '" + filter_address.Text + "%'";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable data = new DataTable();
                con.Open();
                data.Load(cmd.ExecuteReader());

                seller_view.DataSource = data;
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Unable to connect with database server.", "Refresh Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void save_seller_Click(object sender, EventArgs e)
        {
            // Using Some local variable(memory efficient) for calculations.
            double quantity = -1D, price = -1D, loadingPrice = -1D, dieselQuantity = -1D, dieselPrice = -1D, advance = -1D, foodPrice = -1D;
            
            // Fields conversion checks, notifications and obtaining converted values
            quantity = convert(txt_seller_quantity.Text);
            price = convert(txt_seller_price.Text);
            dieselQuantity = convert(txt_diesel_quantity.Text);
            dieselPrice = convert(txt_diesel_price.Text);
            loadingPrice = convert(txt_loading_price.Text);
            advance = convert(txt_seller_advance.Text);
            foodPrice = convert(txt_food_price.Text);

            // Applying field checks to ensure data integrity & error removal.
            if (!(quantity == -1D) && !(price == -1D) && !(dieselQuantity == -1D) && !(dieselPrice == -1D) && !(loadingPrice == -1D) && !(advance == -1D) && !(foodPrice == -1D)) {
                actualPrice = quantity * price;
                actualLoadingPrice = quantity * loadingPrice;
                actualDieselPrice = dieselQuantity * dieselPrice;
            }
            else {
                if(quantity == -1D) {
                    txt_seller_quantity.Text = "";
                }else if(price == -1D) {
                    txt_seller_price.Text = "";
                }else if(dieselQuantity == -1D) {
                    txt_diesel_quantity.Text = "";
                }else if(dieselPrice == -1D) {
                    txt_diesel_price.Text = "";
                }else if(loadingPrice == -1D) {
                    txt_loading_price.Text = "";
                }else if(advance == -1D) {
                    txt_seller_advance.Text = "";
                }else if(foodPrice == -1D) {
                    txt_food_price.Text = "";
                }else if((advance == -1D) && (foodPrice == -1D)) {
                    txt_seller_advance.Text = "";
                    txt_food_price.Text = "";
                }
                else {
                    txt_seller_quantity.Text = "";
                    txt_seller_price.Text = "";
                    txt_diesel_quantity.Text = "";
                    txt_diesel_price.Text = "";
                    txt_loading_price.Text = "";
                    txt_seller_advance.Text = "";
                    txt_food_price.Text = "";
                }
                return;
            }

            try {
                String conString = "insert into db_plastic_management.dbo.KabariaSeller(name, quantityBought, price, usr_address, advance_money) values('" + txt_seller_name.Text + "' , '" + txt_seller_quantity.Text + "' , '" + actualPrice + "' , '" + txt_seller_address.Text + "' , '" + txt_seller_advance.Text + "')";
                SqlCommand cmd = new SqlCommand(conString, con);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Unable to insert into the Database Server...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            insertBuyingExpenses();

            // Resetting values for later use of the variables.
            actualDieselPrice = -1D;
            actualPrice = -1D;
            actualLoadingPrice = -1D;

            // Resetting Fields
            refresh();
            populateLoadingPrice();
        }

        private void btn_close_form_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void config_blades_Click(object sender, EventArgs e)
        {
            CuttingBladesExpenses cuttingBlades = new CuttingBladesExpenses();
            cuttingBlades.Show();
        }

        private void config_chemical_Click(object sender, EventArgs e)
        {
            ChemicalExpenses chemicalExpenses = new ChemicalExpenses();
            chemicalExpenses.Show();
        }

        private void config_packingBags_Click(object sender, EventArgs e)
        {
            PackingBagExpenses packingBagExpenses = new PackingBagExpenses();
            packingBagExpenses.Show();
        }

        private void Purchase_Load(object sender, EventArgs e)
        {
            if (flag_night_mode == true) {
                this.BackColor = Color.DarkGray;
            }
            refreshSellerView();
            populateLoadingPrice();
        }

        private void populateLoadingPrice()
        {
            txt_loading_price.Items.Clear();
            DataTable distinct_WholeLoading_prices = new DataTable();
            DataTable distinct_quantities = new DataTable();
            List<double> wholeLoadingPrice = new List<double>();
            List<double> quantities = new List<double>();
            List<double> LoadingPricePerQuantity = new List<double>();
            try {
                String conString = "select loading_price from db_plastic_management.dbo.BuyingExpenses";
                SqlCommand cmd = new SqlCommand(conString, con);

                
                con.Open();
                distinct_WholeLoading_prices.Load(cmd.ExecuteReader());
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Unable to populate Loading price", "Connectivity Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try {
                String conString = "select quantityBought from db_plastic_management.dbo.KabariaSeller";
                SqlCommand cmd = new SqlCommand(conString, con);


                con.Open();
                distinct_quantities.Load(cmd.ExecuteReader());
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Unable to populate Loading price", "Connectivity Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach(DataRow row in distinct_quantities.Rows) {
                quantities.Add(Convert.ToDouble(row[0].ToString()));
            }

            foreach (DataRow row in distinct_WholeLoading_prices.Rows) {
                wholeLoadingPrice.Add(Convert.ToDouble(row[0].ToString()));
            }

            for (int i = 0; i < wholeLoadingPrice.Count; i++) {
                LoadingPricePerQuantity.Add((wholeLoadingPrice[i] / quantities[i]));
            }

            LoadingPricePerQuantity.Sort();

            LoadingPricePerQuantity = LoadingPricePerQuantity.Distinct().ToList();

            for (int i = 0; i < LoadingPricePerQuantity.Count; i++) {
                txt_loading_price.Items.Add((LoadingPricePerQuantity[i]).ToString());
            }
        }

        private void refresh()
        {
            txt_seller_name.Text = "";
            txt_seller_quantity.Text = "";
            txt_seller_price.Text = "";
            txt_seller_advance.Text = "";
            txt_seller_address.Text = "";
            txt_diesel_quantity.Text = "";
            txt_diesel_price.Text = "";
            txt_food_price.Text = "";
            txt_loading_price.Text = "";

            try {
                String conString = "select KabariaSeller.ks_id, KabariaSeller.name, KabariaSeller.quantityBought, KabariaSeller.price, KabariaSeller.usr_address, KabariaSeller.advance_money, BuyingExpenses.diesel_quantity, BuyingExpenses.diesel_price, BuyingExpenses.food_price, BuyingExpenses.loading_price from db_plastic_management.dbo.KabariaSeller INNER JOIN db_plastic_management.dbo.BuyingExpenses ON KabariaSeller.ks_id = BuyingExpenses.b_id;";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable data = new DataTable();
                con.Open();
                data.Load(cmd.ExecuteReader());

                seller_view.DataSource = data;
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Unable to connect with database server.", "Filteration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void insertBuyingExpenses()
        {
            try {
                String conString = "select ks_id from db_plastic_management.dbo.KabariaSeller where name = '" + txt_seller_name.Text + "' AND quantityBought = '" + txt_seller_quantity.Text + "' AND price = '" + actualPrice + "' AND usr_address = '" + txt_seller_address.Text + "' AND advance_money = '" + txt_seller_advance.Text + "'";
                SqlCommand cmd = new SqlCommand(conString, con);

                int id = -1;
                DataTable dt = new DataTable();

                con.Open();
                var dataAdapter = cmd.ExecuteReader();
                dt.Load(dataAdapter);

                foreach (DataRow row in dt.Rows) {
                    id = Convert.ToInt32(row[0]);
                }
                con.Close();

                if (id != -1) {
                    try {
                        String conString1 = "insert into db_plastic_management.dbo.BuyingExpenses(b_id, ks_id, diesel_quantity, diesel_price, food_price, loading_price) values('" + id + "','" + id + "','" + txt_diesel_quantity.Text + "','" + actualDieselPrice + "','" + txt_food_price.Text + "','" + actualLoadingPrice + "')";
                        cmd = new SqlCommand(conString1, con);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Insertion of new Seller was sucessful!", "Insertion Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex) {
                        MessageBox.Show("Unable to insert buying expenses into the database server...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else {
                    MessageBox.Show("Unable to Forcast seller id from the database server...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } catch (Exception ex) {
                MessageBox.Show("Unable to Forcast seller id.\n Failed to insert buying expenses into the database server.\n Please Check the database server connectivity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
